using Newtonsoft.Json;
using System.Collections.Generic;

namespace JARVIS.SDK.MovieDatabase.Models
{
    public class TVShowKeywords
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("results")]
        public IEnumerable<BaseEntity> Keywords { get; set; }
    }
}
