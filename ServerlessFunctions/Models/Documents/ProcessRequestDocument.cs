using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ServerlessFunctions.Models.Documents
{
    public class ProcessRequestDocument
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        public DateTime Date { get; set; }

        public string UserId { get; set; }
    }
}
