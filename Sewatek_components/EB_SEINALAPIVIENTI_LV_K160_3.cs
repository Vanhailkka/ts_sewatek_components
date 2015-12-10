﻿using System;
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
    [Plugin("EB_SEINALAPIVIENTI_LV_K160_3")]
    [PluginUserInterface(EB_SEINALAPIVIENTI_LV_K160_3_INP.SLVK160component)]

    public partial class EB_SEINALAPIVIENTI_LV_K160_3 : PluginBase
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
        public EB_SEINALAPIVIENTI_LV_K160_3(StructuresData PluginData)
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

        public override bool Run(List<InputDefinition> input)
        {
            try
            {
                var currentPlane = _Model.GetWorkPlaneHandler().GetCurrentTransformationPlane();

                GetValuesFromDialog();

                var points = (ArrayList)input[0].GetInput();
                var startPoint = points[0] as Point;
                var endPoint = points[1] as Point;

                var AxisLine = new LineSegment(startPoint, endPoint);
                var XAxisI = AxisLine.GetDirectionVector();
                var YAxisI = new Vector(0, 0, 1);

                CoordSysI = new CoordinateSystem(startPoint, XAxisI, YAxisI);
                var localPlane = new TransformationPlane(CoordSysI);

                Beam Putki;

                _Model.GetWorkPlaneHandler().SetCurrentTransformationPlane(localPlane);

                double Xdist = 0.0;

                for (int j = 1; j <= 5; j++)
                {
                    var pt = new Point(Xdist, 0.0, 0.0);
                   if (j == 1)
                   {
                      Putki = CreatePutki(pt, "100");
                   }
                   else
                   {
                      Putki = CreatePutki(pt, "0");
                   }
                   Parts.Add(Putki);
                    Welds.Add(new Weld());
                    Xdist += _Xd;
                }

                CreatePlateM(startPoint);
                var putkiMain = Parts[0] as Beam;
                InsertUDAs(ref putkiMain);
                CreateWelds(Parts, Welds);

                _Model.GetWorkPlaneHandler().SetCurrentTransformationPlane(currentPlane);

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
            if (!IsDefaultValue(_Data.wpanel))
                _PanelWidth = _Data.wpanel;
            else
                _PanelWidth = 200;

            if (!IsDefaultValue(_Data.P1a))
                _NameAttribute = _Data.P1a;
            else
                _NameAttribute = "SEINALAPIVIENTI_LV_K160_3";

            if (!IsDefaultValue(_Data.P2a))
                _DescriptionAttribute = _Data.P2a;
            else
                _DescriptionAttribute = "SEWATEK-SEINALAPIVIENTI LAMPO– JA VESIPUTKILLE";

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
                _AspreAttribut1 = "EB_LV_K160_3";
                
            _MaterialAttribute = "Misc_undefined";

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

        private void SetDefaultEmbedObjectAttributes(Part Object, string partClass)
        {
            Object.PartNumber.Prefix = _AspreAttribut1;
            Object.PartNumber.StartNumber = Convert.ToInt32(_AsnumAttribut1);
            Object.AssemblyNumber.Prefix = _AspreAttribut1;
            Object.AssemblyNumber.StartNumber = Convert.ToInt32(_AsnumAttribut1);

            Object.Name = _NameAttribute;
            Object.Material.MaterialString = _MaterialAttribute;
            Object.Finish = _FinishAttribute;
            Object.Class = partClass;
        }

        private void InsertUDAs(ref Beam Object)
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
