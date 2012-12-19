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

        private PartMesh batteryBottom = new PartMesh(@"Models\Battery_bottom.STL");
        private PartMesh boardPlate = new PartMesh(@"Models\Board plate.STL");
        private PartMesh bodyBrace = new PartMesh(@"Models\Body_brace.STL");
        private PartMesh bodyHorn = new PartMesh(@"Models\Body_horn.STL");
        private PartMesh bodyPivot = new PartMesh(@"Models\Body_pivot.STL");
        private PartMesh namePlate = new PartMesh(@"Models\Name_plate.STL");

        public HexyWorldViewModel(Hexy hexy)
        {
            this.hexy = hexy;

            var modelGroup = new Model3DGroup();

            var outsideMaterial = MaterialHelper.CreateMaterial(Colors.Blue);
            var insideMaterial = MaterialHelper.CreateMaterial(Colors.Yellow);

            modelGroup.Children.Add(new GeometryModel3D { Geometry = bodyHorn.Mesh, Material = outsideMaterial, BackMaterial = insideMaterial, });
            modelGroup.Children.Add(new GeometryModel3D { Geometry = bodyPivot.Mesh, Material = outsideMaterial, BackMaterial = insideMaterial });
            modelGroup.Children.Add(new GeometryModel3D { Geometry = bodyBrace.Mesh, Material = outsideMaterial, BackMaterial = insideMaterial });
            modelGroup.Children.Add(new GeometryModel3D { Geometry = bodyBrace.Mesh, Material = outsideMaterial, BackMaterial = insideMaterial });
            modelGroup.Children.Add(new GeometryModel3D { Geometry = boardPlate.Mesh, Material = outsideMaterial, BackMaterial = insideMaterial });
            modelGroup.Children.Add(new GeometryModel3D { Geometry = namePlate.Mesh, Material = outsideMaterial, BackMaterial = insideMaterial });
            modelGroup.Children.Add(new GeometryModel3D { Geometry = batteryBottom.Mesh, Material = outsideMaterial, BackMaterial = insideMaterial });

            this.Model = modelGroup;

            modelGroup.Children[0].Transform = new TranslateTransform3D(new Point3D(0, 0, -22.93) - bodyHorn.Centre);
            modelGroup.Children[1].Transform = new TranslateTransform3D(new Point3D(0, 0, 22.93) - bodyPivot.Centre);

            var transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(-bodyBrace.Centre.ToVector3D()));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), -90)));
            transform.Children.Add(new TranslateTransform3D(new Vector3D(-42, 0, 0)));

            modelGroup.Children[2].Transform = transform;

            transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(-bodyBrace.Centre.ToVector3D()));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), -90)));
            transform.Children.Add(new TranslateTransform3D(new Vector3D(42, 0, 0)));

            modelGroup.Children[3].Transform = transform;

            modelGroup.Children[4].Transform = new TranslateTransform3D(new Point3D(0, 0, -4) - boardPlate.Centre);

            modelGroup.Children[5].Transform = new TranslateTransform3D(new Point3D(0, 0, 28.6) - namePlate.Centre);

            transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(-batteryBottom.Centre.ToVector3D()));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), 90)));
            transform.Children.Add(new TranslateTransform3D(new Vector3D(0, 0, -28.6)));

            modelGroup.Children[6].Transform = transform;
        }

        public Model3DGroup Model { get; set; }

        public double Value { get; set; }

        private void OnValueChanged()
        {
        }
    }
}