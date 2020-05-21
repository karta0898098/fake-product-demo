package controller

import (
	"fake-product/database"
	productserror "fake-product/errors"
	"fake-product/models"
	"github.com/gin-gonic/gin"
	"net/http"
	"strconv"
)

//GetAllProductsInfo 取得產品資訊
func GetAllProductsInfo(c *gin.Context) {
	var products []struct {
		ID    int    `json:"id"`
		Name  string `json:"name"`
		Price int    `json:"price"`
	}

	db := database.Context
	db.Model(models.ProductInfo{}).Select("id,name,price").Scan(&products)

	c.JSON(http.StatusOK, gin.H{
		"products": products,
	})
}

func GetProductInfo(c *gin.Context) {

	parmID := c.Param("id")
	id, err := strconv.Atoi(parmID)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": productserror.ParmIsNotCorrectType})
		return
	}

	var product models.ProductInfo
	recordNotFound := database.Context.Model(models.ProductInfo{}).Where("id = ?", id).Find(&product).RecordNotFound()
	if recordNotFound {
		c.JSON(http.StatusBadRequest, gin.H{"error": productserror.RecordIsNotExist})
		return
	}

	c.JSON(http.StatusOK, product)
}
