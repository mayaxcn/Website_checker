SET FOREIGN_KEY_CHECKS = 0;

DROP TABLE IF EXISTS  `pingcheck`;
CREATE TABLE `pingcheck` (
  `pingID` bigint(20) NOT NULL AUTO_INCREMENT,
  `pingTime` datetime DEFAULT NULL,
  `pingDelay` double DEFAULT NULL,
  `pingStatus` bit(1) DEFAULT NULL,
  PRIMARY KEY (`pingID`),
  KEY `pingID` (`pingID`) USING BTREE,
  KEY `pingTime` (`pingTime`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=7990 DEFAULT CHARSET=utf8;


insert into `pingcheck`(`pingID`,`pingTime`,`pingDelay`,`pingStatus`)
values(7988,'2020-05-14 11:30:23',2001.253,b'0');
insert into `pingcheck`(`pingID`,`pingTime`,`pingDelay`,`pingStatus`)
values(7989,'2020-05-14 11:30:53',2000.212,b'0');
DROP TABLE IF EXISTS  `pingchecklog`;
CREATE TABLE `pingchecklog` (
  `pingTid` int(11) NOT NULL AUTO_INCREMENT,
  `pingDatetime` datetime DEFAULT NULL,
  `pingMin` double DEFAULT NULL,
  `pingMax` double DEFAULT NULL,
  `pingAvg` double DEFAULT NULL,
  `pingSuss` int(11) DEFAULT NULL,
  `pingFail` int(11) DEFAULT NULL,
  PRIMARY KEY (`pingTid`)
) ENGINE=InnoDB AUTO_INCREMENT=75 DEFAULT CHARSET=utf8;


insert into `pingchecklog`(`pingTid`,`pingDatetime`,`pingMin`,`pingMax`,`pingAvg`,`pingSuss`,`pingFail`)
values(73,'2020-05-14 09:00:00',13.452,33.493,17.4416694214876,121,2);
insert into `pingchecklog`(`pingTid`,`pingDatetime`,`pingMin`,`pingMax`,`pingAvg`,`pingSuss`,`pingFail`)
values(74,'2020-05-14 10:00:00',14.727,35.446,25.1558214285714,112,8);
SET FOREIGN_KEY_CHECKS = 1;

