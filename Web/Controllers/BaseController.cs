using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BLL;

namespace Web.Controllers
{
    public class BaseController : Controller
    {
        public string _webApiUrl { get; set; }

        public BaseController()
        {
            _webApiUrl = ConfigurationManager.AppSettings["WebApiUrl"]?.ToString();
            DataOperations._connectionString = ConfigurationManager.AppSettings["StaffConnection"]?.ToString();
        }
    }
}