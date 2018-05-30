/* 

Sample IR Response

<? XML  VERSION='1.0'?>
<RESPONSE_GROUP>
	<RESPONSE ResponseDateTime="10/05/2017 15:06:44" InternalAccountIdentifier="Client ID">
		<KEY _Name="TransID" _Value="1000000001" />
		<KEY _Name="irRequestSource" _Value="Provided by IR" />
		<RESPONSE_DATA>
			<BILLING_RESPONSE _ActionType="RetrieveBillingReport" 
				       OrderIdentifier="1234567890PQ" 
				       LenderCaseIdentifier="100000ABC" 
				       _OrderedDate="10/5/2017 3:06:44 PM" 
				       _BilledDate="10/4/2017 9:27:30 AM" 
				       _BilledAmount="$104">
				<EMBEDDED_FILE _Type="PDF" 
					   _EncodingType="base64" 
					   _Name="BillingReport1234567890PQ" 
					   _Description="Billing Report for order 1234567890PQ">
					<DOCUMENT>Base64 document goes here</DOCUMENT>
				</EMBEDDED_FILE>
			</BILLING_RESPONSE>
		</RESPONSE_DATA>
		<STATUS _Condition="Success" _Code="S0010" _Description="Complete-Product Delivery" />
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
//
using RestSharp;
using RestSharp.Extensions;
using RestSharp.Serializers;
using RestSharp.Deserializers;
using RestSharp.Validation;
//
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;


namespace AFN2IR.DataContracts
{
    [Serializable]
    [XmlRoot(ElementName = "KEY")]
    public class IR_KeyResponse
    {
        [XmlAttribute(AttributeName = "_Name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "_Value")]
        public string Value { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "EMBEDDED_FILE")]
    public class IR_EmbeddedFile
    {
        [XmlElement(ElementName = "DOCUMENT")]
        public string Document { get; set; }
        [XmlAttribute(AttributeName = "_Type")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "_EncodingType")]
        public string EncodingType { get; set; }
        [XmlAttribute(AttributeName = "_Name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "_Description")]
        public string Description { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "BILLING_RESPONSE")]
    public class IR_BillingResponse
    {
        [XmlElement(ElementName = "EMBEDDED_FILE")]
        public IR_EmbeddedFile EmbeddedFile { get; set; }
        [XmlAttribute(AttributeName = "_ActionType")]
        public string ActionType { get; set; }
        [XmlAttribute(AttributeName = "OrderIdentifier")]
        public string OrderIdentifier { get; set; }
        [XmlAttribute(AttributeName = "LenderCaseIdentifier")]
        public string LenderCaseIdentifier { get; set; }
        [XmlAttribute(AttributeName = "_OrderedDate")]
        public string OrderedDate { get; set; }
        [XmlAttribute(AttributeName = "_BilledDate")]
        public string BilledDate { get; set; }
        [XmlAttribute(AttributeName = "_BilledAmount")]
        public string BilledAmount { get; set; }

        public IR_BillingResponse()
        {
            EmbeddedFile = new IR_EmbeddedFile();
        }
    }

    [Serializable]
    [XmlRoot(ElementName = "RESPONSE_DATA")]
    public class IR_ResponseData
    {
        [XmlElement(ElementName = "BILLING_RESPONSE")]
        public IR_BillingResponse BillingResponse { get; set; }

        public IR_ResponseData()
        {
            BillingResponse = new IR_BillingResponse();
        }
    }


    [Serializable]
    [XmlRoot(ElementName = "RESPONSE")]
    public class IR_Response
    {
        [XmlElement(ElementName = "KEY")]
        public List<IR_KeyResponse> KeyResponse { get; set; }
        [XmlElement(ElementName = "RESPONSE_DATA")]
        public IR_ResponseData ResponseData { get; set; }
        [XmlElement(ElementName = "STATUS")]
        public IR_Status Status { get; set; }
        [XmlAttribute(AttributeName = "ResponseDateTime")]
        public string ResponseDateTime { get; set; }
        [XmlAttribute(AttributeName = "InternalAccountIdentifier")]
        public string InternalAccountIdentifier { get; set; }

        public IR_Response()
        {
            KeyResponse = new List<IR_KeyResponse>();
            ResponseData = new IR_ResponseData();
            Status = new IR_Status();
        }
    }

    [Serializable]
    [XmlRoot(ElementName = "RESPONSE_GROUP")]
    public class IR_ResponseGroup
    {
        [XmlElement(ElementName = "RESPONSE")]
        public IR_Response Response { get; set; }

        public IR_ResponseGroup()
        {
            Response = new IR_Response();
        }

        override public string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public string ToXMLString()
        {
            return String.Empty;
        }

        public static IR_ResponseGroup CreateInstance(byte[] oRespAsBytes)
        {
            IR_ResponseGroup oResponseGroup = null;

            using (MemoryStream oRespAsByteStream = new MemoryStream(oRespAsBytes))
            {
                System.Xml.Serialization.XmlSerializer oXmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(IR_ResponseGroup));

                oResponseGroup = (IR_ResponseGroup)oXmlSerializer.Deserialize(oRespAsByteStream);
            }

            return oResponseGroup;
        }
    }
}