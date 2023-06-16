SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;

USE `tabletop`;

#
# Dumping data for table 'tabletop'.'fractions'
#

INSERT INTO `tabletop`.`fractions` (`fraction_id`, `name`, `short_name`, `description`, `image`) VALUES (1,  'Galactic Army of Republic', 'GAR', 'The Grand Army of the Republic is the main military force of the Galactic Republic. It was formed during the Clone Wars to combat the Separatists. The army consists primarily of clone troopers, who were created using the genetic material of Jango Fett as a basis. It encompasses a variety of troops, including infantry, artillery, and special forces, as well as an extensive fleet of starships. The Grand Army of the Republic is renowned for its effective organization, disciplined training, and tactical prowess.', NULL);
INSERT INTO `tabletop`.`fractions` (`fraction_id`, `name`, `short_name`, `description`, `image`) VALUES (2,  'Confoderacy of Independent Systems', 'CIS', 'The Confederation of Independent Systems (CIS) is a military alliance in the Star Wars universe, consisting of renegade planets and organizations that have distanced themselves from the rule of the Galactic Republic. The CIS possesses an impressive military force, primarily composed of droid armies. These automated combat droids are produced in large numbers and pose a dangerous threat due to their numerical superiority.', NULL);
INSERT INTO `tabletop`.`fractions` (`fraction_id`, `name`, `short_name`, `description`, `image`) VALUES (3,  'Galactic Empire', 'EMP', 'The Galactic Empire is a military power in the galaxy that emerged after the fall of the Galactic Republic. It possesses a massive military force based on the suppression and control of planets and systems. The Empire employs a variety of troop types. It is renowned for its iron discipline, rigorous training, and ruthless efficiency. It seeks galaxy-wide dominance and pursues a policy of fear and subjugation.', NULL);
INSERT INTO `tabletop`.`fractions` (`fraction_id`, `name`, `short_name`, `description`, `image`) VALUES (4,  'Rebellion', 'REB', 'The Rebellion is a military resistance movement that fights against the rule of the Galactic Empire. It consists of various factions and planets that rise up against oppression. The Rebellion employs asymmetric warfare and guerrilla tactics to inflict damage upon the Empire. Its forces include both regular troops and partisan units. The Rebellion is known for its determination, belief in freedom, and ability to mobilize resources to challenge the Empire. It also utilizes stolen or improvised starships and weapons to carry out its operations.', NULL);
# 4 records

#
# Dumping data for table 'tabletop'.'weapons'
#

INSERT INTO `tabletop`.`weapons` (`weapon_id`, `name`, `description`, `attack`, `quality`, `range`, `dices`, `image`) VALUES (1, 'DC-15A blaster rifle', '', 5, 5, 60, 1, NULL);
INSERT INTO `tabletop`.`weapons` (`weapon_id`, `name`, `description`, `attack`, `quality`, `range`, `dices`, `image`) VALUES (2, 'DC-15S blaster carbine', '', 5, 6, 45, 1, NULL);
INSERT INTO `tabletop`.`weapons` (`weapon_id`, `name`, `description`, `attack`, `quality`, `range`, `dices`, `image`) VALUES (3, 'DC-15x sniper rifle', '', 9, 7, 80, 1, NULL);
INSERT INTO `tabletop`.`weapons` (`weapon_id`, `name`, `description`, `attack`, `quality`, `range`, `dices`, `image`) VALUES (4, 'DC-17 hand blaster', '', 4, 4, 30, 1, NULL);
INSERT INTO `tabletop`.`weapons` (`weapon_id`, `name`, `description`, `attack`, `quality`, `range`, `dices`, `image`) VALUES (5, 'Z-6 rotary blaster cannon', '', 7, 4, 30, 6, NULL);
INSERT INTO `tabletop`.`weapons` (`weapon_id`, `name`, `description`, `attack`, `quality`, `range`, `dices`, `image`) VALUES (6, 'E-5 blaster rifle', '', 6, 5, 45, 1, NULL);
INSERT INTO `tabletop`.`weapons` (`weapon_id`, `name`, `description`, `attack`, `quality`, `range`, `dices`, `image`) VALUES (7, 'E-5s sniper rifle', '', 6, 5, 45, 1, NULL);
INSERT INTO `tabletop`.`weapons` (`weapon_id`, `name`, `description`, `attack`, `quality`, `range`, `dices`, `image`) VALUES (8, 'RG-4D blaster pistol', '', 10, 7, 80, 1, NULL);
INSERT INTO `tabletop`.`weapons` (`weapon_id`, `name`, `description`, `attack`, `quality`, `range`, `dices`, `image`) VALUES (9, 'Wrist blaster', '', 5, 4, 50, 1, NULL);
INSERT INTO `tabletop`.`weapons` (`weapon_id`, `name`, `description`, `attack`, `quality`, `range`, `dices`, `image`) VALUES (10, 'Droideka', '', 7, 4, 30, 4, NULL);
# 10 records

#
# Dumping data for table 'tabletop'.'units'
#

