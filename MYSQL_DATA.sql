SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;

USE `tabletop`;

#
# Dumping data for table 'tabletop'.'fractions'
#

INSERT INTO `tabletop`.`fractions` (`fraction_id`, `image`) VALUES (1, NULL);
INSERT INTO `tabletop`.`fractions` (`fraction_id`, `image`) VALUES (2, NULL);
INSERT INTO `tabletop`.`fractions` (`fraction_id`, `image`) VALUES (3, NULL);
INSERT INTO `tabletop`.`fractions` (`fraction_id`, `image`) VALUES (4, NULL);
# 4 records

#
# Dumping data for table 'tabletop'.'fraction_description'
#

INSERT INTO `tabletop`.`fraction_description` (`fraction_id`, `code`, `name`, `short_name`, `description`) VALUES (1, 'en', 'Grand Army of Republic', 'GAR', 'The Grand Army of the Republic is the main military force of the Galactic Republic. It was formed during the Clone Wars to combat the Separatists. The army consists primarily of clone troopers, who were created using the genetic material of Jango Fett as a basis. It encompasses a variety of troops, including infantry, artillery, and special forces, as well as an extensive fleet of starships. The Grand Army of the Republic is renowned for its effective organization, disciplined training, and tactical prowess.');
INSERT INTO `tabletop`.`fraction_description` (`fraction_id`, `code`, `name`, `short_name`, `description`) VALUES (1, 'de', 'Große Armee der Republik', 'GAR', '');
INSERT INTO `tabletop`.`fraction_description` (`fraction_id`, `code`, `name`, `short_name`, `description`) VALUES (2, 'en', 'Confederacy of Independent Systems', 'CIS', 'The Confederation of Independent Systems (CIS) is a military alliance in the galaxy, consisting of renegade planets and organizations that have distanced themselves from the rule of the Galactic Republic. The CIS possesses an impressive military force, primarily composed of droid armies. These automated combat droids are produced in large numbers and pose a dangerous threat due to their numerical superiority.');
INSERT INTO `tabletop`.`fraction_description` (`fraction_id`, `code`, `name`, `short_name`, `description`) VALUES (2, 'de', 'Konföderation der unabhängigen Systeme', 'KUS', '');
INSERT INTO `tabletop`.`fraction_description` (`fraction_id`, `code`, `name`, `short_name`, `description`) VALUES (3, 'en', 'Galactic Empire', 'EMP', 'The Galactic Empire is a military power in the galaxy that emerged after the fall of the Galactic Republic. It possesses a massive military force based on the suppression and control of planets and systems. The Empire employs a variety of troop types. It is renowned for its iron discipline, rigorous training, and ruthless efficiency. It seeks galaxy-wide dominance and pursues a policy of fear and subjugation.');
INSERT INTO `tabletop`.`fraction_description` (`fraction_id`, `code`, `name`, `short_name`, `description`) VALUES (3, 'de', 'Galaktisches Imperium', 'IMP', '');
INSERT INTO `tabletop`.`fraction_description` (`fraction_id`, `code`, `name`, `short_name`, `description`) VALUES (4, 'en', 'Rebellion', 'REB', 'The Rebellion is a military resistance movement that fights against the rule of the Galactic Empire. It consists of various factions and planets that rise up against oppression. The Rebellion employs asymmetric warfare and guerrilla tactics to inflict damage upon the Empire. Its forces include both regular troops and partisan units. The Rebellion is known for its determination, belief in freedom, and ability to mobilize resources to challenge the Empire. It also utilizes stolen or improvised starships and weapons to carry out its operations.');
INSERT INTO `tabletop`.`fraction_description` (`fraction_id`, `code`, `name`, `short_name`, `description`) VALUES (4, 'de', 'Rebellion', 'REB', '');
# 8 records

#
# Dumping data for table 'tabletop'.'weapons'
#

