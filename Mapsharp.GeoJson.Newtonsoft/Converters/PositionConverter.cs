using Newtonsoft.Json;
using Mapsharp.GeoJson.Core.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Mapsharp.GeoJson.Newtonsoft.Converters
{
    internal class PositionConverter : JsonConverter<Position>
    {
        public override Position ReadJson(JsonReader reader, Type objectType, Position existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            IEnumerable<double>? coordinates = serializer.Deserialize<IEnumerable<double>>(reader);
            if (coordinates == null) throw new JsonSerializationException($"{reader.Path} could not be parsed to a double[]");

            try
            {
                return Position.FromEnumerable(coordinates);
            }
            catch (Exception e)
            {
                throw new JsonSerializationException(e.Message);
            }
        }

        public override void WriteJson(JsonWriter writer, Position value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.ToArray());
        }
    }

}
