using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Tekla.Structures.Datatype;
using Tekla.Structures.Model;
using Tekla.Structures.Plugins;
using Tekla.Structures.Model.UI;
using Tekla.Structures.Geometry3d;
using Tekla.Structures;
using Tekla.Structures.Solid;

namespace Sewatek_components
{
    partial class EB_PAIKALLAVALULAPIVIENTI
    {
        private TransformationPlane SetLocalPlane(Vector Vec1, CoordinateSystem Csys, Point Point1, double Radians)
        {
            Matrix Tmatrix = MatrixFactory.Rotate(-Radians, Vec1);
            Vector XAxisF = new Vector(Tmatrix.Transform(Csys.AxisX));
            Vector YAxisF = new Vector(Tmatrix.Transform(Csys.AxisY));

            CoordinateSystem CoordSysChanged = new CoordinateSystem(Point1, XAxisF, YAxisF);
            TransformationPlane newTransPlane = new TransformationPlane(CoordSysChanged);

            return newTransPlane;
        }

        private CoordinateSystem ChangeCoordSys(Vector Vec1, CoordinateSystem Csys, Point Point1, double Radians)
        {
            Matrix Tmatrix = MatrixFactory.Rotate(-Radians, Vec1);
            Vector XAxisF = new Vector(Tmatrix.Transform(Csys.AxisX));
            Vector YAxisF = new Vector(Tmatrix.Transform(Csys.AxisY));

            CoordinateSystem CoordSysChanged = new CoordinateSystem(Point1, XAxisF, YAxisF);

            return CoordSysChanged;
        }

