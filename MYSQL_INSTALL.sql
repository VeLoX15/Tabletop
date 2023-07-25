DROP SCHEMA IF EXISTS `tabletop`;
CREATE SCHEMA IF NOT EXISTS `tabletop` DEFAULT CHARACTER SET latin2;
USE `tabletop`;

-- -----------------------------------------------------
-- Table `tabletop`.`fractions`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tabletop`.`fractions` 
(
    `fraction_id` INTEGER NOT NULL AUTO_INCREMENT,
    `name` VARCHAR(50) NOT NULL,
	`short_name` VARCHAR(50) NOT NULL,
    `description` TEXT NOT NULL,
	`image` MEDIUMBLOB NULL,

    PRIMARY KEY (`fraction_id`)
); 

-- -----------------------------------------------------
-- Table `tabletop`.`users`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tabletop`.`users` (
	`user_id` INTEGER NOT NULL AUTO_INCREMENT,
	`username` VARCHAR(50) NOT NULL,
	`display_name` VARCHAR(100) NOT NULL,
	`description` TEXT NOT NULL,
	`main_fraction_id` INT NULL, 
	`password` VARCHAR(255) NOT NULL,
	`salt` VARCHAR(255) NOT NULL,
    `last_login` DATETIME,
	`image` MEDIUMBLOB NULL,

	PRIMARY KEY(`user_id`),
	FOREIGN KEY (`main_fraction_id`) REFERENCES `tabletop`.`fractions`(`fraction_id`)
);

-- -----------------------------------------------------
-- Table `tabletop`.`permissons`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tabletop`.`permissions` (
	`permission_id` INTEGER NOT NULL AUTO_INCREMENT,
	`name` VARCHAR(50) NOT NULL,
	`identifier` VARCHAR(50) NOT NULL,
	`description` TEXT NOT NULL,

	PRIMARY KEY (`permission_id`)
);

-- -----------------------------------------------------
-- Table `tabletop`.`user_permissons`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tabletop`.`user_permissions` (
	`user_id` INTEGER NOT NULL,
	`permission_id` INTEGER NOT NULL,

	PRIMARY KEY(`user_id`, `permission_id`),
	FOREIGN KEY (`user_id`) REFERENCES `tabletop`.`users`(`user_id`) ON DELETE CASCADE ON UPDATE CASCADE,
	FOREIGN KEY (`permission_id`) REFERENCES `tabletop`.`permissions`(`permission_id`) ON DELETE CASCADE ON UPDATE CASCADE
);

-- -----------------------------------------------------
-- Table `tabletop`.`weapons`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tabletop`.`weapons` 
(
    `weapon_id` INTEGER NOT NULL AUTO_INCREMENT,
    `name` VARCHAR(50) NOT NULL,
    `description` TEXT NOT NULL,
    `attack` INTEGER NOT NULL,
    `quality` INTEGER NOT NULL,
    `range` INTEGER NOT NULL,
    `dices` INTEGER NOT NULL,
	`image` MEDIUMBLOB NULL,

    PRIMARY KEY (`weapon_id`)
);

-- -----------------------------------------------------
-- Table `tabletop`.`units`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tabletop`.`units` 
(
    `unit_id` INTEGER NOT NULL AUTO_INCREMENT,
    `name` VARCHAR(50) NOT NULL,
    `fraction_id` INTEGER NOT NULL,
    `description` TEXT NOT NULL,
	`mechanic` TEXT NOT NULL,
    `defense` INTEGER NOT NULL,
    `moving` INTEGER NOT NULL,
	`primary_weapon_id` INTEGER NULL,
	`secondary_weapon_id` INTEGER NULL,
	`image` MEDIUMBLOB NULL,

    PRIMARY KEY (`unit_id`),
	FOREIGN KEY (`fraction_id`) REFERENCES `tabletop`.`fractions`(`fraction_id`),
	FOREIGN KEY (`primary_weapon_id`) REFERENCES `tabletop`.`weapons`(`weapon_id`),
	FOREIGN KEY (`secondary_weapon_id`) REFERENCES `tabletop`.`weapons`(`weapon_id`)
);

