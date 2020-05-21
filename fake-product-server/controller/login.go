package controller

import (
	"fake-product/database"
	"fake-product/middleware"
	"fake-product/models"
	"net/http"

	"github.com/gin-gonic/gin"
)

//RequestLogin 登入帳號必須
type RequestLogin struct {
	Account  string `form:"account" json:"account" binding:"required"`
	Password string `form:"password" json:"password" binding:"required"`
}

//Login 登入
func Login(c *gin.Context) {
	var req RequestLogin
	if err := c.ShouldBindJSON(&req); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{
			"error": err.Error(),
		})
		return
	}

	user := models.User{
		Account:  req.Account,
		Password: req.Password,
	}
	if err := user.AuthUser(); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{
			"error": err.Error(),
		})
		return
	}

	var userInfo models.UserInfo
	database.Context.Model(models.UserInfo{}).Find(&userInfo)

	accessToken := middleware.GenToken(int(user.ID))

	c.JSON(http.StatusOK, gin.H{
		"status": "ok",
		"accessToken":  accessToken,
		"userInfo": gin.H{
			"name":  userInfo.Name,
			"money": userInfo.Money,
		},
	})
}
