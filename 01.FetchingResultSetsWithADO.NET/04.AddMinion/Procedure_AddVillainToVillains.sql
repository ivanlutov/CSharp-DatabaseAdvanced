CREATE PROCEDURE p_AddVillainToVillains (@villainName VARCHAR(50))
AS 
BEGIN
BEGIN TRANSACTION
	DECLARE @IsContainsVillain INT = (SELECT COUNT(*) FROM Villains
								  WHERE [Name] = @villainName)
	IF(@IsContainsVillain != 0)
	BEGIN
		ROLLBACK
		RETURN
	END

	INSERT INTO Villains([Name])
	VALUES (@villainName)
	
	COMMIT
END