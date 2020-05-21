package controller

import (
	"errors"
	"fake-product/database"
	productserror "fake-product/errors"
	"fake-product/models"
	"github.com/gin-gonic/gin"
	"github.com/jinzhu/gorm"
	"net/http"
	"strconv"
	"strings"
	"time"
)

func BuyProduct(c *gin.Context) {
	accessToken := c.GetHeader("accessToken")
	userID, _ := strconv.Atoi(strings.Split(accessToken, "_")[0])

	parmID := c.Param("id")
	productID, err := strconv.Atoi(parmID)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": productserror.ParmIsNotCorrectType})
		return
	}

	var userInfo models.UserInfo
	var product models.ProductInfo
	var record models.Record

	err = database.Context.Transaction(func(tx *gorm.DB) error {
		err := tx.Model(models.ProductInfo{}).Where("id = ?", productID).Find(&product).Error
		if err != nil {
			return err
		}

		err = tx.Model(models.UserInfo{}).Where("account = ?", userID).Find(&userInfo).Error
		if err != nil {
			return err
		}

		result := userInfo.Money - product.Price
		if result < 0 {
			return errors.New(productserror.NotEnoughMoney)
		}

		err = tx.Model(models.UserInfo{}).Where("account = ?", userID).Update("money", result).Find(&userInfo).Error
		if err != nil {
			return err
		}

		record = models.Record{
			CreatedAt: time.Now(),
			AccountID: userID,
			ProductID: product.ID,
		}
		err = tx.Model(models.Record{}).Create(&record).Error
		if err != nil {
			return err
		}
		return nil
	})

	if err != nil {
		c.JSON(http.StatusNotAcceptable, gin.H{"error": err.Error()})
		return
	}

	c.JSON(http.StatusOK, gin.H{
		"create_at": record.CreatedAt.Format("2006-01-02 15:04:05"),
		"record_id": record.ID,
		"product": gin.H{
			"name":  product.Name,
			"price": product.Price,
		},
		"user": gin.H{
			"name":  userInfo.Name,
			"money": userInfo.Money,
		},
	})
}
