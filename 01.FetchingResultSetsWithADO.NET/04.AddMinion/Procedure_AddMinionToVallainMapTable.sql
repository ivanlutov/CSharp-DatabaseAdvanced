CREATE PROCEDURE p_AddMinionToVallainMapTable
(@minionName VARCHAR(50), @minionAge INT, @minionTown VARCHAR(50), @villainName VARCHAR(50))
AS 
BEGIN
	BEGIN TRANSACTION

	DECLARE @TownId INT = (SELECT Id FROM Towns
						   WHERE TownName = @minionTown)
						   
	DECLARE @IsContainsMinion INT = (SELECT COUNT(*) FROM Minions
								  WHERE [Name] = @minionName AND
										Age = @minionAge 
										AND TownId = @TownId)

	IF(@IsContainsMinion = 0)
	BEGIN
		INSERT INTO Minions([Name],Age,TownId)
		VALUES
		(@minionName, @minionAge, @TownId)
	END
	
	DECLARE @VillainId INT = (SELECT TOP 1 Id FROM Villains
							  WHERE [Name] = @villainName)

	DECLARE @MinionId INT = (SELECT TOP 1 Id FROM Minions
							  WHERE [Name] = @minionName)
	
	DECLARE @IsContainsMapTableIds INT = (SELECT COUNT(*) FROM VillainsMinions
										WHERE VillainId = @VillainId
										AND MinionId = @MinionId)

	IF(@IsContainsMapTableIds !=0)
	BEGIN
		ROLLBACK
		RETURN
	END

	INSERT INTO VillainsMinions (VillainId, MinionId)
	VALUES (@VillainId, @MinionId)

	COMMIT
END