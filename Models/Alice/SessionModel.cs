﻿using Newtonsoft.Json;

namespace HatForAlice.Alice
{
    public class SessionModel
    {
        [JsonProperty("new")] public bool New { get; set; }

        [JsonProperty("session_id")] public string SessionId { get; set; }

        [JsonProperty("message_id")] public int MessageId { get; set; }

        [JsonProperty("skill_id")] public string SkillId { get; set; }

        [JsonProperty("user_id")] public string UserId { get; set; }
    }
}