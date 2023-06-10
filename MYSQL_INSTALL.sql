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
	`active_directory_guid` VARCHAR(36),
	`email` VARCHAR(255) NOT NULL,
	`password` VARCHAR(255) NOT NULL,
	`salt` VARCHAR(255) NOT NULL,
	`origin` VARCHAR(5) NOT NULL,
    `last_login` DATETIME NOT NULL,

	PRIMARY KEY(`user_id`)
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
-- Table `tabletop`.`units`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tabletop`.`units` 
(
    `unit_id` INTEGER NOT NULL AUTO_INCREMENT,
    `name` VARCHAR(50) NOT NULL,
    `fraction` VARCHAR(50) NOT NULL,
    `description` VARCHAR(255) NOT NULL,
    `defense` INTEGER NOT NULL,
    `moving` INTEGER NOT NULL,

    PRIMARY KEY (`unit_id`)
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

-- -----------------------------------------------------
-- Table `tabletop`.`unit_weapons`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tabletop`.`unit_weapons`
(
	`unit_id` INTEGER NOT NULL,
	`weapon_id` INTEGER NOT NULL,
    `is_primary` TINYINT NOT NULL DEFAULT 0,

	PRIMARY KEY(`unit_id`, `weapon_id`),
	FOREIGN KEY (`unit_id`) REFERENCES `tabletop`.`units`(`unit_id`),
	FOREIGN KEY (`weapon_id`) REFERENCES `tabletop`.`weapons`(`weapon_id`)
);