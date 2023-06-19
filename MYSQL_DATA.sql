SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;

USE `tabletop`;

#
# Dumping data for table 'tabletop'.'fractions'
#

INSERT INTO `tabletop`.`fractions` (`fraction_id`, `name`, `short_name`, `description`, `image`) VALUES (1,  'Galactic Army of Republic', 'GAR', 'The Grand Army of the Republic is the main military force of the Galactic Republic. It was formed during the Clone Wars to combat the Separatists. The army consists primarily of clone troopers, who were created using the genetic material of Jango Fett as a basis. It encompasses a variety of troops, including infantry, artillery, and special forces, as well as an extensive fleet of starships. The Grand Army of the Republic is renowned for its effective organization, disciplined training, and tactical prowess.', NULL);
INSERT INTO `tabletop`.`fractions` (`fraction_id`, `name`, `short_name`, `description`, `image`) VALUES (2,  'Confoderacy of Independent Systems', 'CIS', 'The Confederation of Independent Systems (CIS) is a military alliance in the galaxy, consisting of renegade planets and organizations that have distanced themselves from the rule of the Galactic Republic. The CIS possesses an impressive military force, primarily composed of droid armies. These automated combat droids are produced in large numbers and pose a dangerous threat due to their numerical superiority.', NULL);
INSERT INTO `tabletop`.`fractions` (`fraction_id`, `name`, `short_name`, `description`, `image`) VALUES (3,  'Galactic Empire', 'EMP', 'The Galactic Empire is a military power in the galaxy that emerged after the fall of the Galactic Republic. It possesses a massive military force based on the suppression and control of planets and systems. The Empire employs a variety of troop types. It is renowned for its iron discipline, rigorous training, and ruthless efficiency. It seeks galaxy-wide dominance and pursues a policy of fear and subjugation.', NULL);
INSERT INTO `tabletop`.`fractions` (`fraction_id`, `name`, `short_name`, `description`, `image`) VALUES (4,  'Rebellion', 'REB', 'The Rebellion is a military resistance movement that fights against the rule of the Galactic Empire. It consists of various factions and planets that rise up against oppression. The Rebellion employs asymmetric warfare and guerrilla tactics to inflict damage upon the Empire. Its forces include both regular troops and partisan units. The Rebellion is known for its determination, belief in freedom, and ability to mobilize resources to challenge the Empire. It also utilizes stolen or improvised starships and weapons to carry out its operations.', NULL);
# 4 records

#
# Dumping data for table 'tabletop'.'weapons'
#

INSERT INTO `tabletop`.`weapons` (`weapon_id`, `name`, `description`, `attack`, `quality`, `range`, `dices`, `image`) VALUES (1, 'DC-15A Blaster Rifle', '', 5, 5, 60, 1, NULL);
INSERT INTO `tabletop`.`weapons` (`weapon_id`, `name`, `description`, `attack`, `quality`, `range`, `dices`, `image`) VALUES (2, 'DC-15S Blaster Carbine', '', 5, 6, 45, 1, NULL);
INSERT INTO `tabletop`.`weapons` (`weapon_id`, `name`, `description`, `attack`, `quality`, `range`, `dices`, `image`) VALUES (3, 'DC-15x Sniper Rifle', '', 9, 7, 80, 1, NULL);
INSERT INTO `tabletop`.`weapons` (`weapon_id`, `name`, `description`, `attack`, `quality`, `range`, `dices`, `image`) VALUES (4, 'DC-17 Hand Blaster', '', 4, 4, 30, 1, NULL);
INSERT INTO `tabletop`.`weapons` (`weapon_id`, `name`, `description`, `attack`, `quality`, `range`, `dices`, `image`) VALUES (5, 'Z-6 Rotary Blaster Cannon', '', 7, 4, 30, 6, NULL);
INSERT INTO `tabletop`.`weapons` (`weapon_id`, `name`, `description`, `attack`, `quality`, `range`, `dices`, `image`) VALUES (6, 'E-5 Blaster Rifle', '', 6, 5, 45, 1, NULL);
INSERT INTO `tabletop`.`weapons` (`weapon_id`, `name`, `description`, `attack`, `quality`, `range`, `dices`, `image`) VALUES (7, 'E-5s Sniper Rifle', '', 10, 7, 80, 1, NULL);
INSERT INTO `tabletop`.`weapons` (`weapon_id`, `name`, `description`, `attack`, `quality`, `range`, `dices`, `image`) VALUES (8, 'RG-4D Blaster Pistol', '', 6, 5, 45, 1, NULL);
INSERT INTO `tabletop`.`weapons` (`weapon_id`, `name`, `description`, `attack`, `quality`, `range`, `dices`, `image`) VALUES (9, 'Wrist Blaster', '', 5, 4, 50, 1, NULL);
INSERT INTO `tabletop`.`weapons` (`weapon_id`, `name`, `description`, `attack`, `quality`, `range`, `dices`, `image`) VALUES (10, 'Droideka', '', 7, 4, 30, 4, NULL);
# 10 records