INSERT INTO `tabletop`.`weapons` (`weapon_id`, `attack`, `quality`, `range`, `dices`) VALUES (1, 5, 5, 60, 1);
INSERT INTO `tabletop`.`weapons` (`weapon_id`, `attack`, `quality`, `range`, `dices`) VALUES (2, 5, 6, 45, 1);
INSERT INTO `tabletop`.`weapons` (`weapon_id`, `attack`, `quality`, `range`, `dices`) VALUES (3, 9, 7, 80, 1);
INSERT INTO `tabletop`.`weapons` (`weapon_id`, `attack`, `quality`, `range`, `dices`) VALUES (4, 4, 4, 30, 1);
INSERT INTO `tabletop`.`weapons` (`weapon_id`, `attack`, `quality`, `range`, `dices`) VALUES (5, 7, 4, 30, 6);
INSERT INTO `tabletop`.`weapons` (`weapon_id`, `attack`, `quality`, `range`, `dices`) VALUES (6, 6, 5, 40, 1);
INSERT INTO `tabletop`.`weapons` (`weapon_id`, `attack`, `quality`, `range`, `dices`) VALUES (7, 10, 7, 80, 1);
INSERT INTO `tabletop`.`weapons` (`weapon_id`, `attack`, `quality`, `range`, `dices`) VALUES (8, 6, 5, 45, 1);
INSERT INTO `tabletop`.`weapons` (`weapon_id`, `attack`, `quality`, `range`, `dices`) VALUES (9, 7, 4, 50, 2);
INSERT INTO `tabletop`.`weapons` (`weapon_id`, `attack`, `quality`, `range`, `dices`) VALUES (10, 6, 4, 45, 1);
INSERT INTO `tabletop`.`weapons` (`weapon_id`, `attack`, `quality`, `range`, `dices`) VALUES (11, 1, 1, 1, 1);
INSERT INTO `tabletop`.`weapons` (`weapon_id`, `attack`, `quality`, `range`, `dices`) VALUES (12, 1, 1, 1, 1);
INSERT INTO `tabletop`.`weapons` (`weapon_id`, `attack`, `quality`, `range`, `dices`) VALUES (13, 1, 1, 1, 1);
INSERT INTO `tabletop`.`weapons` (`weapon_id`, `attack`, `quality`, `range`, `dices`) VALUES (14, 1, 1, 1, 1);
# 14 records

#
# Dumping data for table 'tabletop'.'weapon_description'
#

INSERT INTO `tabletop`.`weapon_description` (`weapon_id`, `code`, `name`, `description`) VALUES (1, 'en', 'DC-15A Blaster Rifle', '');
INSERT INTO `tabletop`.`weapon_description` (`weapon_id`, `code`, `name`, `description`) VALUES (1, 'de', 'DC-15A Blaster Rifle', '');
INSERT INTO `tabletop`.`weapon_description` (`weapon_id`, `code`, `name`, `description`) VALUES (2, 'en', 'DC-15S Blaster Carbine', '');
INSERT INTO `tabletop`.`weapon_description` (`weapon_id`, `code`, `name`, `description`) VALUES (2, 'de', 'DC-15S Blaster Carbine', '');
INSERT INTO `tabletop`.`weapon_description` (`weapon_id`, `code`, `name`, `description`) VALUES (3, 'en', 'DC-15x Sniper Rifle', '');
INSERT INTO `tabletop`.`weapon_description` (`weapon_id`, `code`, `name`, `description`) VALUES (3, 'de', 'DC-15x Sniper Rifle', '');
INSERT INTO `tabletop`.`weapon_description` (`weapon_id`, `code`, `name`, `description`) VALUES (4, 'en', 'DC-17 Hand Blaster', '');
INSERT INTO `tabletop`.`weapon_description` (`weapon_id`, `code`, `name`, `description`) VALUES (4, 'de', 'DC-17 Hand Blaster', '');
INSERT INTO `tabletop`.`weapon_description` (`weapon_id`, `code`, `name`, `description`) VALUES (5, 'en', 'Z-6 Rotary Blaster Cannon', '');
INSERT INTO `tabletop`.`weapon_description` (`weapon_id`, `code`, `name`, `description`) VALUES (5, 'de', 'Z-6 Rotary Blaster Cannon', '');
INSERT INTO `tabletop`.`weapon_description` (`weapon_id`, `code`, `name`, `description`) VALUES (6, 'en', 'E-5 Blaster Rifle', '');
INSERT INTO `tabletop`.`weapon_description` (`weapon_id`, `code`, `name`, `description`) VALUES (6, 'de', 'E-5 Blaster Rifle', '');
INSERT INTO `tabletop`.`weapon_description` (`weapon_id`, `code`, `name`, `description`) VALUES (7, 'en', 'E-5s Sniper Rifle', '');
INSERT INTO `tabletop`.`weapon_description` (`weapon_id`, `code`, `name`, `description`) VALUES (7, 'de', 'E-5s Sniper Rifle', '');
INSERT INTO `tabletop`.`weapon_description` (`weapon_id`, `code`, `name`, `description`) VALUES (8, 'en', 'RG-4D Blaster Pistol', '');
INSERT INTO `tabletop`.`weapon_description` (`weapon_id`, `code`, `name`, `description`) VALUES (8, 'de', 'RG-4D Blaster Pistol', '');
INSERT INTO `tabletop`.`weapon_description` (`weapon_id`, `code`, `name`, `description`) VALUES (9, 'en', 'Wrist Blaster', '');
INSERT INTO `tabletop`.`weapon_description` (`weapon_id`, `code`, `name`, `description`) VALUES (9, 'de', 'Wrist Blaster', '');
INSERT INTO `tabletop`.`weapon_description` (`weapon_id`, `code`, `name`, `description`) VALUES (10, 'en', 'E-11 Blaster Rifle', '');
INSERT INTO `tabletop`.`weapon_description` (`weapon_id`, `code`, `name`, `description`) VALUES (10, 'de', 'E-11 Blaster Rifle', '');
INSERT INTO `tabletop`.`weapon_description` (`weapon_id`, `code`, `name`, `description`) VALUES (11, 'en', 'DLT-19 Heavy Blaster Rifle', '');
INSERT INTO `tabletop`.`weapon_description` (`weapon_id`, `code`, `name`, `description`) VALUES (11, 'de', 'DLT-19 Heavy Blaster Rifle', '');
INSERT INTO `tabletop`.`weapon_description` (`weapon_id`, `code`, `name`, `description`) VALUES (12, 'en', 'E-11s Sniper Rifle', '');
INSERT INTO `tabletop`.`weapon_description` (`weapon_id`, `code`, `name`, `description`) VALUES (12, 'de', 'E-11s Sniper Rifle', '');
INSERT INTO `tabletop`.`weapon_description` (`weapon_id`, `code`, `name`, `description`) VALUES (13, 'en', 'E-22 Blaster Rifle', '');
INSERT INTO `tabletop`.`weapon_description` (`weapon_id`, `code`, `name`, `description`) VALUES (13, 'de', 'E-22 Blaster Rifle', '');
INSERT INTO `tabletop`.`weapon_description` (`weapon_id`, `code`, `name`, `description`) VALUES (14, 'en', 'E-11D Blaster Rifle', '');
INSERT INTO `tabletop`.`weapon_description` (`weapon_id`, `code`, `name`, `description`) VALUES (14, 'de', 'E-11D Blaster Rifle', '');
# 28 records

