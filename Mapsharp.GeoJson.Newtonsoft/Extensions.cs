using Newtonsoft.Json;
using Mapsharp.GeoJson.Core.Geometries;
using Mapsharp.GeoJson.Newtonsoft.Converters;

namespace Mapsharp.GeoJson.Newtonsoft
{
    public static class Extensions
    {
        public static JsonSerializerSettings AddGeoJsonConverters(this JsonSerializerSettings settings, bool ignoreInvalidCoordinateProperties = false)
        {
            settings.Converters.Add(new PositionConverter());
            settings.Converters.Add(new IGeoJsonCoordinateGeometryConverter<Point, Position>(ignoreInvalidCoordinateProperties));
            settings.Converters.Add(new IGeoJsonCoordinateGeometryConverter<MultiPoint, IEnumerable<Position>>(ignoreInvalidCoordinateProperties));
            settings.Converters.Add(new IGeoJsonCoordinateGeometryConverter<LineString, IEnumerable<Position>>(ignoreInvalidCoordinateProperties));
            settings.Converters.Add(new IGeoJsonCoordinateGeometryConverter<MultiLineString, IEnumerable<IEnumerable<Position>>>(ignoreInvalidCoordinateProperties));
            settings.Converters.Add(new IGeoJsonCoordinateGeometryConverter<Polygon, IEnumerable<IEnumerable<Position>>>(ignoreInvalidCoordinateProperties));
            settings.Converters.Add(new IGeoJsonCoordinateGeometryConverter<MultiPolygon, IEnumerable<IEnumerable<IEnumerable<Position>>>>(ignoreInvalidCoordinateProperties));
            settings.Converters.Add(new GeometryCollectionConverter(ignoreInvalidCoordinateProperties));
            return settings;
        }
    }
}