using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using WINFORMS = System.Windows.Forms;
using System.Xml;
//
using RestSharp;
//
using EllieMae.Encompass.Automation;
using EllieMae.Encompass.BusinessEnums;
using EllieMae.Encompass.BusinessObjects.Loans;
using EllieMae.Encompass.BusinessObjects.Loans.Logging;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.ComponentModel;
//
using AFN2IR.DataContracts;
using AFN2IR.Helpers;
//
using RestSharp.Authenticators;

namespace AFN2IR.Plugin
{
    public partial class AFN2IRApp : WINFORMS.Form
    {
        protected void On_RetrieveAndTrackInvoice(string oLenderCID, string oOrderID)
        {
            oAFN2IRSection = AFN2IRHelper.ReadAppConfig();
            if (oAFN2IRSection == null)
            {
                Macro.Alert("Error: AFN2IRAppConfig is missing or corrupted");
                return;
            }

            var oType = oAFN2IRSection.AFNRequest.Endpoint;

            IRServiceEndpoint oEndPoint = (oAFN2IRSection.IRServiceEndpoints.IRServiceEndpoint).Find(ep => ep.Type == oType);

            RestClient oRestClient      = new RestClient(oEndPoint.Url);
            RestRequest oRestRequest    = new RestRequest(Method.POST);

            oRestRequest.AddHeader("cache-control", "no-cache");
            oRestRequest.AddHeader("content-type", "text/xml");

            string oXML = IR_RequestGroup.ToXML(oAFN2IRSection, oEndPoint.InternalAID, oEndPoint.UID, oEndPoint.PwD, oLenderCID, oOrderID);
            oRestRequest.AddParameter("text/xml", oXML, ParameterType.RequestBody);

            Stopwatch oStopWatch = new Stopwatch();
            oStopWatch.Start();
            
            oRestClient.ExecuteAsync(oRestRequest, (IRestResponse oIRResp) => {

                Loan oLoan = EncompassApplication.CurrentLoan;

                oStopWatch.Stop();

                if (oIRResp.StatusCode != HttpStatusCode.OK)
                {
                    Macro.Alert(string.Format(oIRResp.ErrorMessage, oIRResp.ErrorException.InnerException));
                }
                else
                {
                    IR_ResponseGroup oIRResponseGroup   = AFN2IRHelper.ConvertXMLToObject<IR_ResponseGroup>(Encoding.UTF8.GetBytes(oIRResp.Content));
                    IR_Status oIRStatus                 = oIRResponseGroup.Response.Status;
                    var oIRStatusPacked = String.Format("{0}:{1}:{2} ms:{3} Bytes:{4}", oIRStatus.Condition.ToLower(),
                                                                                        oIRStatus.Code,
                                                                                        oStopWatch.ElapsedMilliseconds,
                                                                                        oIRResp.ContentLength,
                                                                                        oIRStatus.Description);
                    //WNW: Pack status into CX.FX.IRRESP.STATUS
                    //     "success:S0010:1354:100747:Complete- Product Delivery"
                    oLoan.Fields["CX.FX.IRRESPSTATUS"].Value        = (object)oIRStatusPacked;

                    if (oIRStatus.Condition.ToLower().Equals("success") == false)
                    {
                        Macro.Alert(string.Format("Condition={0} StatusCode={1}{2}Description={3}", oIRStatus.Condition, oIRStatus.Code, Environment.NewLine, oIRStatus.Description ));

                        oLoan.Fields["CX.FX.IRINVCASEID"].Value     = (object)String.Empty;
                        oLoan.Fields["CX.FX.IRINVORDERID"].Value    = (object)String.Empty;
                        oLoan.Fields["CX.FX.IRINVORDERDT"].Value    = (object)String.Empty;
                        oLoan.Fields["CX.FX.IRINVBILLINGDT"].Value  = (object)String.Empty;
                        oLoan.Fields["CX.FX.IRINVAMNT"].Value       = (object)String.Empty;
                        oLoan.Fields["CX.FX.IRINVRPTNAME"].Value    = (object)String.Empty;
                    }
                    else
                    {
                        try
                        {
                            Attachment oLoanAttachment              = null;
                            TrackedDocument oTrackedDocumentFolder  = null;
                            IR_BillingResponse oBillingResponse     = oIRResponseGroup.Response.ResponseData.BillingResponse;

                            string oAttachmentTitle         = oAFN2IRSection.AFNResponse.AttachmentPrefix + oOrderID;
                            string oTrackedDocumentTitle    = oAFN2IRSection.AFNResponse.TrackedDocumentTitle;

                            oLoanAttachment                 = AFN2IRHelper.NewLoanAttachment(oLoan,  oAttachmentTitle, Convert.FromBase64String(oBillingResponse.EmbeddedFile.Document), "PDF");
                            oTrackedDocumentFolder          = AFN2IRHelper.GetTrackedDocument(oLoan, oTrackedDocumentTitle, EncompassApplication.Session.Loans.Milestones.Submittal.Name);

                            if ((oTrackedDocumentFolder == null) || (oLoanAttachment != null))
                            {
                                oTrackedDocumentFolder.Attach(oLoanAttachment);
                                oLoanAttachment.IsActive    = Convert.ToBoolean(oAFN2IRSection.AFNResponse.AttachementActivated);
                            }

                            oLoan.Fields["CX.FX.IRRESPSTATUS"].Value        = (object)oIRStatusPacked;
                            oLoan.Fields["CX.FX.IRINVCASEID"].Value         = (object)oBillingResponse.LenderCaseIdentifier;
                            oLoan.Fields["CX.FX.IRINVORDERID"].Value        = (object)oBillingResponse.OrderIdentifier;
                            oLoan.Fields["CX.FX.IRINVORDERDT"].Value        = (object)oBillingResponse.OrderedDate;
                            oLoan.Fields["CX.FX.IRINVBILLINGDT"].Value      = (object)oBillingResponse.BilledDate;
                            oLoan.Fields["CX.FX.IRINVAMNT"].Value           = (object)oBillingResponse.BilledAmount;
                            oLoan.Fields["CX.FX.IRINVRPTNAME"].Value        = (object)oBillingResponse.EmbeddedFile.Name;
                            oLoan.Fields["CX.FX.AFNTRACKEDDOCFOLDER"].Value = (object)oTrackedDocumentFolder.Title;


                            //Macro.Alert(string.Format("Received: {0}.{1}Now tracked in eFolder: {2}", oLoanAttachment.Title,
                            //                                                                          Environment.NewLine,
                            //                                                                          oTrackedDocumentFolder.Title));

                        }
                        catch (Exception Ex)
                        { 
                            var oMethodName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                            Macro.Alert(string.Format("{0}: Error moving billing report to eFolder: {1}", oMethodName, Ex.Message));
                        }
                    }
                    if (this.fxIRRespHTTPStatus != null)
                        this.fxIRRespHTTPStatus.Refresh();

                    if (fxIRRespBilling != null)
                        this.fxIRRespBilling.Refresh();
                }
            });
        }
    }
}
