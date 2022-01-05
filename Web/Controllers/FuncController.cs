using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;
using BLL;
using DLL;
using Web.Controllers;
using System.Configuration;
using RestSharp;
using System.Net;

namespace StaffManagementProjects.Controllers
{
    public class FuncController : BaseController
    {

        [Route("login"), HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public JsonResult Login(string User, string Pass)
        {
            try
            {
                string checkUsers = DataOperations.loginFunc(User, Pass);

                if (!string.IsNullOrEmpty(checkUsers))
                {
                    FormsAuthentication.SetAuthCookie(checkUsers, false);

                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        [Route("department-delete"), HttpPost, ValidateAntiForgeryToken, Authorize]
        public JsonResult DepartmentDelete(string DepartmentId)
        {
            try
            {
                return Json(DataOperations.departmentDelete(DepartmentId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        [Route("department-operation"), HttpPost, ValidateAntiForgeryToken, Authorize]
        public JsonResult DepartmentOperation(FormCollection frm)
        {
            try
            {
                return Json(DataOperations.departmentOperation(frm), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        [Route("take-city"), HttpPost, ValidateAntiForgeryToken]
        public JsonResult TakeCity(string id)
        {
            try
            {
                var client = new RestClient($"{_webApiUrl}/api/Location/GetCities?CountryId={id}");
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                IRestResponse response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    if(!string.IsNullOrEmpty(response.Content))
                    {
                        return Json(JsonConvert.DeserializeObject<List<SelectListItem>>(response.Content), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new List<SelectListItem> { }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        [Route("staff-with-text"), HttpPost, ValidateAntiForgeryToken, Authorize]
        public JsonResult StaffWithParams(string searchText)
        {
            try
            {
                return Json(DataOperations.takeStaffList(searchText), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<staff> { }, JsonRequestBehavior.AllowGet);
            }
        }

        [Route("staff-delete"), HttpPost, ValidateAntiForgeryToken, Authorize]
        public JsonResult StaffDelete(string StaffId)
        {
            try
            {
                return Json(DataOperations.staffDelete(StaffId, Server.MapPath(ConfigurationManager.AppSettings["StaffImagePath"]?.ToString())), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        [Route("staff-operation"), HttpPost, ValidateAntiForgeryToken, Authorize]
        public JsonResult StaffOperation(HttpPostedFileBase image_path, FormCollection frmData)
        {
            try
            {
                return Json(DataOperations.staffOperation(image_path, frmData, Server.MapPath(ConfigurationManager.AppSettings["StaffImagePath"]?.ToString())), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
    }
}