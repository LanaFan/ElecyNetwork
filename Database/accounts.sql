/*
Navicat MySQL Data Transfer

Source Server         : Elecy
Source Server Version : 50505
Source Host           : localhost:3306
Source Database       : elecyproject

Target Server Type    : MYSQL
Target Server Version : 50505
File Encoding         : 65001

Date: 2018-02-25 21:21:03
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `accounts`
-- ----------------------------
DROP TABLE IF EXISTS `accounts`;
CREATE TABLE `accounts` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Username` varchar(30) DEFAULT NULL,
  `Password` varchar(30) DEFAULT NULL,
  `Nickname` varchar(30) DEFAULT NULL,
  `IgnisLevel` int(3) DEFAULT NULL,
  `TerraLevel` int(3) DEFAULT NULL,
  `CaeliLevel` int(3) DEFAULT NULL,
  `AquaLevel` int(3) DEFAULT NULL,
  `PrimusLevel` int(3) DEFAULT NULL,
  `IgnisRank` int(7) DEFAULT NULL,
  `TerraRank` int(7) DEFAULT NULL,
  `CaeliRank` int(7) DEFAULT NULL,
  `AquaRank` int(7) DEFAULT NULL,
  `PrimusRank` int(7) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of accounts
-- ----------------------------
