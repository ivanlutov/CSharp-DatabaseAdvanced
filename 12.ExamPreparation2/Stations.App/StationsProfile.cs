namespace Stations.App
{
    using AutoMapper;
    using Stations.DataProcessor.Dto;
    using Stations.Models;
    public class StationsProfile : Profile
	{
		// Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
		public StationsProfile()
		{
		    CreateMap<ImportStationDto, Station>();
		    CreateMap<ImportClassDto, SeatingClass>();
		    CreateMap<ImportSeatDto, TrainSeat>();


        }
    }
}
