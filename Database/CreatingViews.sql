CREATE OR ALTER VIEW getTotalNumberOfGamesOfPlayer AS
SELECT p.playerid, p.playerName, COUNT(gs.gameStateId) AS totalGames
FROM Player p
LEFT JOIN GameState gs ON p.playerid = gs.player1Id OR p.playerid = gs.player2Id
GROUP BY p.playerid, p.playerName;
GO

----------------------------------------
CREATE OR ALTER VIEW getAllGames AS
SELECT* FROM Game
GO

CREATE OR ALTER VIEW getAllPlayers AS
SELECT* FROM Player
Go

CREATE OR ALTER VIEW getAllDartsGames AS
    SELECT *
    FROM GameState gs
    WHERE gameId = (SELECT g.gameid
                    FROM Game g
                    where g.gameName = 'Darts')
        AND gs.winnerPlayer IS NULL

GO

CREATE OR ALTER VIEW getAllChessGames AS
    SELECT *
    FROM GameState gs
    WHERE gameId = (SELECT g.gameid
                    FROM Game g
                    where g.gameName = 'Chess')
        AND gs.winnerPlayer IS NULL
GO

CREATE OR ALTER VIEW getAllConnect4Games AS
    SELECT *
    FROM GameState gs
    WHERE gameId = (SELECT g.gameid
                    FROM Game g
                    where g.gameName = 'Connect4')
        AND gs.winnerPlayer IS NULL
GO

CREATE OR ALTER VIEW getAllObstructionGames AS
    SELECT *
    FROM GameState gs
    WHERE gameId = (SELECT g.gameid
                    FROM Game g
                    where g.gameName = 'Obstruction')
        AND gs.winnerPlayer IS NULL
GO

CREATE OR ALTER VIEW getUnfinishedGames AS
    SELECT *
    FROM GameState gs
    WHERE winnerPlayer IS NULL
GO

CREATE OR ALTER VIEW get2PlayerGames AS
    SELECT *
    FROM Game g
    WHERE gameCategory = '2playerGame'
GO

CREATE OR ALTER VIEW getChessQueue AS
    SELECT *
    FROM PlayerQueue pq
    WHERE pq.gameId = (SELECT g.gameid
                    FROM Game g
                    where g.gameName = 'Chess')
GO

CREATE OR ALTER VIEW getDartsQueue AS
    SELECT *
    FROM PlayerQueue pq
    WHERE pq.gameId = (SELECT g.gameid
                    FROM Game g
                    where g.gameName = 'Darts')
GO

CREATE OR ALTER VIEW getConnect4Queue AS
    SELECT *
    FROM PlayerQueue pq
    WHERE pq.gameId = (SELECT g.gameid
                    FROM Game g
                    where g.gameName = 'Connect4')
GO

CREATE OR ALTER VIEW getObstructionQueue AS
    SELECT *
    FROM PlayerQueue pq
    WHERE pq.gameId = (SELECT g.gameid
                    FROM Game g
                    where g.gameName = 'Obstruction')
GO

CREATE OR ALTER VIEW getChessLeaderboard AS
    SELECT p.playerName, gs.eloRating
    FROM Player p
    JOIN GameStats gs ON p.playerid = gs.playerId
    WHERE gs.gameId = (SELECT g.gameid
                    FROM Game g
                    where g.gameName = 'Chess')
GO

CREATE OR ALTER VIEW getDartsLeaderboard AS
    SELECT p.playerName, gs.eloRating as eloRating
    FROM Player p
    JOIN GameStats gs ON p.playerid = gs.playerId
    WHERE gs.gameId = (SELECT g.gameid
                    FROM Game g
                    where g.gameName = 'Darts')
GO

CREATE OR ALTER VIEW getConnect4Leaderboard AS
    SELECT p.playerName, gs.eloRating as eloRating
    FROM Player p
    JOIN GameStats gs ON p.playerid = gs.playerId
    WHERE gs.gameId = (SELECT g.gameid
                    FROM Game g
                    where g.gameName = 'Connect4')
GO

CREATE OR ALTER VIEW getObstructionLeaderboard AS
    SELECT p.playerName, gs.eloRating as eloRating
    FROM Player p
    JOIN GameStats gs ON p.playerid = gs.playerId
    WHERE gs.gameId = (SELECT g.gameid
                    FROM Game g
                    where g.gameName = 'Obstruction')
GO








