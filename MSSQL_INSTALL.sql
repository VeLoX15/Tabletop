IF EXISTS (SELECT * FROM sys.schemas WHERE name = 'Tabletop')
    DROP SCHEMA Tabletop;

CREATE SCHEMA Tabletop AUTHORIZATION dbo;

USE Tabletop;

-- -----------------------------------------------------
-- Table 'Fractions'
-- -----------------------------------------------------
CREATE TABLE Fractions 
(
    FractionId INT IDENTITY(1,1) PRIMARY KEY,
    Image VARBINARY(MAX) NULL
);

-- -----------------------------------------------------
-- Table 'FractionDescription'
-- -----------------------------------------------------
CREATE TABLE FractionDescription (
    FractionId INT NOT NULL,
    Code VARCHAR(5) NOT NULL DEFAULT '',
    Name VARCHAR(50) NOT NULL,
    ShortName VARCHAR(5) NOT NULL,
    Description TEXT NULL,

    PRIMARY KEY (FractionId, Code),
    FOREIGN KEY (FractionId) REFERENCES Fractions(FractionId) ON DELETE CASCADE ON UPDATE CASCADE
);

-- -----------------------------------------------------
-- Table 'Gamemodes'
-- -----------------------------------------------------
CREATE TABLE Gamemodes 
(
    GamemodeId INT IDENTITY(1,1) PRIMARY KEY,
    Image VARBINARY(MAX) NULL
);

-- -----------------------------------------------------
-- Table 'GamemodeDescription'
-- -----------------------------------------------------
CREATE TABLE GamemodeDescription (
    GamemodeId INT NOT NULL,
    Code VARCHAR(5) NOT NULL DEFAULT '',
    Name VARCHAR(50) NOT NULL,
    Description TEXT NULL,
    Mechanic TEXT NULL,

    PRIMARY KEY (GamemodeId, Code),
    FOREIGN KEY (GamemodeId) REFERENCES Gamemodes(GamemodeId) ON DELETE CASCADE ON UPDATE CASCADE
);

-- -----------------------------------------------------
-- Table 'Weapons'
-- -----------------------------------------------------
CREATE TABLE Weapons 
(
    WeaponId INT IDENTITY(1,1) PRIMARY KEY,
    Attack INT NOT NULL,
    Quality INT NOT NULL,
    Range INT NOT NULL,
    Dices INT NOT NULL,
    Image VARBINARY(MAX) NULL
);

-- -----------------------------------------------------
-- Table 'WeaponDescription'
-- -----------------------------------------------------
CREATE TABLE WeaponDescription (
    WeaponId INT NOT NULL,
    Code VARCHAR(5) NOT NULL DEFAULT '',
    Name VARCHAR(50) NOT NULL,
    Description TEXT NULL,

    PRIMARY KEY (WeaponId, Code),
    FOREIGN KEY (WeaponId) REFERENCES Weapons(WeaponId) ON DELETE CASCADE ON UPDATE CASCADE
);

-- -----------------------------------------------------
-- Table 'Classes'
-- -----------------------------------------------------
CREATE TABLE Classes (
    ClassId INT NOT NULL,
    Quantity INT NOT NULL,
    
    PRIMARY KEY (ClassId)
);

-- -----------------------------------------------------
-- Table 'ClassDescription'
-- -----------------------------------------------------
CREATE TABLE ClassDescription (
    ClassId INT NOT NULL,
    Code VARCHAR(5) NOT NULL DEFAULT '',
    Name VARCHAR(50) NOT NULL,
    Description TEXT NULL,

    PRIMARY KEY (ClassId, Code),
    FOREIGN KEY (ClassId) REFERENCES Classes(ClassId) ON DELETE CASCADE ON UPDATE CASCADE
);

-- -----------------------------------------------------
-- Table 'Abilities'
-- -----------------------------------------------------
CREATE TABLE Abilities (
    AbilityId INT,
    Quality INT NOT NULL,
    Force INT NOT NULL,
    
    PRIMARY KEY (AbilityId)
);

