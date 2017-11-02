CREATE PROC [dbo].[usp_GetOlder] (@minionId INT)
AS
BEGIN
	BEGIN TRANSACTION

	DECLARE @IsMinionExist INT = (SELECT COUNT(*) FROM Minions
								  WHERE Id = @minionId)

    IF(@IsMinionExist = 0)
	BEGIN
	   ROLLBACK
	   RETURN
	END

	UPDATE Minions
	   SET Age += 1
	 WHERE Id = @minionId

	 COMMIT
END