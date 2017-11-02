USE MinionsDB

CREATE TABLE Countries
(
Id INT PRIMARY KEY IDENTITY,
CountryName VARCHAR(50) NOT NULL)

CREATE TABLE Towns(
Id INT PRIMARY KEY IDENTITY,
TownName VARCHAR(50) NOT NULL,
CountryId INT,
CONSTRAINT FK_CountryId_Country FOREIGN KEY(CountryId) REFERENCES Countries(Id))

CREATE TABLE Minions(
Id INT PRIMARY KEY IDENTITY,
[Name] VARCHAR(50),
Age INT,
TownId INT NOT NULL,
CONSTRAINT FK_TownId_Towns FOREIGN KEY(TownId) REFERENCES Towns(Id))

CREATE TABLE Villains(
Id INT PRIMARY KEY IDENTITY,
Name VARCHAR(50),
EvilnessFactor VARCHAR(10) DEFAULT 'evil',
CONSTRAINT CH_EvilnessFactor CHECK (EvilnessFactor IN ('good', 'bad', 'evil', 'super evil')))

CREATE TABLE VillainsMinions(
VillainId INT NOT NULL,
MinionId INT NOT NULL,
CONSTRAINT PK_VillainId_MinionId PRIMARY KEY(VillainId, MinionId),
CONSTRAINT FK_VillainId_Villains FOREIGN KEY(VillainId) REFERENCES Villains(Id),
CONSTRAINT FK_MinionId_Minions FOREIGN KEY(MinionId) REFERENCES Minions(Id))

INSERT INTO Countries(CountryName)
VALUES
('Bulgaria'),
('France'),
('USA'),
('Italy'),
('Belgia')

INSERT INTO Towns (TownName, CountryId)
     VALUES 
            ('Sofia', 1),
            ('Washington ', 3),
            ('Varna', 1),
            ('Burgas', 1),
            ('Pleven', 1)

INSERT INTO Minions([Name], Age, TownId)
     VALUES 
            ('Bob', 14, 1),
            ('Victor', 12, 2),
            ('Steward', 10, 3),
            ('Jilly', 13, 4),
            ('Kevin', 24, 5)

INSERT INTO Villains([Name], EvilnessFactor)
     VALUES 
            ('Gru', 'good'),
            ('Simon', 'bad'),
            ('Ivan', 'evil'),
            ('Niki', 'super evil'),
            ('Pesho', 'bad')

INSERT INTO VillainsMinions(VillainId, MinionId)
     VALUES
            (1,1),
            (1,2),
            (1,4),
            (2,3),
            (2,1),
            (2,4),
            (2,2),
            (3,1),
            (3,5),
            (3,2),
            (4,1),
            (4,2),
            (4,3),
            (4,4),
            (4,5),
            (5,1),
            (5,5)