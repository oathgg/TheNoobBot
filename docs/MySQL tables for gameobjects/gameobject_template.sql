CREATE TABLE `gameobject_template` (
  `entry` int(10) unsigned NOT NULL DEFAULT '0',
  `type` int(10) unsigned NOT NULL DEFAULT '0',
  `name` varchar(100) NOT NULL,
  `iconname` varchar(100) NOT NULL,
  `castbarcaption` varchar(100) NOT NULL,
  `model` int(10) unsigned NOT NULL DEFAULT '0',
  `faction` int(10) unsigned NOT NULL DEFAULT '0',
  `flags` int(10) unsigned NOT NULL DEFAULT '0',
  `size` float NOT NULL DEFAULT '0',
  `data0` int(10) unsigned NOT NULL DEFAULT '0',
  `data1` int(10) unsigned NOT NULL DEFAULT '0',
  `data2` int(10) unsigned NOT NULL DEFAULT '0',
  `data3` int(10) unsigned NOT NULL DEFAULT '0',
  `data4` int(10) unsigned NOT NULL DEFAULT '0',
  `data5` int(10) unsigned NOT NULL DEFAULT '0',
  `data6` int(10) unsigned NOT NULL DEFAULT '0',
  `data7` int(10) unsigned NOT NULL DEFAULT '0',
  `data8` int(10) unsigned NOT NULL DEFAULT '0',
  `data9` int(10) unsigned NOT NULL DEFAULT '0',
  `data10` int(10) unsigned NOT NULL DEFAULT '0',
  `data11` int(10) unsigned NOT NULL DEFAULT '0',
  `data12` int(10) unsigned NOT NULL DEFAULT '0',
  `data13` int(10) unsigned NOT NULL DEFAULT '0',
  `data14` int(10) unsigned NOT NULL DEFAULT '0',
  `data15` int(10) unsigned NOT NULL DEFAULT '0',
  `data16` int(10) unsigned NOT NULL DEFAULT '0',
  `data17` int(10) unsigned NOT NULL DEFAULT '0',
  `data18` int(10) unsigned NOT NULL DEFAULT '0',
  `data19` int(10) unsigned NOT NULL DEFAULT '0',
  `data20` int(10) unsigned NOT NULL DEFAULT '0',
  `data21` int(10) unsigned NOT NULL DEFAULT '0',
  `data22` int(10) unsigned NOT NULL DEFAULT '0',
  `data23` int(10) unsigned NOT NULL DEFAULT '0',
  `data24` int(10) unsigned NOT NULL DEFAULT '0',
  `data25` int(10) unsigned NOT NULL DEFAULT '0',
  `data26` int(10) unsigned NOT NULL DEFAULT '0',
  `data27` int(10) unsigned NOT NULL DEFAULT '0',
  `data28` int(10) unsigned NOT NULL DEFAULT '0',
  `data29` int(10) unsigned NOT NULL DEFAULT '0',
  `data30` int(10) unsigned NOT NULL DEFAULT '0',
  `data31` int(10) unsigned NOT NULL DEFAULT '0',
  `questitem1` int(10) unsigned NOT NULL DEFAULT '0',
  `questitem2` int(10) unsigned NOT NULL DEFAULT '0',
  `questitem3` int(10) unsigned NOT NULL DEFAULT '0',
  `questitem4` int(10) unsigned NOT NULL DEFAULT '0',
  `questitem5` int(10) unsigned NOT NULL DEFAULT '0',
  `questitem6` int(10) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`entry`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;