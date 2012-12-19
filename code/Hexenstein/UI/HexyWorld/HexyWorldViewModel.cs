using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using Hexenstein.Emulator;
using Hexenstein.Framework.Reactive;

namespace Hexenstein.UI.HexyWorld
{
    internal class HexyWorldViewModel : ReactiveScreen
    {
        private readonly Hexy hexy;

        public HexyWorldViewModel(Hexy hexy)
        {
            this.hexy = hexy;

            // Create a model group
            var modelGroup = new Model3DGroup();

            // Create a mesh builder and add a box to it
            var meshBuilder = new MeshBuilder(false, false);
            var centre = AddSTLFile(meshBuilder, @"Models\Name_plate.STL");

            // Create a mesh from the builder (and freeze it)
            var mesh = meshBuilder.ToMesh(true);

            // Create some materials
            var outsideMaterial = MaterialHelper.CreateMaterial(Colors.Blue);
            var insideMaterial = MaterialHelper.CreateMaterial(Colors.Yellow);

            // Add 3 models to the group (using the same mesh, that's why we had to freeze it)
            modelGroup.Children.Add(new GeometryModel3D { Geometry = mesh, Material = outsideMaterial, BackMaterial = insideMaterial, Transform = new TranslateTransform3D(-centre) });
            //modelGroup.Children.Add(new GeometryModel3D { Geometry = mesh, Transform = new TranslateTransform3D(-2, 0, 0), Material = redMaterial, BackMaterial = insideMaterial });

            // Set the property, which will be bound to the Content property of the ModelVisual3D (see MainWindow.xaml)
            this.Model = modelGroup;
        }

        private Vector3D AddSTLFile(MeshBuilder meshBuilder, string filename)
        {
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

            return ((max - min) / 2 + min).ToVector3D();
        }

        private Point3D ReadPoint(BinaryReader reader)
        {
            var x = reader.ReadSingle();
            var y = reader.ReadSingle();
            var z = reader.ReadSingle();
            return new Point3D(x, y, z);
        }

        public Model3D Model { get; set; }
    }
}