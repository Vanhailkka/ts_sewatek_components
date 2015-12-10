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
    partial class EB_SEINALAPIVIENTI_MOD_K100
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
                k++;
            }
        }

        private ContourPlate CreatePlate(Point Point1, double Z, Position.DepthEnum PosDepVal)
        {
            ContourPlate plate1 = new ContourPlate();
            Point Origo = Point1;
            ArrayList Cpoints = new ArrayList();

            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(-(_B / 2), -(_H / 2), Z)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(-(_B / 2), _H / 2, Z)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(_B / 2, _H / 2, Z)), null));
            Cpoints.Add(new ContourPoint(new Point(Origo + new Point(_B / 2, -(_H / 2), Z)), null));

            SetDefaultEmbedObjectAttributes(plate1, "0");
            plate1.Profile.ProfileString = "PL2";
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

        private Beam CreatePipe(Point point1, string partClass)
        {
            var putki1 = new Beam();
            var origo = point1;

            SetDefaultEmbedObjectAttributes(putki1, partClass);
            putki1.StartPoint = new Point(origo + new Point(0.0, 0.0, -2));
            putki1.EndPoint = new Point(origo + new Point(0.0, 0.0, -_PanelWidth + 2));
            putki1.Profile.ProfileString = "PD38*2";
            putki1.Position.Plane = Position.PlaneEnum.MIDDLE;
            putki1.Position.Rotation = Position.RotationEnum.FRONT;
            putki1.Position.Depth = Position.DepthEnum.MIDDLE;

            if (!putki1.Insert())
            {
                MessageBox.Show("Insert failed!");
                putki1 = null;
            }

            return putki1;
        }

        private void CreateWelds(List<ModelObject> parts, List<Weld> welds)
        {
            for (int w = 0; w < welds.Count; w++)
            {
                int npA = parts.Count - welds.Count; //Number of parts for Assembly
                welds[w].MainObject = parts[npA + 2] as ModelObject;
                welds[w].SecondaryObject = parts[npA + w] as ModelObject;
                welds[w].ShopWeld = true;
                welds[w].Insert();
            }
        }
    }
}
