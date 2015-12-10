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
    [Plugin("EB_SEINALAPIVIENTI_MOD_K70")]
    [PluginUserInterface(EB_SEINALAPIVIENTI_MOD_K70_INP.SMODK70component)]

    public partial class EB_SEINALAPIVIENTI_MOD_K70 : PluginBase
    {
        #region Fields
        private StructuresData _data;
        private Model _model;
        CoordinateSystem _coordSysI;

        List<ModelObject> Parts = new List<ModelObject>();

        List<Weld> Welds = new List<Weld>
                    {
                        new Weld{},
                        new Weld{},
                        new Weld{},
                    };

        private string _AspreAttribut1 = string.Empty;
        private string _AsnumAttribut1 = string.Empty;
        private string _NameAttribute = string.Empty;
        private string _DescriptionAttribute = string.Empty;
        private string _ProductCodeAttribute = string.Empty;

        private string _FinishAttribute = string.Empty;
        private string _MaterialAttribute = string.Empty;
        private string _PosAtdepthAttribute = string.Empty;
        private double _PanelWidth;
        private int _NumHorizParts;
        private int _NumVertParts;
        private string _UDAn1 = string.Empty;
        private string _UDAv1 = string.Empty;
        private string _UDAn2 = string.Empty;
        private string _UDAv2 = string.Empty;
        private string _UDAn3 = string.Empty;
        private string _UDAv3 = string.Empty;

        private const double _H = 100;
        private const double _B = 70;

        #endregion

        #region Constructor
        public EB_SEINALAPIVIENTI_MOD_K70(StructuresData PluginData)
        {
            this._data = PluginData;
            _model = new Model();
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
                var currentPlane = _model.GetWorkPlaneHandler().GetCurrentTransformationPlane();

                GetValuesFromDialog();

                var points = (ArrayList)Input[0].GetInput();
                var startPoint = points[0] as Point;
                var endPoint = points[1] as Point;

                var AxisLine = new LineSegment(startPoint, endPoint);
                var XAxisI = AxisLine.GetDirectionVector();
                var YAxisI = new Vector(0, 0, 1);

                _coordSysI = new CoordinateSystem(startPoint, XAxisI, YAxisI);
                var localPlane = new TransformationPlane(_coordSysI);

                Beam pipe;

                _model.GetWorkPlaneHandler().SetCurrentTransformationPlane(localPlane);

                if (_NumHorizParts >= 1 && _NumVertParts >= 1)
                {
                    double Ydist = 0.0;

                    for (int i = 1; i <= _NumVertParts; i++)
                    {
                        double Xdist = 0.0;

                        for (int j = 1; j <= _NumHorizParts; j++)
                        {
                            var point = new Point(Xdist, Ydist, 0.0);
                            CreatePlateM(point);
                           if (j == 1 && i == 1)
                           {
                              pipe = CreatePipe(point, "100");
                           }
                           else
                           {
                              pipe = CreatePipe(point, "0");
                           }

                           Parts.Add(pipe);
                           InsertUDAs(pipe);
                           CreateWelds(Parts, Welds);
                           Xdist += _B;
                        }
                        Ydist += _H;
                    }
                }

                _model.GetWorkPlaneHandler().SetCurrentTransformationPlane(currentPlane);

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

            if (!IsDefaultValue(_data.wpanel))
                _PanelWidth = _data.wpanel;
            else
                _PanelWidth = 200;

            if (!IsDefaultValue(_data.nHorP))
                _NumHorizParts = Convert.ToInt16(_data.nHorP);
            else
                _NumHorizParts = 1;

            if (!IsDefaultValue(_data.nVerP))
                _NumVertParts = Convert.ToInt16(_data.nVerP);
            else
                _NumVertParts = 1;

            if (!IsDefaultValue(_data.P1a))
                _NameAttribute = _data.P1a;
            else
                _NameAttribute = "SEINALAPIVIENTI_MOD_K70";

            if (!IsDefaultValue(_data.P2a))
                _DescriptionAttribute = _data.P2a;
            else
                _DescriptionAttribute = "SEWATEK-SEINALAPIVIENTI MODUULIRAKENTEINEN";

            if (!IsDefaultValue(_data.P3a))
                _ProductCodeAttribute = _data.P3a;
            else
                _ProductCodeAttribute = "SEWATEK";

            if (!IsDefaultValue(_data.P4a))
                _AsnumAttribut1 = _data.P4a;
            else
                _AsnumAttribut1 = "1";

            if (!IsDefaultValue(_data.P5a))
                _FinishAttribute = _data.P5a;
            else
                _FinishAttribute = "Undefined";

            if (!IsDefaultValue(_data.P6a))
                _AspreAttribut1 = _data.P6a;
            else
                _AspreAttribut1 = "EB_MOD_K70";
                
            _MaterialAttribute = "Misc_Undefined";

            if (_data.UDAn1 != String.Empty && _data.UDAv1 != String.Empty)
            {
                _UDAn1 = _data.UDAn1;
                _UDAv1 = _data.UDAv1;
            }

            if (_data.UDAn2 != String.Empty && _data.UDAv2 != String.Empty)
            {
                _UDAn2 = _data.UDAn2;
                _UDAv2 = _data.UDAv2;
            }

            if (_data.UDAn3 != String.Empty && _data.UDAv3 != String.Empty)
            {
                _UDAn3 = _data.UDAn3;
                _UDAv3 = _data.UDAv3;
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

        private void InsertUDAs(ModelObject modelObject)
        {
            modelObject.SetUserProperty("PRODUCT_DESCR", _DescriptionAttribute);
            modelObject.SetUserProperty("PRODUCT_CODE", _ProductCodeAttribute);

            if (_UDAn1 != String.Empty && _UDAv1 != String.Empty)
                modelObject.SetUserProperty(_UDAn1, _UDAv1);

            if (_UDAn2 != String.Empty && _UDAv2 != String.Empty)
                modelObject.SetUserProperty(_UDAn2, _UDAv2);

            if (_UDAn3 != String.Empty && _UDAv3 != String.Empty)
                modelObject.SetUserProperty(_UDAn3, _UDAv3);
        }
        #endregion
    }
}
