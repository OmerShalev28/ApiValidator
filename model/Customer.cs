using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiValidator.model
{
    using System.Text.Json.Serialization;

    public class Customer
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("cityName")]
        public string City { get; set; }

        [JsonPropertyName("mainPhone")]
        public string Phone { get; set; }

        [JsonConstructor]
        public Customer(string name, string address, string city, string phone)
        {
            Name = name;
            Address = address;
            City = city;
            Phone = phone;
        }
    }

}
