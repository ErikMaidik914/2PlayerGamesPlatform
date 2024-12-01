USE [GamesDatabase]
GO


-----------------------------------------------------
GO
CREATE OR ALTER PROCEDURE getHighestEloForAGame
    @playerId UNIQUEIDENTIFIER,
    @gameName VARCHAR(50)
AS
BEGIN
    SELECT TOP 1
        gs.highestEloRating
    FROM
        GameStats gs
    INNER JOIN
        Game g ON gs.gameId = g.gameid
    INNER JOIN
        Player p ON gs.playerId = p.playerId
    WHERE
        p.playerId = @playerId
        AND g.gameName = @gameName
    ORDER BY
        gs.highestEloRating DESC;
END;
---------------------------------------------
GO
CREATE OR ALTER PROCEDURE getTotalNumberOfMatchesForAGame
	@playerId UNIQUEIDENTIFIER,
    @gameName VARCHAR(50)
AS
BEGIN
    SELECT
        COUNT(*) AS TotalGamesPlayed
    FROM
        GameState gs
    INNER JOIN
        Game g ON gs.gameId = g.gameid
    INNER JOIN
        Player p ON (gs.player1Id = p.playerId OR gs.player2Id = p.playerId)
    WHERE
        p.playerId = @playerId
        AND g.gameName = @gameName;
END;

----------------------------------------
GO
CREATE OR ALTER PROCEDURE getHoursPlayedForAGame
	@playerId UNIQUEIDENTIFIER,
	@gameName VARCHAR(50)
AS
BEGIN
	SELECT gs.totalPlayTime
	FROM GameStats gs
	INNER JOIN
		Player p on p.playerId = gs.playerId
	INNER JOIN
		Game g on g.gameid = gs.gameId
	WHERE
		p.playerId = @playerId AND
		g.gameName = @gameName
END;
----------------------------------------------------
GO
GO
CREATE OR ALTER PROCEDURE getHighestEloForAGame
    @playerId UNIQUEIDENTIFIER,
    @gameName VARCHAR(50)
AS
BEGIN
    SELECT TOP 1
        gs.eloRating
    FROM
        GameStats gs
    INNER JOIN
        Game g ON gs.gameId = g.gameid
    INNER JOIN
        Player p ON gs.playerId = p.playerId
    WHERE
        p.playerId = @playerId
        AND g.gameName = @gameName
    ORDER BY
        gs.highestEloRating DESC;
END;

-------------------

GO
CREATE OR ALTER PROCEDURE GetPlayerGameStats
    @playerId UNIQUEIDENTIFIER,
    @gameCategory VARCHAR(50),
    @totalGames INT OUTPUT,
    @totalWins INT OUTPUT
AS
BEGIN
    -- Declare variables to store counts
    DECLARE @totalGamesCount INT;
    DECLARE @totalWinsCount INT;

    -- Get total number of games played by the player for the given game type
    SELECT @totalGamesCount = COUNT(*)
    FROM GameState gs
    INNER JOIN Game g ON gs.gameId = g.gameid
    WHERE (gs.player1Id = @playerId OR gs.player2Id = @playerId)
      AND g.gameCategory = @gameCategory;

    -- Get total number of winning games played by the player for the given game type
    SELECT @totalWinsCount = COUNT(*)
    FROM GameState gs
    INNER JOIN Game g ON gs.gameId = g.gameid
    WHERE (gs.player1Id = @playerId OR gs.player2Id = @playerId)
      AND gs.winnerPlayer = @playerId
      AND g.gameCategory = @gameCategory;

    -- Output the counts
    SET @totalGames = @totalGamesCount;
    SET @totalWins = @totalWinsCount;
END;
GO

CREATE OR ALTER PROCEDURE removeFromQueue
    @playerId UNIQUEIDENTIFIER
AS
BEGIN
    DELETE FROM PlayerQueue WHERE playerId = @playerId
END;
GO

CREATE OR ALTER PROCEDURE addDartsGameToQueue
    @playerId UNIQUEIDENTIFIER,
    @eloRating INT
AS
BEGIN
    DECLARE @gameId UNIQUEIDENTIFIER;
    SET @gameId = (SELECT gameId FROM Game WHERE gameName = 'Darts');
    INSERT INTO PlayerQueue (playerId, gameId, eloRating, chessGameMode, obstructionWidth, obstructionHeight)
        VALUES (@playerId, @gameId, @eloRating, NULL, NULL, NULL);
END
GO

CREATE OR ALTER PROCEDURE addChessGameToQueue
    @playerId UNIQUEIDENTIFIER,
    @eloRating INT,
    @gameMode INT
AS
BEGIN
    DECLARE @gameId UNIQUEIDENTIFIER;
    SET @gameId = (SELECT gameId FROM Game WHERE gameName = 'Chess');
    INSERT INTO PlayerQueue (playerId, gameId, eloRating, chessGameMode, obstructionWidth, obstructionHeight)
        VALUES (@playerId, @gameId, @eloRating, @gameMode, NULL, NULL);
END
GO

