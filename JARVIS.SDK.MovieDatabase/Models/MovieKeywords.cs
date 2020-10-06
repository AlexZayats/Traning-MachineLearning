using Newtonsoft.Json;
using System.Collections.Generic;

namespace JARVIS.SDK.MovieDatabase.Models
{
    public class MovieKeywords
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("keywords")]
        public IEnumerable<BaseEntity> Keywords { get; set; }
    }
}
