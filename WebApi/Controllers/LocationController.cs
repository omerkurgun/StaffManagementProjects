using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using DLL;
using BLL;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace WebApi.Controllers
{
    public class LocationController : ApiController
    {
        public LocationController()
        {
            DataOperations._connectionString = ConfigurationManager.AppSettings["StaffConnection"]?.ToString();
        }

        public List<SelectListItem> GetCountries()
        {
            try
            {
                return DataOperations.takeCountrySelectList();
            }
            catch (Exception ex)
            {
                return new List<SelectListItem>();
            }
        }

        public SelectList GetCities(int CountryId)
        {
            try
            {
                return DataOperations.takeCity(CountryId.ToString());
            }
            catch (Exception ex)
            {
                List<SelectListItem> CitySelectList = new List<SelectListItem>();
                return new SelectList(CitySelectList, "Value", "Text");
            }
        }
    }
}