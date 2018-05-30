/* 
<AFN2IRSection>    
  <IRServiceEndpoints>
    <IRServiceEndpoint   Type="STAG" 
                         InternalAID="2100035"
                         Url="https://www.getdirectcredit.com/STG-MismoReportListener/Listener.aspx" 
                         UID="afn"
                         PwD="afntest" />
    <IRServiceEndpoint   Type="PROD" 
                         InternalAID="2007081"
                         Url="https://www.getdirectcredit.com/MismoReportListener/Listener.aspx"
                         UID="IRbilling"
                         PwD="ir@Billing" />
  </IRServiceEndpoints>    
  <AFNRequest   Endpoint="STAG"
                IRRequestSource="1000146"
                TransID="1000000002"
                LenderCID="1301000020"
                ActionType="ClosedLoanTrigger"
                TriggerField="CX.FX.IRREQTRIGGER" />
  <AFNResponse  TrackedDocumentTitle="Informative Research - Invoices"
                AttachmentPrefix="AFN"
                AttachementActivated="true" />
</AFN2IRSection>
*/

using System;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
//
using EllieMae.Encompass.Automation;

namespace AFN2IR.DataContracts
{
    [XmlRoot(ElementName = "IRServiceEndpoint")]
    public class IRServiceEndpoint
    {
        [XmlAttribute(AttributeName = "Type")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "InternalAID")]
        public string InternalAID { get; set; }
        [XmlAttribute(AttributeName = "Url")]
        public string Url { get; set; }
        [XmlAttribute(AttributeName = "UID")]
        public string UID { get; set; }
        [XmlAttribute(AttributeName = "PwD")]
        public string PwD { get; set; }
    }

    [XmlRoot(ElementName = "IRServiceEndpoints")]
    public class IRServiceEndpoints
    {
        [XmlElement(ElementName = "IRServiceEndpoint")]
        public List<IRServiceEndpoint> IRServiceEndpoint { get; set; }

        public IRServiceEndpoints()
        {
            IRServiceEndpoint = new List<IRServiceEndpoint>();
        }
    }

    [XmlRoot(ElementName = "AFNRequest")]
    public class AFNRequest
    {
        [XmlAttribute(AttributeName = "Endpoint")]
        public string Endpoint { get; set; }
        [XmlAttribute(AttributeName = "IRRequestSource")]
        public string IRRequestSource { get; set; }
        [XmlAttribute(AttributeName = "TransID")]
        public string TransID { get; set; }
        [XmlAttribute(AttributeName = "LenderCID")]
        public string LenderCID { get; set; }
        [XmlAttribute(AttributeName = "ActionType")]
        public string ActionType { get; set; }
        [XmlAttribute(AttributeName = "TriggerField")]
        public string TriggerField { get; set; }
    }

    [XmlRoot(ElementName = "AFNResponse")]
    public class AFNResponse
    {
        [XmlAttribute(AttributeName = "TrackedDocumentTitle")]
        public string TrackedDocumentTitle { get; set; }
        [XmlAttribute(AttributeName = "AttachmentPrefix")]
        public string AttachmentPrefix { get; set; }
        [XmlAttribute(AttributeName = "AttachementActivated")]
        public string AttachementActivated { get; set; }
    }

    [XmlRoot(ElementName = "AFN2IRSection")]
    public class AFN2IRSection
    {
        [XmlElement(ElementName = "IRServiceEndpoints")]
        public IRServiceEndpoints IRServiceEndpoints { get; set; }
        [XmlElement(ElementName = "AFNRequest")]
        public AFNRequest AFNRequest { get; set; }
        [XmlElement(ElementName = "AFNResponse")]
        public AFNResponse AFNResponse { get; set; }

        public static string ToXML(AFN2IRSection oAFN2IRSection)
        {
            string oXMLString = String.Empty;

            try
            {

                XmlSerializer oXmlSerializer = new XmlSerializer(typeof(AFN2IRSection));
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
                        oXmlSerializer.Serialize(oXmlTextWriter, oAFN2IRSection, oXSN);
                    }
                    return oStringWriter.ToString();
                }
            }
            catch (Exception Ex)
            {
                Macro.Alert(String.Format("Error: ToXML {0}", Ex.Message));
            }

            return oXMLString;
        }
    }
}

