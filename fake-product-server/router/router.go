package router

import (
	"fake-product/controller"
	"fake-product/middleware"
	"github.com/gin-gonic/gin"
)

//RegisterRouter 註冊路由
func RegisterRouter(engine *gin.Engine) {
	//engine.GET("/register", controller.RegisterView)
	//engine.POST("/register", controller.Register)

	api := engine.Group("/api")
	api.POST("/login", controller.Login)
	api.POST("/register", controller.Register)

	authAPI:= api.Group("")
	authAPI.Use(middleware.AuthorizationToken)
	authAPI.GET("/products", controller.GetAllProductsInfo)
	authAPI.GET("/products/:id", controller.GetProductInfo)
	authAPI.POST("/products/:id", controller.BuyProduct)
	authAPI.GET("/records",controller.GetRecords)
}