#
# Dumping data for table 'tabletop'.'units'
#

INSERT INTO `tabletop`.`units` (`unit_id`, `name`, `fraction_id`, `description`, `mechanic`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (1, '41st Ranger Platoon', 1, 'The 41st Ranger Platoon is a clone trooper infantry platoon and strike team within the 41st Elite Corps. The platoon participated in sabotage missions on Separatist supply lines and installations.', '', 6, 17, 2, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `name`, `fraction_id`, `description`, `mechanic`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (2, '41st Elite Corps', 1, 'The 41st Elite Corps is a military corps of the Grand Army of the Republic.', '', 5, 17, 1, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `name`, `fraction_id`, `description`, `mechanic`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (3, '212th Attack Battalion', 1, '', '', 6, 17, 1, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `name`, `fraction_id`, `description`, `mechanic`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (4, '2nd Airborne Company', 1, 'The 2nd Airborne Company was an aerial infantry company of the 212th Attack Battalion. The company consisted of clone paratroopers and participated in the Clone Wars.', '', 6, 17, 1, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `name`, `fraction_id`, `description`, `mechanic`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (5, '501st Legion', 1, '', '', 6, 17, 2, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `name`, `fraction_id`, `description`, `mechanic`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (6, '501st Jetpack Company', 1, '', '', 6, 17, 4, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `name`, `fraction_id`, `description`, `mechanic`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (7, '104th Battalion', 1, 'The 104th Battalion, also known as the "Wolfpack" Battalion, was a clone ', '', 6, 17, 2, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `name`, `fraction_id`, `description`, `mechanic`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (8, '501st Heavy Gunner', 1, '', '', 7, 17, 5, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `name`, `fraction_id`, `description`, `mechanic`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (9, '501st Specialist', 1, '', '', 6, 17, 3, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `name`, `fraction_id`, `description`, `mechanic`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (10, 'B1-series Battle Droid', 2, '', '', 2, 12, 6, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `name`, `fraction_id`, `description`, `mechanic`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (11, 'B2-series Super Battle Droid', 2, '', '', 4, 12, 9, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `name`, `fraction_id`, `description`, `mechanic`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (12, 'BX-series Commando Droid', 2, '', '', 5, 16, 6, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `name`, `fraction_id`, `description`, `mechanic`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (13, 'B2-HA series Super Battle Droid', 2, '', '', 4, 12, 9, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `name`, `fraction_id`, `description`, `mechanic`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (14, 'B1-series Rocket Battle Droid', 2, '', '', 2, 22, 6, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `name`, `fraction_id`, `description`, `mechanic`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (15, 'Droideka', 2, '', '', 10, 22, 10, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `name`, `fraction_id`, `description`, `mechanic`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (16, 'B1-series Sniper Droid', 2, '', '', 6, 17, 7, NULL, NULL);
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
INSERT INTO `tabletop`.`permissions` (`permission_id`, `name`, `identifier`, `description`) VALUES (2, 'Add Units', 'ADD_UNITS', '');
INSERT INTO `tabletop`.`permissions` (`permission_id`, `name`, `identifier`, `description`) VALUES (3, 'Add Weapons', 'ADD_WEAPONS', '');
INSERT INTO `tabletop`.`permissions` (`permission_id`, `name`, `identifier`, `description`) VALUES (4, 'Add Fractions', 'ADD_FRACTIONS', '');
INSERT INTO `tabletop`.`permissions` (`permission_id`, `name`, `identifier`, `description`) VALUES (5, 'Add Gamemodes', 'ADD_GAMEMODES', '');
INSERT INTO `tabletop`.`permissions` (`permission_id`, `name`, `identifier`, `description`) VALUES (6, 'View Users', 'VIEW_USERS', '');
INSERT INTO `tabletop`.`permissions` (`permission_id`, `name`, `identifier`, `description`) VALUES (7, 'View Units', 'VIEW_UNITS', '');
INSERT INTO `tabletop`.`permissions` (`permission_id`, `name`, `identifier`, `description`) VALUES (8, 'View Weapons', 'VIEW_WEAPONS', '');
INSERT INTO `tabletop`.`permissions` (`permission_id`, `name`, `identifier`, `description`) VALUES (9, 'View Fractions', 'VIEW_FRACTIONS', '');
INSERT INTO `tabletop`.`permissions` (`permission_id`, `name`, `identifier`, `description`) VALUES (10, 'View Gamemodes', 'VIEW_GAMEMODES', '');
INSERT INTO `tabletop`.`permissions` (`permission_id`, `name`, `identifier`, `description`) VALUES (11, 'Edit Users', 'EDIT_USERS', '');
INSERT INTO `tabletop`.`permissions` (`permission_id`, `name`, `identifier`, `description`) VALUES (12, 'Edit Units', 'EDIT_UNITS', '');
INSERT INTO `tabletop`.`permissions` (`permission_id`, `name`, `identifier`, `description`) VALUES (13, 'Edit Weapons', 'EDIT_WEAPONS', '');
INSERT INTO `tabletop`.`permissions` (`permission_id`, `name`, `identifier`, `description`) VALUES (14, 'Edit Fractions', 'EDIT_FRACTIONS', '');
INSERT INTO `tabletop`.`permissions` (`permission_id`, `name`, `identifier`, `description`) VALUES (15, 'Edit Gamemodes', 'EDIT_GAMEMODES', '');
INSERT INTO `tabletop`.`permissions` (`permission_id`, `name`, `identifier`, `description`) VALUES (16, 'Delete Users', 'DELETE_USERS', '');
INSERT INTO `tabletop`.`permissions` (`permission_id`, `name`, `identifier`, `description`) VALUES (17, 'Delete Units', 'DELETE_UNITS', '');
INSERT INTO `tabletop`.`permissions` (`permission_id`, `name`, `identifier`, `description`) VALUES (18, 'Delete Weapons', 'DELETE_WEAPONS', '');
INSERT INTO `tabletop`.`permissions` (`permission_id`, `name`, `identifier`, `description`) VALUES (19, 'Delete Fractions', 'DELETE_FRACTIONS', '');
INSERT INTO `tabletop`.`permissions` (`permission_id`, `name`, `identifier`, `description`) VALUES (20, 'Delete Gamemodes', 'DELETE_GAMEMODES', '');
# 20 records

#
# Optional: Adds an admin user
#
#------------------------------------------------------------------
#

#
# Dumping data for table 'users'
#

INSERT INTO `tabletop`.`users` (`user_id`, `username`, `display_name`, `description`, `password`, `salt`, `last_login`, `image`) VALUES (1, 'admin', 'Administrator', '', 'AQAAAAIAAYagAAAAECDOJwBpi0sqraVzpbiMs47xjH/gr8+/QcCClDnZ2oHzn1xA1jmU50ym+jODGjAHiQ==', '5Vjt5j4785', '2023-05-23 12:00:00', NULL);
# 1 records

#
# Dumping data for table 'tabletop'.'user_permissions'
#

INSERT INTO `tabletop`.`user_permissions` (`user_id`, `permission_id`) VALUES (1, 1);
INSERT INTO `tabletop`.`user_permissions` (`user_id`, `permission_id`) VALUES (1, 2);
INSERT INTO `tabletop`.`user_permissions` (`user_id`, `permission_id`) VALUES (1, 3);
INSERT INTO `tabletop`.`user_permissions` (`user_id`, `permission_id`) VALUES (1, 4);
INSERT INTO `tabletop`.`user_permissions` (`user_id`, `permission_id`) VALUES (1, 5);
INSERT INTO `tabletop`.`user_permissions` (`user_id`, `permission_id`) VALUES (1, 6);
INSERT INTO `tabletop`.`user_permissions` (`user_id`, `permission_id`) VALUES (1, 7);
INSERT INTO `tabletop`.`user_permissions` (`user_id`, `permission_id`) VALUES (1, 8);
INSERT INTO `tabletop`.`user_permissions` (`user_id`, `permission_id`) VALUES (1, 9);
INSERT INTO `tabletop`.`user_permissions` (`user_id`, `permission_id`) VALUES (1, 10);
INSERT INTO `tabletop`.`user_permissions` (`user_id`, `permission_id`) VALUES (1, 11);
INSERT INTO `tabletop`.`user_permissions` (`user_id`, `permission_id`) VALUES (1, 12);
INSERT INTO `tabletop`.`user_permissions` (`user_id`, `permission_id`) VALUES (1, 13);
INSERT INTO `tabletop`.`user_permissions` (`user_id`, `permission_id`) VALUES (1, 14);
INSERT INTO `tabletop`.`user_permissions` (`user_id`, `permission_id`) VALUES (1, 15);
INSERT INTO `tabletop`.`user_permissions` (`user_id`, `permission_id`) VALUES (1, 16);
INSERT INTO `tabletop`.`user_permissions` (`user_id`, `permission_id`) VALUES (1, 17);
INSERT INTO `tabletop`.`user_permissions` (`user_id`, `permission_id`) VALUES (1, 18);
INSERT INTO `tabletop`.`user_permissions` (`user_id`, `permission_id`) VALUES (1, 19);
INSERT INTO `tabletop`.`user_permissions` (`user_id`, `permission_id`) VALUES (1, 20);
# 20 records

SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;