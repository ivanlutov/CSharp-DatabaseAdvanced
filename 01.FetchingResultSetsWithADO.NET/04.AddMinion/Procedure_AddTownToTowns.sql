CREATE PROCEDURE p_AddTownToTowns (@townName VARCHAR(50))
AS 
BEGIN
BEGIN TRANSACTION
	DECLARE @IsContainsTown INT = (SELECT COUNT(*) FROM Towns
								  WHERE TownName = @townName)
	IF(@IsContainsTown != 0)
	BEGIN
		ROLLBACK
		RETURN
	END

	INSERT INTO Towns(TownName)
	VALUES (@townName)
	
	COMMIT
END