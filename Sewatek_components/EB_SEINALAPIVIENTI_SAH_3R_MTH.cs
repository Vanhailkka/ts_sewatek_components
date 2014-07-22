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
    partial class EB_SEINALAPIVIENTI_SAH_3R
    {
        private void CreatePlateM(Point Point1)
        {
            Point StartPoint = Point1;
            int k = 0;
            while (k < 2)
            {
                double[] Zcoord = { 0.0, -_PanelWidth };
                Position.DepthEnum[] DepthVal = { Position.DepthEnum.BEHIND, Position.DepthEnum.FRONT };
                Parts.Add(CreatePlate(StartPoint, Zcoord[k], DepthVal[k]));
                Welds.Add(new Weld());
                k++;
            }
        }

        private ContourPlate CreatePlate(Point Point1, double Z, Position.DepthEnum PosDepVal)
        {
            ContourPlate plate1 = new ContourPlate();
            Point Origo = Point1;
            ArrayList Cpoints = new ArrayList();

            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(-_Pd, -(_H / 2), Z)), new Chamfer(_H / 2, 0, Chamfer.ChamferTypeEnum.CHAMFER_ROUNDING)));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(-_Pd, _H / 2, Z)), new Chamfer(_H / 2, 0, Chamfer.ChamferTypeEnum.CHAMFER_ROUNDING)));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(_Xd * 4 + _Pd, _H / 2, Z)), new Chamfer(_H / 2, 0, Chamfer.ChamferTypeEnum.CHAMFER_ROUNDING)));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(_Xd * 4 + _Pd, -(_H / 2), Z)), new Chamfer(_H / 2, 0, Chamfer.ChamferTypeEnum.CHAMFER_ROUNDING)));

            SetDefaultEmbedObjectAttributes(ref plate1);
            plate1.Profile.ProfileString = "PL5";
            plate1.Position.Plane = Position.PlaneEnum.MIDDLE;
            plate1.Position.Rotation = Position.RotationEnum.FRONT;
            plate1.Position.Depth = PosDepVal;

            foreach (ContourPoint point in Cpoints)
            {
                plate1.AddContourPoint(point);
            }

            if (!plate1.Insert())
            {
                MessageBox.Show("Insert failed!");
                plate1 = null;
            }
            return plate1;
        }

        private Beam CreatePutki(Point Point1)
        {
            Beam Putki1 = new Beam();
            Point Origo = Point1;

            SetDefaultEmbedObjectAttributes(ref Putki1);
            Putki1.StartPoint = new Point(Origo + new Point(0.0, 0.0, -5));
            Putki1.EndPoint = new Point(Origo + new Point(0.0, 0.0, -_PanelWidth + 5));
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

        private Beam CreatePutkiL(Point Point1, double X, double Y)
        {
            Beam Putki1 = new Beam();
            Point Origo = Point1;

            SetDefaultEmbedObjectAttributes(ref Putki1);
            Putki1.StartPoint = new Point(Origo + new Point(X, Y, -5));
            Putki1.EndPoint = new Point(Origo + new Point(X, Y, -_PanelWidth + 5));
            Putki1.Profile.ProfileString = "D30";
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

        private void CreatePutkiLM(Point Point1)
        {
            Point StartPoint = Point1;
            int l = 0;
            while (l < 3)
            {
                double[] Xcoord = { -20, 0.0, 20 };
                double[] Ycoord = { -11.67, 23.33, -11.67 };

                Welds.Add(new Weld());
                Parts.Add(CreatePutkiL(StartPoint, Xcoord[l], Ycoord[l]));
                l++;
            }
        }

        private void CreateWelds(List<ModelObject> parts, List<Weld> welds)
        {
            for (int w = 0; w < welds.Count; w++)
            {
                welds[w].MainObject = parts[0] as ModelObject;
                welds[w].SecondaryObject = parts[w] as ModelObject;
                welds[w].ShopWeld = true;
                welds[w].Insert();
            }
        }
    }
}
