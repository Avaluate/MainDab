﻿using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net;
using System.Threading.Tasks;


namespace MainDabRedo.ScriptHub
{
    // We will declare these 4 stuff first
    public struct GameScriptData
    {
        public string Title;
        public string Script;
        public string Desc;
        public string Credits;
        public string ImageURL;

    }
    public static class MainDabGSC
    {

        private static readonly WebClient Web = new WebClient();

        // We want to supress this warning just to keep the error list clean
        // should really be ran asynchronously

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public static async Task<GameScriptData[]> GetGSCData()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

        {
            // Now we parse the json, making use of Newtonsoft
            var json = Web.DownloadString("https://raw.githubusercontent.com/MainDabRblx/ProjectDab/master/UpdateStuff/GameHubScripts.json");
            var arrays = JArray.Parse(json);

            // Then we return each of it
            return arrays.Values<JObject>()
                .Select(wow => new GameScriptData
                {
                    Title = wow.GetValue("title").ToObject<string>(),
                    Credits = wow.GetValue("credits").ToObject<string>(),
                    Desc = wow.GetValue("desc").ToObject<string>(),
                    Script = wow.GetValue("script").ToObject<string>(),
                    ImageURL = wow.GetValue("imgurl").ToObject<string>()
                }).ToArray();
        }
    }
}
