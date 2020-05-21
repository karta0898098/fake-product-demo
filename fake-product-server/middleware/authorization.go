package middleware

import (
	"fake-product/database"
	"net/http"
	"strconv"
	"strings"
	"time"

	"github.com/gin-gonic/gin"
	uuid "github.com/satori/go.uuid"
	"github.com/speps/go-hashids"
)

//ScreetKey Token的金鑰
const ScreetKey = "GK5zvfctGQzuCcDn"

//HashUserID Hash User ID
func HashUserID(id int) string {
	hd := hashids.NewData()
	hd.Salt = ScreetKey
	hd.MinLength = 4
	h, _ := hashids.NewWithData(hd)
	userID, _ := h.Encode([]int{id})

	return userID
}

//GenToken 生成Token
func GenToken(id int) string {
	// hd := hashids.NewData()
	// hd.Salt = ScreetKey
	// hd.MinLength = 8
	// h, _ := hashids.NewWithData(hd)
	// userID, _ := h.Encode([]int{id})

	userID := strconv.Itoa(id)
	uid := uuid.NewV4()
	token := userID + "_" + uid.String()

	database.Redis.Set(userID, uid.String(), time.Hour*24)
	return token
}

//AuthorizationToken 驗證Token
func AuthorizationToken(c *gin.Context) {

	accessToken := c.GetHeader("accessToken")

	if accessToken == "" {
		c.AbortWithStatusJSON(http.StatusBadRequest, gin.H{"error": "you need token"})
		return
	}

	userIDAndAccessToken := strings.Split(accessToken, "_")

	if len(userIDAndAccessToken) != 2 {
		c.AbortWithStatusJSON(http.StatusBadRequest, gin.H{"error": "token is not vailed"})
	}

	userID := userIDAndAccessToken[0]
	currentAccessToken := userIDAndAccessToken[1]
	token, _ := database.Redis.Get(userID).Result()
	isExist := database.Redis.Exists(userID).Val()

	if isExist == 1 && token == currentAccessToken {
		c.Next()
	} else {
		c.AbortWithStatusJSON(http.StatusBadRequest, gin.H{"error": "token is not vailed"})
	}
}
