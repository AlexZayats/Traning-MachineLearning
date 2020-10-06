using Newtonsoft.Json;
using System.Collections.Generic;

namespace JARVIS.SDK.MovieDatabase.Models
{
    public class GenresResponse : Response
    {
        [JsonProperty("genres")]
        public IEnumerable<BaseEntity> Genres { get; set; }
    }
}
