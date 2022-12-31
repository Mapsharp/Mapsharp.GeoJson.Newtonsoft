using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Mapsharp.GeoJson.Core.Geometries;

namespace Mapsharp.GeoJson.Newtonsoft.Converters
{
    public class GeometryCollectionConverter : GeoJsonConverterBase<GeometryCollection>
    {
        private readonly bool _skipValidationOnSerializing;

        public GeometryCollectionConverter(bool skipValidationOnSerializing = false)
        {
            _skipValidationOnSerializing = skipValidationOnSerializing;
        }

        public override GeometryCollection? ReadJson(JsonReader reader, Type objectType, GeometryCollection? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            GeometryCollection geometryCollection = new();

            if (reader.TokenType == JsonToken.StartObject)
            {
                try
                {
                    string typePropertyName = GetPropertyTokenName(nameof(geometryCollection.Type), serializer);
                    string coordinatesPropertyName = GetPropertyTokenName(nameof(geometryCollection.Geometries), serializer);

                    JObject jo = JObject.Load(reader);
                    geometryCollection.Type = ReadRequiredProperty<string>(serializer, jo, typePropertyName);
                    AssignGeometriesProperty(geometryCollection, serializer, jo);
                }
                catch (Exception ex)
                {
                    throw new JsonSerializationException(ex.Message, ex);
                }
            }

            if (!_skipValidationOnSerializing)
            {
                ThrowIfNotValid(geometryCollection);
            }

            return geometryCollection;
        }

        public override void WriteJson(JsonWriter writer, GeometryCollection? value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            writer.WriteStartObject();
            writer.WritePropertyName(GetPropertyTokenName(nameof(value.Type), serializer));
            serializer.Serialize(writer, value.Type);
            writer.WritePropertyName(GetPropertyTokenName(nameof(value.Geometries), serializer));
            serializer.Serialize(writer, value.Geometries);
            writer.WriteEndObject();
        }

        private void AssignGeometriesProperty(GeometryCollection geometryCollection, JsonSerializer serializer, JObject jo)
        {
            string geometriesProprtyTokenName = GetPropertyTokenName(nameof(geometryCollection.Geometries), serializer);
            string typePropertyName = GetPropertyTokenName(nameof(geometryCollection.Type), serializer);

            JToken geometriesPropertyValue = GetGeometriesPropertyValue(jo, geometriesProprtyTokenName);

            var geometries = new List<GeometryBase>();
            foreach (JToken token in geometriesPropertyValue.ToArray())
            {
                if (token.Type != JTokenType.Object)
                {
                    throw new FormatException($"The token {token} could not parsed as an object.");
                }

                string type = ReadRequiredProperty<string>(serializer, (JObject)token, typePropertyName);

                if (!Enum.TryParse(type, false, out GeometryType geometryType))
                {
                    throw new FormatException($"The required property {typePropertyName} is invalid.");
                }

                geometries.Add(DeserializeGeometryBase(token, geometryType, serializer));
            }

            geometryCollection.Geometries = geometries;
        }

        private JToken GetGeometriesPropertyValue(JObject jo, string geometriesProprtyTokenName)
        {
            var geometriesProperty = jo.Property(geometriesProprtyTokenName);

            if (geometriesProperty == null)
            {
                throw new FormatException($"The required property {geometriesProprtyTokenName} could not be found.");
            }

            if (geometriesProperty.Value.Type != JTokenType.Array)
            {
                throw new FormatException($"The required property {geometriesProprtyTokenName} could not be parsed.");
            }

            return geometriesProperty.Value;
        }

        private GeometryBase DeserializeGeometryBase(JToken token, GeometryType geometryType, JsonSerializer serializer)
        {
            GeometryBase? geometryBase;
            switch (geometryType)
            {
                case GeometryType.Point:
                    geometryBase = token.ToObject<Point>(serializer);
                    break;
                case GeometryType.MultiPoint:
                    geometryBase = token.ToObject<MultiPoint>(serializer);
                    break;
                case GeometryType.LineString:
                    geometryBase = token.ToObject<LineString>(serializer);
                    break;
                case GeometryType.Polygon:
                    geometryBase = token.ToObject<Polygon>(serializer);
                    break;
                case GeometryType.MultiPolygon:
                    geometryBase = token.ToObject<MultiPolygon>(serializer);
                    break;
                case GeometryType.GeometryCollection:
                    geometryBase = token.ToObject<GeometryCollection>(serializer);
                    break;
                default:
                    throw new FormatException($"Unsupported geometry type {geometryType}");
            }

            if (geometryBase == null)
                throw new FormatException($"could not parse token to a valid GeoJson Geometry.");

            return geometryBase;
        }
    }

}
