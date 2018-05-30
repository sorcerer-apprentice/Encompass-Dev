/* 
Sample IR Request

<? XML  VERSION='1.0'?>
<REQUEST_GROUP>
	<REQUESTING_PARTY  Name="MortgageTech">
		<CONTACT_DETAIL  Name="Wilton">
			<CONTACT_POINT  RoleType="Cell"  Type="Phone"  Value="510.600.9577" />
			<CONTACT_POINT  RoleType="Work"  Type="Fax"  Value="888.217.2299" />
			<CONTACT_POINT  RoleType="Work"  Type="Email"    Value="wilton@mortgagetech.com"/>
		</CONTACT_DETAIL>
		<PREFERRED_RESPONSE  Format="PDF"  Method="File"  UseEmbeddedFileIndicator="Y" />
	</REQUESTING_PARTY>
	<SUBMITTING_PARTY  Name="AFN" />
		<REQUEST RequestDatetime="2017-11-03T00:42:46"
		   InternalAccountIdentifier="2100035"
		   LoginAccountIdentifier="afn" 
		   LoginAccountPassword="afntest">
			<KEY  Name ="TransID"  Value="1000000002" />
			<KEY  Name ="irRequestSource"  Value="1000146" />
		<REQUEST_DATA>
			<BILLING_REQUEST   	ActionType="ClosedLoanTrigger" 
								OrderIdentifier="1010744996PQ"
								LenderCaseIdentifier="1301000020" />
		</REQUEST_DATA>
		</REQUEST>
</REQUEST_GROUP>

*/

using System;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
//
using RestSharp;
using RestSharp.Extensions;
//using RestSharp.Serializers;
using RestSharp.Deserializers;
using RestSharp.Validation;
//
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
// 
using AFN2IR.Helpers;
//
using EllieMae.Encompass.Automation;

namespace AFN2IR.DataContracts
{

    [XmlRoot(ElementName = "KEY")]
    public class IR_KeyRequest
    {
        [XmlAttribute(AttributeName = "_Name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "_Value")]
        public string Value { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "CONTACT_POINT")]
    public class IR_ContactPoint
    {
        [XmlAttribute(AttributeName = "_RoleType")]
        public string RoleType { get; set; }
        [XmlAttribute(AttributeName = "_Type")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "_Value")]
        public string Value { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "CONTACT_DETAIL")]
    public class IR_ContactDetail
    {
        [XmlElement(ElementName = "CONTACT_POINT")]
        public List<IR_ContactPoint> ContactPoint { get; set; }
        [XmlAttribute(AttributeName = "_Name")]
        public string Name { get; set; }

        public IR_ContactDetail()
        {
            ContactPoint = new List<IR_ContactPoint>();
        }
    }

    [XmlRoot(ElementName = "PREFERRED_RESPONSE")]
    public class IR_PreferrdResponse
    {
        [XmlAttribute(AttributeName = "_Format")]
        public string Format { get; set; }
        [XmlAttribute(AttributeName = "_Method")]
        public string Method { get; set; }
        [XmlAttribute(AttributeName = "_UseEmbeddedFileIndicator")]
        public string UseEmbeddedFileIndicator { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "REQUESTING_PARTY")]
    public class IR_RequestingParty
    {
        [XmlElement(ElementName = "CONTACT_DETAIL")]
        public IR_ContactDetail ContactDetail { get; set; }
        [XmlElement(ElementName = "PREFERRED_RESPONSE")]
        public IR_PreferrdResponse PreferrdResponse { get; set; }
        [XmlAttribute(AttributeName = "_Name")]
        public string Name { get; set; }

        public IR_RequestingParty()
        {
            ContactDetail = new IR_ContactDetail();
            PreferrdResponse = new IR_PreferrdResponse();
        }
    }

    [XmlRoot(ElementName = "SUBMITTING_PARTY")]
    public class IR_SubmittingParty
    {
        [XmlAttribute(AttributeName = "_Name")]
        public string Name { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "BILLING_REQUEST")]
    public class IR_BillingRequest
    {
        [XmlAttribute(AttributeName = "_ActionType")]
        public string ActionType { get; set; }
        [XmlAttribute(AttributeName = "OrderIdentifier")]
        public string OrderIdentifier { get; set; }
        [XmlAttribute(AttributeName = "LenderCaseIdentifier")]
        public string LenderCaseIdentifier { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "REQUEST_DATA")]
    public class IR_RequestData
    {
        [XmlElement(ElementName = "BILLING_REQUEST")]
        public IR_BillingRequest BillingRequest { get; set; }

        public IR_RequestData()
        {
            BillingRequest = new IR_BillingRequest();
        }

    }

