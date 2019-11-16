﻿using System.IO;
using Newtonsoft.Json;

namespace Systems
{
    public class Deserializer
    {
        public static T GetComponent<T>(string pathToJson)
        {
            try
            {
                using (var file = File.OpenText(pathToJson))
                {
                    var serializer = new JsonSerializer();
                    var component = (T) serializer.Deserialize(file, typeof(T));
                    return component;
                }
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