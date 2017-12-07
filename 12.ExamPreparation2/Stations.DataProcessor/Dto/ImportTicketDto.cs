namespace Stations.DataProcessor.Dto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Ticket")]
    public class ImportTicketDto
    {
        [Required]
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        [XmlAttribute("price")]
        public decimal Price { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]{2}[0-9]{1,6}$")]
        [XmlAttribute("seat")]
        public string Seat { get; set; }

        [XmlElement("Trip")]
        [Required]
        public TripDtoXml Trip { get; set; }

        [XmlElement("Card")]
        public CardDtoXml Card  { get; set; }

    }
}