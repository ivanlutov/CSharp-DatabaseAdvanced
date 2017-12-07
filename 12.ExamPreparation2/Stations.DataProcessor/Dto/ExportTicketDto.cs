namespace Stations.DataProcessor.Dto
{
    using System.Xml.Serialization;

    [XmlType("Ticket")]
    public class ExportTicketDto
    {
        [XmlElement("OriginStation")]
        public string OriginStation { get; set; }

        [XmlElement("DestinationStation")]
        public string DestinationStation { get; set; }

        [XmlElement("DepartureTime")]
        public string DepartureTime { get; set; }
    }
}