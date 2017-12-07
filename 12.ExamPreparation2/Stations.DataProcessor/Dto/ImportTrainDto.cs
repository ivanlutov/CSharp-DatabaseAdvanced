namespace Stations.DataProcessor.Dto
{
    using System.ComponentModel.DataAnnotations;

    public class ImportTrainDto
    {
        [Required]
        [MaxLength(10)]
        public string TrainNumber { get; set; }

        public string Type { get; set; } = "HighSpeed";

        public ImportSeatDto[] Seats { get; set; } = new ImportSeatDto[0];
    }
}