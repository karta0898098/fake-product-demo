package controller

import (
	"fake-product/middleware"
	"fake-product/models"
	"net/http"

	"github.com/gin-gonic/gin"
)

//RequestNewUser 新增帳號必須
type RequestNewUser struct {
	Account  string `form:"account" json:"account" binding:"required"`
	Password string `form:"password" json:"password" binding:"required"`
	Name     string `form:"name" json:"name" binding:"required"`
}

//RegisterView 註冊的View
func RegisterView(c *gin.Context) {
	c.HTML(http.StatusOK, "register.html", nil)
}

//Register 新增角色
func Register(c *gin.Context) {
	var req RequestNewUser
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

	var userInfo models.UserInfo
	if err := user.NewUser(req.Name, &userInfo); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{
			"error": err.Error(),
		})
		return
	}

	accessToken := middleware.GenToken(int(user.ID))

	c.JSON(http.StatusOK, gin.H{
		"status":      "ok",
		"accessToken": accessToken,
		"userInfo": gin.H{
			"name":  userInfo.Name,
			"money": userInfo.Money,
		},
	})
}
