namespace FastFood.DataProcessor.Dto.Import
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Order")]
    public class ImportOrderXmlDto
    {
        [Required]
        public string Customer { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string Employee { get; set; }

        [Required]
        public string DateTime { get; set; }

        [Required]
        public string Type { get; set; }
        
        public ImportItemXmlDto[] Items { get; set; }
    }
}