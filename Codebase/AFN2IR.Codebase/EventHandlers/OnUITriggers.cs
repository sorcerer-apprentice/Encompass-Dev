using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
//
using EMFORMS = EllieMae.Encompass.Forms;
using EllieMae.Encompass.Automation;
using EllieMae.Encompass.BusinessEnums;
using EllieMae.Encompass.BusinessObjects.Loans;
using EllieMae.Encompass.BusinessObjects.Loans.Logging;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.ComponentModel;
//
using AFN2IR.DataContracts;
using AFN2IR.Helpers;

namespace AFN2IR.Codebase
{
    public partial class AFN2IRApp : EMFORMS.Form
    {

        protected void On_IRServiceEndpointChanged(object sender, EventArgs e)
        {
            //WNW: ONLY IN THE UI WE DO THIS!
            if (oAFN2IRSection == null)
                oAFN2IRSection = AFN2IRHelper.ReadAppConfig();

            oAFN2IRSection.AFNRequest.Endpoint = this.fxIRServiceEndpoint.Value;
            AFN2IRHelper.WriteAppConfig(oAFN2IRSection);

            string oType =  oAFN2IRSection.AFNRequest.Endpoint;
            IRServiceEndpoint oEndPoint = (oAFN2IRSection.IRServiceEndpoints.IRServiceEndpoint).Find(ep => ep.Type == oType);

            this.fxAFNReqUID.Text       = oEndPoint.UID;
            this.fxAFNReqPwD.Text       = "*******";
            this.fxAFNIntrnlAID.Text    = oEndPoint.InternalAID;
            this.fxIRReqSrc.Text        = oAFN2IRSection.AFNRequest.IRRequestSource;

            Macro.Alert(String.Format("Now using {0} IRService EndPoint", oAFN2IRSection.AFNRequest.Endpoint));
        }

        private void On_IRRespCondition_DataBind(object sender, EMFORMS.DataBindEventArgs e)
        {
            var oIRRespCondition = EncompassApplication.CurrentLoan.Fields["CX.FX.IRRESPSTATUS"].Value.ToString();

            string[] oFields = oIRRespCondition.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            if (oFields != null && oFields.Length == 5)
            {
                this.fxIRRespCond.Text = oFields[0];
                this.fxIRRespStatus.Text = oFields[1];
                this.fxIRRespTime.Text = oFields[2];
                this.fxIRRespSize.Text = oFields[3];
                this.fxIRRespDesc.Text = oFields[4];
            }

        }

        protected void On_AFNReqTrigger(object sender, EventArgs e)
        {
            EncompassApplication.CurrentLoan.Fields[oAFN2IRSection.AFNRequest.TriggerField].Value = this.fxAFNReqTrigger.Value;
        }

    }
}
