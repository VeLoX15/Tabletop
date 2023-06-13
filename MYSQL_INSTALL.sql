DROP SCHEMA IF EXISTS `tabletop`;
CREATE SCHEMA IF NOT EXISTS `tabletop` DEFAULT CHARACTER SET latin2;
USE `tabletop`;

-- -----------------------------------------------------
-- Table `tabletop`.`users`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tabletop`.`users` (
	`user_id` INTEGER NOT NULL AUTO_INCREMENT,
	`username` VARCHAR(50) NOT NULL,
	`display_name` VARCHAR(100) NOT NULL,
	`image` IMAGE,
	`password` VARCHAR(255) NOT NULL,
	`salt` VARCHAR(255) NOT NULL,
	`main_fraction_id` VARCHAR(50),
    `last_login` DATETIME NOT NULL,

	PRIMARY KEY(`user_id`)
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
-- Table `tabletop`.`fractions`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tabletop`.`fractions` 
(
    `fraction_id` INTEGER NOT NULL AUTO_INCREMENT,
    `name` VARCHAR(50) NOT NULL,
    `description` VARCHAR(255) NOT NULL,

    PRIMARY KEY (`fraction_id`)
);

-- -----------------------------------------------------
-- Table `tabletop`.`user_fractions`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tabletop`.`fractions` 
(
    `user_id` INTEGER NOT NULL,
	`fraction_id` INTEGER NOT NULL,

    PRIMARY KEY (`user_id`),
	PRIMARY KEY (`fraction_id`)
); 

-- -----------------------------------------------------
-- Table `tabletop`.`user_units`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tabletop`.`user_units` 
(
    `user_id` INTEGER NOT NULL,
	`unit_id` INTEGER NOT NULL,
	`count` INTEGER NOT NULL,

    PRIMARY KEY (`user_id`),
	PRIMARY KEY (`unit_id`)
); 

-- -----------------------------------------------------
-- Table `tabletop`.`units`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tabletop`.`units` 
(
    `unit_id` INTEGER NOT NULL AUTO_INCREMENT,
    `name` VARCHAR(50) NOT NULL,
    `fraction_id` INTEGER NOT NULL,
    `description` VARCHAR(255) NOT NULL,
	`mechanic` VAR(255) NOT NULL,
    `defense` INTEGER NOT NULL,
    `moving` INTEGER NOT NULL,
	`primary_weapon_id` INTEGER NULL,
	`secondary_weapon_id` INTEGER NULL,

    PRIMARY KEY (`unit_id`),
	FOREIGN KEY (`fraction_id`) REFERENCES `tabletop`.`fractions`(`fraction_id`),
	FOREIGN KEY (`primary_weapon`) REFERENCES `tabletop`.`weapons`(`primary_weapon`),
	FOREIGN KEY (`secondary_weapon`) REFERENCES `tabletop`.`weapons`(`secondary_weapon`)
); 

-- -----------------------------------------------------
-- Table `tabletop`.`weapons`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tabletop`.`weapons` 
(
    `weapon_id` INTEGER NOT NULL AUTO_INCREMENT,
    `name` VARCHAR(50) NOT NULL,
    `description` VARCHAR(255) NOT NULL,
    `attack` INTEGER NOT NULL,
    `quality` INTEGER NOT NULL,
    `range` INTEGER NOT NULL,
    `dices` INTEGER NOT NULL,

    PRIMARY KEY (`weapon_id`)
); 