-- -----------------------------------------------------
-- Table 'AbilityDescription'
-- -----------------------------------------------------
CREATE TABLE AbilityDescription (
    AbilityId INT NOT NULL,
    Code VARCHAR(5) NOT NULL DEFAULT '',
    Name VARCHAR(50) NOT NULL,
    Description TEXT NULL,
    Mechanic TEXT NULL,

    PRIMARY KEY (AbilityId, Code),
    FOREIGN KEY (AbilityId) REFERENCES Abilities(AbilityId) ON DELETE CASCADE ON UPDATE CASCADE
);

-- -----------------------------------------------------
-- Table 'Units'
-- -----------------------------------------------------
CREATE TABLE Units
(
    UnitId INT IDENTITY(1,1) PRIMARY KEY,
    FractionId INT NOT NULL,
    ClassId INT NOT NULL,
    TroopQuantity INT NOT NULL,
    Defense INT NOT NULL,
    Moving INT NOT NULL,
    PrimaryWeaponId INT NULL,
    SecondaryWeaponId INT NULL,
    FirstAbilityId INT NULL,
    SecondAbilityId INT NULL,
    Image VARBINARY(MAX) NULL,

    FOREIGN KEY (FractionId) REFERENCES Fractions(FractionId) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (ClassId) REFERENCES Classes(ClassId),
    FOREIGN KEY (PrimaryWeaponId) REFERENCES Weapons(WeaponId),
    FOREIGN KEY (SecondaryWeaponId) REFERENCES Weapons(WeaponId),
    FOREIGN KEY (FirstAbilityId) REFERENCES Abilities(AbilityId),
    FOREIGN KEY (SecondAbilityId) REFERENCES Abilities(AbilityId)
);

-- -----------------------------------------------------
-- Table 'UnitDescription'
-- -----------------------------------------------------
CREATE TABLE UnitDescription (
    UnitId INT NOT NULL,
    Code VARCHAR(5) NOT NULL DEFAULT '',
    Name VARCHAR(50) NOT NULL,
    Description TEXT NULL,
    Mechanic TEXT NULL,

    PRIMARY KEY (UnitId, Code),
    FOREIGN KEY (UnitId) REFERENCES Units(UnitId) ON DELETE CASCADE ON UPDATE CASCADE
);

-- -----------------------------------------------------
-- Table 'Users'
-- -----------------------------------------------------
CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    Username VARCHAR(50) NOT NULL,
    DisplayName VARCHAR(100) NOT NULL,
    Description TEXT NULL,
    MainFractionId INT NULL, 
    Password VARCHAR(255) NOT NULL,
    Salt VARCHAR(255) NOT NULL,
    LastLogin DATETIME,
    RegistrationDate TIMESTAMP,
    Image VARBINARY(MAX) NULL,

    FOREIGN KEY (MainFractionId) REFERENCES Fractions(FractionId)
);

-- -----------------------------------------------------
-- Table 'Permissions'
-- -----------------------------------------------------
CREATE TABLE Permissions (
    PermissionId INT PRIMARY KEY,
    Identifier VARCHAR(50) NOT NULL
);

-- -----------------------------------------------------
-- Table 'PermissionDescription'
-- -----------------------------------------------------
CREATE TABLE PermissionDescription (
    PermissionId INT NOT NULL,
    Code VARCHAR(5) NOT NULL DEFAULT '',
    Name VARCHAR(50) NOT NULL,
    Description TEXT NULL,

    PRIMARY KEY (PermissionId, Code),
    FOREIGN KEY (PermissionId) REFERENCES Permissions(PermissionId) ON DELETE CASCADE ON UPDATE CASCADE
);

-- -----------------------------------------------------
-- Table 'UserPermissions'
-- -----------------------------------------------------
CREATE TABLE UserPermissions (
    UserId INT NOT NULL,
    PermissionId INT NOT NULL,

    PRIMARY KEY(UserId, PermissionId),
    FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (PermissionId) REFERENCES Permissions(PermissionId) ON DELETE CASCADE ON UPDATE CASCADE
);

