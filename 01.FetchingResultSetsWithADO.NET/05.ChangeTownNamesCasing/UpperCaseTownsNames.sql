USE MinionsDB

UPDATE Towns
   SET TownName = UPPER(TownName)
 WHERE CountryId = (SELECT Id FROM Countries
					 WHERE CountryName = @countryName)