        private ContourPlate CreateTuki(Point Point1, double Z, Chamfer Chamfer)
        {
            ContourPlate TukiPl = new ContourPlate();
            Point Origo = Point1;
            ArrayList Cpoints = new ArrayList();

            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(0, -_B, Z)), Chamfer));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(15, 0.0, Z)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(60, 0.0, Z)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(_L / 2, -30, Z)), Chamfer));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(_L / 2 + 15, 0.0, Z)), Chamfer));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(_L - 15, 0.0, Z)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(_L, -_B, Z)), Chamfer));

            SetDefaultEmbedObjectAttributes(ref TukiPl);
            TukiPl.Profile.ProfileString = "PL3";
            TukiPl.Position.Plane = Position.PlaneEnum.MIDDLE;
            TukiPl.Position.Rotation = Position.RotationEnum.FRONT;
            TukiPl.Position.Depth = Position.DepthEnum.BEHIND;

            foreach (ContourPoint point in Cpoints)
            {
                TukiPl.AddContourPoint(point);
            }

            if (!TukiPl.Insert())
            {
                MessageBox.Show("Insert failed!");
                TukiPl = null;
            }
            return TukiPl;
        }

        private void CreateTukiM(Point Point1)
        {
            Point StartPoint = Point1;
            double Zcoord = 0.0;
            Chamfer chf;
            int l = 0;
            while (l < 2)
            {
                if (l == 0)
                {
                    Zcoord = -(_SHeight - _H);
                    chf = new Chamfer(30, 0, Chamfer.ChamferTypeEnum.CHAMFER_ROUNDING);
                }
                else
                {
                    Zcoord = -(_SHeight - _H1);
                    chf = null;
                }

                Parts.Add(CreateTuki(StartPoint, Zcoord, chf));
                l++;
            }
        }

        private Beam CreatePutket(Point Point1, double X, double Y)
        {
            Beam Putki1 = new Beam();
            Point Origo = Point1;

            SetDefaultEmbedObjectAttributes(ref Putki1);
            Putki1.StartPoint = new Point(Origo + new Point(X, Y, -_SHeight));
            Putki1.EndPoint = new Point(Origo + new Point(X, Y, 20));
            Putki1.Profile.ProfileString = "D40";
            Putki1.Position.Plane = Position.PlaneEnum.MIDDLE;
            Putki1.Position.Rotation = Position.RotationEnum.FRONT;
            Putki1.Position.Depth = Position.DepthEnum.MIDDLE;

            if (!Putki1.Insert())
            {
                MessageBox.Show("Insert failed!");
                Putki1 = null;
            }

            return Putki1;
        }

        private void CreatePutketM(Point Point1)
        {
            Point StartPoint = Point1;
            int l = 0;

            double[] Xcoord = { (_L - _C) / 2, (_L - _C) / 2 + _C, };
            double[] Ycoord = { -_B / 2, -_B / 2 };

            while (l < Xcoord.Length)
            {
                Parts.Add(CreatePutket(StartPoint, Xcoord[l], Ycoord[l]));
                l++;
            }
        }

        private Beam CreateBackPl(Point Point1)
        {
            Beam Peltituki1 = new Beam();
            Point Origo = Point1;
            string prof = Convert.ToString(_H - _H1 - 3);

            SetDefaultEmbedObjectAttributes(ref Peltituki1);
            Peltituki1.StartPoint = new Point(Origo + new Point(6, -_B, -(_SHeight - _H1)));
            Peltituki1.EndPoint = new Point(Origo + new Point(_L, -_B, -(_SHeight - _H1)));
            Peltituki1.Profile.ProfileString = prof + "X3";
            Peltituki1.Position.Plane = Position.PlaneEnum.LEFT;
            Peltituki1.Position.Rotation = Position.RotationEnum.TOP;
            Peltituki1.Position.Depth = Position.DepthEnum.FRONT;

            if (!Peltituki1.Insert())
            {
                MessageBox.Show("Insert failed!");
                Peltituki1 = null;
            }

            return Peltituki1;
        }

        private ContourPlate CreatePeltituki(Point Point1)
        {
            ContourPlate BackPl = new ContourPlate();
            Point Origo = Point1;
            ArrayList Cpoints = new ArrayList();

            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(0.0, -_B, -(_SHeight - _H1))), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(107.97, -(74.18 + _B), -(_SHeight - _H1))), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(107.97, -(74.18 + _B), -_SHeight)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(123.63, -(84.94 + _B), -_SHeight)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(123.63, -(84.94 + _B), -_SHeight + 50)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(41.21, -(28.31 + _B), -(_SHeight - _H) - 3)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(0.0, -_B, -(_SHeight - _H) - 3)), null));

            SetDefaultEmbedObjectAttributes(ref BackPl);
            BackPl.Profile.ProfileString = "PL3";
            BackPl.Position.Plane = Position.PlaneEnum.MIDDLE;
            BackPl.Position.Rotation = Position.RotationEnum.FRONT;
            BackPl.Position.Depth = Position.DepthEnum.BEHIND;

            foreach (ContourPoint point in Cpoints)
            {
                BackPl.AddContourPoint(point);
            }

            if (!BackPl.Insert())
            {
                MessageBox.Show("Insert failed!");
                BackPl = null;
            }
            return BackPl;
        }

        private BooleanPart CreateCutTuki(Point Point1, ContourPlate TukiH, double X, double Y, double Z)
        {
            Beam cuttingBeam = new Beam();
            BooleanPart cut = new BooleanPart();
            double width = 0.0;
            TukiH.GetReportProperty("WIDTH", ref width);
            Point Origo = Point1;

            cuttingBeam.StartPoint = new Point(Origo + new Point(X, Y, Z));
            cuttingBeam.EndPoint = new Point(Origo + new Point(X, Y, Z - width));
            cuttingBeam.Profile.ProfileString = "D41";
            cuttingBeam.Material.MaterialString = "Steel_Undefined";
            cuttingBeam.Class = BooleanPart.BooleanOperativeClassName;
            cuttingBeam.Position.Depth = Position.DepthEnum.MIDDLE;

            if (cuttingBeam.Insert())
            {
                cut.Father = TukiH;
                cut.SetOperativePart(cuttingBeam);

                if (cut.Insert())
                    cuttingBeam.Delete();
            }
            return cut;
        }

        private void CreateCutTukiM(Point Point1, ContourPlate TukiH)
        {
            ContourPlate Tuki = TukiH;
            Point StartPoint = Point1;
            double Zcoord = 0.0;
            int j = 0;

            double[] Xcoord = { (_L - _C) / 2, (_L - _C) / 2 + _C, };
            double[] Ycoord = { -_B / 2, -_B / 2 };

            while (j < Xcoord.Length)
            {
                if (Tuki.Identifier == Parts[0].Identifier)
                {
                    Zcoord = -(_SHeight - _H);
                }
                else
                {
                    Zcoord = -(_SHeight - _H1);
                }

                CreateCutTuki(StartPoint, Tuki, Xcoord[j], Ycoord[j], Zcoord);
                j++;
            }
        }

        private ContourPlate CreateBase1(Point Point1)
        {
            ContourPlate Base1 = new ContourPlate();
            Point Origo = Point1;
            ArrayList Cpoints = new ArrayList();

            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(_L, -_B, -(_SHeight - _H1 + 3))), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(_L, -_B, -_SHeight)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(157.53, -75.2, -_SHeight)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(156.71, -70.27, -_SHeight + 23.5)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(148.29, -19.73, -_SHeight + 23.5)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(147.47, -14.8, -_SHeight)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(_L - 15, 0.0, -_SHeight)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(_L - 15, 0.0, -(_SHeight - _H1 + 3))), null));

            SetDefaultEmbedObjectAttributes(ref Base1);
            Base1.Profile.ProfileString = "PL3";
            Base1.Position.Plane = Position.PlaneEnum.MIDDLE;
            Base1.Position.Rotation = Position.RotationEnum.FRONT;
            Base1.Position.Depth = Position.DepthEnum.BEHIND;

            foreach (ContourPoint point in Cpoints)
            {
                Base1.AddContourPoint(point);
            }

            if (!Base1.Insert())
            {
                MessageBox.Show("Insert failed!");
                Base1 = null;
            }
            return Base1;
        }

        private ContourPlate CreateBase2(Point Point1)
        {
            ContourPlate Base2 = new ContourPlate();
            Point Origo = Point1;
            ArrayList Cpoints = new ArrayList();

            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(0.0, -_B, -(_SHeight - _H1 + 3))), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(0.0, -_B, -_SHeight)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(_L - 157.53, -75.2, -_SHeight)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(_L - 156.71, -70.27, -_SHeight + 23.5)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(_L - 148.29, -19.73, -_SHeight + 23.5)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(_L - 147.47, -14.8, -_SHeight)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(15, 0.0, -_SHeight)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(15, 0.0, -(_SHeight - _H1 + 3))), null));

            SetDefaultEmbedObjectAttributes(ref Base2);
            Base2.Profile.ProfileString = "PL3";
            Base2.Position.Plane = Position.PlaneEnum.MIDDLE;
            Base2.Position.Rotation = Position.RotationEnum.FRONT;
            Base2.Position.Depth = Position.DepthEnum.BEHIND;

            foreach (ContourPoint point in Cpoints)
            {
                Base2.AddContourPoint(point);
            }

            if (!Base2.Insert())
            {
                MessageBox.Show("Insert failed!");
                Base2 = null;
            }
            return Base2;
        }

        private void CreateWelds(List<ModelObject> parts, List<Weld> welds)
        {
            for (int w = 0; w < welds.Count; w++)
            {
                welds[w].MainObject = parts[2] as ModelObject;
                welds[w].SecondaryObject = parts[w] as ModelObject;
                welds[w].ShopWeld = true;
                welds[w].Insert();
            }
        }
    }
}
