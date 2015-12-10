using System.Windows.Forms;
using System.Collections.Generic;
using System.Collections;
using Tekla.Structures.Model;
using Tekla.Structures.Geometry3d;

namespace Sewatek_components
{
    partial class EB_SEINALAPIVIENTI_MOD_K70
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
            var plate1 = new ContourPlate();
            var origo = Point1;
            var contourPoints = new ArrayList();

            contourPoints.Add(new ContourPoint(new Point(origo + new Point(-(_B / 2), -(_H / 2), Z)), null));
            contourPoints.Add(new ContourPoint(new Point(origo + new Point(-(_B / 2), _H / 2, Z)), null));
            contourPoints.Add(new ContourPoint(new Point(origo + new Point(_B / 2, _H / 2, Z)), null));
            contourPoints.Add(new ContourPoint(new Point(origo + new Point(_B / 2, -(_H / 2), Z)), null));

            SetDefaultEmbedObjectAttributes(plate1, "0");
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

        private Beam CreatePipe(Point point, string partClass)
        {
            var pipe = new Beam();
            var origo = point;

            SetDefaultEmbedObjectAttributes(pipe, partClass);
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
            for (int weldIndex = 0; weldIndex < welds.Count; weldIndex++)
            {
                int npA = parts.Count - welds.Count; //Number of parts for Assembly
                welds[weldIndex].MainObject = parts[npA + 2] ;
                welds[weldIndex].SecondaryObject = parts[npA + weldIndex];
                welds[weldIndex].ShopWeld = true;
                welds[weldIndex].Insert();
            }
        }
    }
}
