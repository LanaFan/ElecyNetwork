/*
Navicat MySQL Data Transfer

Source Server         : Elecy
Source Server Version : 50505
Source Host           : localhost:3306
Source Database       : elecyproject

Target Server Type    : MYSQL
Target Server Version : 50505
File Encoding         : 65001

Date: 2018-08-14 15:31:46
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `skillbuilds`
-- ----------------------------
DROP TABLE IF EXISTS `skillbuilds`;
CREATE TABLE `skillbuilds` (
  `Nickname` varchar(25) NOT NULL,
  `Ignis 0 Spell` varchar(8) DEFAULT NULL,
  `Ignis 1 Spell` varchar(8) DEFAULT NULL,
  `Ignis 2 Spell` varchar(8) DEFAULT NULL,
  `Ignis 3 Spell` varchar(8) DEFAULT NULL,
  `Ignis 4 Spell` varchar(8) DEFAULT NULL,
  `Ignis 5 Spell` varchar(8) DEFAULT NULL,
  `Ignis 6 Spell` varchar(8) DEFAULT NULL,
  `Ignis 7 Spell` varchar(8) DEFAULT NULL,
  `Ignis 8 Spell` varchar(8) DEFAULT NULL,
  PRIMARY KEY (`Nickname`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of skillbuilds
-- ----------------------------
INSERT INTO `skillbuilds` VALUES ('Ludaris', '00000000', '00000000', '00000000', '00000000', '00000000', '00000000', '00000000', '00000000', '00000000');
INSERT INTO `skillbuilds` VALUES ('Onn', '00000000', '00000000', '00000000', '00000000', '00000000', '00000000', '00000000', '00000000', '00000000');
INSERT INTO `skillbuilds` VALUES ('Tarkes', '00000000', '00000000', '00000000', '00000000', '00000000', '00000000', '00000000', '00000000', '00000000');
