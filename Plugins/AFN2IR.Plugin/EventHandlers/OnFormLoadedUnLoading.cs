using System;
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
using EMFORMS = EllieMae.Encompass.Forms;
//
using AFN2IR.DataContracts;
using AFN2IR.Helpers;

namespace AFN2IR.Plugin
{
    public partial class AFN2IRApp : WINFORMS.Form
    {
        EMFORMS.Panel fxIRRespHTTPStatus = null;
        EMFORMS.Panel fxIRRespBilling = null;

        protected void On_FormLoaded(object sender, FormChangeEventArgs oFCEvt)
        {
            // WNW: Temporary Workaround
            if (EncompassApplication.Screens.Current.ScreenType == EncompassScreen.Loans)
            {
                this.fxIRRespHTTPStatus = (EMFORMS.Panel)oFCEvt.Form.FindControl("FX_AFN2IR_HTTPSTATUS"); 
                this.fxIRRespBilling    = (EMFORMS.Panel)oFCEvt.Form.FindControl("FX_AFN2IR_IRBILLINGRESP");
            }

            if (oAFN2IRSection == null)
                oAFN2IRSection = AFN2IRHelper.ReadAppConfig();

        }

        protected void On_FormUnLoading(object sender, FormChangeEventArgs oFCEvt)
        {
            oAFN2IRSection = null;
        }


    }
}
