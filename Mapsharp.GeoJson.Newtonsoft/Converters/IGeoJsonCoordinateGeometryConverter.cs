using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Mapsharp.GeoJson.Core.Geometries;

namespace Mapsharp.GeoJson.Newtonsoft.Converters
{
    public class IGeoJsonCoordinateGeometryConverter<T, K> : GeoJsonConverterBase<T>
        where T : class, IGeoJsonCoordinateGeometry<K>, new()
        where K : System.Collections.IEnumerable
    {
        private readonly bool _skipValidationOnSerializing;

        public IGeoJsonCoordinateGeometryConverter(bool skipValidationOnSerializing = false)
        {
            _skipValidationOnSerializing = skipValidationOnSerializing;
        }

        public override T? ReadJson(JsonReader reader, Type objectType, T? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            T t = new();

            if (reader.TokenType == JsonToken.StartObject)
            {
                try
                {
                    string typePropertyName = GetPropertyTokenName(nameof(t.Type), serializer);
                    string coordinatesPropertyName = GetPropertyTokenName(nameof(t.Coordinates), serializer);

                    JObject jo = JObject.Load(reader);

                    t.Type = ReadRequiredProperty<string>(serializer, jo, typePropertyName);
                    t.Coordinates = ReadRequiredProperty<K>(serializer, jo, coordinatesPropertyName);
                }
                catch (Exception ex)
                {
                    throw new JsonSerializationException(ex.Message, ex);
                }
            }

            if (!_skipValidationOnSerializing)
            {
                ThrowIfNotValid(t);
            }

            return t;
        }

        public override void WriteJson(JsonWriter writer, T? value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            writer.WriteStartObject();
            writer.WritePropertyName(GetPropertyTokenName(nameof(value.Type), serializer));
            serializer.Serialize(writer, value.Type);
            writer.WritePropertyName(GetPropertyTokenName(nameof(value.Coordinates), serializer));
            serializer.Serialize(writer, value.Coordinates);
            writer.WriteEndObject();
        }
    }

}
