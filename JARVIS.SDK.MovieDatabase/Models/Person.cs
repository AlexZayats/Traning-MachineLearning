using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace JARVIS.SDK.MovieDatabase.Models
{
    public class Person
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("biography")]
        public string Biography { get; set; }

        [JsonProperty("place_of_birth")]
        public string PlaceOfBirth { get; set; }

        [JsonProperty("homepage")]
        public string HomePage { get; set; }

        [JsonProperty("profile_path")]
        public string ProfilePath { get; set; }

        [JsonProperty("gender")]
        public int Gender { get; set; }

        [JsonProperty("imdb_id")]
        public string IMDBId { get; set; }

        [JsonProperty("adult")]
        public bool Adult { get; set; }

        [JsonProperty("popularity")]
        public double Popularity { get; set; }

        [JsonProperty("birthday")]
        public DateTime Birthday { get; set; }

        [JsonProperty("deathday")]
        public DateTime? Deathday { get; set; }

        [JsonProperty("also_known_as")]
        public IEnumerable<string> AlsoKnownAs { get; set; }
    }
}
