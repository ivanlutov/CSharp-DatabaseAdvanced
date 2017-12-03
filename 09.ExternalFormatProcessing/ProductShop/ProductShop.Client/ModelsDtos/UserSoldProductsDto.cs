namespace ProductShop.Client.ModelsDtos
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    public class UserSoldProductsDto
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("soldProducts")]
        public ICollection<SoldProductDto> SoldProducts { get; set; } = new HashSet<SoldProductDto>();
    }
}