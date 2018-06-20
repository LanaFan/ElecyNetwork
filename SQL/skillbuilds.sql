/*
Navicat MySQL Data Transfer

Source Server         : Elecy
Source Server Version : 50505
Source Host           : localhost:3306
Source Database       : elecyproject

Target Server Type    : MYSQL
Target Server Version : 50505
File Encoding         : 65001

Date: 2018-06-20 21:29:01
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `skillbuilds`
-- ----------------------------
DROP TABLE IF EXISTS `skillbuilds`;
CREATE TABLE `skillbuilds` (
  `Nickname` varchar(25) NOT NULL,
  `Ignis 1 Spell` int(5) DEFAULT NULL,
  `Ignis 2 Spell` int(5) DEFAULT NULL,
  `Ignis 3 Spell` int(5) DEFAULT NULL,
  `Ignis 4 Spell` int(5) DEFAULT NULL,
  `Ignis 5 Spell` int(5) DEFAULT NULL,
  `Ignis 6 Spell` int(5) DEFAULT NULL,
  `Ignis 7 Spell` int(5) DEFAULT NULL,
  PRIMARY KEY (`Nickname`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of skillbuilds
-- ----------------------------
INSERT INTO `skillbuilds` VALUES ('Ludaris', '0', '0', '0', '0', '0', '0', '0');
INSERT INTO `skillbuilds` VALUES ('Tarkes', '0', '0', '0', '0', '0', '0', '0');
