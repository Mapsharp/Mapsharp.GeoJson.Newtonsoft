using Newtonsoft.Json;
using Mapsharp.GeoJson.Core.Geometries;

namespace Mapsharp.GeoJson.Newtonsoft.Tests
{
    public class PositionSerializerTests : SerializerTestBase
    {

        private string CreatePositionJson(params double[] coordinates)
        {
            return JsonConvert.SerializeObject(coordinates, Settings);
        }


        [Theory]
        [InlineData(0.678, 77.12)]
        [InlineData(-8, 0.001)]
        [InlineData(1200000, -87654.3)]
        public void CanDeserialize2DPositions(double x, double y)
        {
            Position expected = new Position(x, y);
            string json = CreatePositionJson(x, y);
            Position parsed = JsonConvert.DeserializeObject<Position>(json, Settings);

            Assert.Equal(expected, parsed);
        }

        [Theory]
        [InlineData(0.678, 77.12)]
        [InlineData(-8, 0.001)]
        [InlineData(1200000, -87654.3)]
        public void CanSerialize2DPositions(double x, double y)
        {
            string expected = CreatePositionJson(x, y);
            string serialized = JsonConvert.SerializeObject(new Position(x, y), Settings);

            Assert.Equal(expected, serialized);
        }

        [Theory]
        [InlineData(0.678, 77.12, 123)]
        [InlineData(-8, 0.001, 0.98)]
        [InlineData(1200000, -87654.3, -1029.23)]
        public void CanDeserialize3DPositions(double x, double y, double z)
        {
            Position expected = new Position(x, y, z);
            string json = CreatePositionJson(x, y, z);
            Position parsed = JsonConvert.DeserializeObject<Position>(json, Settings);

            Assert.Equal(expected, parsed);
        }

        [Theory]
        [InlineData(0.678, 77.12, 123)]
        [InlineData(-8, 0.001, 0.98)]
        [InlineData(1200000, -87654.3, -1029.23)]
        public void CanSerialize3DPositions(double x, double y, double z)
        {
            string expected = CreatePositionJson(x, y, z);
            string serialized = JsonConvert.SerializeObject(new Position(x, y, z), Settings);

            Assert.Equal(expected, serialized);
        }


    }
}