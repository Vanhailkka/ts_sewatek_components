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
    [Plugin("EB_ONTELOLAATTALAPIVIENTI")]
    [PluginUserInterface(EB_ONTELOLAATTALAPIVIENTI_INP.ONTcomponent)]

    public partial class EB_ONTELOLAATTALAPIVIENTI : PluginBase
    {
        #region Fields
        private StructuresData _Data;
        private Model _Model;
        CoordinateSystem CoordSysI;
        Vector Vec1;
        Vector Vec2;
        TransformationPlane localPlane = null;
        ContourPlate Ylatuki;
        ContourPlate Alatuki;

        List<Weld> Welds = new List<Weld>
                    {
                        new Weld{},
                        new Weld{},
                        new Weld{},
                        new Weld{},
                        new Weld{},
                        new Weld{},
                        new Weld{},
                        new Weld{},
                    };

        List<ModelObject> Parts = new List<ModelObject>();

        private string _AspreAttribut1 = string.Empty;
        private string _AsnumAttribut1 = string.Empty;
        private string _NameAttribute = string.Empty;
        private string _DescriptionAttribute = string.Empty;
        private string _ProductCodeAttribute = string.Empty;

        private string _FinishAttribute = string.Empty;
        private string _MaterialAttribute = string.Empty;
        private string _PosAtdepthAttribute = string.Empty;
        private double _SHeight;
        private string _Content1;
        private int _PositionParameter;
        private string _UDAn1 = string.Empty;
        private string _UDAv1 = string.Empty;
        private string _UDAn2 = string.Empty;
        private string _UDAv2 = string.Empty;
        private string _UDAn3 = string.Empty;
        private string _UDAv3 = string.Empty;

        private const double _L = 278;
        private const double _L1 = 82;
        private const double _B = 139;
        private const double _B1 = 82;
        private const double _C1 = 45;
        private const double _C2 = 70;
        private const double _cogX = 28.5;
        private const double _cogY = 20.03;
        double Radians = Math.PI;
        double Radians1 = Math.PI / 2;

        #endregion

        #region Constructor
        public EB_ONTELOLAATTALAPIVIENTI(StructuresData PluginData)
        {
            this._Data = PluginData;
            _Model = new Model();
        }
        #endregion

        #region Overrides
        public override List<InputDefinition> DefineInput()
        {
            Picker POKPicker = new Picker();
            List<InputDefinition> PointList = new List<InputDefinition>();
            ArrayList PickedPoints = POKPicker.PickPoints(Picker.PickPointEnum.PICK_TWO_POINTS);

            PointList.Add(new InputDefinition(PickedPoints));

            return PointList;
        }

        public override bool Run(List<InputDefinition> Input)
        {
            try
            {
                TransformationPlane CurrentPlane = _Model.GetWorkPlaneHandler().GetCurrentTransformationPlane();

                GetValuesFromDialog();

                ArrayList Points = (ArrayList)Input[0].GetInput();
                Point StartPoint = Points[0] as Point;
                Point EndPoint = Points[1] as Point;
                
                LineSegment AxisLine = new LineSegment(StartPoint, EndPoint);
                Vector YAxisI = new Vector(0, 0, 1);
                Vector XAxisI = AxisLine.GetDirectionVector();
                CoordSysI = new CoordinateSystem(StartPoint, XAxisI, YAxisI);

                if (_PositionParameter == 0 || _PositionParameter == 1)
                {
                    Vec2 = CoordSysI.AxisY;
                    CoordinateSystem CoordSysI1 = ChangeCoordSys(Vec2, CoordSysI, StartPoint, Radians);

                    Vec1 = CoordSysI1.AxisX;
                    localPlane = SetLocalPlane(Vec1, CoordSysI1, StartPoint, Radians1);
                    _Model.GetWorkPlaneHandler().SetCurrentTransformationPlane(localPlane);

                    if (_PositionParameter == 1)
                    {
                        Ylatuki = CreateYlatukiPos1(StartPoint);
                        CreateCutYlatukiMPos1(StartPoint, Ylatuki);
                    }
                    else
                    {
                        Ylatuki = CreateYlatuki(StartPoint);
                        CreateCutYlatukiM(StartPoint, Ylatuki);
                    }
                    Alatuki = CreateAlatuki(StartPoint);
                    CreateCutAlatukiM(StartPoint, Alatuki);
                    CreateBasesM(StartPoint);
                    CreatePutketM(StartPoint);
                }
                else
                {
                    Vec1 = CoordSysI.AxisX;
                    localPlane = SetLocalPlane(Vec1, CoordSysI, StartPoint, Radians1);
                    _Model.GetWorkPlaneHandler().SetCurrentTransformationPlane(localPlane);

                    if (_PositionParameter == 2)
                    {
                        Ylatuki = CreateYlatuki(StartPoint);
                        CreateCutYlatukiM(StartPoint, Ylatuki);
                        Alatuki = CreateAlatukiV(StartPoint);
                        CreateCutAlatukiMV(StartPoint, Alatuki);
                        CreateBasesMV(StartPoint);
                        CreatePutketMV(StartPoint);
                    }
                    else
                    {
                        Ylatuki = CreateYlatukiPos1(StartPoint);
                        CreateCutYlatukiMPos1(StartPoint, Ylatuki);
                        Alatuki = CreateAlatukiVPos1(StartPoint);
                        CreateCutAlatukiMVPos1(StartPoint, Alatuki);
                        CreateBasesMVPos1(StartPoint);
                        CreatePutketMVPos1(StartPoint);
                    }
                }

                Parts.Add(Ylatuki);
                Parts.Add(Alatuki);
                Beam MainPart = Parts[4] as Beam;
                InsertUDAs(ref MainPart);
                CreateWelds(Parts, Welds);

                _Model.GetWorkPlaneHandler().SetCurrentTransformationPlane(CurrentPlane);

            }
            catch (Exception Exc)
            {
                MessageBox.Show(Exc.Message);
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
            if (!IsDefaultValue(_Data.hslab))
                _SHeight = Convert.ToDouble(_Data.hslab);
            else
                _SHeight = 200;

            if (!IsDefaultValue(_Data.Content1))
                _Content1 = _Data.Content1;
            else
                _Content1 = "empty";

            _PositionParameter = _Data.posC;

            if (!IsDefaultValue(_Data.P1a))
                _NameAttribute = _Data.P1a;
            else
                _NameAttribute = "ONTELOLAATTALAPIVIENTI";

            if (!IsDefaultValue(_Data.P2a))
                _DescriptionAttribute = _Data.P2a;
            else
                _DescriptionAttribute = "SEWATEK-ONTELOLAATTALAPIVIENTI";

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
                _AspreAttribut1 = "EB_ONT";
                
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

        private void SetDefaultEmbedObjectAttributes(ref ContourPlate Object)
        {
            Object.PartNumber.Prefix = _AspreAttribut1;
            Object.PartNumber.StartNumber = Convert.ToInt32(_AsnumAttribut1);
            Object.AssemblyNumber.Prefix = _AspreAttribut1;
            Object.AssemblyNumber.StartNumber = Convert.ToInt32(_AsnumAttribut1);

            Object.Name = _NameAttribute;
            Object.Material.MaterialString = _MaterialAttribute;
            Object.Finish = _FinishAttribute;
            Object.Class = "100";
        }

        private void SetDefaultEmbedObjectAttributes(ref Beam Object)
        {
            Object.PartNumber.Prefix = _AspreAttribut1;
            Object.PartNumber.StartNumber = Convert.ToInt32(_AsnumAttribut1);
            Object.AssemblyNumber.Prefix = _AspreAttribut1;
            Object.AssemblyNumber.StartNumber = Convert.ToInt32(_AsnumAttribut1);

            Object.Name = _NameAttribute;
            Object.Material.MaterialString = _MaterialAttribute;
            Object.Finish = _FinishAttribute;
            Object.Class = "100";
        }

        private void InsertUDAs(ref Beam Object)
        {
            Object.SetUserProperty("SEWATEK_CONTENT_1", _Content1);
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