    [Serializable]
    [XmlRoot(ElementName = "REQUEST")]
    public class IR_Request
    {
        [XmlElement(ElementName = "KEY")]
        public List<IR_KeyRequest> KeyRequest { get; set; }
        [XmlElement(ElementName = "REQUEST_DATA")]
        public IR_RequestData RequestData { get; set; }
        [XmlAttribute(AttributeName = "RequestDatetime")]
        public string RequestDateTime { get; set; }
        [XmlAttribute(AttributeName = "InternalAccountIdentifier")]
        public string InternalAccountIdentifier { get; set; }
        [XmlAttribute(AttributeName = "LoginAccountIdentifier")]
        public string LoginAccountIdentifier { get; set; }
        [XmlAttribute(AttributeName = "LoginAccountPassword")]
        public string LoginAccountPassword { get; set; }

        public IR_Request()
        {
            KeyRequest = new List<IR_KeyRequest>();
            RequestData = new IR_RequestData();
        }
    }

    [Serializable]
    [XmlRoot(ElementName = "REQUEST_GROUP")]
    public class IR_RequestGroup
    {
        [XmlElement(ElementName = "REQUESTING_PARTY")]
        public IR_RequestingParty RequestingParty { get; set; }
        [XmlElement(ElementName = "SUBMITTING_PARTY")]
        public IR_SubmittingParty SubmittingParty { get; set; }
        [XmlElement(ElementName = "REQUEST")]
        public IR_Request Request { get; set; }

        public IR_RequestGroup()
        {
            RequestingParty = new IR_RequestingParty();
            SubmittingParty = new IR_SubmittingParty();
            Request = new IR_Request();
        }

        public static string ToXML(AFN2IRSection oAFN2IRSection, string oInternalAID, string oUID, string oPWD, string oLenderCID, string oOrderID)
        {
            string oIRMessageReq            = null;
            IR_RequestGroup oIRRequestGrp   = null;

            try
            {
                oIRRequestGrp = new IR_RequestGroup();
                oIRRequestGrp.RequestingParty.Name = "AFN";
                oIRRequestGrp.RequestingParty.ContactDetail.ContactPoint.Add(new IR_ContactPoint()
                {
                    RoleType    = "Work",
                    Type        = "Cell",
                    Value       = "800.248.4760"
                });

                oIRRequestGrp.SubmittingParty.Name                      = "AFN";
                oIRRequestGrp.RequestingParty.PreferrdResponse.Format   = "PDF";
                oIRRequestGrp.RequestingParty.PreferrdResponse.Method   = "File";
                oIRRequestGrp.RequestingParty.PreferrdResponse.UseEmbeddedFileIndicator = "Y";
                oIRRequestGrp.Request.RequestDateTime                   = string.Format("{0:MM/dd/yyyy HH:mm:ss}", DateTime.Now);
                oIRRequestGrp.Request.InternalAccountIdentifier         = oInternalAID;
                oIRRequestGrp.Request.LoginAccountIdentifier            = oUID;
                oIRRequestGrp.Request.LoginAccountPassword              = oPWD;
                oIRRequestGrp.Request.KeyRequest.Add(new IR_KeyRequest()
                {
                    Name = "TransID",
                    Value = oAFN2IRSection.AFNRequest.TransID
                });
                oIRRequestGrp.Request.KeyRequest.Add(new IR_KeyRequest()
                {
                    Name = "irRequestSource",
                    Value = oAFN2IRSection.AFNRequest.IRRequestSource
                });
                oIRRequestGrp.Request.RequestData.BillingRequest.ActionType             = oAFN2IRSection.AFNRequest.ActionType;
                oIRRequestGrp.Request.RequestData.BillingRequest.OrderIdentifier        = oOrderID;
                oIRRequestGrp.Request.RequestData.BillingRequest.LenderCaseIdentifier   = oLenderCID;

                XmlSerializer oXmlSerializer = new XmlSerializer(typeof(IR_RequestGroup));
                XmlWriterSettings oXmlWriterSettings = new XmlWriterSettings()
                {
                    Encoding = Encoding.UTF8, 
                    Indent = false,
                    OmitXmlDeclaration = true
                };

                using (StringWriter oStringWriter = new StringWriter())
                {
                    using (XmlWriter oXmlTextWriter = XmlWriter.Create(oStringWriter, oXmlWriterSettings))
                    {
                        XmlSerializerNamespaces oXSN = new XmlSerializerNamespaces();
                        oXSN.Add("", "");
                        oXmlSerializer.Serialize(oXmlTextWriter, oIRRequestGrp, oXSN);
                    }
                    oIRMessageReq = oStringWriter.ToString();
                }
            }
            catch (Exception Ex)
            {
                Macro.Alert(String.Format("Error: ToXML IRMessage Request", Ex.Message));
            }
            finally
            {
                oIRRequestGrp = null;
            }
            return oIRMessageReq;
        }
    }

}