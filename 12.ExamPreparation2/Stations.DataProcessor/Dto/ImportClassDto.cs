namespace Stations.DataProcessor.Dto
{
    using System.ComponentModel.DataAnnotations;

    public class ImportClassDto
    {

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        [StringLength(2, MinimumLength = 2)]
        public string Abbreviation { get; set; }
    }
}