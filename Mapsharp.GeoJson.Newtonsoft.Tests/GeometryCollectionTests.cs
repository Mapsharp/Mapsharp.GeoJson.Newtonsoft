using Newtonsoft.Json;
using Mapsharp.GeoJson.Core.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mapsharp.GeoJson.Newtonsoft.Tests
{
    public class GeometryCollectionTests : SerializerTestBase
    {

        private GeometryCollection CreateGeometryCollection()
        {
            Point p = new Point(55.67, 34.567);
            LineString ls = new LineString(new[] { new Position(98.45, 26.3), new Position(23.45, 92.2), new Position(345.9, 0.028) });
            GeometryCollection gc = new GeometryCollection(new GeometryBase[] { p, ls });

            return new GeometryCollection(new GeometryBase[] { ls, gc });
        }

        [Fact]
        public void CanSerializeGeometryCollection()
        {
            GeometryCollection gc = CreateGeometryCollection();
            string json = JsonConvert.SerializeObject(gc, Settings);
        }

        [Fact]
        public void CanDeserializeGeometryCollection()
        {
            GeometryCollection gc = CreateGeometryCollection();
            string json = JsonConvert.SerializeObject(gc, Settings);
            var gc2 = JsonConvert.DeserializeObject<GeometryCollection>(json, Settings);

            Assert.NotNull(gc2);
        }
    }
}
