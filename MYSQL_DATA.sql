SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;

USE `tabletop`;

#
# Dumping data for table 'units'
#

INSERT INTO `units` (`unit_id`, `name`, `fraction`, `description`, `defense`, `moving`) VALUES (1, '212th Attack Battalion', 'GAR', 'Clone Trooper', 6, 17);
INSERT INTO `units` (`unit_id`, `name`, `fraction`, `description`, `defense`, `moving`) VALUES (2, 'B1 Battle Droid', 'KUS', 'Droid', 2, 15);
INSERT INTO `units` (`unit_id`, `name`, `fraction`, `description`, `defense`, `moving`) VALUES (3, 'B1 Sniper Droid', 'KUS', 'Droid', 2, 13);
# 3 records

#
# Dumping data for table 'weapons'
#

INSERT INTO `weapons` (`weapon_id`, `name`, `description`, `attack`, `quality`, `range`, `dices`) VALUES (1, 'DC-15S', 'Blaster Carbine', 6, 5, 45, 1);
INSERT INTO `weapons` (`weapon_id`, `name`, `description`, `attack`, `quality`, `range`, `dices`) VALUES (2, 'E-5', 'Blaster Rifle', 6, 4, 40, 1);
INSERT INTO `weapons` (`weapon_id`, `name`, `description`, `attack`, `quality`, `range`, `dices`) VALUES (3, 'E-5S', 'Sniper Rifle', 10, 6, 80, 1);
# 3 records

#
# Dumping data for table 'unit_weapons'
#

INSERT INTO `unit_weapons` (`unit_id`, `weapon_id`, `is_primary`) VALUES (1, 1, 0);
INSERT INTO `unit_weapons` (`unit_id`, `weapon_id`, `is_primary`) VALUES (2, 2, 0);
INSERT INTO `unit_weapons` (`unit_id`, `weapon_id`, `is_primary`) VALUES (3, 3, 0);
# 3 records

#
# Dumping data for table 'permissions'
#

INSERT INTO `permissions` (`permission_id`, `name`, `identifier`, `description`) VALUES (1, 'Edit Entities', 'EDIT_ENTITIES', 'Allows the user to create, edit and delete units and weapons.');
INSERT INTO `permissions` (`permission_id`, `name`, `identifier`, `description`) VALUES (2, 'User management','EDIT_USERS','Allows the user to manage the users.');
# 2 records

SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;