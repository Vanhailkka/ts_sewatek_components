using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tekla.Structures.Plugins;

namespace Sewatek_components
{
    public class StructuresData
    {
        #region Fields

        //ATTRIBUTES
        //P1a Name
        //P2a Description
        //P3a Product code
        //P4a Numbering start number
        //P5a Finish
        //P6a Numbering Prefix

        [StructuresField("P1a")]
        public string P1a;

        [StructuresField("P2a")]
        public string P2a;

        [StructuresField("P3a")]
        public string P3a;

        [StructuresField("P4a")]
        public string P4a;

        [StructuresField("P5a")]
        public string P5a;

        [StructuresField("P6a")]
        public string P6a;

        /*[StructuresField("P7a")]
        public string P6a;

        [StructuresField("P8a")]
        public string P6a;*/

        [StructuresField("hslab")]
        public string hslab;

        [StructuresField("pWidth")]
        public string pWidth;

        [StructuresField("Content1")]
        public string Content1;

        [StructuresField("posC")]
        public int posC;

        [StructuresField("nHorP")]
        public string nHorP;

        [StructuresField("nVerP")]
        public string nVerP;

        [StructuresField("wpanel")]
        public double wpanel;

        [StructuresField("profpipe")]
        public string profpipe;

        [StructuresField("UDAn1")]
        public string UDAn1;

        [StructuresField("UDAv1")]
        public string UDAv1;

        [StructuresField("UDAn2")]
        public string UDAn2;

        [StructuresField("UDAv2")]
        public string UDAv2;

        [StructuresField("UDAn3")]
        public string UDAn3;

        [StructuresField("UDAv3")]
        public string UDAv3;
        
        #endregion
    }
}
