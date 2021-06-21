using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace StudentMentor.Domain.Models.ViewModels
{
    public class PushModel
    {
        [JsonProperty("ref")]
        public string Ref { get; set; }

        [JsonProperty("commits")]
        public ICollection<Commit> Commits { get; set; }

        [JsonProperty("repository")]
        public Repository Repository { get; set; }
    }

    public class Commit
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("timestamp")]
        public DateTime TimeStamp { get; set; }
        
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("added")]
        public List<string> Added { get; set; }
        [JsonProperty("modified")]
        public List<string> Modified { get; set; }
        [JsonProperty("removed")]
        public List<string> Removed { get; set; }
    }


    public class Repository
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }

}
