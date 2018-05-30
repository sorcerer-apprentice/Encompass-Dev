using System;
using System.Collections;
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
using RestSharp;
//
using AFN2IR.DataContracts;
using AFN2IR.Helpers;

namespace AFN2IR.Codebase
{
    public partial class AFN2IRApp : EMFORMS.Form
    {
        protected AFN2IRSection oAFN2IRSection = null;

        protected void On_FormLoad(object sender, EventArgs evt)
        {
            oAFN2IRSection = AFN2IRHelper.ReadAppConfig();

            if (oAFN2IRSection != null && this.fxIRServiceEndpoint != null)
            {
                var oOptionDDL = (EMFORMS.DropdownOptionCollection)this.fxIRServiceEndpoint.Options;

                var oDefaultOption = oAFN2IRSection.AFNRequest.IRRequestSource;

                this.fxIRServiceEndpoint.SelectedIndex = oOptionDDL.IndexOf(new EMFORMS.DropdownOption(oDefaultOption));

                this.fxIRServiceEndpoint.Select();
            }
        }

        protected void On_FormUnLoad(object sender, EventArgs evt)
        {
            oAFN2IRSection = null;
        }

    }
}
