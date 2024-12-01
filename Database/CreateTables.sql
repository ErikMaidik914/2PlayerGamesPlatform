USE TwoPlayerDB;
GO
DROP TABLE IF EXISTS PlayerSettings
DROP TABLE IF EXISTS PlayerQueue
DROP TABLE IF EXISTS GameStats
DROP TABLE IF EXISTS GameState
DROP TABLE IF EXISTS Player
DROP TABLE IF EXISTS Game
DROP TABLE IF EXISTS SoundEffects

GO

CREATE TABLE Player (
    playerId UNIQUEIDENTIFIER PRIMARY KEY,
    playerName VARCHAR(50),
    playerPcIp VARCHAR(15), 
    playerPcPort INT
);


CREATE TABLE Game(
	gameId UNIQUEIDENTIFIER PRIMARY KEY,
	gameName VARCHAR(50),
	gameCategory VARCHAR(50)
)

CREATE TABLE GameState(
	gameStateId UNIQUEIDENTIFIER PRIMARY KEY,
	player1Id UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Player(playerId),
	player2Id UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Player(playerId),
	gameId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Game(gameId),
	turn INT NOT NULL,
    timePlayed INT NOT NULL,
	winnerPlayer UNIQUEIDENTIFIER DEFAULT NULL,
	jsonGameState VARCHAR(MAX)
)

CREATE TABLE GameStats(
	playerId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Player(playerId),
	gameId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Game(gameId),
    PRIMARY KEY(playerId, gameId),
	eloRating INT NOT NULL ,
	highestEloRating INT NOT NULL,
	totalMatches INT NOT NULL,
	totalWins INT NOT NULL,
	totalDraws INT NOT NULL,
	totalPlayTime INT NOT NULL,
	totalNumberOfTurns INT NOT NULL
)

CREATE TABLE PlayerQueue(
	playerId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Player(playerId),
	gameId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Game(gameId),
	PRIMARY KEY(playerId, gameId),
    eloRating INT NOT NULL,
	chessGameMode INT,
	obstructionWidth INT,
	obstructionHeight INT
)

CREATE TABLE PlayerSettings(
	playerId UNIQUEIDENTIFIER REFERENCES Player(playerId),
	playerPcIp VARCHAR(15),
	PRIMARY KEY(playerId, playerPcIp),
	isSoundOn BIT NOT NULL ,
	isMusicOn BIT NOT NULL,
	soundVolume FLOAT NOT NULL,
	musicVolume FLOAT NOT NULL
)

CREATE TABLE SoundEffects(
    soundEffectId UNIQUEIDENTIFIER PRIMARY KEY,
    soundEffectName VARCHAR(20),
    soundEffectCategory VARCHAR(15),
    soundEffectPath VARCHAR(50)
)



SELECT* FROM Player
SELECT* FROM Game
SELECT* FROM GameState
SELECT* FROM GameStats
SELECT* FROM PlayerSettings
SELECT* FROM PlayerQueue
SELECT* FROM SoundEffects

CREATE NONCLUSTERED INDEX indexGameName ON Game(gameName);
CREATE NONCLUSTERED INDEX indexGameStateGameId ON GameState(gameId);
CREATE NONCLUSTERED INDEX indexGameStatePlayer1Id ON GameState(player1Id);
CREATE NONCLUSTERED INDEX indexGameStatePlayer2Id ON GameState(player2Id);
CREATE NONCLUSTERED INDEX indexGameStatsPlayerId ON GameStats(playerId);
CREATE NONCLUSTERED INDEX indexQueueGameId ON PlayerQueue(gameId);
CREATE NONCLUSTERED INDEX indexGameStateWinnerPlayer ON GameState(winnerPlayer);