#
# Dumping data for table 'tabletop'.'units'
#

INSERT INTO `tabletop`.`units` (`unit_id`, `fraction_id`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (1, 1, 6, 17, 2, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `fraction_id`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (2, 1, 5, 17, 1, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `fraction_id`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (3, 1, 6, 17, 1, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `fraction_id`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (4, 1, 6, 17, 2, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `fraction_id`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (5, 1, 6, 17, 2, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `fraction_id`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (6, 1, 6, 17, 4, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `fraction_id`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (7, 1, 6, 17, 2, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `fraction_id`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (8, 1, 7, 17, 5, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `fraction_id`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (9, 1, 6, 17, 3, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `fraction_id`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (10, 2, 2, 12, 6, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `fraction_id`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (11, 2, 4, 12, 9, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `fraction_id`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (12, 2, 5, 16, 6, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `fraction_id`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (13, 2, 4, 12, 9, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `fraction_id`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (14, 2, 4, 12, 6, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `fraction_id`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (15, 2, 10, 22, 10, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `fraction_id`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (16, 2, 6, 17, 11, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `fraction_id`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (17, 3, 5, 16, 10, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `fraction_id`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (18, 3, 6, 14, 10, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `fraction_id`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (19, 3, 7, 16, 14, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `fraction_id`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (20, 3, 5, 15, 13, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `fraction_id`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (21, 3, 6, 14, 11, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `fraction_id`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (22, 3, 7, 16, 10, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `fraction_id`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (23, 3, 4, 16, 12, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `fraction_id`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (24, 4, 4, 16, 11, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `fraction_id`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (25, 4, 4, 16, 11, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `fraction_id`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (26, 4, 4, 16, 11, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `fraction_id`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (27, 4, 4, 16, 11, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `fraction_id`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (28, 4, 4, 16, 11, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `fraction_id`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (29, 4, 4, 16, 11, NULL, NULL);
INSERT INTO `tabletop`.`units` (`unit_id`, `fraction_id`, `defense`, `moving`, `primary_weapon_id`, `secondary_weapon_id`, `image`) VALUES (30, 4, 4, 16, 11, NULL, NULL);
# 30 records

#
# Dumping data for table 'tabletop'.'unit_description'
#

INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (1, 'en', '41st Ranger Platoon', 'The 41st Ranger Platoon is a clone trooper infantry platoon and strike team within the 41st Elite Corps. The platoon participated in sabotage missions on Separatist supply lines and installations.', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (1, 'de', '41st Ranger Platoon', '', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (2, 'en', '41st Elite Corps', 'The 41st Elite Corps is a military corps of the Grand Army of the Republic. They are specialized in conducting missions on exotic and often primitve worlds. One of their main tasks was to contact and cooperate with the native indigenous population. in addition, the unit was capable of successfully carrying out both infatry and reconnaissance missions.', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (2, 'de', '41st Elite Korp', '', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (3, 'en', '212th Attack Battalion', 'The 212th Attack Battalion is a clone trooper battalion in the Grand Army of the Republic. The Battalion is famous for its bravery, discipline and effectivness in combat. As an extremly versatile infantry unit of the Regular Forces, it is optimally equipped for a variety of different tasks.', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (3, 'de', '212. Angriffsbattalion', '', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (4, 'en', '2nd Airborne Company', 'The 2nd Airborne Company was an aerial infantry company of the 212th Attack Battalion. The company consisted of clone paratroopers and participated in the Clone Wars.', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (4, 'de', '2. Luftlandekompanie', '', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (5, 'en', '501st Legion', 'The 501st Legion is an elite military battalion of clone troopers in the Grand Army of the Galactic Republic. The soldiers of the Legion are known for their courage, unconventional tactics and loyalty to the Republic.', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (5, 'de', '501. Legion', '', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (6, 'en', '501st Jetpack Company', 'The 501st Jetpack Company is an special unit within the 501st Legion whose soldiers are equipped with jetpacks. The jetpack troopers use their jetpacks with limited flight capabilities to quickly cover large distances and gain an advantage over their enemies in the air. Thanks to their exceptional agility, they are hard to hit and can skillfully attack their enemies from behind.', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (6, 'de', '501. Jetpack Kompanie', '', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (7, 'en', '104th Battalion', 'The 104th Battalion, is a military unit within the Galactic Republic. Its primary mission is to perform relief and recovery missions. The battalions distinctive mark is a symbol with a wolfs snout, which is why they are also called the Wolfpack battalion.', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (7, 'de', '104. Battalion', '', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (8, 'en', '501st Heavy Gunner', 'The 501st Heavy Gunners form an elite special forces unit within the 501st Legion, whose members are equipped with powerful Heavy Guns. These soldiers possess advanced clone trooper armor that offers significant improvements over traditional army clone troopers. Their heavy blasters also give them superior firepower, making them a formidable presence on the battlefield.', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (8, 'de', '501. Schwerer Schütze', '', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (9, 'en', '501st Specialist', 'The 501st Specialist represent an special unit within the 501st Legion. Their expertise lies in precise attacks from a considerable distance, which makes them an extremely dangerous threat. Their primary mission is to conduct patrols and lead reconnaissance missions.', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (9, 'de', '501. Scharfschütze', '', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (10, 'en', 'B1-series Battle Droid', 'The B1 battle droids are developed for mass combat and are known to operate in large numbers against enemy forces. They have a relatively simple artificial intelligence and are often quite clumsy and easy to overcome.', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (10, 'de', 'B1-Serie Kampfdroide', '', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (11, 'en', 'B2-series Super Battle Droid', 'The B2-Series Super Battle Droid is a more advanced and much more durable version of the B1-Series Battle Droid. Equipped with reinforced armor and more advanced weapons, the B2 significantly surpasses its predecessor in terms of combat power. Added to this is its improved artificial intelligence, which makes it an extremely dangerous and efficient combat unit.', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (11, 'de', 'B2-Serie Superkampfdroide', '', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (12, 'en', 'BX-series Commando Droid', 'The BX-series Commando Droid represents a specialized and sophisticated class of combat droids. Their main role is in covert missions and tactical operations. Equipped with advanced cloaking devices that can make them almost invisible, they are capable of carrying out surprise attacks skillfully.', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (12, 'de', 'BX-Serie Kommandodroide', '', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (13, 'en', 'B2-HA series Super Battle Droid', 'The B2-HA Super Battle Droid, also known as the Heavy Assault Super Battle Droid, is a variant of the B2-Series Battle Droid equipped with a missile launcher. Its ability to perform targeted missile attacks makes it an extremely dangerous combat unit in various situations.', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (13, 'de', 'B2-HA Serie Raketenkampfdroide', '', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (14, 'en', 'B1-series Rocket Battle Droid', '', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (14, 'de', 'B1-Serie Jetpack Kampfdroide', '', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (15, 'en', 'Droideka', 'Droidekas are specialized battle droids characterized by their characteristic spherical shape. In this spherical form, they are able to attack, but they are also extremely fast and agile. However, once they assume their attack position, they unfold to reveal their three-legged form. In this upright position, they have devastating energy weapon salvos and are also equipped with powerful energy shields that protect them from enemy fire.', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (15, 'de', 'Droideka', '', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (16, 'en', 'B1-series Sniper Droid', 'The B1-series Sniper Droid is a special variant of the B1-series combat droid. Their gyroscopic stabilizers and detailed target selection programs made them very effective snipers.', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (16, 'de', 'Scharfschützendroide der B1-Serie', '', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (17, 'en', 'Imperial Storm Trooper', 'Stormtroopers are the primary unit of the Galactic Empire. In action, the Stormtroopers are capable of a variety of tasks, including counterinsurgency, defense of Imperial installations, as well as assault missions and conducting operations as part of the Galactic Conquest of the Empire.', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (17, 'de', 'Imperialer Sturmtruppe', '', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (18, 'en', 'Imperial Snow Trooper', 'The Snowtroopers are specialized stormtroopers of the Galactic Empire, equipped with special armors designed to protect them from extreme conditions, including cold, ice, and snow. Their main mission is to pursue hostile targets in snow-covered regions.', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (18, 'de', 'Imperialer Schneetruppe', '', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (19, 'en', 'Imperial Death Trooper', 'Death Troopers are elite soldiers assigned to particularly dangerous and clandestine missions. They often serve as bodyguards for high-ranking Imperial officers or perform special tasks where discretion and efficiency are of the utmost importance.', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (19, 'de', 'Imperiale Todestruppe', '', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (20, 'en', 'Imperial Shore Trooper', 'Shore Troopers are specialized stormtroopers equipped with armor optimized for combat in maritime environments. Their main task is to engage hostile forces on beaches or coastlines and secure important locations. Additionally, they are often deployed in the defense of Imperial bases along coastal lines.', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (20, 'de', 'Imperiale Küstentruppe', '', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (21, 'en', 'Imperial Shock Trooper', 'Shock troopers are elite and highly trained Storm Troopers of the Galactic Empire. They are known for wearing red armor. Their role is to maintain imperial order and combat threats to the Empire.', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (21, 'de', 'Imperiale Stocktruppe', '', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (22, 'en', 'Imperial Jetpack Trooper', 'Jetpack Troopers are specialized troops equipped with jetpacks, allowing them to hover in the air and move faster and more agilely in combat, making them dangerous and versatile adversaries.', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (22, 'de', 'Imperiale Jetpack Truppe', '', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (23, 'en', 'Imperial Scout Trooper', 'The Imperial Scout Troopers are specialized stormtroopers who place special emphasis on speed and agility. Therefore, they wear lighter and slimmer armor compared to regular stormtroopers. The main task of this unit is to conduct patrols and perform reconnaissance missions.', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (23, 'de', 'Imperiale Aufklärungstruppe', '', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (24, 'en', 'Rebel Trooper', '', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (24, 'de', 'Rebellentruppe', '', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (25, 'en', 'Rebel Commando Trooper', '', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (25, 'de', 'Kommandotruppe der Rebellion', '', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (26, 'en', 'Rebel Fleet Trooper', '', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (26, 'de', 'Flootentruppe der Rebellion', '', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (27, 'en', 'Rebel Snow Trooper', '', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (27, 'de', 'Schneetruppe der Rebellion', '', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (28, 'en', 'Rebel Jetpack Trooper', '', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (28, 'de', 'Jetpack Truppe der Rebellion', '', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (29, 'en', 'Rebel Sniper', '', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (29, 'de', 'Scharfschütze der Rebellion', '', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (30, 'en', 'Rebel Heavy Gunner', '', '');
INSERT INTO `tabletop`.`unit_description` (`unit_id`, `code`, `name`, `description`, `mechanic`) VALUES (30, 'de', 'Schwerer Schütze der Rebellion', '', '');
# 60 records

#
# Dumping data for table 'tabletop'.'gamemodes'
#

INSERT INTO `tabletop`.`gamemodes` (`gamemode_id`, `image`) VALUES (1, NULL);
INSERT INTO `tabletop`.`gamemodes` (`gamemode_id`, `image`) VALUES (2, NULL);
INSERT INTO `tabletop`.`gamemodes` (`gamemode_id`, `image`) VALUES (3, NULL);
INSERT INTO `tabletop`.`gamemodes` (`gamemode_id`, `image`) VALUES (4, NULL);
# 4 records

#
# Dumping data for table 'tabletop'.'gamemode_description'
#

INSERT INTO `tabletop`.`gamemode_description` (`gamemode_id`, `code`, `name`, `description`, `mechanic`) VALUES (1, 'en', 'Skirmish', 'The game mode "Skirmish" is the most common of all and focuses on the battle between armies. In this mode, 2-4 armies compete against each other, aiming to eliminate as many units from the opponents field as possible. The game mode is round-limited. The winner is the player who has the most power points remaining on the field at the end.', '');
INSERT INTO `tabletop`.`gamemode_description` (`gamemode_id`, `code`, `name`, `description`, `mechanic`) VALUES (1, 'de', 'Gefecht', '', '');
INSERT INTO `tabletop`.`gamemode_description` (`gamemode_id`, `code`, `name`, `description`, `mechanic`) VALUES (2, 'en', 'Extraction', 'The game mode "Escape" revolves around both players escorting a VIP and trying to get them out of the area. The conflict arises because there is only one extraction point. It is also possible to eliminate the enemys VIP from the game. Once the enemy VIP has been eliminated or your own VIP has escaped through the evacuation zone, the player wins.', '');
INSERT INTO `tabletop`.`gamemode_description` (`gamemode_id`, `code`, `name`, `description`, `mechanic`) VALUES (2, 'de', 'Flucht', '', '');
INSERT INTO `tabletop`.`gamemode_description` (`gamemode_id`, `code`, `name`, `description`, `mechanic`) VALUES (3, 'en', 'Assault', 'In the game mode "Assault," the objective is to occupy or defend three defense sectors in succession. The attacker begins with their entire army on one side, while the defender distributes their units across the defense sectors. The attacker has six rounds to capture each sector. The round limit resets after a successful capture. The attacker wins after capturing the third sector, while the defender wins if the round limit expires at any of the sectors.', '');
INSERT INTO `tabletop`.`gamemode_description` (`gamemode_id`, `code`, `name`, `description`, `mechanic`) VALUES (3, 'de', 'Ansturm', '', '');
INSERT INTO `tabletop`.`gamemode_description` (`gamemode_id`, `code`, `name`, `description`, `mechanic`) VALUES (4, 'en', 'Domination', 'In the game mode "Domination," there are three zones that each side must control.', '');
INSERT INTO `tabletop`.`gamemode_description` (`gamemode_id`, `code`, `name`, `description`, `mechanic`) VALUES (4, 'de', 'Herrschaft', '', '');
# 8 records

#
# Dumping data for table 'tabletop'.'permissions'
#

INSERT INTO `tabletop`.`permissions` (`permission_id`, `identifier`) VALUES (1, 'ADD_USERS');
INSERT INTO `tabletop`.`permissions` (`permission_id`, `identifier`) VALUES (2, 'ADD_UNITS');
INSERT INTO `tabletop`.`permissions` (`permission_id`, `identifier`) VALUES (3, 'ADD_WEAPONS');
INSERT INTO `tabletop`.`permissions` (`permission_id`, `identifier`) VALUES (4, 'ADD_FRACTIONS');
INSERT INTO `tabletop`.`permissions` (`permission_id`, `identifier`) VALUES (5, 'ADD_GAMEMODES');
INSERT INTO `tabletop`.`permissions` (`permission_id`, `identifier`) VALUES (6, 'VIEW_USERS');
INSERT INTO `tabletop`.`permissions` (`permission_id`, `identifier`) VALUES (7, 'VIEW_UNITS');
INSERT INTO `tabletop`.`permissions` (`permission_id`, `identifier`) VALUES (8, 'VIEW_WEAPONS');
INSERT INTO `tabletop`.`permissions` (`permission_id`, `identifier`) VALUES (9, 'VIEW_FRACTIONS');
INSERT INTO `tabletop`.`permissions` (`permission_id`, `identifier`) VALUES (10, 'VIEW_GAMEMODES');
INSERT INTO `tabletop`.`permissions` (`permission_id`, `identifier`) VALUES (11, 'EDIT_USERS');
INSERT INTO `tabletop`.`permissions` (`permission_id`, `identifier`) VALUES (12, 'EDIT_UNITS');
INSERT INTO `tabletop`.`permissions` (`permission_id`, `identifier`) VALUES (13, 'EDIT_WEAPONS');
INSERT INTO `tabletop`.`permissions` (`permission_id`, `identifier`) VALUES (14, 'EDIT_FRACTIONS');
INSERT INTO `tabletop`.`permissions` (`permission_id`, `identifier`) VALUES (15, 'EDIT_GAMEMODES');
INSERT INTO `tabletop`.`permissions` (`permission_id`, `identifier`) VALUES (16, 'DELETE_USERS');
INSERT INTO `tabletop`.`permissions` (`permission_id`, `identifier`) VALUES (17, 'DELETE_UNITS');
INSERT INTO `tabletop`.`permissions` (`permission_id`, `identifier`) VALUES (18, 'DELETE_WEAPONS');
INSERT INTO `tabletop`.`permissions` (`permission_id`, `identifier`) VALUES (19, 'DELETE_FRACTIONS');
INSERT INTO `tabletop`.`permissions` (`permission_id`, `identifier`) VALUES (20, 'DELETE_GAMEMODES');
# 20 records

#
# Dumping data for table 'tabletop'.'permission_description'
#

INSERT INTO `tabletop`.`permission_description` (`permission_id`, `code`, `name`, `description`) VALUES (1, 'en', 'Add Users', '');
INSERT INTO `tabletop`.`permission_description` (`permission_id`, `code`, `name`, `description`) VALUES (1, 'de', 'Benutzer erstellen', '');
INSERT INTO `tabletop`.`permission_description` (`permission_id`, `code`, `name`, `description`) VALUES (2, 'en', 'Add Units', '');
INSERT INTO `tabletop`.`permission_description` (`permission_id`, `code`, `name`, `description`) VALUES (2, 'de', 'Einheiten erstellen', '');
INSERT INTO `tabletop`.`permission_description` (`permission_id`, `code`, `name`, `description`) VALUES (3, 'en', 'Add Weapons', '');
INSERT INTO `tabletop`.`permission_description` (`permission_id`, `code`, `name`, `description`) VALUES (3, 'de', 'Waffen erstellen', '');
INSERT INTO `tabletop`.`permission_description` (`permission_id`, `code`, `name`, `description`) VALUES (4, 'en', 'Add Fractions', '');
INSERT INTO `tabletop`.`permission_description` (`permission_id`, `code`, `name`, `description`) VALUES (4, 'de', 'Fraktionen erstellen', '');
INSERT INTO `tabletop`.`permission_description` (`permission_id`, `code`, `name`, `description`) VALUES (5, 'en', 'Add Gamemodes', '');
INSERT INTO `tabletop`.`permission_description` (`permission_id`, `code`, `name`, `description`) VALUES (5, 'de', 'Spielmodi erstellen', '');
INSERT INTO `tabletop`.`permission_description` (`permission_id`, `code`, `name`, `description`) VALUES (6, 'en', 'View Users', '');
INSERT INTO `tabletop`.`permission_description` (`permission_id`, `code`, `name`, `description`) VALUES (6, 'de', 'Benutzer einsehen', '');
INSERT INTO `tabletop`.`permission_description` (`permission_id`, `code`, `name`, `description`) VALUES (7, 'en', 'View Units', '');
INSERT INTO `tabletop`.`permission_description` (`permission_id`, `code`, `name`, `description`) VALUES (7, 'de', 'Einheiten einsehen', '');
INSERT INTO `tabletop`.`permission_description` (`permission_id`, `code`, `name`, `description`) VALUES (8, 'en', 'View Weapons', '');
INSERT INTO `tabletop`.`permission_description` (`permission_id`, `code`, `name`, `description`) VALUES (8, 'de', 'Waffen einsehen', '');
INSERT INTO `tabletop`.`permission_description` (`permission_id`, `code`, `name`, `description`) VALUES (9, 'en', 'View Fractions', '');
INSERT INTO `tabletop`.`permission_description` (`permission_id`, `code`, `name`, `description`) VALUES (9, 'de', 'Fraktionen einsehen', '');
INSERT INTO `tabletop`.`permission_description` (`permission_id`, `code`, `name`, `description`) VALUES (10, 'en', 'View Gamemodes', '');
INSERT INTO `tabletop`.`permission_description` (`permission_id`, `code`, `name`, `description`) VALUES (10, 'de', 'Spielmodi einsehen', '');
INSERT INTO `tabletop`.`permission_description` (`permission_id`, `code`, `name`, `description`) VALUES (11, 'en', 'Edit Users', '');
INSERT INTO `tabletop`.`permission_description` (`permission_id`, `code`, `name`, `description`) VALUES (11, 'de', 'Benutzer bearbeiten', '');
INSERT INTO `tabletop`.`permission_description` (`permission_id`, `code`, `name`, `description`) VALUES (12, 'en', 'Edit Units', '');
INSERT INTO `tabletop`.`permission_description` (`permission_id`, `code`, `name`, `description`) VALUES (12, 'de', 'Einheiten bearbeiten', '');
INSERT INTO `tabletop`.`permission_description` (`permission_id`, `code`, `name`, `description`) VALUES (13, 'en', 'Edit Weapons', '');
INSERT INTO `tabletop`.`permission_description` (`permission_id`, `code`, `name`, `description`) VALUES (13, 'de', 'Waffen bearbeiten', '');
INSERT INTO `tabletop`.`permission_description` (`permission_id`, `code`, `name`, `description`) VALUES (14, 'en', 'Edit Fractions', '');
INSERT INTO `tabletop`.`permission_description` (`permission_id`, `code`, `name`, `description`) VALUES (14, 'de', 'Fraktionen bearbeiten', '');
INSERT INTO `tabletop`.`permission_description` (`permission_id`, `code`, `name`, `description`) VALUES (15, 'en', 'Edit Gamemodes', '');
INSERT INTO `tabletop`.`permission_description` (`permission_id`, `code`, `name`, `description`) VALUES (15, 'de', 'Spielmodi bearbeiten', '');
INSERT INTO `tabletop`.`permission_description` (`permission_id`, `code`, `name`, `description`) VALUES (16, 'en', 'Delete Users', '');
INSERT INTO `tabletop`.`permission_description` (`permission_id`, `code`, `name`, `description`) VALUES (16, 'de', 'Benutzer löschen', '');
INSERT INTO `tabletop`.`permission_description` (`permission_id`, `code`, `name`, `description`) VALUES (17, 'en', 'Delete Units', '');
INSERT INTO `tabletop`.`permission_description` (`permission_id`, `code`, `name`, `description`) VALUES (17, 'de', 'Einheiten löschen', '');
INSERT INTO `tabletop`.`permission_description` (`permission_id`, `code`, `name`, `description`) VALUES (18, 'en', 'Delete Weapons', '');
INSERT INTO `tabletop`.`permission_description` (`permission_id`, `code`, `name`, `description`) VALUES (18, 'de', 'Waffen löschen', '');
INSERT INTO `tabletop`.`permission_description` (`permission_id`, `code`, `name`, `description`) VALUES (19, 'en', 'Delete Fractions', '');
INSERT INTO `tabletop`.`permission_description` (`permission_id`, `code`, `name`, `description`) VALUES (19, 'de', 'Fraktionen löschen', '');
INSERT INTO `tabletop`.`permission_description` (`permission_id`, `code`, `name`, `description`) VALUES (20, 'en', 'Delete Gamemodes', '');
INSERT INTO `tabletop`.`permission_description` (`permission_id`, `code`, `name`, `description`) VALUES (20, 'de', 'Spielmodi löschen', '');
# 40 records

#
# Optional: Adds an admin user
#
#------------------------------------------------------------------
#

#
# Dumping data for table 'users'
#

INSERT INTO `tabletop`.`users` (`user_id`, `username`, `display_name`, `description`, `main_fraction_id`, `password`, `salt`, `last_login`, `image`) VALUES (1, 'admin', 'Administrator', '', NULL, 'AQAAAAIAAYagAAAAECDOJwBpi0sqraVzpbiMs47xjH/gr8+/QcCClDnZ2oHzn1xA1jmU50ym+jODGjAHiQ==', '5Vjt5j4785', '2023-01-01 12:00:00', NULL);
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
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;z