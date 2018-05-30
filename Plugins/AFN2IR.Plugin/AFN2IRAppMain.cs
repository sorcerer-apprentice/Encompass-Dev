using System;
using System.IO;
using System.Text;
using System.Net;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Resources;
//    EllieMae Encompass Required
using EllieMae.Encompass.Automation;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.BusinessEnums;
using EllieMae.Encompass.ComponentModel;
using EllieMae.Encompass.BusinessObjects;
using EllieMae.Encompass.BusinessObjects.Loans;
using EllieMae.Encompass.BusinessObjects.Loans.Logging;
using EllieMae.Encompass.Collections;
//
using WINFORMS = System.Windows.Forms;

namespace AFN2IR.Plugin
{
    [Plugin]
    public partial class AFN2IRApp
    {
        public AFN2IRApp()
        {
            // WireUp for login/logout events
            //EncompassApplication.Login += this.On_Login;
            //EncompassApplication.Logout += this.On_Logout;
            EncompassApplication.LoanOpened += this.On_LoanOpened;
       
            EncompassApplication.LoanClosing += this.On_LoanClosing;
        }

        ~AFN2IRApp()
        {
            // UnWire Loan Close/Open events
            EncompassApplication.LoanClosing -= this.On_LoanClosing;

            EncompassApplication.LoanOpened -= this.On_LoanOpened;

            // UnWire Logout/Login events
            //EncompassApplication.Logout -= this.On_Logout;
            //EncompassApplication.Login -= this.On_Login;
        }
    }
}
