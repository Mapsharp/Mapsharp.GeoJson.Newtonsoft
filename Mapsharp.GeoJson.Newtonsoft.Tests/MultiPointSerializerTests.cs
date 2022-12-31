using Newtonsoft.Json;
using Mapsharp.GeoJson.Core.Geometries;

namespace Mapsharp.GeoJson.Newtonsoft.Tests
{
    public class MultiPointSerializerTests : SerializerTestBase
    {
        [Theory]
        [InlineData(22, 34)]
        [InlineData(0.122, 4.67)]
        [InlineData(22.34567, 0.0094)]
        public void CanDeserializePoint2D(double x, double y)
        {
            var pointJson =
@"{
    type: 'MultiPoint',
    coordinates: [[#X#, #Y#], [#X#, #Y#], [#X#, #Y#]]
}";
            pointJson = pointJson.Replace("#X#", x.ToString()).Replace("#Y#", y.ToString());
            MultiPoint? p = JsonConvert.DeserializeObject<MultiPoint>(pointJson, Settings);

            Assert.NotNull(p);
            for (int i = 0; i < 3; i++)
            {
                Assert.Equal(x, p?.Coordinates.ToArray()[i].X);
                Assert.Equal(y, p?.Coordinates.ToArray()[i].Y);
            }
        }

        [Theory]
        [InlineData(22, 34, 58)]
        [InlineData(0.122, 4.67, 2.4578)]
        [InlineData(22.34567, 0.0094, 100002)]
        public void CanDeserializePoint3D(double x, double y, double z)
        {
            var pointJson =
@"{
    type: 'MultiPoint',
    coordinates: [[#X#, #Y#, #Z#], [#X#, #Y#, #Z#], [#X#, #Y#, #Z#]]
}";
            pointJson = pointJson.Replace("#X#", x.ToString()).Replace("#Y#", y.ToString()).Replace("#Z#", z.ToString());
            MultiPoint? p = JsonConvert.DeserializeObject<MultiPoint>(pointJson, Settings);

            Assert.NotNull(p);
            for (int i = 0; i < 3; i++)
            {
                Assert.Equal(x, p?.Coordinates.ToArray()[i].X);
                Assert.Equal(y, p?.Coordinates.ToArray()[i].Y);
                Assert.Equal(z, p?.Coordinates.ToArray()[i].Z);
            }
        }

        [Theory]
        [InlineData(22, 34)]
        [InlineData(0.122, 4.67)]
        [InlineData(22.34567, 0.0094)]
        public void CanSerializePoint2D(double x, double y)
        {
            Point p1 = new Point(x, y);
            Point p2 = new Point(y, x);
            MultiPoint mp1 = new MultiPoint(new[] { p1.Coordinates, p2.Coordinates });

            string json = JsonConvert.SerializeObject(mp1, Settings);
            MultiPoint? mp2 = JsonConvert.DeserializeObject<MultiPoint>(json, Settings);

            Assert.NotNull(p2);
            Assert.Equal(mp2?.Coordinates.First().X, p1.Coordinates.X);
            Assert.Equal(mp2?.Coordinates.First().Y, p1.Coordinates.Y);
            Assert.Equal(mp2?.Coordinates.Last().X, p2.Coordinates.X);
            Assert.Equal(mp2?.Coordinates.Last().Y, p2.Coordinates.Y);
        }
    }
}
