USE `tabletop`;

#
# Dumping data for table 'units'
#

INSERT INTO `units` (`name`, `fraction`, `description`, `defense`, `moving`) VALUES ("212th Attack Battalion", "GAR", "Clone Trooper", 6, 17);
INSERT INTO `units` (`name`, `fraction`, `description`, `defense`, `moving`) VALUES ("B1 Battle Droid", "KUS", "Droid", 2, 15);
# 2 records

#
# Dumping data for table 'weapons'
#

INSERT INTO `weapons` (`name`, `description`, `attack`, `quality`, `range`, `dices`) VALUES ("DC-15S", "Blaster", 6, 5, 45, 1);
INSERT INTO `weapons` (`name`, `description`, `attack`, `quality`, `range`, `dices`) VALUES ("E-5", "Blaster", 6, 4, 40, 1);
# 2 records