CREATE OR ALTER PROCEDURE addObstructionGameToQueue
    @playerId UNIQUEIDENTIFIER,
    @eloRating INT,
    @obstructionWidth INT,
    @obstructionHeight INT
AS
BEGIN
    DECLARE @gameId UNIQUEIDENTIFIER;
    SET @gameId = (SELECT gameId FROM Game WHERE gameName = 'Obstruction');
    INSERT INTO PlayerQueue (playerId, gameId, eloRating, chessGameMode, obstructionWidth, obstructionHeight)
        VALUES (@playerId, @gameId, @eloRating, NULL, @obstructionWidth, @obstructionHeight);
END
GO

CREATE OR ALTER PROCEDURE addConnectFourGameToQueue
    @playerId UNIQUEIDENTIFIER,
    @eloRating INT
AS
BEGIN
    DECLARE @gameId UNIQUEIDENTIFIER;
    SET @gameId = (SELECT gameId FROM Game WHERE gameName = 'Connect4');
    INSERT INTO PlayerQueue (playerId, gameId, eloRating, chessGameMode, obstructionWidth, obstructionHeight)
        VALUES (@playerId, @gameId, @eloRating, NULL, NULL, NULL);
END
GO

CREATE OR ALTER PROCEDURE addToQueue
    @playerId UNIQUEIDENTIFIER,
    @gameId UNIQUEIDENTIFIER,
    @eloRating INT,
    @chessGameMode INT,
    @obstructionWidth INT,
    @obstructionHeight INT
AS
BEGIN
    INSERT INTO PlayerQueue (playerId, gameId, eloRating, chessGameMode, obstructionWidth, obstructionHeight)
        VALUES (@playerId, @gameId, @eloRating, @chessGameMode, @obstructionWidth, @obstructionHeight);
END

CREATE OR ALTER PROCEDURE addPlayer
    @playerId UNIQUEIDENTIFIER,
    @playerName VARCHAR(50),
    @playerPcIp VARCHAR(15),
    @playerPcPort INT
AS
BEGIN
    INSERT INTO Player (playerId, playerName, playerPcIp, playerPcPort)
        VALUES (@playerId, @playerName, @playerPcIp, @playerPcPort);
END
GO

CREATE OR ALTER PROCEDURE updatePlayer
    @playerId UNIQUEIDENTIFIER,
    @playerName VARCHAR(50),
    @playerPcIp VARCHAR(15),
    @playerPcPort INT
AS
BEGIN
    UPDATE Player
    SET playerName = @playerName,
        playerPcIp = @playerPcIp,
        playerPcPort = @playerPcPort
    WHERE playerId = @playerId;
END
GO

CREATE OR ALTER PROCEDURE removePlayerAddress
    @playerId UNIQUEIDENTIFIER
AS
BEGIN
    UPDATE Player
    SET playerPcIp = NULL,
        playerPcPort = NULL
    WHERE playerId = @playerId;
END
GO

CREATE OR ALTER PROCEDURE addGameState
    @gameStateId UNIQUEIDENTIFIER,
    @player1Id UNIQUEIDENTIFIER,
    @player2Id UNIQUEIDENTIFIER,
    @gameId UNIQUEIDENTIFIER,
    @turn INT,
    @timePlayed INT,
    @winnerPlayer UNIQUEIDENTIFIER,
    @gameState NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO GameState (gameStateId, player1Id, player2Id, gameId, turn, timePlayed, winnerPlayer, jsonGameState)
        VALUES (@gameStateId, @player1Id, @player2Id, @gameId, @turn, @timePlayed, @winnerPlayer, @gameState);
END
GO

CREATE OR ALTER PROCEDURE incrementGameStateTurn
    @gameStateId UNIQUEIDENTIFIER
AS
BEGIN
    UPDATE GameState
    SET turn = turn + 1
    WHERE gameStateId = @gameStateId;
END
GO

CREATE OR ALTER PROCEDURE saveGameState
    @gameStateId UNIQUEIDENTIFIER,
    @gameState NVARCHAR(MAX)
AS
BEGIN
    UPDATE GameState
    SET jsonGameState = @gameState
    WHERE gameStateId = @gameStateId;
END
GO

CREATE OR ALTER PROCEDURE updateGameState
    @gameStateId UNIQUEIDENTIFIER,
    @turn INT,
    @timePlayed INT,
    @winnerPlayer UNIQUEIDENTIFIER,
    @gameState NVARCHAR(MAX)
AS
BEGIN
    UPDATE GameState
    SET turn = @turn,
        timePlayed = @timePlayed,
        winnerPlayer = @winnerPlayer,
        jsonGameState = @gameState
    WHERE gameStateId = @gameStateId;
END
GO

CREATE OR ALTER PROCEDURE addWinToPlayer
    @gameStateId UNIQUEIDENTIFIER,
    @playerId UNIQUEIDENTIFIER
AS
BEGIN
    UPDATE GameState
    SET winnerPlayer = @playerId
    WHERE gameStateId = @gameStateId;
END
GO

