using Newtonsoft.Json;
using System.Collections.Generic;

namespace JARVIS.SDK.MovieDatabase.Models
{
    public class Configuration : Response
    {
        [JsonProperty("images")]
        public ConfigurationImages Images { get; set; }

        [JsonProperty("change_keys")]
        public IEnumerable<string> ChangeKeys { get; set; }
    }
}
