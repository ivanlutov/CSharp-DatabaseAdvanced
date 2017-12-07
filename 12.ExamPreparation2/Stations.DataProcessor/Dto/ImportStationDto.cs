namespace Stations.DataProcessor.Dto
{
    using System.ComponentModel.DataAnnotations;

    public class ImportStationDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Town { get; set; }
    }
}