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

        private PartMesh bodyHorn = new PartMesh(@"Models\Body_horn.STL");
        private PartMesh bodyPivot = new PartMesh(@"Models\Body_pivot.STL");
        private PartMesh bodyBrace = new PartMesh(@"Models\Body_brace.STL");

        public HexyWorldViewModel(Hexy hexy)
        {
            this.hexy = hexy;

            var modelGroup = new Model3DGroup();

            var namePlate = new PartMesh(@"Models\Name_plate.STL");

            var outsideMaterial = MaterialHelper.CreateMaterial(Colors.Blue);
            var insideMaterial = MaterialHelper.CreateMaterial(Colors.Yellow);

            modelGroup.Children.Add(new GeometryModel3D { Geometry = bodyHorn.Mesh, Material = outsideMaterial, BackMaterial = insideMaterial, });
            modelGroup.Children.Add(new GeometryModel3D { Geometry = bodyPivot.Mesh, Material = outsideMaterial, BackMaterial = insideMaterial });
            modelGroup.Children.Add(new GeometryModel3D { Geometry = bodyBrace.Mesh, Material = outsideMaterial, BackMaterial = insideMaterial });
            modelGroup.Children.Add(new GeometryModel3D { Geometry = bodyBrace.Mesh, Material = outsideMaterial, BackMaterial = insideMaterial });

            this.Model = modelGroup;

            Gap = 22.93;
            Braces = 42;
        }

        public Model3D Model { get; set; }

        public double Gap { get; set; }

        public double Braces { get; set; }

        private void OnGapChanged()
        {
            var modelGroup = (Model3DGroup)Model;

            modelGroup.Children[0].Transform = new TranslateTransform3D(new Point3D(0, 0, -Gap) - bodyHorn.Centre);
            modelGroup.Children[1].Transform = new TranslateTransform3D(new Point3D(0, 0, Gap) - bodyPivot.Centre);
        }

        private void OnBracesChanged()
        {
            var modelGroup = (Model3DGroup)Model;

            var transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(-bodyBrace.Centre.ToVector3D()));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), -90)));
            transform.Children.Add(new TranslateTransform3D(new Vector3D(-Braces, 0, 0)));

            modelGroup.Children[2].Transform = transform;

            transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(-bodyBrace.Centre.ToVector3D()));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), -90)));
            transform.Children.Add(new TranslateTransform3D(new Vector3D(Braces, 0, 0)));

            modelGroup.Children[3].Transform = transform;
        }
    }
}