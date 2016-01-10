using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TheWorld.Configs;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace TheWorld.Services
{
    public class CoordService
    {
        private ILogger<CoordService> logger;
        private AppSettings options;

        public CoordService(ILogger<CoordService> logger, IOptions<AppSettings> optionsAccessor)
        {
            this.logger = logger;
            this.options = optionsAccessor.Value;
        }

        public async Task<CoordServiceResult> Lookup(string location)
        {
            var encodedName = WebUtility.UrlEncode(location);
            var bingKey = options.BingKey;
            var url = $"http://dev.virtualearth.net/REST/v1/Locations?q={encodedName}&key={bingKey}";

            var client = new HttpClient();
            var json = await client.GetStringAsync(url);
            return parseResponse(json, location);
        }

        /// <summary>
        /// Parse the Bing response, courtesy of the Pluralsight instructor
        /// </summary>
        /// <param name="json"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        private CoordServiceResult parseResponse(string json, string location)
        {
            var result = new CoordServiceResult()
            {
                Success = false,
                Message = "Undetermined failure because reasons"
            };
            var results = JObject.Parse(json);

            var resources = results["resourceSets"][0]["resources"];
            if (!resources.HasValues)
            {
                result.Message = $"Could not find '{location}' as a location";
                return result;
            }

            var confidence = (string)resources[0]["confidence"];
            if (confidence != "High")
            {
                result.Message = $"Could not find a confident match for '{location}' as a location";
                return result;
            }
            
            var coords = resources[0]["geocodePoints"][0]["coordinates"];
            result.Latitude = (double)coords[0];
            result.Longitude = (double)coords[1];
            result.Success = true;
            result.Message = "Success";

            return result;
        }
    }
}
