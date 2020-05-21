package models

import (
	"fake-product/database"
	"time"
)

type Record struct {
	ID        uint `gorm:"primary_key"`
	CreatedAt time.Time
	AccountID int
	ProductID int
}

func (r *Record) NewDBRecordSchema()  {
	db := database.Context
	if !db.HasTable(Record{}) {
		db.CreateTable(Record{})
	}
}