using System.IO;
using Newtonsoft.Json;

namespace Systems
{
    public class Deserializer
    {
        public static T GetComponent<T>(string pathToJson)
        {
            try
            {
                var file = File.ReadAllText(pathToJson);
                return JsonConvert.DeserializeObject<T>(file);
            }
            catch (JsonException e)
            {
                throw new JsonSerializationException(
                    "Check the existence of file and correctness of json data.",
                    e);
            }
        }
    }
}