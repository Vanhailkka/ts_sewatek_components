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
    partial class EB_ONTELOLAATTALAPIVIENTI
    {
        private TransformationPlane SetLocalPlane(Vector Vec1, CoordinateSystem Csys, Point Point1, double Radians)
        {
            Matrix Tmatrix = MatrixFactory.Rotate(Radians, Vec1);
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

        private ContourPlate CreateYlatuki(Point Point1)
        {
            ContourPlate YlatukiPl = new ContourPlate();
            Point Origo = Point1;
            ArrayList Cpoints = new ArrayList();

            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(-(_B1 / 2), -(_L / 2 + _cogY), 0.0)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(-(_B1 / 2), _L / 2 - _cogY, 0.0)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point((_B - _B1 / 2), _L / 2 - _cogY, 0.0)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point((_B - _B1 / 2), _L / 2 - _L1 - _cogY, 0.0)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point((_B1 / 2), _L / 2 - _L1 - _cogY, 0.0)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point((_B1 / 2), -(_L / 2 + _cogY), 0.0)), null));

            SetDefaultEmbedObjectAttributes(ref YlatukiPl);
            YlatukiPl.Profile.ProfileString = "PL3";
            YlatukiPl.Position.Plane = Position.PlaneEnum.MIDDLE;
            YlatukiPl.Position.Rotation = Position.RotationEnum.FRONT;
            YlatukiPl.Position.Depth = Position.DepthEnum.FRONT;

            foreach (ContourPoint point in Cpoints)
            {
                YlatukiPl.AddContourPoint(point);
            }

            if (!YlatukiPl.Insert())
            {
                MessageBox.Show("Insert failed!");
                YlatukiPl = null;
            }
            return YlatukiPl;
        }

        private ContourPlate CreateYlatukiPos1(Point Point1)
        {
            ContourPlate YlatukiPl = new ContourPlate();
            Point Origo = Point1;
            ArrayList Cpoints = new ArrayList();

            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(_B - (_B1 / 2), -(_L / 2 + _cogY), 0.0)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(_B - (_B1 / 2), _L / 2 - _cogY, 0.0)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(-(_B1 / 2), _L / 2 - _cogY, 0.0)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(-(_B1 / 2), _L / 2 - _L1 - _cogY, 0.0)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(_B - (_B1 / 2) - _B1, _L / 2 - _L1 - _cogY, 0.0)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(_B - (_B1 / 2) - _B1, -(_L / 2 + _cogY), 0.0)), null));

            SetDefaultEmbedObjectAttributes(ref YlatukiPl);
            YlatukiPl.Profile.ProfileString = "PL3";
            YlatukiPl.Position.Plane = Position.PlaneEnum.MIDDLE;
            YlatukiPl.Position.Rotation = Position.RotationEnum.FRONT;
            YlatukiPl.Position.Depth = Position.DepthEnum.FRONT;

            foreach (ContourPoint point in Cpoints)
            {
                YlatukiPl.AddContourPoint(point);
            }

            if (!YlatukiPl.Insert())
            {
                MessageBox.Show("Insert failed!");
                YlatukiPl = null;
            }
            return YlatukiPl;
        }

        private ContourPlate CreateAlatuki(Point Point1)
        {
            ContourPlate AlatukiPl = new ContourPlate();
            Point Origo = Point1;
            double PosX = _SHeight - 30;
            ArrayList Cpoints = new ArrayList();

            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(-(_B1 / 2), _L / 2 - _L1 - _cogY, -PosX)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(-(_B1 / 2), _L / 2 - _cogY, -PosX)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point((49.5 - _B1 / 2), _L / 2 - _cogY, -PosX)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point((49.5 - _B1 / 2), _L / 2 - _cogY - 10, -PosX)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point((89.5 - _B1 / 2), _L / 2 - _cogY - 10, -PosX)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point((89.5 - _B1 / 2), _L / 2 - _cogY, -PosX)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point((_B - _B1 / 2), _L / 2 - _cogY, -PosX)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point((_B - _B1 / 2), _L / 2 - _L1 - _cogY, -PosX)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point((89.5 - _B1 / 2), _L / 2 - _L1 - _cogY, -PosX)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point((89.5 - _B1 / 2 - 15), _L / 2 - _L1 - _cogY + 15, -PosX)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point((49.5 - _B1 / 2 + 15), _L / 2 - _L1 - _cogY + 15, -PosX)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point((49.5 - _B1 / 2), _L / 2 - _L1 - _cogY, -PosX)), null));

            SetDefaultEmbedObjectAttributes(ref AlatukiPl);
            AlatukiPl.Profile.ProfileString = "PL3";
            AlatukiPl.Position.Plane = Position.PlaneEnum.MIDDLE;
            AlatukiPl.Position.Rotation = Position.RotationEnum.FRONT;
            AlatukiPl.Position.Depth = Position.DepthEnum.FRONT;

            foreach (ContourPoint point in Cpoints)
            {
                AlatukiPl.AddContourPoint(point);
            }

            if (!AlatukiPl.Insert())
            {
                MessageBox.Show("Insert failed!");
                AlatukiPl = null;
            }
            return AlatukiPl;
        }

        private ContourPlate CreateAlatukiV(Point Point1)
        {
            ContourPlate AlatukiPl = new ContourPlate();
            Point Origo = Point1;
            double PosX = _SHeight - 30;
            ArrayList Cpoints = new ArrayList();

            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(-(_B1 / 2), -(_L / 2 + _cogY), -PosX)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(-(_B1 / 2), -(_L / 2 + _cogY) + 49.5, -PosX)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(-(_B1 / 2) + 10, -(_L / 2 + _cogY) + 49.5, -PosX)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(-(_B1 / 2) + 10, -(_L / 2 + _cogY) + _L / 2 - 49.5, -PosX)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(-(_B1 / 2), -(_L / 2 + _cogY) + _L / 2 - 49.5, -PosX)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(-(_B1 / 2), -(_L / 2 + _cogY) + _L / 2, -PosX)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(_B1 / 2, -(_L / 2 + _cogY) + _L / 2, -PosX)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(_B1 / 2, -(_L / 2 + _cogY) + _L / 2 - 49.5, -PosX)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(_B1 / 2 - 15, -(_L / 2 + _cogY) + _L / 2 - 49.5 - 15, -PosX)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(_B1 / 2 - 15, -(_L / 2 + _cogY) + 49.5 + 15, -PosX)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(_B1 / 2, -(_L / 2 + _cogY) + 49.5, -PosX)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(_B1 / 2, -(_L / 2 + _cogY), -PosX)), null));

            SetDefaultEmbedObjectAttributes(ref AlatukiPl);
            AlatukiPl.Profile.ProfileString = "PL3";
            AlatukiPl.Position.Plane = Position.PlaneEnum.MIDDLE;
            AlatukiPl.Position.Rotation = Position.RotationEnum.FRONT;
            AlatukiPl.Position.Depth = Position.DepthEnum.FRONT;

            foreach (ContourPoint point in Cpoints)
            {
                AlatukiPl.AddContourPoint(point);
            }

            if (!AlatukiPl.Insert())
            {
                MessageBox.Show("Insert failed!");
                AlatukiPl = null;
            }
            return AlatukiPl;
        }

        private ContourPlate CreateAlatukiVPos1(Point Point1)
        {
            ContourPlate AlatukiPl = new ContourPlate();
            Point Origo = Point1;
            double PosX = _SHeight - 30;
            ArrayList Cpoints = new ArrayList();

            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(-(_B1 / 2) + 57, -(_L / 2 + _cogY), -PosX)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(-(_B1 / 2) + 57, -(_L / 2 + _cogY) + 49.5, -PosX)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(-(_B1 / 2) + 10 + 57, -(_L / 2 + _cogY) + 49.5, -PosX)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(-(_B1 / 2) + 10 + 57, -(_L / 2 + _cogY) + _L / 2 - 49.5, -PosX)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(-(_B1 / 2) + 57, -(_L / 2 + _cogY) + _L / 2 - 49.5, -PosX)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(-(_B1 / 2) + 57, -(_L / 2 + _cogY) + _L / 2, -PosX)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(_B1 / 2 + 57, -(_L / 2 + _cogY) + _L / 2, -PosX)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(_B1 / 2 + 57, -(_L / 2 + _cogY) + _L / 2 - 49.5, -PosX)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(_B1 / 2 - 15 + 57, -(_L / 2 + _cogY) + _L / 2 - 49.5 - 15, -PosX)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(_B1 / 2 - 15 + 57, -(_L / 2 + _cogY) + 49.5 + 15, -PosX)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(_B1 / 2 + 57, -(_L / 2 + _cogY) + 49.5, -PosX)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(_B1 / 2 + 57, -(_L / 2 + _cogY), -PosX)), null));

            SetDefaultEmbedObjectAttributes(ref AlatukiPl);
            AlatukiPl.Profile.ProfileString = "PL3";
            AlatukiPl.Position.Plane = Position.PlaneEnum.MIDDLE;
            AlatukiPl.Position.Rotation = Position.RotationEnum.FRONT;
            AlatukiPl.Position.Depth = Position.DepthEnum.FRONT;

            foreach (ContourPoint point in Cpoints)
            {
                AlatukiPl.AddContourPoint(point);
            }

            if (!AlatukiPl.Insert())
            {
                MessageBox.Show("Insert failed!");
                AlatukiPl = null;
            }
            return AlatukiPl;
        }

        private Beam CreatePutket(Point Point1, double X, double Y)
        {
            Beam Putki1 = new Beam();
            Point Origo = Point1;

            SetDefaultEmbedObjectAttributes(ref Putki1);
            Putki1.StartPoint = new Point(Origo + new Point(X, Y, -_SHeight));
            Putki1.EndPoint = new Point(Origo + new Point(X, Y, 25));
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
            while (l < 2)
            {
                double[] Xcoord = { -_B1 / 2 + (_B / 2 - _C2 / 2), -_B1 / 2 + (_B / 2 - _C2 / 2) + _C2 };
                double[] Ycoord = { _L / 2 - _cogY - _C1, _L / 2 - _cogY - _C1 };

                Parts.Add(CreatePutket(StartPoint, Xcoord[l], Ycoord[l]));
                l++;
            }
        }

        private void CreatePutketMV(Point Point1)
        {
            Point StartPoint = Point1;
            int l = 0;
            while (l < 2)
            {
                double[] Xcoord = { -_B1 / 2 + _C1, -_B1 / 2 + _C1 };
                double[] Ycoord = { -(_L / 2 + _cogY) + _C2 / 2 - 0.5, -(_L / 2 + _cogY) + _C2 / 2 - 0.5 + _C2 };

                Parts.Add(CreatePutket(StartPoint, Xcoord[l], Ycoord[l]));
                l++;
            }
        }

        private void CreatePutketMVPos1(Point Point1)
        {
            Point StartPoint = Point1;
            int l = 0;
            while (l < 2)
            {
                double[] Xcoord = { -_B1 / 2 + _C1 + 49, -_B1 / 2 + _C1 + 49 };
                double[] Ycoord = { -(_L / 2 + _cogY) + _C2 / 2 - 0.5, -(_L / 2 + _cogY) + _C2 / 2 - 0.5 + _C2 };

                Parts.Add(CreatePutket(StartPoint, Xcoord[l], Ycoord[l]));
                l++;
            }
        }

        private BooleanPart CreateCutYlatuki(Point Point1, ContourPlate YlatukiH, double X, double Y)
        {
            Beam cuttingBeam = new Beam();
            BooleanPart cut = new BooleanPart();
            double width = 0.0;
            YlatukiH.GetReportProperty("WIDTH", ref width);
            Point Origo = Point1;

            cuttingBeam.StartPoint = new Point(Origo + new Point(X, Y, 0.0));
            cuttingBeam.EndPoint = new Point(Origo + new Point(X, Y, width));
            cuttingBeam.Profile.ProfileString = "D41";
            cuttingBeam.Material.MaterialString = "Steel_Undefined";
            cuttingBeam.Class = BooleanPart.BooleanOperativeClassName;
            cuttingBeam.Position.Depth = Position.DepthEnum.MIDDLE;

            if (cuttingBeam.Insert())
            {
                cut.Father = YlatukiH;
                cut.SetOperativePart(cuttingBeam);

                if (cut.Insert())
                    cuttingBeam.Delete();
            }
            return cut;
        }

        private void CreateCutYlatukiM(Point Point1, ContourPlate YlatukiH)
        {
            ContourPlate Ylatuki = YlatukiH;
            Point StartPoint = Point1;
            int i = 0;
            while (i < 4)
            {
                double[] Xcoord = { -_B1 / 2 + (_B / 2 - _C2 / 2), -_B1 / 2 + (_B / 2 - _C2 / 2) + _C2, -_B1 / 2 + _C1, -_B1 / 2 + _C1 };
                double[] Ycoord = { _L / 2 - _cogY - _C1, _L / 2 - _cogY - _C1, -(_L / 2 + _cogY) + _C2 / 2 - 0.5, -(_L / 2 + _cogY) + _C2 / 2 - 0.5 + _C2 };

                CreateCutYlatuki(StartPoint, Ylatuki, Xcoord[i], Ycoord[i]);
                i++;
            }
        }

        private void CreateCutYlatukiMPos1(Point Point1, ContourPlate YlatukiH)
        {
            ContourPlate Ylatuki = YlatukiH;
            Point StartPoint = Point1;
            int i = 0;
            while (i < 4)
            {
                double[] Xcoord = { -_B1 / 2 + (_B / 2 - _C2 / 2), -_B1 / 2 + (_B / 2 - _C2 / 2) + _C2, _B - _B1 / 2 - _C1, _B - _B1 / 2 - _C1 };
                double[] Ycoord = { _L / 2 - _cogY - _C1, _L / 2 - _cogY - _C1, -(_L / 2 + _cogY) + _C2 / 2 - 0.5, -(_L / 2 + _cogY) + _C2 / 2 - 0.5 + _C2 };

                CreateCutYlatuki(StartPoint, Ylatuki, Xcoord[i], Ycoord[i]);
                i++;
            }
        }

        private BooleanPart CreateCutAlatuki(Point Point1, ContourPlate AlatukiH, double X, double Y)
        {
            Beam cuttingBeam = new Beam();
            BooleanPart cut = new BooleanPart();
            double width = 0.0;
            AlatukiH.GetReportProperty("WIDTH", ref width);
            Point Origo = Point1;
            double PosX = _SHeight - 30;

            cuttingBeam.StartPoint = new Point(Origo + new Point(X, Y, -PosX));
            cuttingBeam.EndPoint = new Point(Origo + new Point(X, Y, -PosX + width));
            cuttingBeam.Profile.ProfileString = "D41";
            cuttingBeam.Material.MaterialString = "Steel_Undefined";
            cuttingBeam.Class = BooleanPart.BooleanOperativeClassName;
            cuttingBeam.Position.Depth = Position.DepthEnum.MIDDLE;

            if (cuttingBeam.Insert())
            {
                cut.Father = AlatukiH;
                cut.SetOperativePart(cuttingBeam);

                if (cut.Insert())
                    cuttingBeam.Delete();
            }
            return cut;
        }

        private void CreateCutAlatukiM(Point Point1, ContourPlate AlatukiH)
        {
            ContourPlate Alatuki = AlatukiH;
            Point StartPoint = Point1;
            int j = 0;
            while (j < 2)
            {
                double[] Xcoord = { -_B1 / 2 + (_B / 2 - _C2 / 2), -_B1 / 2 + (_B / 2 - _C2 / 2) + _C2 };
                double[] Ycoord = { _L / 2 - _cogY - _C1, _L / 2 - _cogY - _C1 };

                CreateCutAlatuki(StartPoint, Alatuki, Xcoord[j], Ycoord[j]);
                j++;
            }
        }

        private void CreateCutAlatukiMV(Point Point1, ContourPlate AlatukiH)
        {
            ContourPlate Alatuki = AlatukiH;
            Point StartPoint = Point1;
            int j = 0;
            while (j < 2)
            {
                double[] Xcoord = { -_B1 / 2 + _C1, -_B1 / 2 + _C1 };
                double[] Ycoord = { -(_L / 2 + _cogY) + _C2 / 2 - 0.5, -(_L / 2 + _cogY) + _C2 / 2 - 0.5 + _C2 };

                CreateCutAlatuki(StartPoint, Alatuki, Xcoord[j], Ycoord[j]);
                j++;
            }
        }

        private void CreateCutAlatukiMVPos1(Point Point1, ContourPlate AlatukiH)
        {
            ContourPlate Alatuki = AlatukiH;
            Point StartPoint = Point1;
            int j = 0;
            while (j < 2)
            {
                double[] Xcoord = { -_B1 / 2 + _C1 + 49, -_B1 / 2 + _C1 + 49};
                double[] Ycoord = { -(_L / 2 + _cogY) + _C2 / 2 - 0.5, -(_L / 2 + _cogY) + _C2 / 2 - 0.5 + _C2 };

                CreateCutAlatuki(StartPoint, Alatuki, Xcoord[j], Ycoord[j]);
                j++;
            }
        }

        private Beam CreateBases(Point Point1, double StartX, double EndX, double Y, Position.PlaneEnum PosPlaneVal)
        {
            Beam Base1 = new Beam();
            double PosX = _SHeight - 30;
            Point Origo = Point1;

            SetDefaultEmbedObjectAttributes(ref Base1);
            Base1.StartPoint = new Point(Origo + new Point(StartX, Y, -PosX));
            Base1.EndPoint = new Point(Origo + new Point(EndX, Y, -PosX));
            Base1.Profile.ProfileString = "30X2";
            Base1.Position.Plane = PosPlaneVal;
            Base1.Position.Rotation = Position.RotationEnum.BELOW;
            Base1.Position.Depth = Position.DepthEnum.BEHIND;

            if (!Base1.Insert())
            {
                MessageBox.Show("Insert failed!");
                Base1 = null;
            }
            return Base1;
        }

        private void CreateBasesM(Point Point1)
        {
            Point StartPoint = Point1;
            int k = 0;
            while (k < 4)
            {
                double[] StartXcoord = { -(_B1 / 2), (_B - _B1 / 2), (_B - _B1 / 2), -(_B1 / 2) };
                double[] EndXcoord = { -(_B1 / 2) + 6, (_B - _B1 / 2) - 6, (_B - _B1 / 2) - 6, -(_B1 / 2) + 6 };
                double[] Ycoord = { _L / 2 - _cogY, _L / 2 - _cogY, _L / 2 - _L1 - _cogY, _L / 2 - _L1 - _cogY };
                Position.PlaneEnum[] PlaneVal = { Position.PlaneEnum.RIGHT, Position.PlaneEnum.LEFT, Position.PlaneEnum.RIGHT, Position.PlaneEnum.LEFT };

                Parts.Add(CreateBases(StartPoint, StartXcoord[k], EndXcoord[k], Ycoord[k], PlaneVal[k]));
                k++;
            }
        }

        private void CreateBasesMV(Point Point1)
        {
            Point StartPoint = Point1;
            int k = 0;
            while (k < 4)
            {
                double[] StartXcoord = { -(_B1 / 2), -(_B1 / 2), _B1 / 2, _B1 / 2 };
                double[] EndXcoord = { -(_B1 / 2) + 6, -(_B1 / 2) + 6, _B1 / 2 - 6, _B1 / 2 - 6 };
                double[] Ycoord = { -(_L / 2 + _cogY), -_cogY, -_cogY, -(_L / 2 + _cogY) };
                Position.PlaneEnum[] PlaneVal = { Position.PlaneEnum.LEFT, Position.PlaneEnum.RIGHT, Position.PlaneEnum.LEFT, Position.PlaneEnum.RIGHT };

                Parts.Add(CreateBases(StartPoint, StartXcoord[k], EndXcoord[k], Ycoord[k], PlaneVal[k]));
                k++;
            }
        }

        private void CreateBasesMVPos1(Point Point1)
        {
            Point StartPoint = Point1;
            int k = 0;
            while (k < 4)
            {
                double[] StartXcoord = { -(_B1 / 2) + 57, -(_B1 / 2) + 57, _B1 / 2 + 57, _B1 / 2 + 57 };
                double[] EndXcoord = { -(_B1 / 2) + 6 + 57, -(_B1 / 2) + 6 + 57, _B1 / 2 - 6 + 57, _B1 / 2 - 6 + 57 };
                double[] Ycoord = { -(_L / 2 + _cogY), -_cogY, -_cogY, -(_L / 2 + _cogY) };
                Position.PlaneEnum[] PlaneVal = { Position.PlaneEnum.LEFT, Position.PlaneEnum.RIGHT, Position.PlaneEnum.LEFT, Position.PlaneEnum.RIGHT };

                Parts.Add(CreateBases(StartPoint, StartXcoord[k], EndXcoord[k], Ycoord[k], PlaneVal[k]));
                k++;
            }
        }

        private void CreateWelds(List<ModelObject> parts, List<Weld> welds)
        {
            for (int w = 0; w < welds.Count; w++)
            {
                welds[w].MainObject = parts[4] as ModelObject;
                welds[w].SecondaryObject = parts[w] as ModelObject;
                welds[w].ShopWeld = true;
                welds[w].Insert();
            }
        }
    }
}
