using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapsharp.GeoJson.Newtonsoft.Tests
{
    public abstract class SerializerTestBase
    {
        protected JsonSerializerSettings Settings => CreateGeoJsonConverterSettings();
        private static JsonSerializerSettings CreateGeoJsonConverterSettings()
        {
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            settings.AddGeoJsonConverters();
            return settings;
        }
    }
}
