using Newtonsoft.Json;
using System.Collections.Generic;

namespace JARVIS.SDK.MovieDatabase.Models
{
    public class CertificationsResponse : Response
    {
        [JsonProperty("certifications")]
        public IDictionary<string, IEnumerable<Certification>> Certifications { get; set; }
    }
}
