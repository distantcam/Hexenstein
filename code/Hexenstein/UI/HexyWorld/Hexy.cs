using System;
using System.Linq;
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
        private const double ThighOffset = -50.4;
        private const double ThighRotate = -26;
        private const double CalfOffset = -58.4;
        private const double CalfRotate = -24.5;

        private PartMesh batteryBottom = new PartMesh(@"Battery_bottom.STL");
        private PartMesh boardPlate = new PartMesh(@"Board plate.STL");
        private PartMesh bodyBrace = new PartMesh(@"Body_brace.STL");
        private PartMesh bodyHorn = new PartMesh(@"Body_horn.STL");
        private PartMesh bodyPivot = new PartMesh(@"Body_pivot.STL");
        private PartMesh calfHorn = new PartMesh(@"Calf_horn.STL");
        private PartMesh calfPivot = new PartMesh(@"Calf_pivot.STL");
        private PartMesh foot = new PartMesh(@"Foot.STL");
        private PartMesh hipBack = new PartMesh(@"Hip_back_side.STL");
        private PartMesh hipHorn = new PartMesh(@"Hip_horn_side.STL");
        private PartMesh hipPivot = new PartMesh(@"Hip_pivot_side.STL");
        private PartMesh hipTop = new PartMesh(@"Hip_top_side.STL");
        private PartMesh namePlate = new PartMesh(@"Name_plate.STL");
        private PartMesh servo = new PartMesh(@"Servo.STL");
        private PartMesh thighBottom = new PartMesh(@"Thigh_bottom.STL");
        private PartMesh thighMiddle = new PartMesh(@"Thigh_middle.STL");
        private PartMesh thighTop = new PartMesh(@"Thigh_top.STL");

        private ModelVisual3D body;
        private ModelVisual3D[] hips;
        private ModelVisual3D[] thighs;
        private ModelVisual3D[] calves;

        public Hexy()
        {
            body = CreateBody();
            this.Children.Add(body);

            hips = new ModelVisual3D[6];
            thighs = new ModelVisual3D[6];
            calves = new ModelVisual3D[6];
            for (int i = 0; i < 6; i++)
            {
                hips[i] = CreateHip();
                this.Children.Add(hips[i]);

                thighs[i] = CreateThigh();
                this.Children.Add(thighs[i]);

                calves[i] = CreateCalf();
                this.Children.Add(calves[i]);
            }

            Update(Enumerable.Repeat(1500, 32).ToArray());
        }

        public void Update(int[] servos)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                var angles = servos.Select(s => (s - 1500.0) / 1000.0 * 90.0).ToArray();

                UpdateHips(angles);

                UpdateThighs(angles);

                UpdateCalves(angles);
            }));
        }

        private ModelVisual3D CreateBody()
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

            return new ModelVisual3D() { Content = modelGroup };
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

        private ModelVisual3D CreateThigh()
        {
            var modelGroup = new Model3DGroup();

            var outsideMaterial = MaterialHelper.CreateMaterial(Colors.Blue);
            var insideMaterial = MaterialHelper.CreateMaterial(Colors.Yellow);
            var debugMaterial = MaterialHelper.CreateMaterial(Colors.Red);

            modelGroup.Children.Add(new GeometryModel3D { Geometry = thighMiddle.Mesh, Material = outsideMaterial, BackMaterial = insideMaterial });
            modelGroup.Children.Add(new GeometryModel3D { Geometry = thighMiddle.Mesh, Material = outsideMaterial, BackMaterial = insideMaterial });
            modelGroup.Children.Add(new GeometryModel3D { Geometry = thighTop.Mesh, Material = outsideMaterial, BackMaterial = insideMaterial });
            modelGroup.Children.Add(new GeometryModel3D { Geometry = thighBottom.Mesh, Material = outsideMaterial, BackMaterial = insideMaterial });
            modelGroup.Children.Add(new GeometryModel3D { Geometry = servo.Mesh, Material = debugMaterial, BackMaterial = insideMaterial });
            modelGroup.Children.Add(new GeometryModel3D { Geometry = servo.Mesh, Material = debugMaterial, BackMaterial = insideMaterial });

            var transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(-thighMiddle.Centre.ToVector3D()));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), 90)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), 90)));
            transform.Children.Add(new TranslateTransform3D(new Vector3D(0, -4.5, 0)));

            modelGroup.Children[0].Transform = transform;

            transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(-thighMiddle.Centre.ToVector3D()));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), 90)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), 90)));
            transform.Children.Add(new TranslateTransform3D(new Vector3D(0, 4.5, 0)));

            modelGroup.Children[1].Transform = transform;

            transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(-thighTop.Centre.ToVector3D()));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), 90)));
            transform.Children.Add(new TranslateTransform3D(new Vector3D(-14.1, 0, 0)));

            modelGroup.Children[2].Transform = transform;

            transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(-thighBottom.Centre.ToVector3D()));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), 90)));
            transform.Children.Add(new TranslateTransform3D(new Vector3D(13.8, 0, 0)));

            modelGroup.Children[3].Transform = transform;

            transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(-servo.Centre.ToVector3D()));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), 90)));
            transform.Children.Add(new TranslateTransform3D(new Vector3D(-4.5, -19, 0)));

            modelGroup.Children[4].Transform = transform;

            transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(-servo.Centre.ToVector3D()));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), 90)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), 180)));
            transform.Children.Add(new TranslateTransform3D(new Vector3D(-4.5, 19, 0)));

            modelGroup.Children[5].Transform = transform;

            return new ModelVisual3D() { Content = modelGroup };
        }

        private ModelVisual3D CreateCalf()
        {
            var modelGroup = new Model3DGroup();

            var outsideMaterial = MaterialHelper.CreateMaterial(Colors.Blue);
            var insideMaterial = MaterialHelper.CreateMaterial(Colors.Yellow);

            modelGroup.Children.Add(new GeometryModel3D { Geometry = calfPivot.Mesh, Material = outsideMaterial, BackMaterial = insideMaterial });
            modelGroup.Children.Add(new GeometryModel3D { Geometry = calfHorn.Mesh, Material = outsideMaterial, BackMaterial = insideMaterial });
            modelGroup.Children.Add(new GeometryModel3D { Geometry = foot.Mesh, Material = outsideMaterial, BackMaterial = insideMaterial });

            var transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(-calfPivot.Centre.ToVector3D()));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), 90)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), 90)));
            transform.Children.Add(new TranslateTransform3D(new Vector3D(23.2, 20.9, 0)));

            modelGroup.Children[0].Transform = transform;

            transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(-calfHorn.Centre.ToVector3D()));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), 90)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), 90)));
            transform.Children.Add(new TranslateTransform3D(new Vector3D(-23.2, 20.9, 0)));

            modelGroup.Children[1].Transform = transform;

            transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(-foot.Centre.ToVector3D()));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), -90)));

            modelGroup.Children[2].Transform = transform;

            return new ModelVisual3D() { Content = modelGroup };
        }

        private void UpdateHips(double[] angles)
        {
            var transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(new Vector3D(HipOffsetX1, HipOffsetY1, 0)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), 140 + angles[24]), new Point3D(HipOffsetX1, HipOffsetY1, 0)));

            hips[0].Transform = transform;

            transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(new Vector3D(HipOffsetX2, HipOffsetY2, 0)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), 90 + angles[20]), new Point3D(HipOffsetX2, HipOffsetY2, 0)));

            hips[1].Transform = transform;

            transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(new Vector3D(HipOffsetX1, -HipOffsetY1, 0)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), 40 + angles[16]), new Point3D(HipOffsetX1, -HipOffsetY1, 0)));

            hips[2].Transform = transform;

            transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(new Vector3D(-HipOffsetX1, -HipOffsetY1, 0)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), -40 + angles[15]), new Point3D(-HipOffsetX1, -HipOffsetY1, 0)));

            hips[3].Transform = transform;

            transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(new Vector3D(-HipOffsetX2, HipOffsetY2, 0)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), -90 + angles[11]), new Point3D(-HipOffsetX2, HipOffsetY2, 0)));

            hips[4].Transform = transform;

            transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(new Vector3D(-HipOffsetX1, HipOffsetY1, 0)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), -140 + angles[7]), new Point3D(-HipOffsetX1, HipOffsetY1, 0)));

            hips[5].Transform = transform;
        }

        private void UpdateThighs(double[] angles)
        {
            var transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(new Vector3D(0, ThighOffset, 0)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), angles[25]), new Point3D(0, ThighRotate, 0)));
            transform.Children.Add(new TranslateTransform3D(new Vector3D(HipOffsetX1, HipOffsetY1, 0)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), 140 + angles[24]), new Point3D(HipOffsetX1, HipOffsetY1, 0)));

            thighs[0].Transform = transform;

            transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(new Vector3D(0, ThighOffset, 0)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), angles[21]), new Point3D(0, ThighRotate, 0)));
            transform.Children.Add(new TranslateTransform3D(new Vector3D(HipOffsetX2, HipOffsetY2, 0)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), 90 + angles[20]), new Point3D(HipOffsetX2, HipOffsetY2, 0)));

            thighs[1].Transform = transform;

            transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(new Vector3D(0, ThighOffset, 0)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), angles[17]), new Point3D(0, ThighRotate, 0)));
            transform.Children.Add(new TranslateTransform3D(new Vector3D(HipOffsetX1, -HipOffsetY1, 0)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), 40 + angles[16]), new Point3D(HipOffsetX1, -HipOffsetY1, 0)));

            thighs[2].Transform = transform;

            transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(new Vector3D(0, ThighOffset, 0)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), angles[14]), new Point3D(0, ThighRotate, 0)));
            transform.Children.Add(new TranslateTransform3D(new Vector3D(-HipOffsetX1, -HipOffsetY1, 0)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), -40 + angles[15]), new Point3D(-HipOffsetX1, -HipOffsetY1, 0)));

            thighs[3].Transform = transform;

            transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(new Vector3D(0, ThighOffset, 0)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), angles[10]), new Point3D(0, ThighRotate, 0)));
            transform.Children.Add(new TranslateTransform3D(new Vector3D(-HipOffsetX2, HipOffsetY2, 0)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), -90 + angles[11]), new Point3D(-HipOffsetX2, HipOffsetY2, 0)));

            thighs[4].Transform = transform;

            transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(new Vector3D(0, ThighOffset, 0)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), angles[6]), new Point3D(0, ThighRotate, 0)));
            transform.Children.Add(new TranslateTransform3D(new Vector3D(-HipOffsetX1, HipOffsetY1, 0)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), -140 + angles[7]), new Point3D(-HipOffsetX1, HipOffsetY1, 0)));

            thighs[5].Transform = transform;
        }

        private void UpdateCalves(double[] angles)
        {
            var transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(new Vector3D(0, CalfOffset, 0)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), -angles[26]), new Point3D(0, CalfRotate, 0)));
            transform.Children.Add(new TranslateTransform3D(new Vector3D(0, ThighOffset, 0)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), angles[25]), new Point3D(0, ThighRotate, 0)));
            transform.Children.Add(new TranslateTransform3D(new Vector3D(HipOffsetX1, HipOffsetY1, 0)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), 140 + angles[24]), new Point3D(HipOffsetX1, HipOffsetY1, 0)));

            calves[0].Transform = transform;

            transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(new Vector3D(0, CalfOffset, 0)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), -angles[22]), new Point3D(0, CalfRotate, 0)));
            transform.Children.Add(new TranslateTransform3D(new Vector3D(0, ThighOffset, 0)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), angles[21]), new Point3D(0, ThighRotate, 0)));
            transform.Children.Add(new TranslateTransform3D(new Vector3D(HipOffsetX2, HipOffsetY2, 0)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), 90 + angles[20]), new Point3D(HipOffsetX2, HipOffsetY2, 0)));

            calves[1].Transform = transform;

            transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(new Vector3D(0, CalfOffset, 0)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), -angles[18]), new Point3D(0, CalfRotate, 0)));
            transform.Children.Add(new TranslateTransform3D(new Vector3D(0, ThighOffset, 0)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), angles[17]), new Point3D(0, ThighRotate, 0)));
            transform.Children.Add(new TranslateTransform3D(new Vector3D(HipOffsetX1, -HipOffsetY1, 0)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), 40 + angles[16]), new Point3D(HipOffsetX1, -HipOffsetY1, 0)));

            calves[2].Transform = transform;

            transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(new Vector3D(0, CalfOffset, 0)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), -angles[13]), new Point3D(0, CalfRotate, 0)));
            transform.Children.Add(new TranslateTransform3D(new Vector3D(0, ThighOffset, 0)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), angles[14]), new Point3D(0, ThighRotate, 0)));
            transform.Children.Add(new TranslateTransform3D(new Vector3D(-HipOffsetX1, -HipOffsetY1, 0)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), -40 + angles[15]), new Point3D(-HipOffsetX1, -HipOffsetY1, 0)));

            calves[3].Transform = transform;

            transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(new Vector3D(0, CalfOffset, 0)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), -angles[9]), new Point3D(0, CalfRotate, 0)));
            transform.Children.Add(new TranslateTransform3D(new Vector3D(0, ThighOffset, 0)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), angles[10]), new Point3D(0, ThighRotate, 0)));
            transform.Children.Add(new TranslateTransform3D(new Vector3D(-HipOffsetX2, HipOffsetY2, 0)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), -90 + angles[11]), new Point3D(-HipOffsetX2, HipOffsetY2, 0)));

            calves[4].Transform = transform;

            transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(new Vector3D(0, CalfOffset, 0)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), -angles[5]), new Point3D(0, CalfRotate, 0)));
            transform.Children.Add(new TranslateTransform3D(new Vector3D(0, ThighOffset, 0)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), angles[6]), new Point3D(0, ThighRotate, 0)));
            transform.Children.Add(new TranslateTransform3D(new Vector3D(-HipOffsetX1, HipOffsetY1, 0)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), -140 + angles[7]), new Point3D(-HipOffsetX1, HipOffsetY1, 0)));

            calves[5].Transform = transform;
        }
    }
}