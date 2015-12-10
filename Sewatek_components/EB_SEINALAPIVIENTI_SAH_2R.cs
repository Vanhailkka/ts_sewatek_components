using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using Tekla.Structures.Model.UI;
using Tekla.Structures.Plugins;
using Tekla.Structures.Catalogs;
using Tekla.Structures;
using Tekla.Structures.Dialog;
using Tekla.Structures.Dialog.UIControls;
using TSD = Tekla.Structures.Datatype;
using Point = Tekla.Structures.Geometry3d.Point;
using System.Globalization;

namespace Sewatek_components
{
    [Plugin("EB_SEINALAPIVIENTI_SAH_2R")]
    [PluginUserInterface(EB_SEINALAPIVIENTI_SAH_2R_INP.SAH2Rcomponent)]

    public partial class EB_SEINALAPIVIENTI_SAH_2R : PluginBase
    {
        #region Fields
        private StructuresData _Data;
        private Model _Model;
        CoordinateSystem CoordSysI;

        List<ModelObject> Parts = new List<ModelObject>();

        List<Weld> Welds = new List<Weld>();

        private string _AspreAttribut1 = string.Empty;
        private string _AsnumAttribut1 = string.Empty;
        private string _NameAttribute = string.Empty;
        private string _DescriptionAttribute = string.Empty;
        private string _ProductCodeAttribute = string.Empty;

        private string _FinishAttribute = string.Empty;
        private string _MaterialAttribute = string.Empty;
        private string _PosAtdepthAttribute = string.Empty;
        private double _PanelWidth;
        private string _Profile = string.Empty;
        private string _UDAn1 = string.Empty;
        private string _UDAv1 = string.Empty;
        private string _UDAn2 = string.Empty;
        private string _UDAv2 = string.Empty;
        private string _UDAn3 = string.Empty;
        private string _UDAv3 = string.Empty;

        private const double _Xd = 80;
        private const double _H = 97;
        private const double _Pd = 50;

        #endregion

        #region Constructor
        public EB_SEINALAPIVIENTI_SAH_2R(StructuresData PluginData)
        {
            this._Data = PluginData;
            _Model = new Model();
        }
        #endregion

        #region Overrides
        public override List<InputDefinition> DefineInput()
        {
            var picker = new Picker();
            var pointList = new List<InputDefinition>();
            var pickedPoints = picker.PickPoints(Picker.PickPointEnum.PICK_TWO_POINTS);

            pointList.Add(new InputDefinition(pickedPoints));

            return pointList;
        }

        public override bool Run(List<InputDefinition> Input)
        {
            try
            {
                var currentPlane = _Model.GetWorkPlaneHandler().GetCurrentTransformationPlane();

                GetValuesFromDialog();

                var points = (ArrayList)Input[0].GetInput();
                var startPoint = points[0] as Point;
                var endPoint = points[1] as Point;

                var axisLine = new LineSegment(startPoint, endPoint);
                var XAxisI = axisLine.GetDirectionVector();
                var YAxisI = new Vector(0, 0, 1);

                CoordSysI = new CoordinateSystem(startPoint, XAxisI, YAxisI);
                var localPlane = new TransformationPlane(CoordSysI);

                Beam pipe;

                _Model.GetWorkPlaneHandler().SetCurrentTransformationPlane(localPlane);

                double Xdist = 0.0;

               var measureBeam = CreatePutki(new Point(0, 0, 0), "100");
               measureBeam.Profile.ProfileString = "D5";
               measureBeam.Modify();
               Parts.Add(measureBeam);
               Welds.Add(new Weld());

                for (int j = 1; j <= 4; j++)
                {
                    var pt = new Point(Xdist, 0.0, 0.0);
                    if (j == 1 || j == 3)
                    {
                        CreatePipeGroup(pt);
                    }
                    else
                    {
                        pipe = CreatePutki(pt, "0");
                        Parts.Add(pipe);
                        Welds.Add(new Weld());
                    }
                    Xdist += _Xd;
                }

                CreatePlateM(startPoint);
                var pipeMain = Parts[0] as Beam;
                InsertUserdefinedAttributes(pipeMain);
                CreateWelds(Parts, Welds);

                _Model.GetWorkPlaneHandler().SetCurrentTransformationPlane(currentPlane);

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

            return true;
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Gets the values from the dialog and sets the default values if needed
        /// </summary>
        private void GetValuesFromDialog()
        {
            if (!IsDefaultValue(_Data.wpanel))
                _PanelWidth = _Data.wpanel;
            else
                _PanelWidth = 200;

            if (!IsDefaultValue(_Data.P1a))
                _NameAttribute = _Data.P1a;
            else
                _NameAttribute = "Sewatek";

            if (!IsDefaultValue(_Data.P2a))
                _DescriptionAttribute = _Data.P2a;
            else
               _DescriptionAttribute = "1 + 3, 2R, S200";

            if (!IsDefaultValue(_Data.P3a))
                _ProductCodeAttribute = _Data.P3a;
            else
                _ProductCodeAttribute = "SEWATEK";

            if (!IsDefaultValue(_Data.P4a))
                _AsnumAttribut1 = _Data.P4a;
            else
                _AsnumAttribut1 = "1";

            if (!IsDefaultValue(_Data.P5a))
                _FinishAttribute = _Data.P5a;
            else
                _FinishAttribute = "Undefined";

            if (!IsDefaultValue(_Data.P6a))
                _AspreAttribut1 = _Data.P6a;
            else
                _AspreAttribut1 = "EB_SAH_2R";
                
            _MaterialAttribute = "Misc_Undefined";

            if (_Data.UDAn1 != String.Empty && _Data.UDAv1 != String.Empty)
            {
                _UDAn1 = _Data.UDAn1;
                _UDAv1 = _Data.UDAv1;
            }

            if (_Data.UDAn2 != String.Empty && _Data.UDAv2 != String.Empty)
            {
                _UDAn2 = _Data.UDAn2;
                _UDAv2 = _Data.UDAv2;
            }

            if (_Data.UDAn3 != String.Empty && _Data.UDAv3 != String.Empty)
            {
                _UDAn3 = _Data.UDAn3;
                _UDAv3 = _Data.UDAv3;
            }
        }


        private void SetDefaultEmbedObjectAttributes(Part part, string partClass)
        {
            part.PartNumber.Prefix = _AspreAttribut1;
            part.PartNumber.StartNumber = Convert.ToInt32(_AsnumAttribut1);
            part.AssemblyNumber.Prefix = _AspreAttribut1;
            part.AssemblyNumber.StartNumber = Convert.ToInt32(_AsnumAttribut1);

            part.Name = _NameAttribute;
            part.Material.MaterialString = _MaterialAttribute;
            part.Finish = _FinishAttribute;
            part.Class = partClass;
        }

        private void InsertUserdefinedAttributes(ModelObject Object)
        {
            Object.SetUserProperty("PRODUCT_DESCR", _DescriptionAttribute);
            Object.SetUserProperty("PRODUCT_CODE", _ProductCodeAttribute);

            if (_UDAn1 != String.Empty && _UDAv1 != String.Empty)
                Object.SetUserProperty(_UDAn1, _UDAv1);

            if (_UDAn2 != String.Empty && _UDAv2 != String.Empty)
                Object.SetUserProperty(_UDAn2, _UDAv2);

            if (_UDAn3 != String.Empty && _UDAv3 != String.Empty)
                Object.SetUserProperty(_UDAn3, _UDAv3);
        }
        #endregion
    }
}
