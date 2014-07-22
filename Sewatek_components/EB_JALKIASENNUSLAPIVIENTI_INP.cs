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
    partial class EB_JALKIASENNUSLAPIVIENTI_INP
    {
        public const string JALcomponent = @"" +
        @"page(""TeklaStructures"","""")" + "\n" +
        @"{" + "\n" +
        @"	detail(1, EB_JALKIASENNUSLAPIVIENTI)" + "\n" +
        @"	{" + "\n" +
        @"		helpurl(""sewatek_help.chm::/jalkiasennuslapivienti.htm"")" + "\n" +
        @"" + "\n" +
        @"		tab_page("""", j_Parameters, 1)" + "\n" +
        @"		{" + "\n" +
        @"			picture(""sewatek.bmp"",,,470,0)" + "\n" +

        @"          attribute("""", ""Numbering series"", label2,""%s"",  none, none, ""0.0"", ""0.0"",10,1)" + "\n" +
		@"          parameter("""", ""P6a"", string, text, 180,30,150)" + "\n" +
        @"          attribute("""", ""Prefix"", label2,""%s"",  none, none, ""0.0"", ""0.0"",106,30)" + "\n" +
		@"          parameter("""", ""P4a"", integer, number, 180,60,150)" + "\n" +
        @"          attribute("""", ""Start number"", label2,""%s"",  none, none, ""0.0"", ""0.0"",59,60)" + "\n" +

        @"          picture(""line"", 200,10,10,110)"  + "\n" +
        @"          attribute("""", ""Attributes"", label2,""%s"",  none, none, ""0.0"", ""0.0"",10,121)"  + "\n" +
        @"          attribute("""", ""Name"", label2,""%s"",  none, none, ""0.0"", ""0.0"",106,145)" + "\n" +
        @"          parameter("""", ""P1a"", string, text, 180,145,400)"  + "\n" +
        @"          attribute("""", ""Description"", label2,""%s"",  none, none, ""0.0"", ""0.0"",65,175)" + "\n" +
        @"          parameter("""", ""P2a"", string, text, 180,175,400)"  + "\n" +
        @"          attribute("""", ""Finish"", label2,""%s"",  none, none, ""0.0"", ""0.0"",102,205)" + "\n" +
        @"          parameter("""", ""P5a"", string, text, 180,205,400)"  + "\n" +
	    
        @"          picture(""line"", 200,10,10,230)"  + "\n" +
        @"          attribute("""", ""Product code"", label2,""%s"",  none, none, ""0.0"", ""0.0"",59,264)" + "\n" +
        @"          parameter("""", ""P3a"", string, text, 180,264,400)" + "\n" +
        @"          attribute("""", ""Panel widht"", label2,""%s"",  none, none, ""0.0"", ""0.0"",59,294)" + "\n" +
        @"          parameter("""", ""wpanel"", distance, number, 180,294,400)" + "\n" +
        @"          attribute("""", ""Pipe profile"", label2,""%s"",  none, none, ""0.0"", ""0.0"",59,324)" + "\n" +
        @"          parameter("""", ""profpipe"", profile, text, 180,324,400)" + "\n" +
        
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

