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
        LoansScreen oLoansScreen = null;

        AFN2IRSection oAFN2IRSection = null;

        protected void On_LoanOpened(object sender, EventArgs evt)
        {
            oLoansScreen = (LoansScreen)EncompassApplication.Screens[EncompassScreen.Loans];

            if (oLoansScreen != null)
            {
                oLoansScreen.FormLoaded     += On_FormLoaded;
                oLoansScreen.FormUnloading  += On_FormUnLoading; ;
            }

            EncompassApplication.CurrentLoan.FieldChange +=On_LoanFieldChanged;
               
        }

   
        protected void On_LoanClosing(object sender, EventArgs evt)
        {
            EncompassApplication.CurrentLoan.FieldChange -= On_LoanFieldChanged;

            oLoansScreen = (LoansScreen)EncompassApplication.Screens[EncompassScreen.Loans];

            if (oLoansScreen != null)
            {
                oLoansScreen.FormLoaded     -= On_FormLoaded;
                oLoansScreen.FormUnloading  -= On_FormUnLoading;
            }
        }
    }
}
