DROP SCHEMA IF EXISTS `tabletop`;
CREATE SCHEMA IF NOT EXISTS `tabletop` DEFAULT CHARACTER SET latin2;
USE `tabletop`;

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
-- Table `tabletop`.`weapons`
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