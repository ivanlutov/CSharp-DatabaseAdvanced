USE MinionsDB

    SELECT v.[Name], COUNT(MinionId) AS MinionsCount 
      FROM Villains AS v
INNER JOIN VillainsMinions AS vm ON vm.VillainId = v.Id
  GROUP BY v.[Name]
    HAVING COUNT(MinionId) > 3
  ORDER BY MinionsCount DESC