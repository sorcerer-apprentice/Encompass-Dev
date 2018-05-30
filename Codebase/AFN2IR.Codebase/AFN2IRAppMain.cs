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
        private IContainer components = null;

        protected EllieMae.Encompass.Forms.Label fxAFNReqUID;

        protected EllieMae.Encompass.Forms.Label fxAFNReqPwD;

        protected EllieMae.Encompass.Forms.Label fxAFNIntrnlAID;

        protected EllieMae.Encompass.Forms.Label fxIRReqSrc;

        protected EllieMae.Encompass.Forms.DropdownBox fxIRServiceEndpoint;

        protected EllieMae.Encompass.Forms.DropdownBox fxAFNReqTrigger;

        protected EllieMae.Encompass.Forms.Panel fxIRRespHTTPStatus;

        protected EllieMae.Encompass.Forms.Label fxIRRespCond;

        protected EllieMae.Encompass.Forms.Label fxIRRespStatus;

        protected EllieMae.Encompass.Forms.Label fxIRRespTime;

        protected EllieMae.Encompass.Forms.Label fxIRRespSize;

        protected EllieMae.Encompass.Forms.Label fxIRRespDesc;

        protected EllieMae.Encompass.Forms.Panel fxIRRespBilling;

        protected EllieMae.Encompass.Forms.Label fxIRInvCaseID;

        protected EllieMae.Encompass.Forms.Label fxIRInvOrderID;

        protected EllieMae.Encompass.Forms.Label fxIRInvOrderDT;

        protected EllieMae.Encompass.Forms.Label fxIRInvBillDT;

        protected EllieMae.Encompass.Forms.Label fxIRInvAmnt;

        protected EllieMae.Encompass.Forms.Label fxIRInvRptName;

        protected EllieMae.Encompass.Forms.Label fxTrackedDocTitle;

        protected EllieMae.Encompass.Forms.TextBox fxIRRespCondition;

        public AFN2IRApp()
        {
        }

        public override void CreateControls()
        {
            this.components = new Container();
            this.fxAFNReqUID = (EllieMae.Encompass.Forms.Label)base.FindControl("FX_AFNReqUID");
            this.fxAFNReqPwD = (EllieMae.Encompass.Forms.Label)base.FindControl("FX_AFNReqPwD");
            this.fxAFNIntrnlAID = (EllieMae.Encompass.Forms.Label)base.FindControl("FX_AFNInternalAID");
            this.fxIRReqSrc = (EllieMae.Encompass.Forms.Label)base.FindControl("FX_IRReqSrc");
            this.fxIRServiceEndpoint = (EllieMae.Encompass.Forms.DropdownBox)base.FindControl("FX_IRServiceEndpoint");
            this.fxAFNReqTrigger = (EllieMae.Encompass.Forms.DropdownBox)base.FindControl("FX_AFNReqTrigger");
            this.fxIRRespCond = (EllieMae.Encompass.Forms.Label)base.FindControl("FX_IRRespCond");
            this.fxIRRespStatus = (EllieMae.Encompass.Forms.Label)base.FindControl("FX_IRRespStat");
            this.fxIRRespTime = (EllieMae.Encompass.Forms.Label)base.FindControl("FX_IRRespTime");
            this.fxIRRespSize = (EllieMae.Encompass.Forms.Label)base.FindControl("FX_IRRespSize");
            this.fxIRRespDesc = (EllieMae.Encompass.Forms.Label)base.FindControl("FX_IRRespDesc");
            this.fxIRInvCaseID = (EllieMae.Encompass.Forms.Label)base.FindControl("FX_IRInvCaseID");
            this.fxIRInvOrderID = (EllieMae.Encompass.Forms.Label)base.FindControl("FX_IRInvOrderID");
            this.fxIRInvOrderDT = (EllieMae.Encompass.Forms.Label)base.FindControl("FX_IRInvOrderDT");
            this.fxIRInvBillDT = (EllieMae.Encompass.Forms.Label)base.FindControl("FX_IRInvBillingDT");
            this.fxIRInvAmnt = (EllieMae.Encompass.Forms.Label)base.FindControl("FX_IRInvAmnt");
            this.fxIRInvRptName = (EllieMae.Encompass.Forms.Label)base.FindControl("FX_IRInvRptName");
            this.fxTrackedDocTitle = (EllieMae.Encompass.Forms.Label)base.FindControl("FX_TrackedDocTitle");

            this.fxIRRespCondition  = (EllieMae.Encompass.Forms.TextBox)base.FindControl("FX_CONDITION");
            this.fxIRRespHTTPStatus = (EllieMae.Encompass.Forms.Panel)base.FindControl("FX_AFN2IR_HTTPSTATUS");
            this.fxIRRespBilling    = (EllieMae.Encompass.Forms.Panel)base.FindControl("FX_AFN2IR_IRBILLINGRESP"); 

            this.WireUP();
        }

        protected override void Dispose(bool disposing)
        {
            if ((!disposing ? false : this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void WireUP()
        {
            if (this.fxIRRespCondition != null)
            {
                this.fxIRRespCondition.DataBind += On_IRRespCondition_DataBind; 
            }
            if (this.fxAFNReqTrigger != null)
            {
                this.fxAFNReqTrigger.Change += On_AFNReqTrigger;
            }
            if (this.fxIRServiceEndpoint != null)
            {
                this.fxIRServiceEndpoint.ValueChanged += On_IRServiceEndpointChanged;
            }
            this.Load   += On_FormLoad;
            this.Unload += On_FormUnLoad;
        }


        private void FxIRRespCondition_DataBind(object sender, EMFORMS.DataBindEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void UnWireUP()
        {
            if (this.fxIRRespCondition != null)
            {
                this.fxIRRespCondition.DataBind -= On_IRRespCondition_DataBind;
            }
            if (this.fxAFNReqTrigger != null)
            {
                this.fxAFNReqTrigger.ValueChanged -= On_AFNReqTrigger;
            }
            if (this.fxIRServiceEndpoint != null)
            {
                this.fxIRServiceEndpoint.ValueChanged -= On_IRServiceEndpointChanged;
            }
            this.Load    -= On_FormLoad;
            this.Unload  -= On_FormUnLoad;

        }

    }
}
