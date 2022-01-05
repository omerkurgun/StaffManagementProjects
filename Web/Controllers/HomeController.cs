using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BLL;
using DLL;
using Newtonsoft.Json;
using RestSharp;
using Web.Controllers;

namespace StaffManagementProjects.Controllers
{
    public class HomeController : BaseController
    {

        [Route, AllowAnonymous]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("~/dashboard");
            }
            else
            {
                return View();
            }
        }

        [Route("dashboard"), Authorize]
        public ActionResult Dashboard()
        {
            ViewBag.DepartmentCount = DataOperations.takeDepartmentCount();
            ViewBag.StaffCount = DataOperations.takeStaffCount();
            return View();
        }

        [Route("department-list"), Authorize]
        public ActionResult DepartmentList()
        {
            return View(DataOperations.takeDepartmentList());
        }

        [Route("staff-list"), Authorize]
        public ActionResult StaffList()
        {
            ViewData["Departments"] = DataOperations.takeDepartmentSelectList();

            var client = new RestClient($"{_webApiUrl}/api/Location/GetCountries");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (!string.IsNullOrEmpty(response.Content))
                {
                    ViewData["Countryies"] = JsonConvert.DeserializeObject<List<SelectListItem>>(response.Content);
                }
                else
                {
                    ViewData["Countryies"] = new List<SelectListItem> { };
                }
            }

            return View(DataOperations.takeStaffList(String.Empty));
        }

        [Route("logout"), Authorize]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return Redirect("~/");
        }
    }
}