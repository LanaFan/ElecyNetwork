/*
Navicat MySQL Data Transfer

Source Server         : Elecy
Source Server Version : 50505
Source Host           : localhost:3306
Source Database       : elecyproject

Target Server Type    : MYSQL
Target Server Version : 50505
File Encoding         : 65001

Date: 2018-06-20 21:28:57
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `mapsinfo`
-- ----------------------------
DROP TABLE IF EXISTS `mapsinfo`;
CREATE TABLE `mapsinfo` (
  `MapNumber` int(10) NOT NULL,
  `MapLenght` int(3) NOT NULL,
  `MapWidth` int(3) NOT NULL,
  `FirstSpawnPointX` float(5,2) NOT NULL,
  `FirstSpawnPointY` float(5,2) NOT NULL,
  `FirstSpawnPointZ` float(5,2) NOT NULL,
  `SecondSpawnPointX` float(5,2) NOT NULL,
  `SecondSpawnPointY` float(5,2) NOT NULL,
  `SecondSpawnPointZ` float(5,2) NOT NULL,
  `FirstSpawnPointRotX` float(5,2) NOT NULL,
  `FirstSpawnPointRotY` float(5,2) NOT NULL,
  `FirstSpawnPointRotZ` float(5,2) NOT NULL,
  `FirstSpawnPointRotW` float(5,2) NOT NULL,
  `SecondSpawnPointRotX` float(5,2) NOT NULL,
  `SecondSpawnPointRotY` float(5,2) NOT NULL,
  `SecondSpawnPointRotZ` float(5,2) NOT NULL,
  `SecondSpawnPointRotW` float(5,2) NOT NULL,
  PRIMARY KEY (`MapNumber`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of mapsinfo
-- ----------------------------
INSERT INTO `mapsinfo` VALUES ('2', '5', '5', '-10.00', '0.50', '0.00', '10.00', '0.50', '0.00', '0.00', '90.00', '0.00', '0.00', '0.00', '-90.00', '0.00', '1.00');
INSERT INTO `mapsinfo` VALUES ('3', '8', '8', '-34.50', '0.50', '0.00', '34.50', '0.50', '0.00', '0.00', '90.00', '0.00', '0.00', '0.00', '-90.00', '0.00', '1.00');
