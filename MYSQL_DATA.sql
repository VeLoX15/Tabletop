SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;

USE `tabletop`;

#
# Dumping data for table 'tabletop'.'units'
#

INSERT INTO `tabletop`.`units` (`unit_id`, `name`, `fraction_id`, `description`, `mechanic`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`) VALUES (1, '212th Attack Battalion', 1, '', '', 6, 17, 1, 1);
INSERT INTO `tabletop`.`units` (`unit_id`, `name`, `fraction_id`, `description`, `mechanic`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`) VALUES (2, 'B1 Battle Droid', 2, '', '', 2, 15, 1, 1);
INSERT INTO `tabletop`.`units` (`unit_id`, `name`, `fraction_id`, `description`, `mechanic`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`) VALUES (3, 'B1 Sniper Droid', 2, '', '', 2, 13, 1, 1);
# 3 records

#
# Dumping data for table 'tabletop'.'fractions'
#

INSERT INTO `tabletop`.`fractions` (`fraction_id`, `name`, `short_name`, `description`) VALUES (1,  'Galactic Army of Republic', 'GAR', '');
INSERT INTO `tabletop`.`fractions` (`fraction_id`, `name`, `short_name`, `description`) VALUES (2,  'Confoderacy of Independent Systems', 'CIS', '');
INSERT INTO `tabletop`.`fractions` (`fraction_id`, `name`, `short_name`, `description`) VALUES (3,  'Galactic Empire', 'EMP', '');
INSERT INTO `tabletop`.`fractions` (`fraction_id`, `name`, `short_name`, `description`) VALUES (4,  'Rebels', 'REB', '');
# 3 records

#
# Dumping data for table 'tabletop'.'weapons'
#

INSERT INTO `tabletop`.`weapons` (`weapon_id`, `name`, `description`, `attack`, `quality`, `range`, `dices`) VALUES (1, 'DC-15S', 'Blaster Carbine', 6, 5, 45, 1);
INSERT INTO `tabletop`.`weapons` (`weapon_id`, `name`, `description`, `attack`, `quality`, `range`, `dices`) VALUES (2, 'E-5', 'Blaster Rifle', 6, 4, 40, 1);
INSERT INTO `tabletop`.`weapons` (`weapon_id`, `name`, `description`, `attack`, `quality`, `range`, `dices`) VALUES (3, 'E-5S', 'Sniper Rifle', 10, 6, 80, 1);
# 3 records

#
# Dumping data for table 'tabletop'.'gamemodes'
#

INSERT INTO `tabletop`.`gamemodes` (`gamemode_id`, `name`, `description`, `mechanic`) VALUES (1, 'Skirmish', '', '');
INSERT INTO `tabletop`.`gamemodes` (`gamemode_id`, `name`, `description`, `mechanic`) VALUES (2, 'Extraction', '', '');
INSERT INTO `tabletop`.`gamemodes` (`gamemode_id`, `name`, `description`, `mechanic`) VALUES (3, 'Rush', '', '');
INSERT INTO `tabletop`.`gamemodes` (`gamemode_id`, `name`, `description`, `mechanic`) VALUES (3, 'Domination', '', '');
# 3 records

#
# Dumping data for table 'tabletop'.'unit_weapons'
#

INSERT INTO `tabletop`.`unit_weapons` (`unit_id`, `weapon_id`, `is_primary`) VALUES (1, 1, 0);
INSERT INTO `tabletop`.`unit_weapons` (`unit_id`, `weapon_id`, `is_primary`) VALUES (2, 2, 0);
INSERT INTO `tabletop`.`unit_weapons` (`unit_id`, `weapon_id`, `is_primary`) VALUES (3, 3, 0);
# 3 records

#
# Dumping data for table 'tabletop'.'permissions'
#

INSERT INTO `tabletop`.`permissions` (`permission_id`, `name`, `identifier`, `description`) VALUES (1, 'Edit Entities', 'EDIT_ENTITIES', 'Allows the user to create, edit and delete units and weapons.');
INSERT INTO `tabletop`.`permissions` (`permission_id`, `name`, `identifier`, `description`) VALUES (2, 'User management','EDIT_USERS','Allows the user to manage the users.');
# 2 records

#
# Optional: Adds an admin user
#
#------------------------------------------------------------------
#

#
# Dumping data for table 'users'
#

INSERT INTO `erp`.`users` (`user_id`, `username`, `display_name`, `image`, `password`, `salt`, `main_fraction_id`, `last_login`) VALUES (NULL, 'admin', 'Administrator', NULL, 'AQAAAAIAAYagAAAAECDOJwBpi0sqraVzpbiMs47xjH/gr8+/QcCClDnZ2oHzn1xA1jmU50ym+jODGjAHiQ==', '5Vjt5j4785', NULL, '2023-05-23 12:00:00');
# 1 records

SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;