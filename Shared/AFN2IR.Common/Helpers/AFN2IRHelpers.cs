using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;
using System.Net;
using System.Linq;
//
using Newtonsoft.Json.Serialization;

// EM Required
using EllieMae.Encompass.Automation;
using EllieMae.Encompass.Forms;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.BusinessEnums;
using EllieMae.Encompass.BusinessObjects;
using EllieMae.Encompass.BusinessObjects.Loans;
using EllieMae.Encompass.BusinessObjects.Loans.Logging;
using EllieMae.Encompass.Collections;
//
using AFN2IR.DataContracts;

namespace AFN2IR.Helpers
{
    public class EncodedStringWriter : StringWriter
    {
        private Encoding _Encoding;

        public EncodedStringWriter(Encoding aSpecificEncoding) : base()
        {
            _Encoding = aSpecificEncoding;
        }

        public override Encoding Encoding
        {
            get { return _Encoding; }
        }
    }

    public class AFN2IRHelper
    {
        public static readonly Encoding LocalEncoding = Encoding.UTF8;
        public static readonly string AppConfigName   = "AFN2IRApp.config";

        public static T ConvertXMLToObject<T>(byte[] oXMLContent)
        {
            List<T> oObjects = new List<T>();

            try
            {
                using (MemoryStream oByteStream = new MemoryStream(oXMLContent))
                {
                    var oSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));

                    T oObject = (T)oSerializer.Deserialize(oByteStream);

                    oObjects.Add(oObject);
                }
            }
            catch (Exception Ex)
            {
               Macro.Alert(String.Format("Error: ConvertXMLToObject:{0} ({1})", Ex.Message, Ex.InnerException));
            }

            return (T)oObjects[0];
        }

        public static AFN2IRSection ReadAppConfig()
        {
            DataObject oDaO = null;

            AFN2IRSection oAppConfig = null;

            try
            {
                oDaO = EncompassApplication.Session.DataExchange.GetCustomDataObject(AFN2IRHelper.AppConfigName);

                if (oDaO != null && oDaO.Data != null)
                {

                    using (MemoryStream oXMLDataStream= new MemoryStream(oDaO.Data))
                    {
                        var oSerializer = new System.Xml.Serialization.XmlSerializer(typeof(AFN2IRSection));

                        oAppConfig = (AFN2IRSection)oSerializer.Deserialize(oXMLDataStream);
                    }

                }
            }
            catch (Exception Ex)
            {
                Macro.Alert(String.Format("Error: Reading AppConfig - {0}", Ex.Message));
            }
            finally
            {
                if (oDaO != null)
                    oDaO.Dispose();
            }

            return oAppConfig;
        }

        public static void WriteAppConfig(AFN2IRSection oAFN2IRSection)
        {
            DataObject oDaO = null;

            try
            {
                var oXMLString = AFN2IRSection.ToXML(oAFN2IRSection);

                oDaO = new DataObject();
                oDaO.Load(Encoding.ASCII.GetBytes(oXMLString.ToCharArray()));

                EncompassApplication.Session.DataExchange.SaveCustomDataObject(AppConfigName, oDaO);
            }
            catch (Exception Ex)
            {
                var oMethodName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                Macro.Alert(String.Format("{0}: Error Saving AppConfig", oMethodName, Ex.Message));
            }
            finally
            {
                if (oDaO != null)
                    oDaO.Dispose();
            }
        }

        public static Attachment NewLoanAttachment(Loan oLoan, string oAttachmentTitle, byte[] oDataContent, string oExtension)
        {
            Attachment oNewAttachment = null;

            if (oLoan == null)
                return oNewAttachment;

            DataObject oDaO = null;

            try
            {
                oDaO = new DataObject(oDataContent);

                if (oDaO != null && oDaO.Data != null)
                {
                    oNewAttachment = oLoan.Attachments.AddObject(oDaO, oExtension);

                    oNewAttachment.Title = oAttachmentTitle;
                }
            }
            catch (Exception Ex)
            {
                Macro.Alert(String.Format("Error: Creating New Loan Attachment - {0}", Ex.Message));
            }
            finally
            {
                if (oDaO != null)
                    oDaO.Dispose();
            }

            return oNewAttachment;
        }

        public static TrackedDocument GetTrackedDocument(Loan oLoan, string oDocmentTitle, string oLoanMilestone)
        {
            TrackedDocument oTrackedDocument = null;

            if (oLoan == null)
                return oTrackedDocument;

            try
            {
                LogEntryList oTrackedDocuments = oLoan.Log.TrackedDocuments.GetDocumentsByTitle(oDocmentTitle);

                if (oTrackedDocuments != null && oTrackedDocuments.Count > 0)
                    oTrackedDocument = (TrackedDocument)oTrackedDocuments[0];
                else
                    oTrackedDocument = oLoan.Log.TrackedDocuments.Add(oDocmentTitle, oLoanMilestone);
            }
            catch (Exception Ex)
            {
                Macro.Alert(String.Format("Error: Get Tracked Document Folder: {0}", Ex.Message));
            }

            return oTrackedDocument;
        }
    }
}
