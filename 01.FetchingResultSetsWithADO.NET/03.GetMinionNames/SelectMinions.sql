USE MinionsDB

    SELECT m.Name, m.Age 
	  FROM Minions AS m
INNER JOIN VillainsMinions AS vm 
        ON vm.MinionId = m.Id
     WHERE vm.VillainId = @villainId