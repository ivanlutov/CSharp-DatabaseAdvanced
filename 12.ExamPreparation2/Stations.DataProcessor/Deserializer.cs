namespace Stations.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using Stations.Data;
    using Stations.DataProcessor.Dto;
    using Stations.Models;
    using Stations.Models.Enums;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;
    public static class Deserializer
	{
		private const string FailureMessage = "Invalid data format.";
		private const string SuccessMessage = "Record {0} successfully imported.";

		public static string ImportStations(StationsDbContext context, string jsonString)
		{
		    var sb = new StringBuilder();
		    var deserializeDtos = JsonConvert.DeserializeObject<ImportStationDto[]>(jsonString);

            var validStations = new List<Station>();

		    foreach (var stationDto in deserializeDtos)
		    {
		        if (!IsValid(stationDto))
		        {
		            sb.AppendLine(FailureMessage);
                    continue;
		        }

		        if (stationDto.Town == null)
		        {
		            stationDto.Town = stationDto.Name;
		        }

		        var isAlreadyExistStation = validStations.Any(s => s.Name == stationDto.Name);
		        if (isAlreadyExistStation)
		        {
		            sb.AppendLine(FailureMessage);
		            continue;
                }

                var station = Mapper.Map<Station>(stationDto);

                validStations.Add(station);
		        sb.AppendLine(string.Format(SuccessMessage, station.Name));

		    }

            context.Stations.AddRange(validStations);
		    context.SaveChanges();

		    var result = sb.ToString();
		    return result;
		}

		public static string ImportClasses(StationsDbContext context, string jsonString)
		{
		    var sb = new StringBuilder();
		    var deserializeClasses = JsonConvert.DeserializeObject<ImportClassDto[]>(jsonString);

            var validClasses = new List<SeatingClass>();

		    foreach (var classDto in deserializeClasses)
		    {
		        if (!IsValid(classDto))
		        {
		            sb.AppendLine(FailureMessage);
                    continue;;
		        }

		        var isAlreadyExistSeatClass =
		            validClasses.Any(c => c.Name == classDto.Name || c.Abbreviation == classDto.Abbreviation);

		        if (isAlreadyExistSeatClass)
		        {
		            sb.AppendLine(FailureMessage);
		            continue; ;
                }

		        var validClass = Mapper.Map<SeatingClass>(classDto);

                validClasses.Add(validClass);
		        sb.AppendLine(string.Format(SuccessMessage, classDto.Name));
		    }

            context.SeatingClasses.AddRange(validClasses);
		    context.SaveChanges();


		    var result = sb.ToString();
		    return result;
        }

		public static string ImportTrains(StationsDbContext context, string jsonString)
		{
		    var sb = new StringBuilder();
		    var deserializeTrains = JsonConvert.DeserializeObject<ImportTrainDto[]>(jsonString, new JsonSerializerSettings()
		    {
		        NullValueHandling = NullValueHandling.Ignore
		    });

            var validTrains = new List<Train>();

		    foreach (var trainDto in deserializeTrains)
		    {
		        if (!IsValid(trainDto))
		        {
		            sb.AppendLine(FailureMessage);
                    continue;
		        }

		        var seatsDtoAreValid = trainDto.Seats.All(IsValid);
		        if (!seatsDtoAreValid)
		        {
		            sb.AppendLine(FailureMessage);
		            continue;
                }

		        var trainAlreadyExists = validTrains.Any(t => t.TrainNumber == trainDto.TrainNumber);
		        if (trainAlreadyExists)
		        {
		            sb.AppendLine(FailureMessage);
		            continue;
		        }

                var seatingClassesAreValid = trainDto.Seats
		            .All(s => context.SeatingClasses.Any(sc => sc.Name == s.Name && sc.Abbreviation == s.Abbreviation));
                if (!seatingClassesAreValid)
		        {
		            sb.AppendLine(FailureMessage);
		            continue;
                }

		        var type = Enum.Parse<TrainType>(trainDto.Type);


		        var trainSeats = trainDto.Seats.Select(s => new TrainSeat
		            {
		                SeatingClass =
		                    context.SeatingClasses.SingleOrDefault(sc => sc.Name == s.Name && sc.Abbreviation == s.Abbreviation),
		                Quantity = s.Quantity.Value
		            })
		            .ToArray();


		        var train = new Train
		        {
		            TrainNumber = trainDto.TrainNumber,
		            Type = type,
		            TrainSeats = trainSeats
		        };

		        validTrains.Add(train);

		        sb.AppendLine(string.Format(SuccessMessage, trainDto.TrainNumber));
            }


            context.Trains.AddRange(validTrains);
		    context.SaveChanges();

		    var result = sb.ToString();
		    return result;
        }

		public static string ImportTrips(StationsDbContext context, string jsonString)
		{
		    var sb = new StringBuilder();
		    var deserializedTrips = JsonConvert.DeserializeObject<ImportTripDto[]>(jsonString);
            var validTrips = new List<Trip>();


		    foreach (var tripDto in deserializedTrips)
		    {
		        if (!IsValid(tripDto))
		        {
		            sb.AppendLine(FailureMessage);
		            continue;
                }

		        var train = context.Trains.SingleOrDefault(t => t.TrainNumber == tripDto.Train);
		        var originStation = context.Stations.SingleOrDefault(s => s.Name == tripDto.OriginStation);
                var destinationStation = context.Stations.SingleOrDefault(s => s.Name == tripDto.DestinationStation);

		        if (train == null || originStation == null || destinationStation == null)
		        {
		            sb.AppendLine(FailureMessage);
		            continue;
                }

		        var departureTime =
		            DateTime.ParseExact(tripDto.DepartureTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
		        var arrivalTime =  
                    DateTime.ParseExact(tripDto.ArrivalTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

		        if (departureTime > arrivalTime)
		        {
		            sb.AppendLine(FailureMessage);
		            continue;
                }

		        TripStatus status = TripStatus.OnTime;
		        if (tripDto.Status != null)
		        {
		            status = Enum.Parse<TripStatus>(tripDto.Status);
		        }

		        TimeSpan timeDifference;
		        if (tripDto.TimeDifference != null)
		        {
		            timeDifference = TimeSpan.ParseExact(tripDto.TimeDifference, "hh\\:mm", CultureInfo.InvariantCulture);
		        }

		        var trip = new Trip
		        {
                    Train = train,
                    DestinationStation = destinationStation,
                    OriginStation = originStation,
                    Status = status,
                    TimeDifference = timeDifference,
                    ArrivalTime = arrivalTime,
                    DepartureTime = departureTime
		        };

                validTrips.Add(trip);
		        sb.AppendLine($"Trip from {trip.OriginStation.Name} to {trip.DestinationStation.Name} imported.");
		    }

            context.Trips.AddRange(validTrips);
		    context.SaveChanges();

		    var result = sb.ToString();
		    return result;
        }

		public static string ImportCards(StationsDbContext context, string xmlString)
		{
		    var sb = new StringBuilder();

		    var serializer = new XmlSerializer(typeof(ImportCardDto[]), new XmlRootAttribute("Cards"));
		    var deserializedCards = (ImportCardDto[])serializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(xmlString)));
            var validCards = new List<CustomerCard>();

		    foreach (var cardDto in deserializedCards)
		    {
		        if (!IsValid(cardDto))
		        {
		            sb.AppendLine(FailureMessage);
                    continue;
		        }

		        CardType cardType = CardType.Normal;
		        if (cardDto.CardType != null)
		        {
		            cardType = Enum.Parse<CardType>(cardDto.CardType);
		        }

                var card = new CustomerCard()
                {
                   Name = cardDto.Name,
                   Age = cardDto.Age,
                   Type = cardType
                };

                validCards.Add(card);
		        sb.AppendLine(string.Format(SuccessMessage, card.Name));
		    }

            context.Cards.AddRange(validCards);
		    context.SaveChanges();

            var result = sb.ToString();
		    return result;
        }

		public static string ImportTickets(StationsDbContext context, string xmlString)
		{
		    var sb = new StringBuilder();
		    var serializer = new XmlSerializer(typeof(ImportTicketDto[]), new XmlRootAttribute("Tickets"));
		    var deserializedTickets = (ImportTicketDto[])serializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(xmlString)));
		    var validTickets = new List<Ticket>();

		    foreach (var ticketDto in deserializedTickets)
		    {
		        if (!IsValid(ticketDto))
		        {
		            sb.AppendLine(FailureMessage);
                    continue;
		        }

		        var tripDto = ticketDto.Trip;
		        if (!IsValid(tripDto))
		        {
		            sb.AppendLine(FailureMessage);
		            continue;
                }

		        var departureTime = DateTime.ParseExact(ticketDto.Trip.DepartureTime, "dd/MM/yyyy HH:mm",
		            CultureInfo.InvariantCulture);

		        var trip = context.Trips.SingleOrDefault(t => t.DepartureTime == departureTime
		                                                      && t.OriginStation.Name == tripDto.OriginStation
		                                                      && t.DestinationStation.Name == tripDto.DestinationStation);

		        if (trip == null)
		        {
		            sb.AppendLine(FailureMessage);
		            continue;
                }

		        CustomerCard card = null;
		        if (ticketDto.Card != null)
		        {
		            if (!IsValid(ticketDto.Card))
		            {
		                sb.AppendLine(FailureMessage);
		                continue;
                    }

		            card = context.Cards.SingleOrDefault(c => c.Name == ticketDto.Card.Name);
		            if (card == null)
		            {
		                sb.AppendLine(FailureMessage);
		                continue;
                    }
		        }

		        var seatingClassAbbreviation = ticketDto.Seat.Substring(0, 2);
		        var quantity = int.Parse(ticketDto.Seat.Substring(2));

		        var seatExists = trip.Train.TrainSeats
		            .SingleOrDefault(s => s.SeatingClass.Abbreviation == seatingClassAbbreviation && quantity <= s.Quantity);
		        if (seatExists == null)
		        {
		            sb.AppendLine(FailureMessage);
		            continue;
		        }

                var ticket = new Ticket()
		        {
		            Trip = trip,
		            CustomerCard = card,
		            Price = ticketDto.Price,
		            SeatingPlace = ticketDto.Seat
		        };

		        validTickets.Add(ticket);
		        sb.AppendLine(
		            $"Ticket from {ticket.Trip.OriginStation.Name} to {ticket.Trip.DestinationStation.Name} departing at {ticket.Trip.DepartureTime.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)} imported.");
            }

            context.Tickets.AddRange(validTickets);
		    context.SaveChanges();

            var result = sb.ToString();
		    return result;
        }

	    private static bool IsValid(object obj)
	    {
	        var validationContext = new ValidationContext(obj);
	        var validationResults = new List<ValidationResult>();

	        var isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);
	        return isValid;
	    }
    }
}