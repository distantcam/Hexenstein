using System;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace Hexenstein.UI.HexyWorld
{
    public class Hexy : ModelVisual3D
    {
        private const double HipOffsetX1 = 64.3;
        private const double HipOffsetX2 = 100;
        private const double HipOffsetY1 = 76.6;
        private const double HipOffsetY2 = 0;

        private PartMesh batteryBottom = new PartMesh(@"Models\Battery_bottom.STL");
        private PartMesh boardPlate = new PartMesh(@"Models\Board plate.STL");
        private PartMesh bodyBrace = new PartMesh(@"Models\Body_brace.STL");
        private PartMesh bodyHorn = new PartMesh(@"Models\Body_horn.STL");
        private PartMesh bodyPivot = new PartMesh(@"Models\Body_pivot.STL");
        private PartMesh hipBack = new PartMesh(@"Models\Hip_back_side.STL");
        private PartMesh hipHorn = new PartMesh(@"Models\Hip_horn_side.STL");
        private PartMesh hipPivot = new PartMesh(@"Models\Hip_pivot_side.STL");
        private PartMesh hipTop = new PartMesh(@"Models\Hip_top_side.STL");
        private PartMesh namePlate = new PartMesh(@"Models\Name_plate.STL");
        private PartMesh servo = new PartMesh(@"Models\Servo.STL");

        private ModelVisual3D body;
        private ModelVisual3D[] hips;

        public Hexy()
        {
            CreateBody();

            this.Children.Add(body);

            hips = new ModelVisual3D[6];
            for (int i = 0; i < hips.Length; i++)
            {
                hips[i] = CreateHip();
                this.Children.Add(hips[i]);
            }

            var transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(new Vector3D(HipOffsetX1, HipOffsetY1, 0)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), 140), new Point3D(HipOffsetX1, HipOffsetY1, 0)));

            hips[0].Transform = transform;

            transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(new Vector3D(HipOffsetX2, HipOffsetY2, 0)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), 90), new Point3D(HipOffsetX2, HipOffsetY2, 0)));

            hips[1].Transform = transform;

            transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(new Vector3D(HipOffsetX1, -HipOffsetY1, 0)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), 40), new Point3D(HipOffsetX1, -HipOffsetY1, 0)));

            hips[2].Transform = transform;

            transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(new Vector3D(-HipOffsetX1, -HipOffsetY1, 0)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), -40), new Point3D(-HipOffsetX1, -HipOffsetY1, 0)));

            hips[3].Transform = transform;

            transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(new Vector3D(-HipOffsetX2, HipOffsetY2, 0)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), -90), new Point3D(-HipOffsetX2, HipOffsetY2, 0)));

            hips[4].Transform = transform;

            transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(new Vector3D(-HipOffsetX1, HipOffsetY1, 0)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), -140), new Point3D(-HipOffsetX1, HipOffsetY1, 0)));

            hips[5].Transform = transform;
        }

        public void SetHip(int hip, int value)
        {
            if (hip < 0 || hip > 5)
                throw new IndexOutOfRangeException();

            var angle = (value - 1500.0) / 1000.0 * 90.0;
            var offsetX = 0.0;
            var offsetY = 0.0;

            switch (hip)
            {
                case 0:
                    offsetX = HipOffsetX1;
                    offsetY = HipOffsetY1;
                    angle = 140 + angle;
                    break;
                case 1:
                    offsetX = HipOffsetX2;
                    offsetY = HipOffsetY2;
                    angle = 90 + angle;
                    break;
                case 2:
                    offsetX = HipOffsetX1;
                    offsetY = -HipOffsetY1;
                    angle = 40 + angle;
                    break;
                case 3:
                    offsetX = -HipOffsetX1;
                    offsetY = -HipOffsetY1;
                    angle = -40 + angle;
                    break;
                case 4:
                    offsetX = -HipOffsetX2;
                    offsetY = HipOffsetY2;
                    angle = -90 + angle;
                    break;
                case 5:
                    offsetX = -HipOffsetX1;
                    offsetY = HipOffsetY1;
                    angle = -140 + angle;
                    break;
            }

            Dispatcher.BeginInvoke(new Action(() =>
            {
                var transform = new Transform3DGroup();
                transform.Children.Add(new TranslateTransform3D(new Vector3D(offsetX, offsetY, 0)));
                transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), angle), new Point3D(offsetX, offsetY, 0)));

                hips[hip].Transform = transform;
            }));
        }

        private void CreateBody()
        {
            var modelGroup = new Model3DGroup();

            var outsideMaterial = MaterialHelper.CreateMaterial(Colors.Blue);
            var insideMaterial = MaterialHelper.CreateMaterial(Colors.Yellow);

            modelGroup.Children.Add(new GeometryModel3D { Geometry = bodyHorn.Mesh, Material = outsideMaterial, BackMaterial = insideMaterial });
            modelGroup.Children.Add(new GeometryModel3D { Geometry = bodyPivot.Mesh, Material = outsideMaterial, BackMaterial = insideMaterial });
            modelGroup.Children.Add(new GeometryModel3D { Geometry = bodyBrace.Mesh, Material = outsideMaterial, BackMaterial = insideMaterial });
            modelGroup.Children.Add(new GeometryModel3D { Geometry = bodyBrace.Mesh, Material = outsideMaterial, BackMaterial = insideMaterial });
            modelGroup.Children.Add(new GeometryModel3D { Geometry = boardPlate.Mesh, Material = outsideMaterial, BackMaterial = insideMaterial });
            modelGroup.Children.Add(new GeometryModel3D { Geometry = namePlate.Mesh, Material = outsideMaterial, BackMaterial = insideMaterial });
            modelGroup.Children.Add(new GeometryModel3D { Geometry = batteryBottom.Mesh, Material = outsideMaterial, BackMaterial = insideMaterial });

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

            body = new ModelVisual3D() { Content = modelGroup };
        }

        private ModelVisual3D CreateHip()
        {
            var modelGroup = new Model3DGroup();

            var outsideMaterial = MaterialHelper.CreateMaterial(Colors.Blue);
            var insideMaterial = MaterialHelper.CreateMaterial(Colors.Yellow);
            var debugMaterial = MaterialHelper.CreateMaterial(Colors.Red);

            modelGroup.Children.Add(new GeometryModel3D { Geometry = hipPivot.Mesh, Material = outsideMaterial, BackMaterial = insideMaterial });
            modelGroup.Children.Add(new GeometryModel3D { Geometry = hipTop.Mesh, Material = outsideMaterial, BackMaterial = insideMaterial });
            modelGroup.Children.Add(new GeometryModel3D { Geometry = hipHorn.Mesh, Material = outsideMaterial, BackMaterial = insideMaterial });
            modelGroup.Children.Add(new GeometryModel3D { Geometry = hipBack.Mesh, Material = outsideMaterial, BackMaterial = insideMaterial });
            modelGroup.Children.Add(new GeometryModel3D { Geometry = servo.Mesh, Material = debugMaterial, BackMaterial = insideMaterial });

            modelGroup.Children[0].Transform = new TranslateTransform3D(new Point3D(0, 0, 12.2) - hipPivot.Centre);

            modelGroup.Children[1].Transform = new TranslateTransform3D(new Point3D(0, 0, -12.2) - hipTop.Centre);

            var transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(-hipHorn.Centre.ToVector3D()));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), -90)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), 90)));
            transform.Children.Add(new TranslateTransform3D(new Vector3D(-23.2, -9.7, 0)));

            modelGroup.Children[2].Transform = transform;

            transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(-hipBack.Centre.ToVector3D()));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), -90)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), 90)));
            transform.Children.Add(new TranslateTransform3D(new Vector3D(23.2, -8.5, 0)));

            modelGroup.Children[3].Transform = transform;

            transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(-servo.Centre.ToVector3D()));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), -90)));
            transform.Children.Add(new TranslateTransform3D(new Vector3D(6.3, 0, -3.7)));

            modelGroup.Children[4].Transform = transform;

            return new ModelVisual3D() { Content = modelGroup };
        }

        public void SetValue(double value)
        {
            //var modelGroup = (Model3DGroup)hip.Content;

            //var transform = new Transform3DGroup();
            //transform.Children.Add(new TranslateTransform3D(new Vector3D(value, 0, 0)));
            //transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), value + 60), new Point3D(64.3, 76.6, 0)));

            //hips[1].Transform = transform;
        }
    }
}