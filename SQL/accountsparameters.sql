/*
Navicat MySQL Data Transfer

Source Server         : Elecy
Source Server Version : 50505
Source Host           : localhost:3306
Source Database       : elecyproject

Target Server Type    : MYSQL
Target Server Version : 50505
File Encoding         : 65001

Date: 2018-08-14 15:21:33
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `accountsparameters`
-- ----------------------------
DROP TABLE IF EXISTS `accountsparameters`;
CREATE TABLE `accountsparameters` (
  `Nickname` varchar(30) NOT NULL,
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
  PRIMARY KEY (`Nickname`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of accountsparameters
-- ----------------------------
INSERT INTO `accountsparameters` VALUES ('Ludaris', '1', '1', '1', '1', '1', '0', '0', '0', '0', '0');
INSERT INTO `accountsparameters` VALUES ('Onn', '1', '1', '1', '1', '1', '0', '0', '0', '0', '0');
INSERT INTO `accountsparameters` VALUES ('Tarkes', '1', '1', '1', '1', '1', '0', '0', '0', '0', '0');
