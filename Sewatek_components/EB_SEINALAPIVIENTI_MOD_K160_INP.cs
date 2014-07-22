using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

namespace Sewatek_components
{
    class EB_SEINALAPIVIENTI_MOD_K160_INP
    {
        public const string SMODK160component = @"" +
            @"page(""TeklaStructures"","""")" + "\n" +
            @"{" + "\n" +
            @"	detail(1, EB_SEINALAPIVIENTI_MOD_K160)" + "\n" +
            @"	{" + "\n" +
            @"		helpurl(""sewatek_help.chm::/seinalapivienti_mod.htm"")" + "\n" +
            @"" + "\n" +
            @"		tab_page("""", j_Parameters, 1)" + "\n" +
            @"		{" + "\n" +
            @"			picture(""sewatek.bmp"",,,470,0)" + "\n" +
            @"			attribute("""", ""Numbering series"", label2,""%s"",  none, none, ""0.0"", ""0.0"",10,1)" + "\n" +
            @"			parameter("""", ""P6a"", string, text, 180,30,140)" + "\n" +
            @"			attribute("""", ""Prefix"", label2,""%s"",  none, none, ""0.0"", ""0.0"",108,30)" + "\n" +
            @"			parameter("""", ""P4a"", string, text, 180,60,140)" + "\n" +
            @"			attribute("""", ""Start number"", label2,""%s"",  none, none, ""0.0"", ""0.0"",58,60)" + "\n" +
            @"			picture(""line"", 200,10,10,110)" + "\n" +
            @"			" + "\n" +
            @"			attribute("""", ""Attributes"", label2,""%s"",  none, none, ""0.0"", ""0.0"",10,121)" + "\n" +
            @"			attribute("""", ""Name"", label2,""%s"",  none, none, ""0.0"", ""0.0"",106,145)" + "\n" +
            @"			parameter("""", ""P1a"", string, text, 180,145,400)" + "\n" +
            @"			attribute("""", ""Description"", label2,""%s"",  none, none, ""0.0"", ""0.0"",65,175)" + "\n" +
            @"			parameter("""", ""P2a"", string, text, 180,175,400)" + "\n" +
            @"			attribute("""", ""Finish"", label2,""%s"",  none, none, ""0.0"", ""0.0"",102,205)" + "\n" +
            @"			parameter("""", ""P5a"", string, text, 180,205,400)" + "\n" +
            @"			" + "\n" +
            @"			picture(""line"", 200,10,10,255)" + "\n" +
            @"          attribute("""", ""Product code"", label2,""%s"",  none, none, ""0.0"", ""0.0"",59,290)" + "\n" +
            @"          parameter("""", ""P3a"", string, text, 180,290,400)" + "\n" +
            @"			attribute("""", ""Panel width"", label2,""%s"",  none, none, ""0.0"", ""0.0"",64,320)" + "\n" +
            @"			parameter("""", ""wpanel"", distance, number, 180,320,400)" + "\n" +
            @"			attribute("""", ""N°. horizontal parts"", label2,""%s"",  none, none, ""0.0"", ""0.0"",11,350)" + "\n" +
            @"			parameter("""", ""nHorP"", string, text, 180,350,400)" + "\n" +
            @"			attribute("""", ""N°. vertical parts"", label2,""%s"",  none, none, ""0.0"", ""0.0"",34,380)" + "\n" +
            @"			parameter("""", ""nVerP"", string, text, 180,380,400)" + "\n" +
            @"		}" + "\n" +
            @"      tab_page("""", j_Attributes, 2)" + "\n" +
            @"		{" + "\n" +
            @"			attribute("""", ""User defined attributes"", label2,""%s"",  none, none, ""0.0"", ""0.0"",10,1)" + "\n" +
            @"			attribute("""", ""UDA name"", label2,""%s"",  none, none, ""0.0"", ""0.0"",215,25)" + "\n" +
            @"			attribute("""", ""UDA value"", label2,""%s"",  none, none, ""0.0"", ""0.0"",485,25)" + "\n" +
            @"          parameter("""", ""UDAn1"", string, text, 115,55,280)" + "\n" +
            @"			parameter("""", ""UDAv1"", string, text, 460,55,140)" + "\n" +
            @"          parameter("""", ""UDAn2"", string, text, 115,85,280)" + "\n" +
            @"			parameter("""", ""UDAv2"", string, text, 460,85,140)" + "\n" +
            @"          parameter("""", ""UDAn3"", string, text, 115,115,280)" + "\n" +
            @"			parameter("""", ""UDAv3"", string, text, 460,115,140)" + "\n" +
            @"		}" + "\n" +
            @"	}" + "\n" +
            @"}" + "\n";

    }
}

//P1a numbering prefix
//P2a
//P3a
//P4a
//P5a
//P6a

