SELECT TownName
  FROM Towns
 WHERE CountryId = (SELECT Id FROM Countries
					 WHERE CountryName = @countryName)