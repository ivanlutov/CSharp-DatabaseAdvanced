namespace FastFood.DataProcessor.Dto.Export
{
    using System.Xml.Serialization;

    [XmlType("MostPopularItem")]
    public class MostPopularItemDtoXml
    {
        public string Name { get; set; }
        public decimal TotalMade { get; set; }
        public int TimesSold { get; set; }
    }
}