﻿using System.Windows.Forms;
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
    partial class EB_SEINALAPIVIENTI_LV_K160_3
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
            var plate1 = new ContourPlate();
            var origo = Point1;
            var contourPoints = new ArrayList();

            contourPoints.Add(new ContourPoint(new Point(origo + new Point(-_Pd, -(_H / 2), Z)), new Chamfer(_H / 2, 0, Chamfer.ChamferTypeEnum.CHAMFER_ROUNDING)));
            contourPoints.Add(new ContourPoint(new Point(origo + new Point(-_Pd, _H / 2, Z)), new Chamfer(_H / 2, 0, Chamfer.ChamferTypeEnum.CHAMFER_ROUNDING)));
            contourPoints.Add(new ContourPoint(new Point(origo + new Point(_Xd * 4 + _Pd, _H / 2, Z)), new Chamfer(_H / 2, 0, Chamfer.ChamferTypeEnum.CHAMFER_ROUNDING)));
            contourPoints.Add(new ContourPoint(new Point(origo + new Point(_Xd * 4 + _Pd, -(_H / 2), Z)), new Chamfer(_H / 2, 0, Chamfer.ChamferTypeEnum.CHAMFER_ROUNDING)));

            SetDefaultEmbedObjectAttributes(plate1, "0");
            plate1.Profile.ProfileString = "PL5";
            plate1.Position.Plane = Position.PlaneEnum.MIDDLE;
            plate1.Position.Rotation = Position.RotationEnum.FRONT;
            plate1.Position.Depth = PosDepVal;

            foreach (ContourPoint point in contourPoints)
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

        private Beam CreatePutki(Point Point1, string partClass)
        {
            var pipe = new Beam();
            var origo = Point1;

            SetDefaultEmbedObjectAttributes(pipe, partClass);
            pipe.StartPoint = new Point(origo + new Point(0.0, 0.0, -5));
            pipe.EndPoint = new Point(origo + new Point(0.0, 0.0, -_PanelWidth + 5));
            pipe.Profile.ProfileString = "D40";
            pipe.Position.Plane = Position.PlaneEnum.MIDDLE;
            pipe.Position.Rotation = Position.RotationEnum.FRONT;
            pipe.Position.Depth = Position.DepthEnum.MIDDLE;

            if (!pipe.Insert())
            {
                MessageBox.Show("Insert failed!");
                pipe = null;
            }

            return pipe;
        }

        private void CreateWelds(List<ModelObject> parts, List<Weld> welds)
        {
            for (int w = 0; w < welds.Count; w++)
            {
                welds[w].MainObject = parts[0];
                welds[w].SecondaryObject = parts[w];
                welds[w].ShopWeld = true;
                welds[w].Insert();
            }
        }
    }
}