INSERT INTO `tabletop`.`units` (`unit_id`, `name`, `fraction_id`, `description`, `mechanic`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (1, '41. Ranger Platoon', 1, '', '', 6, 17, 1, 1, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `name`, `fraction_id`, `description`, `mechanic`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (2, '41. Elitekorps', 1, '', '', 5, 17, 1, 1, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `name`, `fraction_id`, `description`, `mechanic`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (3, '212. Angriffsbataillon', 1, '', '', 6, 17, 1, 1, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `name`, `fraction_id`, `description`, `mechanic`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (4, '2. Luftlandekompanie', 1, '', '', 6, 17, 1, 1, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `name`, `fraction_id`, `description`, `mechanic`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (5, '501. Bataillon', 1, '', '', 6, 17, 1, 1, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `name`, `fraction_id`, `description`, `mechanic`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (6, '501. Jetpack', 1, '', '', 6, 17, 1, 1, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `name`, `fraction_id`, `description`, `mechanic`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (7, '104. Bataillon', 1, '', '', 6, 17, 1, 1, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `name`, `fraction_id`, `description`, `mechanic`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (8, '501. Heavy Gunner', 1, '', '', 7, 17, 1, 1, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `name`, `fraction_id`, `description`, `mechanic`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (9, '501. Specialist', 1, '', '', 6, 17, 1, 1, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `name`, `fraction_id`, `description`, `mechanic`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (10, 'B1-series battle droid', 2, '', '', 2, 12, 1, 1, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `name`, `fraction_id`, `description`, `mechanic`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (11, 'B2-series super battle droid', 2, '', '', 4, 12, 1, 1, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `name`, `fraction_id`, `description`, `mechanic`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (12, 'BX-series commando droid', 2, '', '', 5, 16, 1, 1, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `name`, `fraction_id`, `description`, `mechanic`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (13, 'B2-HA series super battle droid', 2, '', '', 4, 12, 1, 1, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `name`, `fraction_id`, `description`, `mechanic`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (14, 'B1-series rocket battle droid', 2, '', '', 2, 22, 1, 1, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `name`, `fraction_id`, `description`, `mechanic`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (15, 'Droideka', 2, '', '', 10, 22, 1, 1, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `name`, `fraction_id`, `description`, `mechanic`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (16, 'B1-series sniper droid', 2, '', '', 6, 17, 1, 1, NULL);
# 16 records

#
# Dumping data for table 'tabletop'.'gamemodes'
#

INSERT INTO `tabletop`.`gamemodes` (`gamemode_id`, `name`, `description`, `mechanic`, `image`) VALUES (1, 'Skirmish', 'The game mode "Skirmish" is the most common of all and focuses on the battle between armies. In this mode, 2-4 armies compete against each other, aiming to eliminate as many units from the opponents field as possible. The game mode is round-limited. The winner is the player who has the most power points remaining on the field at the end.', '', NULL);
INSERT INTO `tabletop`.`gamemodes` (`gamemode_id`, `name`, `description`, `mechanic`, `image`) VALUES (2, 'Extraction', 'The game mode "Escape" revolves around both players escorting a VIP and trying to get them out of the area. The conflict arises because there is only one extraction point. It is also possible to eliminate the enemys VIP from the game. Once the enemy VIP has been eliminated or your own VIP has escaped through the evacuation zone, the player wins.', '', NULL);
INSERT INTO `tabletop`.`gamemodes` (`gamemode_id`, `name`, `description`, `mechanic`, `image`) VALUES (3, 'Assault', 'In the game mode "Assault," the objective is to occupy or defend three defense sectors in succession. The attacker begins with their entire army on one side, while the defender distributes their units across the defense sectors. The attacker has six rounds to capture each sector. The round limit resets after a successful capture. The attacker wins after capturing the third sector, while the defender wins if the round limit expires at any of the sectors.', '', NULL);
INSERT INTO `tabletop`.`gamemodes` (`gamemode_id`, `name`, `description`, `mechanic`, `image`) VALUES (4, 'Domination', 'In the game mode "Domination," there are three zones that each side must control.', '', NULL);
# 4 records

#
# Dumping data for table 'tabletop'.'permissions'
#

INSERT INTO `tabletop`.`permissions` (`permission_id`, `name`, `identifier`, `description`) VALUES (1, 'Add Users', 'ADD_USERS', '');
INSERT INTO `tabletop`.`permissions` (`permission_id`, `name`, `identifier`, `description`) VALUES (2, 'View Users', 'VIEW_USERS', '');
INSERT INTO `tabletop`.`permissions` (`permission_id`, `name`, `identifier`, `description`) VALUES (3, 'Edit Users', 'EDIT_USERS', '');
INSERT INTO `tabletop`.`permissions` (`permission_id`, `name`, `identifier`, `description`) VALUES (4, 'Delete Users', 'DELETE_USERS', '');
# 4 records

#
# Optional: Adds an admin user
#
#------------------------------------------------------------------
#

#
# Dumping data for table 'users'
#

INSERT INTO `tabletop`.`users` (`user_id`, `username`, `display_name`, `password`, `salt`, `last_login`, `image`) VALUES (1, 'admin', 'Administrator', 'AQAAAAIAAYagAAAAECDOJwBpi0sqraVzpbiMs47xjH/gr8+/QcCClDnZ2oHzn1xA1jmU50ym+jODGjAHiQ==', '5Vjt5j4785', '2023-05-23 12:00:00', NULL);
# 1 records

#
# Dumping data for table 'tabletop'.'user_permissions'
#

INSERT INTO `tabletop`.`user_permissions` (`user_id`, `permission_id`) VALUES (1, 1);
INSERT INTO `tabletop`.`user_permissions` (`user_id`, `permission_id`) VALUES (1, 2);
INSERT INTO `tabletop`.`user_permissions` (`user_id`, `permission_id`) VALUES (1, 3);
INSERT INTO `tabletop`.`user_permissions` (`user_id`, `permission_id`) VALUES (1, 4);
# 4 records

SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;