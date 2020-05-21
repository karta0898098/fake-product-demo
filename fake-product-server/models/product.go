package models

import (
	"fake-product/database"
)

//ProductInfo 產品資料
type ProductInfo struct {
	ID          int    `json:"id"gorm:"primary_key"`
	Name        string `json:"name"gorm:"type:nvarchar(100)"`
	Description string `json:"description"gorm:"type:nvarchar(1000)"`
	Price       int    `json:"price"`
}

//NewDBProductInfoModel 新增使用者DB Schema
func (u *ProductInfo) NewDBProductInfoModel() {

	db := database.Context
	if !db.HasTable(ProductInfo{}) {
		db.CreateTable(ProductInfo{})
	}
}
