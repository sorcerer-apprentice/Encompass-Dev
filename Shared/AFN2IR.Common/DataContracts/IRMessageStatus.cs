/*

<? XML  VERSION='1.0'?>
<RESPONSE_GROUP>
	<RESPONSE ResponseDateTime = "10/10/2017 12:28:14" >

        < STATUS _Code="E0030" 
		        _Condition="Error" 
		        _Description="An invalid data item-OrderIdentifier has been submitted." />
	</RESPONSE>
</RESPONSE_GROUP>

*/
using System;
using System.IO;
using System.Text;
using System.Net;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace AFN2IR.DataContracts
{

    [Serializable]
    [XmlRoot(ElementName = "STATUS")]
    public class IR_Status
    {
        [XmlAttribute(AttributeName = "_Condition")]
        public string Condition { get; set; }
        [XmlAttribute(AttributeName = "_Code")]
        public string Code { get; set; }
        [XmlAttribute(AttributeName = "_Description")]
        public string Description { get; set; }
    }
}