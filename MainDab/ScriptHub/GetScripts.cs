using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MainDabRedo.ScriptHub
{
    // We will declare these 4 stuff first
    public struct ScriptData
    {
        public string Title;
        public string Script;
        public string Desc;
        public string Credits;
        public string ImageURL;

    }
    public static class MainDabSC
    {

        private static readonly WebClient Web = new WebClient();

        // We want to supress this warning just to keep the error list clean
        // should really be ran asynchronously

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public static async Task<ScriptData[]> GetSCData()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        
        {
            // Now we parse the json, making use of Newtonsoft
            var json = Web.DownloadString("https://raw.githubusercontent.com/Avaluate/MainDabWeb/master/UpdateStuff/Scripts.json");
            var arrays = JArray.Parse(json);

            // Then we return each of it
            return arrays.Values<JObject>()
                .Select(wow => new ScriptData
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
