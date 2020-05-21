package main

import (
	"context"
	"net/http"
	"os"
	"os/signal"
	"syscall"

	"github.com/gin-contrib/sessions"
	"github.com/gin-contrib/sessions/cookie"
	"github.com/gin-gonic/gin"
	log "github.com/sirupsen/logrus"

	"fake-product/config"
	"fake-product/database"
	"fake-product/models"
	"fake-product/router"

	"time"

	_ "github.com/go-sql-driver/mysql"
)

func main() {

	//設定log format
	log.SetFormatter(&log.TextFormatter{
		ForceColors:     true,
		FullTimestamp:   true,
		TimestampFormat: time.RFC3339,
	})

	//讀入config.ini 配置檔案
	setting := config.NewConfig("config.ini")

	//Database連線
	database.NewDatabase(setting.Database)
	database.NewRedisDatabase()
	defer database.CloseDatabase()
	defer database.CloseRedis()

	NewDatabaseTable()

	//設定Gin Mode
	gin.SetMode(setting.Runtime.Mode)

	//初始化 cookie base Session
	store := cookie.NewStore([]byte("KUk3SF5J"))

	//初始化 Gin Router Engine
	engine := gin.New()

	//如果./templates沒有檔案 49行會出錯
	engine.LoadHTMLGlob("./templates/*.html")

	engine.Use(gin.Logger(), gin.Recovery())
	engine.Use(sessions.Sessions("fake-product", store))

	//註冊網頁路由
	router.RegisterRouter(engine)

	//Graceful shutdown server
	server := &http.Server{
		Addr:    setting.Runtime.Port,
		Handler: engine,
	}

	done := make(chan bool, 1)
	quit := make(chan os.Signal, 1)
	signal.Notify(quit, syscall.SIGINT, syscall.SIGTERM)
	go ShutdownServer(server, quit, done)

	if err := server.ListenAndServe(); err != nil && err != http.ErrServerClosed {
		log.Fatalf("Could not listen on  %v\n", err)
	}

	<-done
	log.Println("Server is exit")
}

//ShutdownServer 關閉Server
func ShutdownServer(server *http.Server, quit <-chan os.Signal, done chan<- bool) {
	<-quit
	log.Println("Shutdown Server ...")

	ctx, cancel := context.WithTimeout(context.Background(), 30*time.Second)
	defer cancel()

	server.SetKeepAlivesEnabled(false)
	if err := server.Shutdown(ctx); err != nil {
		log.Fatal("Server Shutdown: ", err)
	}
	close(done)
}

//NewDatabaseTable 新增或創建Table
func NewDatabaseTable() {
	users := models.User{}
	users.NewDBUserModel()

	productInfo := models.ProductInfo{}
	productInfo.NewDBProductInfoModel()

	userInfo := models.UserInfo{}
	userInfo.NewDBUserInfoModel()

	record := models.Record{}
	record.NewDBRecordSchema()
}