-- -----------------------------------------------------
-- Table 'UserUnits'
-- -----------------------------------------------------
CREATE TABLE UserUnits 
(
    UserId INT NOT NULL,
    UnitId INT NOT NULL,
    Quantity INT NOT NULL,

    PRIMARY KEY (UserId, UnitId),
    FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (UnitId) REFERENCES Units(UnitId) ON DELETE CASCADE ON UPDATE CASCADE
);

-- -----------------------------------------------------
-- Table 'UserFriends'
-- -----------------------------------------------------
CREATE TABLE UserFriends 
(
    UserId INT NOT NULL,
    FriendId INT NOT NULL,

    PRIMARY KEY (UserId, FriendId),
    FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (FriendId) REFERENCES Users(UserId) ON DELETE CASCADE ON UPDATE CASCADE
);

-- -----------------------------------------------------
-- Table 'Games'
-- -----------------------------------------------------
CREATE TABLE Games 
(
    GameId INT IDENTITY(1,1) PRIMARY KEY,
    GamemodeId INT NOT NULL,
    UserId INT NOT NULL,
    Name VARCHAR(50) NOT NULL,
    Rounds INT NULL,
    Force INT NOT NULL,
    NumberOfTeams INT NOT NULL,
    NumberOfPlayers INT NOT NULL,
    Date DATETIME NOT NULL,

    FOREIGN KEY (GamemodeId) REFERENCES Gamemodes(GamemodeId)
);

-- -----------------------------------------------------
-- Table 'Players'
-- -----------------------------------------------------
CREATE TABLE Players 
(
    PlayerId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    GameId INT NOT NULL,
    FractionId INT NULL,
    Team INT NOT NULL,
    UsedForce INT NOT NULL,

    FOREIGN KEY (UserId) REFERENCES Users(UserId),
    FOREIGN KEY (GameId) REFERENCES Games(GameId) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (FractionId) REFERENCES Fractions(FractionId)
);

-- -----------------------------------------------------
-- Table 'PlayerUnits'
-- -----------------------------------------------------
CREATE TABLE PlayerUnits 
(
    PlayerId INT NOT NULL,
    UnitId INT NOT NULL,
    Quantity INT NOT NULL,

    PRIMARY KEY (PlayerId, UnitId),
    FOREIGN KEY (PlayerId) REFERENCES Players(PlayerId) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (UnitId) REFERENCES Units(UnitId) ON DELETE CASCADE ON UPDATE CASCADE
);

-- -----------------------------------------------------
-- Table 'Teams'
-- -----------------------------------------------------
CREATE TABLE Teams 
(
    TeamId INT NOT NULL,
    GameId INT NOT NULL,
    Name VARCHAR(50) NOT NULL,
    Points INT NOT NULL, 

    PRIMARY KEY (TeamId, GameId),
    FOREIGN KEY (GameId) REFERENCES Games(GameId) ON DELETE CASCADE ON UPDATE CASCADE
);

-- -----------------------------------------------------
-- Table 'Templates'
-- -----------------------------------------------------
CREATE TABLE Templates 
(
    TemplateId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    FractionId INT NOT NULL,
    Name VARCHAR(50) NOT NULL,
    Force INT NOT NULL, 
    UsedForce INT NOT NULL,

    FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (FractionId) REFERENCES Fractions(FractionId) ON DELETE CASCADE ON UPDATE CASCADE
);

-- -----------------------------------------------------
-- Table 'TemplateUnits'
-- -----------------------------------------------------
CREATE TABLE TemplateUnits 
(
    TemplateId INT NOT NULL,
    UnitId INT NOT NULL,
    Quantity INT NOT NULL,

    PRIMARY KEY (TemplateId, UnitId),
    FOREIGN KEY (TemplateId) REFERENCES Templates(TemplateId) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (UnitId) REFERENCES Units(UnitId) ON DELETE CASCADE ON UPDATE CASCADE
);