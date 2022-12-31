using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Mapsharp.GeoJson.Core.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Mapsharp.GeoJson.Newtonsoft.Tests
{
    public class PointSerializerTests : SerializerTestBase
    {
        [Theory]
        [InlineData(22, 34)]
        [InlineData(0.122, 4.67)]
        [InlineData(22.34567, 0.0094)]
        public void CanDeserializePoint2D(double x, double y)
        {
            var pointJson =
@"{
    type: 'Point',
    coordinates: [#X#, #Y#]
}";
            pointJson = pointJson.Replace("#X#", x.ToString()).Replace("#Y#", y.ToString());
            Point? p = JsonConvert.DeserializeObject<Point>(pointJson, Settings);

            Assert.NotNull(p);
            Assert.Equal(x, p?.Coordinates.X);
            Assert.Equal(y, p?.Coordinates.Y);
        }

        [Theory]
        [InlineData(22, 34, 58)]
        [InlineData(0.122, 4.67, 2.4578)]
        [InlineData(22.34567, 0.0094, 100002)]
        public void CanDeserializePoint3D(double x, double y, double z)
        {
            var pointJson =
@"{
    type: 'Point',
    coordinates: [#X#, #Y#, #Z#]
}";
            pointJson = pointJson.Replace("#X#", x.ToString()).Replace("#Y#", y.ToString()).Replace("#Z#", z.ToString());
            Point? p = JsonConvert.DeserializeObject<Point>(pointJson, Settings);

            Assert.NotNull(p);
            Assert.Equal(x, p?.Coordinates.X);
            Assert.Equal(y, p?.Coordinates.Y);
            Assert.Equal(z, p?.Coordinates.Z);
        }

        [Theory]
        [InlineData(22, 34)]
        [InlineData(0.122, 4.67)]
        [InlineData(22.34567, 0.0094)]
        public void CanSerializePoint2D(double x, double y)
        {
            Point p = new Point(x, y);
            string json = JsonConvert.SerializeObject(p, Settings);
            Point? p2 = JsonConvert.DeserializeObject<Point>(json, Settings);

            Assert.NotNull(p2);
            Assert.Equal(p2?.Coordinates.X, p.Coordinates.X);
            Assert.Equal(p2?.Coordinates.Y, p.Coordinates.Y);
        }

        [Fact]
        public void PointDeserializingThrowsSerializationException()
        {
            string json = "{\"Coordinates\":[11.0,23.0],\"Type\":\"NotAPoint\"}";
            Assert.Throws<JsonSerializationException>(() =>
            {
                Point? p = JsonConvert.DeserializeObject<Point>(json, Settings);
            });
        }
    }
}
