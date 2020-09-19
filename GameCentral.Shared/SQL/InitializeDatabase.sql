CREATE SCHEMA Products;
GO;

CREATE SCHEMA Users;
GO;

CREATE TABLE GameCentral.Products.Games (
        GameId UNIQUEIDENTIFIER DEFAULT NEWID(),
        Title VARCHAR (128) NOT NULL ,
        Description VARCHAR(4096) DEFAULT 'The game hasn`t description',
        Studio VARCHAR (64) NOT NULL,
        Genre VARCHAR (32) NOT NULL DEFAULT ('UNKNOWN'),
        Publisher VARCHAR (64) NOT NULL DEFAULT ('INDIE'),
        Cost INT NOT NULL DEFAULT 0
        CONSTRAINT PK_GameKey PRIMARY KEY (Game_Id)
)

CREATE TABLE GameCentral.Users.Admins (
        NickName VARCHAR(32) PRIMARY KEY ,
        PasswordHash VARCHAR(32) NOT NULL,
        Role INT DEFAULT 0     /* 0 - god */                  
)

INSERT INTO GameCentral.Products.Games (Title, Studio, Genre, Publisher, Cost)
VALUES ('Mass Effect', 'Bioware', 'Sci-Fi action shooter', 'EA games', 20),
       ('METAL GEAR SOLID V: PHANTOM PAIN', 'Konami, Kojima Productins', 'Tactical shooter', 'Konami', 20),
       ('Nier: Automata', 'Platinum games', 'JRPG Slasher', 'EA games', 20),
       ('Prey', 'Arcane Studios', 'Sci-Fi action shooter', 'Bethesda', 30),
       ('Mass Effect 2', 'Bioware', 'Sci-Fi action shooter', 'EA games', 20),
       ('Mass Effect 3', 'Bioware', 'Sci-Fi action shooter', 'EA games', 20),
       ('Mass Effect: Andromeda', 'Bioware', 'Sci-Fi action shooter', 'EA games', 20),
       ('Witcher 3: Wild Hunt', 'CD PROJECT RED', 'Action RPG', 'CD PROJECT', 15),
       ('Portal', 'Valve', 'Pazzle game', 'Valve', 2),
       ('Portal 2', 'valve', 'Pazzle game', 'valve', 2),
       ('Divinity Original Sin 2', 'Larian Studios', 'Tactical RPG game', 'Larian', 10),
       ('Divinity Original Sin', 'Larian Studios', 'Tactical RPG game', 'Larian', 10),
       ('The Elder Scrolls V: Skyrim', 'Bethesda', 'Action RPG', 'Bethesda', 20)