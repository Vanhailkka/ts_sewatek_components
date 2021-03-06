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
    partial class EB_SEINALAPIVIENTI_MOD_K160
    {
        private void CreatePlateM(Point Point1)
        {
            Point StartPoint = Point1;
            int k = 0;
            while (k < 2)
            {
                double[] Zcoord = { 0.0, -_PanelWidth };
                Position.DepthEnum[] DepthVal = { Position.DepthEnum.BEHIND, Position.DepthEnum.FRONT };

                Parts.Add(CreatePlate(StartPoint, Zcoord[k], DepthVal[k], "plate"+k));
                k++;
            }
        }

        private ContourPlate CreatePlate(Point Point1, double Z, Position.DepthEnum PosDepVal, string label)
        {
            var plate1 = new ContourPlate();
            var origo = Point1;
            var contourPoints = new ArrayList();

            contourPoints.Add(new ContourPoint(new Point(origo + new Point(-(_B / 2), -(_H / 2), Z)), null));
            contourPoints.Add(new ContourPoint(new Point(origo + new Point(-(_B / 2), _H / 2, Z)), null));
            contourPoints.Add(new ContourPoint(new Point(origo + new Point(_B / 2, _H / 2, Z)), null));
            contourPoints.Add(new ContourPoint(new Point(origo + new Point(_B / 2, -(_H / 2), Z)), null));

            SetDefaultEmbedObjectAttributes(plate1, "0", label);
            plate1.Profile.ProfileString = "PL2";
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

        private Beam CreatePutki(Point point1, string partClass, string label)
        {
            var pipe = new Beam();
            var origo = point1;

            SetDefaultEmbedObjectAttributes(pipe, partClass, label);
            pipe.StartPoint = new Point(origo + new Point(0.0, 0.0, -2));
            pipe.EndPoint = new Point(origo + new Point(0.0, 0.0, -_PanelWidth + 2));
            pipe.Profile.ProfileString = "PD38*2";
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
                int npA = parts.Count - welds.Count; //Number of parts for Assembly
                welds[w].MainObject = parts[npA + 2] ;
                welds[w].SecondaryObject = parts[npA + w] ;
                welds[w].ShopWeld = true;
                welds[w].Insert();
            }
        }
    }
}
