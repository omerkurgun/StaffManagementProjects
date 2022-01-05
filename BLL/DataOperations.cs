using DLL;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BLL
{
    public class DataOperations
    {
        public static string _connectionString { get; set; }

        public static int takeDepartmentCount()
        {
            using (var db = new StaffDBModel(_connectionString))
            {
                return db.department.Count();
            }
        }

        public static int takeStaffCount()
        {
            using (var db = new StaffDBModel(_connectionString))
            {
                return db.staff.Count();
            }
        }

        public static List<department> takeDepartmentList()
        {
            using (var db = new StaffDBModel(_connectionString))
            {
                return db.department.OrderByDescending(i => i.id).ToList();
            }
        }

        public static List<staff> takeStaffList(string searchText)
        {
            using (var db = new StaffDBModel(_connectionString))
            {
                List<staff> list = new List<staff>();

                if (!string.IsNullOrEmpty(searchText))
                {
                    list = db.staff.Include("country").Include("city").Include("department").OrderByDescending(i => i.id).Where(x => x.identity_number.Contains(searchText) || x.name_surname.Contains(searchText) || x.mission.Contains(searchText)).ToList();
                }
                else
                {
                    list = db.staff.Include("country").Include("city").Include("department").OrderByDescending(i => i.id).ToList();
                }

                list.ForEach((item) => {

                    if (item.birth_date != null)
                    {
                        var baseDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).ToLocalTime();
                        var toDate = (DateTime)item.birth_date;
                        item.birthDateTimestamp = toDate.Subtract(baseDate).TotalMilliseconds;
                    }
                    else
                    {
                        item.birthDateTimestamp = 0;
                    }

                    item.department = new department
                    {
                        id = item.department.id,
                        department_name = item.department.department_name
                    };

                    item.country = new country
                    {
                        id = item.country.id,
                        country_name = item.country.country_name
                    };

                    item.city = new city
                    {
                        id = item.city.id,
                        city_name = item.city.city_name
                    };
                });

                return list;
            }
        }

        public static List<SelectListItem> takeDepartmentSelectList()
        {
            using (var db = new StaffDBModel(_connectionString))
            {
                return db.department.Select(d => new SelectListItem { Value = (d.id).ToString(), Text = d.department_name }).ToList();
            }
        }

        public static List<SelectListItem> takeCountrySelectList()
        {
            using (var db = new StaffDBModel(_connectionString))
            {
                return db.country.Select(c => new SelectListItem { Value = (c.id).ToString(), Text = c.country_name }).ToList();
            }
        }

        public static string loginFunc(string User, string Pass)
        {
            using (var db = new StaffDBModel(_connectionString))
            {
                var checkUsers = db.users.FirstOrDefault(i => i.uname.Equals(User) && i.pass.Equals(Pass));

                if (checkUsers != null)
                {
                    string userJSON = JsonConvert.SerializeObject(checkUsers);

                    return userJSON;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static int departmentDelete(string DepartmentId)
        {
            int idg;
            bool checkInt = int.TryParse(DepartmentId, out idg);
            if (checkInt)
            {
                using (var db = new StaffDBModel(_connectionString))
                {
                    var checkForeignKey = db.staff.Any(x => x.department_id == idg);

                    if (checkForeignKey)
                    {
                        return 4;
                    }
                    else
                    {
                        var isThereAnyRecord = db.department.Find(idg);
                        if (isThereAnyRecord != null)
                        {
                            db.department.Remove(isThereAnyRecord);
                            db.SaveChanges();
                            return 1;
                        }
                        else
                        {
                            return 2;
                        }
                    }
                }
            }
            else
            {
                return 3;
            }
        }

        public static int departmentOperation(FormCollection frm)
        {
            string operationName = frm["operationName"];

            if (operationName == "INSERT")
            {
                bool check = (
                !string.IsNullOrEmpty(frm["eklDepartmentName"])
            ) ? false : true;
                if (check)
                {
                    return 2;
                }
                else
                {
                    using (var db = new StaffDBModel(_connectionString))
                    {
                        db.department.Add(
                            new department
                            {
                                department_name = frm["eklDepartmentName"]
                            }
                        );

                        db.SaveChanges();
                        return 1;
                    }
                }
            }
            else if (operationName == "UPDATE")
            {
                bool check = (
                    !string.IsNullOrEmpty(frm["gncDepartmentName"]) &&
                    !string.IsNullOrEmpty(frm["gncDepartmentId"])
                ) ? false : true;
                if (check)
                {
                    return 2;
                }
                else
                {
                    int dId;
                    bool sayimi = int.TryParse(frm["gncDepartmentId"], out dId);

                    if (sayimi)
                    {
                        using (var db = new StaffDBModel(_connectionString))
                        {
                            var isThereAnyOneRecord = db.department.Find(dId);
                            if (isThereAnyOneRecord != null)
                            {
                                isThereAnyOneRecord.department_name = frm["gncDepartmentName"];
                                db.SaveChanges();
                                return 1;
                            }
                            else
                            {
                                return 3;
                            }
                        }
                    }
                    else
                    {
                        return 4;
                    }
                }
            }
            else
            {
                return 0;
            }
        }

        public static int staffDelete(string StaffId, string ServerMapPath)
        {
            int idg;
            bool checkInt = int.TryParse(StaffId, out idg);
            if (checkInt)
            {
                using (var db = new StaffDBModel(_connectionString))
                {
                    var isThereAnyRecord = db.staff.Find(idg);
                    if (isThereAnyRecord != null)
                    {
                        string fullPath = ServerMapPath + isThereAnyRecord.image_path;

                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }

                        db.staff.Remove(isThereAnyRecord);
                        db.SaveChanges();
                        return 1;
                    }
                    else
                    {
                        return 2;
                    }
                }
            }
            else
            {
                return 3;
            }
        }

        public static SelectList takeCity(string CountryId)
        {
            int idg;
            bool CheckInt = int.TryParse(CountryId, out idg);


            List<SelectListItem> CountrySelectList = new List<SelectListItem>();

            if (CheckInt)
            {
                using (var db = new StaffDBModel(_connectionString))
                {
                    var country = db.country.Find(idg);

                    var city = db.city.Where(s => s.country_id == idg).ToList();
                    if (country != null)
                    {
                        if (city.Count > 0)
                        {
                            CountrySelectList.Add(new SelectListItem { Text = "Select City;", Value = "" });
                        }
                        else
                        {
                            CountrySelectList.Add(new SelectListItem { Text = $" Country {country.country_name} Doesn't Have Cities", Value = "" });
                        }

                        foreach (var item in city)
                        {
                            CountrySelectList.Add(new SelectListItem { Text = item.city_name, Value = item.id.ToString() });
                        }
                    }
                    else
                    {
                        CountrySelectList.Add(new SelectListItem { Text = "Country Not Found", Value = "" });
                    }

                    return new SelectList(CountrySelectList, "Value", "Text");
                }
            }
            else
            {
                return new SelectList(CountrySelectList, "Value", "Text");
            }
        }

        public static int staffOperation(HttpPostedFileBase image_path, FormCollection frmData, string ServerMapPath)
        {
            string operationName = frmData["operationName"];
            staffFormModel formData = JsonConvert.DeserializeObject<staffFormModel>(frmData["frmData"]);

            if (!string.IsNullOrEmpty(formData.mobile_phone))
            {
                if (!formData.mobile_phone.StartsWith("05"))
                {
                    return 4;
                }
            }

            if (!string.IsNullOrEmpty(formData.home_phone))
            {
                if (!formData.home_phone.StartsWith("05"))
                {
                    return 5;
                }
            }

            string fileName = string.Empty;

            if (image_path != null)
            {
                if (image_path.ContentType == "image/jpeg")
                {
                    fileName = Guid.NewGuid() + ".jpg";

                    image_path.SaveAs(Path.Combine(ServerMapPath, fileName));
                }
                else
                {
                    return 2;
                }
            }

            if (formData.operationName == "INSERT")
            {
                using (var db = new StaffDBModel(_connectionString))
                {
                    if (!string.IsNullOrEmpty(formData.identity_number))
                    {
                        if (db.staff.Any(x => x.identity_number.Equals(formData.identity_number)))
                        {
                            return 6;
                        }
                    }

                    var staffModel = new staff
                    {
                        identity_number = formData.identity_number,
                        birth_place = formData.birth_place,
                        blood_type = formData.blood_type,
                        city_id = formData.city_id,
                        country_id = formData.country_id,
                        department_id = formData.department_id,
                        email = formData.email,
                        gender = formData.gender == true ? true : false,
                        home_address = formData.home_address,
                        home_phone = formData.home_phone,
                        image_path = fileName,
                        last_graduated_school = formData.last_graduated_school,
                        mission = formData.mission,
                        mobile_phone = formData.mobile_phone,
                        name_surname = formData.name_surname,
                        nationality = formData.nationality
                    };

                    if (formData.birth_dateTimeStamp != null && formData.birth_dateTimeStamp > 0)
                    {
                        DateTime dateTimeBirthDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                        dateTimeBirthDate = dateTimeBirthDate.AddMilliseconds((double)formData.birth_dateTimeStamp).ToLocalTime();

                        staffModel.birth_date = dateTimeBirthDate;
                    }

                    db.staff.Add(staffModel);

                    db.SaveChanges();

                    return 1;
                }
            }
            else if (formData.operationName == "UPDATE")
            {
                using (var db = new StaffDBModel(_connectionString))
                {
                    var isThereAnyRecord = db.staff.Find(formData.staffId);

                    if (isThereAnyRecord != null)
                    {
                        if (!string.IsNullOrEmpty(formData.identity_number))
                        {
                            if (!isThereAnyRecord.identity_number.Equals(formData.identity_number))
                            {
                                if (db.staff.Any(x => x.identity_number.Equals(formData.identity_number)))
                                {
                                    return 6;
                                }
                            }
                        }

                        isThereAnyRecord.identity_number = formData.identity_number;
                        isThereAnyRecord.birth_place = formData.birth_place;
                        isThereAnyRecord.blood_type = formData.blood_type;
                        isThereAnyRecord.city_id = formData.city_id;
                        isThereAnyRecord.country_id = formData.country_id;
                        isThereAnyRecord.department_id = formData.department_id;
                        isThereAnyRecord.email = formData.email;
                        isThereAnyRecord.gender = formData.gender == true ? true : false;
                        isThereAnyRecord.home_address = formData.home_address;
                        isThereAnyRecord.home_phone = formData.home_phone;
                        isThereAnyRecord.last_graduated_school = formData.last_graduated_school;
                        isThereAnyRecord.mission = formData.mission;
                        isThereAnyRecord.mobile_phone = formData.mobile_phone;
                        isThereAnyRecord.name_surname = formData.name_surname;
                        isThereAnyRecord.nationality = formData.nationality;

                        if (formData.birth_dateTimeStamp != null && formData.birth_dateTimeStamp > 0)
                        {
                            DateTime dateTimeBirthDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                            dateTimeBirthDate = dateTimeBirthDate.AddMilliseconds((double)formData.birth_dateTimeStamp).ToLocalTime();

                            isThereAnyRecord.birth_date = dateTimeBirthDate;
                        }
                        else
                        {
                            isThereAnyRecord.birth_date = null;
                        }

                        if (image_path != null)
                        {
                            if (image_path.ContentType == "image/jpeg")
                            {
                                string fullPath = ServerMapPath + isThereAnyRecord.image_path;

                                if (System.IO.File.Exists(fullPath))
                                {
                                    System.IO.File.Delete(fullPath);
                                }

                                isThereAnyRecord.image_path = fileName;
                            }
                            else
                            {
                                return 2;
                            }
                        }

                        db.SaveChanges();

                        return 1;
                    }
                    else
                    {
                        return 3;
                    }
                }
            }
            else
            {
                return 0;
            }
        }
    }
}
