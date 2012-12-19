using System;
using System.IO;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace Hexenstein.UI.HexyWorld
{
    internal class PartMesh
    {
        public PartMesh(string filename)
        {
            var meshBuilder = new MeshBuilder(false, false);

            var min = new Point3D(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity);
            var max = new Point3D(double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity);

            using (var stream = new FileStream(filename, FileMode.Open))
            using (var reader = new BinaryReader(stream))
            {
                var header = reader.ReadBytes(80);

                var num = reader.ReadUInt32();

                for (int i = 0; i < num; i++)
                {
                    var normal = ReadPoint(reader);
                    var v1 = ReadPoint(reader);
                    var v2 = ReadPoint(reader);
                    var v3 = ReadPoint(reader);
                    var extra = reader.ReadInt16();

                    meshBuilder.AddTriangle(v1, v2, v3);

                    min = new Point3D(
                        Math.Min(Math.Min(Math.Min(min.X, v1.X), v2.X), v3.X),
                        Math.Min(Math.Min(Math.Min(min.Y, v1.Y), v2.Y), v3.Y),
                        Math.Min(Math.Min(Math.Min(min.Z, v1.Z), v2.Z), v3.Z));

                    max = new Point3D(
                        Math.Max(Math.Max(Math.Max(max.X, v1.X), v2.X), v3.X),
                        Math.Max(Math.Max(Math.Max(max.Y, v1.Y), v2.Y), v3.Y),
                        Math.Max(Math.Max(Math.Max(max.Z, v1.Z), v2.Z), v3.Z));
                }
            }

            Centre = ((max - min) / 2 + min);
            Mesh = meshBuilder.ToMesh(true);
        }

        public Point3D Centre { get; set; }

        public MeshGeometry3D Mesh { get; set; }

        private Point3D ReadPoint(BinaryReader reader)
        {
            var x = reader.ReadSingle();
            var y = reader.ReadSingle();
            var z = reader.ReadSingle();
            return new Point3D(x, y, z);
        }
    }
}