-- -----------------------------------------------------
-- Table `tabletop`.`gamemodes`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tabletop`.`gamemodes` 
(
    `gamemode_id` INTEGER NOT NULL AUTO_INCREMENT,
	`name` VARCHAR(50) NOT NULL,
    `description` TEXT NOT NULL,
	`mechanic` TEXT NOT NULL,
	`image` MEDIUMBLOB NULL,

    PRIMARY KEY (`gamemode_id`)
);

-- -----------------------------------------------------
-- Table `tabletop`.`user_units`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tabletop`.`user_units` 
(
    `user_id` INTEGER NOT NULL,
	`unit_id` INTEGER NOT NULL,
	`quantity` INTEGER NOT NULL,

    PRIMARY KEY (`user_id`, `unit_id`),
	FOREIGN KEY (`user_id`) REFERENCES `tabletop`.`users`(`user_id`),
	FOREIGN KEY (`unit_id`) REFERENCES `tabletop`.`units`(`unit_id`)
);

-- -----------------------------------------------------
-- Table `tabletop`.`user_friends`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tabletop`.`user_friends` 
(
    `user_id` INTEGER NOT NULL,
	`friend_id` INTEGER NOT NULL,

    PRIMARY KEY (`user_id`, `friend_id`),
	FOREIGN KEY (`user_id`) REFERENCES `tabletop`.`users`(`user_id`),
	FOREIGN KEY (`friend_id`) REFERENCES `tabletop`.`users`(`user_id`)
);

-- -----------------------------------------------------
-- Table `tabletop`.`games`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tabletop`.`games` 
(
    `game_id` INTEGER NOT NULL AUTO_INCREMENT,
	`gamemode_id` INTEGER NOT NULL,
	`user_id` INTEGER NOT NULL,
	`name` VARCHAR(50) NOT NULL,
	`rounds` INTEGER NOT NULL,
	`force` INTEGER NOT NULL,
	`date` DATETIME NOT NULL,

    PRIMARY KEY (`game_id`),
	FOREIGN KEY (`gamemode_id`) REFERENCES `tabletop`.`gamemodes`(`gamemode_id`)
);

-- -----------------------------------------------------
-- Table `tabletop`.`players`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tabletop`.`players` 
(
	`player_id` INTEGER NOT NULL AUTO_INCREMENT,
    `user_id` INTEGER NOT NULL,
	`game_id` INTEGER NOT NULL,
	`fraction_id` INTEGER NOT NULL,
	`team` INTEGER NOT NULL,

    PRIMARY KEY (`player_id`),
	FOREIGN KEY (`user_id`) REFERENCES `tabletop`.`users`(`user_id`),
	FOREIGN KEY (`game_id`) REFERENCES `tabletop`.`games`(`game_id`),
	FOREIGN KEY (`fraction_id`) REFERENCES `tabletop`.`fractions`(`fraction_id`)
);

-- -----------------------------------------------------
-- Table `tabletop`.`game_players`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tabletop`.`game_players` 
(
	`game_id` INTEGER NOT NULL,
	`player_id` INTEGER NOT NULL,

    PRIMARY KEY (`game_id`, `player_id`),
	FOREIGN KEY (`game_id`) REFERENCES `tabletop`.`games`(`game_id`),
	FOREIGN KEY (`player_id`) REFERENCES `tabletop`.`players`(`player_id`)
);

-- -----------------------------------------------------
-- Table `tabletop`.`player_units`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tabletop`.`player_units` 
(
	`player_id` INTEGER NOT NULL,
	`unit_id` INTEGER NOT NULL,
	`quantity` INTEGER NOT NULL,

    PRIMARY KEY (`player_id`, `unit_id`),
	FOREIGN KEY (`player_id`) REFERENCES `tabletop`.`players`(`player_id`),
	FOREIGN KEY (`unit_id`) REFERENCES `tabletop`.`units`(`unit_id`)
);