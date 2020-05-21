package models

import (
	"crypto/sha256"
	"encoding/hex"
	"errors"
	"fake-product/database"
	proudcterros "fake-product/errors"

	"github.com/jinzhu/gorm"
)

//User 使用者模型
type User struct {
	gorm.Model
	Account  string `gorm:"type:varchar(64)"`
	Password string `gorm:"type:varchar(64)"`
}

type UserInfo struct {
	gorm.Model
	Account int
	Name    string `gorm:"type:nvarchar(10)"`
	Money   int
}

//NewDBUserModel 新增使用者DB Schema
func (u *User) NewDBUserModel() {
	db := database.Context
	if !db.HasTable(User{}) {
		db.CreateTable(User{})
	}
}

func (c *UserInfo) NewDBUserInfoModel() {
	db := database.Context
	if !db.HasTable(UserInfo{}) {
		db.CreateTable(UserInfo{})
	}
}

//NewUser 新增使用者模型
func (u *User) NewUser(name string, info *UserInfo) error {
	return database.Context.Transaction(func(tx *gorm.DB) error {

		recordNotFound := tx.Model(User{}).Where("account = ?", u.Account).Find(&User{}).RecordNotFound()

		if !recordNotFound {
			return errors.New(proudcterros.AccountIsExist)
		}

		hash := sha256.Sum256([]byte(u.Password))
		u.Password = hex.EncodeToString(hash[:])

		err := tx.Model(User{}).Create(&u).Error
		if err != nil {
			return err
		}

		*info = UserInfo{
			Account: int(u.ID),
			Name:    name,
			Money:   1000,
		}
		err = tx.Model(UserInfo{}).Create(info).Error
		if err != nil{
			return err
		}
		return nil
	})
}

//AuthUser 檢查帳號密碼
func (u *User) AuthUser() error {
	var user User
	recordNotFound := database.Context.Model(User{}).Where("account = ?", u.Account).Find(&user).RecordNotFound()
	if recordNotFound {
		return errors.New(proudcterros.AccountIsNotExist)
	}

	hash := sha256.Sum256([]byte(u.Password))
	u.Password = hex.EncodeToString(hash[:])

	if user.Password != u.Password {
		return errors.New(proudcterros.AccountPasswordError)
	}
	u.Model = user.Model

	return nil
}
