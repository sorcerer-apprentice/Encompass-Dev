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
using AFN2IR.DataContracts;
using AFN2IR.Helpers;

namespace AFN2IR.Plugin
{
    public partial class AFN2IRApp : WINFORMS.Form
    {
        protected void On_LoanFieldChanged(object sender, FieldChangeEventArgs oFldEvt)
        {
            if (oAFN2IRSection == null)
                oAFN2IRSection = AFN2IRHelper.ReadAppConfig();

            string oTriggerField = (oAFN2IRSection != null) ? oTriggerField = oAFN2IRSection.AFNRequest.TriggerField : "300";

            if ((oFldEvt.FieldID == oTriggerField) && oFldEvt.NewValue.EndsWith("PQ"))
            {
                On_RetrieveAndTrackInvoice(EncompassApplication.CurrentLoan.Fields["364"].Value.ToString(), oFldEvt.NewValue);
            }          
        }
    }
}
