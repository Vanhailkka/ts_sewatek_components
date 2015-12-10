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
    [Plugin("EB_JALKIASENNUSLAPIVIENTI")]
    [PluginUserInterface(EB_JALKIASENNUSLAPIVIENTI_INP.JALcomponent)]

    public partial class EB_JALKIASENNUSLAPIVIENTI : PluginBase
    {
        #region Fields
        private StructuresData _Data;
        private Model _Model;
        CoordinateSystem CoordSysI;

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
        private string _Content1;
        private string _Profile = string.Empty;
        private double _PanelWidth = 0.0;
        private string _UDAn1 = string.Empty;
        private string _UDAv1 = string.Empty;
        private string _UDAn2 = string.Empty;
        private string _UDAv2 = string.Empty;
        private string _UDAn3 = string.Empty;
        private string _UDAv3 = string.Empty;

        #endregion

        #region Constructor
        public EB_JALKIASENNUSLAPIVIENTI(StructuresData PluginData)
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

        private bool InsertObject(Beam InputBeam)
        {
            bool Result = false;

            if (!InputBeam.Insert())
            {
                MessageBox.Show("Insert failed!");
                InputBeam = null;
            }
            else
            {
                Result = true;
            }

            return Result;
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
                Vector XAxisI = AxisLine.GetDirectionVector();
                Vector YAxisI = new Vector(0, 0, 1);

                CoordSysI = new CoordinateSystem(StartPoint, XAxisI, YAxisI);
                TransformationPlane localPlane = new TransformationPlane(CoordSysI);

                Beam Putki = new Beam();
                Beam Putki1 = new Beam();
                Beam Putki2 = new Beam();

                _Model.GetWorkPlaneHandler().SetCurrentTransformationPlane(localPlane);

                SetDefaultEmbedObjectAttributes(ref Putki);

                //main part
                Putki.StartPoint = new Point(0, 0, 0);
                Putki.EndPoint = new Point(0, 0, -_PanelWidth);
                InsertObject(Putki);
                InsertUDAs(ref Putki);
                Parts.Add(Putki);

                //Laippa 1
                Putki1.StartPoint = new Point(0, 0, 0);
                Putki1.EndPoint = new Point(0, 0, 2);
                Putki1.Profile.ProfileString = "D112.5";
                Putki1.Class = "100";
                Putki1.Position.Plane = Position.PlaneEnum.MIDDLE;
                Putki1.Position.Rotation = Position.RotationEnum.FRONT;
                Putki1.Position.Depth = Position.DepthEnum.MIDDLE;
                InsertObject(Putki1);
                Parts.Add(Putki1);

                //Laippa 2
                Putki2.StartPoint = new Point(0, 0, -_PanelWidth);
                Putki2.EndPoint = new Point(0, 0, -_PanelWidth - 2);
                Putki2.Profile.ProfileString = "D112.5";
                Putki2.Class = "100";
                Putki2.Position.Plane = Position.PlaneEnum.MIDDLE;
                Putki2.Position.Rotation = Position.RotationEnum.FRONT;
                Putki2.Position.Depth = Position.DepthEnum.MIDDLE;
                InsertObject(Putki2);
                Parts.Add(Putki2);

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
        /// In order to have consistent functionality in connections, seams and details which
        ///are using embeds, the parameters must have common names: 
        ///P1a for Product name
        ///P2a for Description
        ///P3a for Product code
        ///P4a for Start number
        ///P5a for Finish 
        ///P6a for Numbering prefix.
        ///For embeds with varying length or area the following parameters are used: 
        ///P7a for Product unit 
        ///P8a for Product weight
        /// 
        /// </summary>
        private void GetValuesFromDialog()
        {
            if (!IsDefaultValue(_Data.Content1))
                _Content1 = _Data.Content1;
            else
                _Content1 = "empty";

            if (!IsDefaultValue(_Data.P1a))
                _NameAttribute = _Data.P1a;
            else
                _NameAttribute = "JALKIASENNUSLAPIVIENTI";

            if (!IsDefaultValue(_Data.P2a))
                _DescriptionAttribute = _Data.P2a;
            else
                _DescriptionAttribute = "SEWATEK-JALKIASENNUSLAPIVIENTI";

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
                _AspreAttribut1 = "EB_JAL";

            _MaterialAttribute = "Misc_Undefined";

            if (!IsDefaultValue(_Data.wpanel))
                _PanelWidth = _Data.wpanel;
            else
                _PanelWidth = 300;

            if (!IsDefaultValue(_Data.profpipe))
                _Profile = _Data.profpipe;
            else
                _Profile = "PD44*2";

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
            Object.Profile.ProfileString = _Profile;

            Object.Position.Plane = Position.PlaneEnum.MIDDLE;
            Object.Position.Rotation = Position.RotationEnum.FRONT;
            Object.Position.Depth = Position.DepthEnum.MIDDLE;
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
        #endregion
    }
}
