using System.Windows.Forms;
using System.Collections.Generic;
using System.Collections;
using Tekla.Structures.Model;
using Tekla.Structures.Geometry3d;

namespace Sewatek_components
{
    partial class EB_SEINALAPIVIENTI_LV_K160_2
    {
        private void CreatePlateM(Point point1)
        {
            Point StartPoint = point1;
            int k = 0;
            while (k < 2)
            {
                double[] Zcoord = { 0.0, -_PanelWidth };
                Position.DepthEnum[] depthEnums = { Position.DepthEnum.BEHIND, Position.DepthEnum.FRONT };
                Parts.Add(CreatePlate(StartPoint, Zcoord[k], depthEnums[k]));
                Welds.Add(new Weld());
                k++;
            }
        }

        private ContourPlate CreatePlate(Point point1, double Z, Position.DepthEnum positionDepthValue)
        {
            var plate1 = new ContourPlate();
            var origo = point1;
            var contourPoints = new ArrayList
            {
               new ContourPoint(new Point(origo + new Point(-_Pd, -(_H/2), Z)), new Chamfer(_H/2, 0, Chamfer.ChamferTypeEnum.CHAMFER_ROUNDING)),
               new ContourPoint(new Point(origo + new Point(-_Pd, _H/2, Z)), new Chamfer(_H/2, 0, Chamfer.ChamferTypeEnum.CHAMFER_ROUNDING)),
               new ContourPoint(new Point(origo + new Point(_Xd*3 + _Pd, _H/2, Z)), null),
               new ContourPoint(new Point(origo + new Point(_Xd*3 + _Pd, -(_H/2), Z)), null)
            };

            SetDefaultEmbedPartAttributes(plate1, "0");
            plate1.Profile.ProfileString = "PL5";
            plate1.Position.Plane = Position.PlaneEnum.MIDDLE;
            plate1.Position.Rotation = Position.RotationEnum.FRONT;
            plate1.Position.Depth = positionDepthValue;

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
            var beam = new Beam();
            var origo = Point1;

            SetDefaultEmbedPartAttributes(beam, partClass);
            beam.StartPoint = new Point(origo + new Point(0.0, 0.0, -5));
            beam.EndPoint = new Point(origo + new Point(0.0, 0.0, -_PanelWidth + 5));
            beam.Profile.ProfileString = "D40";
            beam.Position.Plane = Position.PlaneEnum.MIDDLE;
            beam.Position.Rotation = Position.RotationEnum.FRONT;
            beam.Position.Depth = Position.DepthEnum.MIDDLE;

            if (!beam.Insert())
            {
                MessageBox.Show("Insert failed!");
                beam = null;
            }

            return beam;
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
