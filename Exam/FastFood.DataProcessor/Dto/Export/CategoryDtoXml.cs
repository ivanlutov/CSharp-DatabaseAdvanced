namespace FastFood.DataProcessor.Dto.Export
{
    using System.Xml.Serialization;

    [XmlType("Category")]
    public class CategoryDtoXml
    {
        public string Name { get; set; }

        public MostPopularItemDtoXml MostPopularItem { get; set; }
    }
}