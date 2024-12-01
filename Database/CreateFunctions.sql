Use ISS
go

CREATE OR ALTER FUNCTION getPlayerStatsGood(@playerId UNIQUEIDENTIFIER)
RETURNS @playerStats TABLE
(
    mostPlayedGame UNIQUEIDENTIFIER,
    averageElo INT,
    trophies INT
)
AS
BEGIN
--     (
--     SELECT p.playerId, SUM(gs.totalWins) as trophies, AVG(gs.eloRating) as overhaulElo
--     FROM Player p
--         INNER JOIN GameStats gs ON p.playerId = gs.playerId
--     WHERE p.playerId = @playerId
--     group by p.playerId)
--     SELECT (SELECT TOP 1 gameId FROM k ORDER BY totalMatches DESC) as mostPlayedGame,
--            AVG(k.eloRating) as averageElo, SUM(k.totalWins) as throphies
--     FROM (
--         SELECT gs.gameId as gameId, gs.totalMatches as totalMatches, gs.totalWins as totalWins, gs.eloRating as eloRating
--         FROM GameStats gs
--         WHERE gs.playerId = @playerId
--          ) k
--     GROUP BY
--     k.gameI
    DECLARE @temp TABLE
    (
        gameId UNIQUEIDENTIFIER,
        totalMatches INT,
        totalWins INT,
        eloRating INT
    )
    INSERT INTO @temp
    SELECT gs.gameId as gameId, gs.totalMatches as totalMatches, gs.totalWins as totalWins, gs.eloRating as eloRating
        FROM GameStats gs
        WHERE gs.playerId = @playerId
    DECLARE @mostPlayedGame UNIQUEIDENTIFIER
    SELECT TOP 1 @mostPlayedGame = gameId FROM @temp ORDER BY totalMatches DESC
    DECLARE @averageElo INT
    DECLARE @trophies INT
    SELECT @averageElo = AVG(eloRating), @trophies = SUM(totalWins) FROM @temp
    INSERT INTO @playerStats
    VALUES (@mostPlayedGame, @averageElo, @trophies)
    RETURN
END
GO


CREATE OR ALTER FUNCTION getActiveGame(@playerId UNIQUEIDENTIFIER)
RETURNS TABLE
AS
RETURN
    SELECT *
    FROM GameState gs
    WHERE (gs.player1Id = @playerId OR gs.player2Id = @playerId)
        AND gs.winnerPlayer IS NULL
GO

CREATE OR ALTER FUNCTION getGameHistory(@playerId UNIQUEIDENTIFIER)
RETURNS TABLE
AS
RETURN
    SELECT gs.player1Id as player1, gs.player2Id as player2, gs.gameId as game, gs.winnerPlayer as winner
    FROM GameState gs
    WHERE (gs.player1Id = @playerId OR gs.player2Id = @playerId)
        AND gs.winnerPlayer IS NOT NULL
GO

CREATE OR ALTER FUNCTION getAvailableChessGames(@playerId UNIQUEIDENTIFIER, @eloRating INT, @chessGameMode INT)
RETURNS TABLE
AS
RETURN
    SELECT pq.playerId as PlayerId, pq.gameId as GameId, pq.eloRating as Elo, pq.chessGameMode
    FROM PlayerQueue pq
        JOIN Game g ON pq.gameId = g.gameId
        WHERE g.gameName = 'Chess'
            AND ABS(pq.eloRating - @eloRating) < 200
            AND pq.chessGameMode = @chessGameMode
            AND pq.playerId != @playerId
GO

CREATE OR ALTER FUNCTION getAvailableDartsGames(@playerId UNIQUEIDENTIFIER, @eloRating INT)
RETURNS TABLE
AS
RETURN
    SELECT pq.playerId as PlayerId, pq.gameId as GameId, pq.eloRating as Elo
    FROM PlayerQueue pq
        JOIN Game g ON pq.gameId = g.gameId
        WHERE g.gameName = 'Darts'
            AND ABS(pq.eloRating - @eloRating) < 200
            AND pq.playerId != @playerId
GO

CREATE OR ALTER FUNCTION getAvailableConnect4Games(@playerId UNIQUEIDENTIFIER, @eloRating INT)
RETURNS TABLE
AS
RETURN
    SELECT pq.playerId as PlayerId, pq.gameId as GameId, pq.eloRating as Elo
    FROM PlayerQueue pq
        JOIN Game g ON pq.gameId = g.gameId
        WHERE g.gameName = 'Connect4'
            AND ABS(pq.eloRating - @eloRating) < 200
            AND pq.playerId != @playerId
GO

CREATE OR ALTER FUNCTION getAvailableObstructionGames(@playerId UNIQUEIDENTIFIER, @eloRating INT, @obstructionWidth INT, @obstructionHeight INT)
RETURNS TABLE
AS
RETURN
    SELECT pq.playerId as PlayerId, pq.gameId as GameId, pq.eloRating as Elo, pq.obstructionWidth as ObsWidth, pq.obstructionHeight as ObsHeight
    FROM PlayerQueue pq
        JOIN Game g ON pq.gameId = g.gameId
        WHERE g.gameName = 'Obstruction'
            AND ABS(pq.eloRating - @eloRating) < 200
            AND pq.obstructionWidth = @obstructionWidth
            AND pq.obstructionHeight = @obstructionHeight
            AND pq.playerId != @playerId
GO

CREATE OR ALTER FUNCTION getPlayer(@playerId UNIQUEIDENTIFIER)
RETURNS TABLE
AS
RETURN
    SELECT *
    FROM Player
    WHERE playerId = @playerId
GO


CREATE OR ALTER FUNCTION getPlayerQueue(@playerId UNIQUEIDENTIFIER)
RETURNS TABLE
AS
RETURN
    SELECT *
    FROM PlayerQueue
    WHERE playerId = @playerId
GO

CREATE OR ALTER FUNCTION getGameStats(@playerId UNIQUEIDENTIFIER, @gameId UNIQUEIDENTIFIER)
RETURNS TABLE
AS
RETURN
    SELECT *
    FROM GameStats
    WHERE playerId = @playerId AND gameId = @gameId
GO