CREATE OR ALTER PROCEDURE alterGameStateTimePlayed
    @gameStateId UNIQUEIDENTIFIER,
    @timePlayed INT
AS
BEGIN
    UPDATE GameState
    SET timePlayed = @timePlayed
    WHERE gameStateId = @gameStateId;
END
GO

CREATE OR ALTER PROCEDURE addGameStats
    @playerId UNIQUEIDENTIFIER,
    @gameId UNIQUEIDENTIFIER,
    @eloRating INT,
    @highestEloRating INT,
    @totalPlayTime INT,
    @totalGamesPlayed INT,
    @totalWins INT,
    @totalDraws INT,
    @numberTurns INT
AS
BEGIN
    INSERT INTO GameStats (playerId, gameId, eloRating, highestEloRating, totalPlayTime, totalMatches, totalWins, totalDraws, totalNumberOfTurns)
        VALUES (@playerId, @gameId, @eloRating, @highestEloRating, @totalPlayTime, @totalGamesPlayed, @totalWins, @totalDraws, @numberTurns);
END
GO

CREATE OR ALTER PROCEDURE updateGameStatsManual
    @playerId UNIQUEIDENTIFIER,
    @gameId UNIQUEIDENTIFIER,
    @eloRating INT,
    @highestEloRating INT,
    @totalPlayTime INT,
    @totalGamesPlayed INT,
    @totalWins INT,
    @totalDraws INT,
    @numberTurns INT
AS
BEGIN
    UPDATE GameStats
    SET eloRating = @eloRating,
        highestEloRating = @highestEloRating,
        totalPlayTime = @totalPlayTime,
        totalMatches = @totalGamesPlayed,
        totalWins = @totalWins,
        totalDraws = @totalDraws,
        totalNumberOfTurns = @numberTurns
    WHERE playerId = @playerId
      AND gameId = @gameId;
END
GO

CREATE OR ALTER PROCEDURE updateGameStatsAuto
    @playerId UNIQUEIDENTIFIER,
    @gameId UNIQUEIDENTIFIER,
    @eloRating INT
AS
BEGIN
    DECLARE @highestEloRating INT;
    SET @highestEloRating = (SELECT highestEloRating FROM GameStats WHERE playerId = @playerId AND gameId = @gameId);
    SET @highestEloRating = IIF(@eloRating > @highestEloRating, @eloRating, @highestEloRating);
    DECLARE @totalGamesPlayed INT;
    SET @totalGamesPlayed = (SELECT COUNT(*)
                                FROM GameState
                                WHERE (player1Id = @playerId OR player2Id = @playerId)
                                    AND gameId = @gameId);
    DECLARE @totalPlayTime INT;
    DECLARE @numberTurns INT;
    SELECT @totalPlayTime = SUM(timePlayed), @numberTurns = SUM(turn)
    FROM GameState
    WHERE (player1Id = @playerId OR player2Id = @playerId)
        AND gameId = @gameId;
    DECLARE @totalWins INT;
    SET @totalWins = (SELECT COUNT(*)
                        FROM GameState
                        WHERE winnerPlayer = @playerId
                            AND gameId = @gameId);
    DECLARE @totalDraws INT;
    SET @totalDraws = (SELECT COUNT(*)
                        FROM GameState
                        WHERE winnerPlayer = '00000000-0000-0000-0000-000000000000'
                            AND gameId = @gameId);
    UPDATE GameStats
    SET eloRating = @eloRating,
        highestEloRating = @highestEloRating,
        totalPlayTime = @totalPlayTime,
        totalMatches = @totalGamesPlayed,
        totalWins = @totalWins,
        totalDraws = @totalDraws,
        totalNumberOfTurns = @numberTurns
    WHERE playerId = @playerId
      AND gameId = @gameId;
END
GO

CREATE OR ALTER PROCEDURE addSettings
    @playerId UNIQUEIDENTIFIER,
    @playerIp VARCHAR(15),
    @isSoundOn BIT,
	@isMusicOn BIT,
	@soundVolume FLOAT,
	@musicVolume FLOAT
AS
BEGIN
    INSERT INTO PlayerSettings (playerId, playerPcIp, isSoundOn, isMusicOn, soundVolume, musicVolume)
        VALUES (@playerId, @playerIp, @isSoundOn, @isMusicOn, @soundVolume, @musicVolume);
END
GO

CREATE OR ALTER PROCEDURE updateSettings
    @playerId UNIQUEIDENTIFIER,
    @playerIp VARCHAR(15),
    @isSoundOn BIT,
    @isMusicOn BIT,
    @soundVolume FLOAT,
    @musicVolume FLOAT
AS
BEGIN
    UPDATE PlayerSettings
    SET isSoundOn = @isSoundOn,
        isMusicOn = @isMusicOn,
        soundVolume = @soundVolume,
        musicVolume = @musicVolume
    WHERE playerId = @playerId AND
        playerPcIp = @playerIp;
END
GO

CREATE OR ALTER PROCEDURE deleteGame
    @id UNIQUEIDENTIFIER
AS
BEGIN
    DELETE FROM GameState where GameState.id = @id
end


