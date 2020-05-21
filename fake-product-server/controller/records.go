package controller

import (
	"fake-product/database"
	"fake-product/models"
	"github.com/gin-gonic/gin"
	"net/http"
	"strconv"
	"strings"
)

func GetRecords(c *gin.Context) {
	accessToken := c.GetHeader("accessToken")
	userID, _ := strconv.Atoi(strings.Split(accessToken, "_")[0])

	var records []struct {
		ID          int             `json:"id"`
		CreatedAt   models.JSONTime `json:"created_at"`
		ProductID   int             `json:"product_id"`
		ProductName string          `json:"product_name" gorm:"column:name"`
		Price       int             `json:"price"`
	}

	selectString := "records.id,records.created_at,records.product_id,product_infos.name,product_infos.price"
	joinsString := "left join product_infos on records.product_id =product_infos.id"
	database.Context.Model(models.Record{}).Select(selectString).Joins(joinsString).Where("account_id = ?", userID).Scan(&records)

	c.JSON(http.StatusOK, gin.H{
		"records": records,
	})
}
