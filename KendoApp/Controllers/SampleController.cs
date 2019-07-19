using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using KendoApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KendoApp.Controllers
{
    public class SampleController : Controller
    {
        private TrainingEntities db = new TrainingEntities();
        // GET: Sample
        public ActionResult KendoMulti()
        {
            //TrainingEntities cotext = new TrainingEntities();
            
            //    var employeeCollection = (from employees in cotext.Employees select employees).ToList();
            //    if (employeeCollection != null)
            //    {
            //        ViewBag.Employees = employeeCollection.Select(N => new SelectListItem
            //        {
            //            Text = N.FacilityId + " - " + N.Facility,
            //            Value = N.FacilityId.ToString()
            //        });
            //    }
            
            return View();
        }

        public ActionResult Index()
        {

            return View();
           
        }

         public JsonResult GetPharmacyList()
        {
            return Json(db.Pharmacies.Select(x => new { Pharmacy_id = x.PharmacyId, Pharmacy_Name = x.PharmacyName }), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetFacilityList([DataSourceRequest] DataSourceRequest request,int? PharmacyId)
        {

            var Facility = db.Facilities.AsQueryable();
            if (PharmacyId != null)
            {
                Facility = Facility.Where(s => s.PharmacyId == PharmacyId);

            }
            
            return Json(Facility.Select(s => new { Facility_ID = s.FacilityId, Facility_Name = s.FacilityName,Facility_status=s.IsActive}), JsonRequestBehavior.AllowGet);
        }


        //[HttpPost]
        //public ActionResult GetEmployeeList([DataSourceRequest] DataSourceRequest request, string[] facilityId)
        //{
        //    TempData["facId"] = facilityId;
        //    List<int> FacIds = new List<int>();

        //    var EmpData = db.Employees.AsQueryable();
        //    foreach (var num in facilityId)
        //    {
        //        FacIds.Add(Int32.Parse(num));  
        //        foreach(var its in FacIds) { 
        //        if (FacIds.Count() > 0)
        //        {
        //            EmpData = EmpData.Where(s => s.FacilityId == its);
        //        }
        //        }
        //    }
        //    return Json(EmpData.Select(e => new { EmployeeId = e.EmployeeId, EmployeeName = e.EmployeeName, EmployeeAddress = e.EmployeeAddress, EmployeeDesignation = e.EmployeeDesignation, FacilityId = e.FacilityId }), JsonRequestBehavior.AllowGet);
        //}


        //[HttpPost]
        //public ActionResult GetEmployeeList([DataSourceRequest] DataSourceRequest request, string[] facilityId)
        //{
        //    TempData["facId"] = facilityId;

        //    List<int> facidlist = new List<int>();

        //    var EmpData = db.Employees.AsQueryable();
        //    List<Employee> emplist = new List<Employee>();
        //    if (facilityId.Count() > 0)
        //    {
        //        foreach (var item in facilityId)
        //        {
        //            int? i = Int32.Parse(item);
        //            EmpData = EmpData.Where(s => s.FacilityId == i);
        //            foreach (var data in EmpData)
        //            {
        //                emplist.Add(data);
        //            }

        //            facidlist.Add(Int32.Parse(item));
        //        }
        //        var EmpData = db.Employees.Where(s => facidlist.Contains(Convert.ToInt32(s.FacilityId)).ToList();
        //        return Json(emplist.Select(e => new { EmployeeId = e.EmployeeId, EmployeeName = e.EmployeeName, EmployeeAddress = e.EmployeeAddress, EmployeeDesignation = e.EmployeeDesignation, FacilityId = e.FacilityId }), JsonRequestBehavior.AllowGet);

        //    }
        //    return Json("null");
        //}


        [HttpPost]
        public ActionResult GetEmployeeList([DataSourceRequest] DataSourceRequest request, string[] facilityId)
        {
            if (facilityId != null)
            {
                TempData["facId"] = facilityId[0];
                List<int> facidlist = new List<int>();

                var EmpData = db.Employees.AsQueryable();
                List<Employee> emplist = new List<Employee>();
                if (facilityId.Count() > 0)
                {
                    foreach (var item in facilityId)
                    {
                        int i = Int32.Parse(item);
                      

                        foreach (var data in EmpData)
                        {
                            if (data.FacilityId == i)
                            {
                                emplist.Add(data);
                            }
                        }
                        facidlist.Add(Int32.Parse(item));
                    }
                            
                    return Json(emplist.Select(e => new { EmployeeId = e.EmployeeId, EmployeeName = e.EmployeeName, EmployeeAddress = e.EmployeeAddress, EmployeeDesignation = e.EmployeeDesignation, FacilityId = e.FacilityId }), JsonRequestBehavior.AllowGet);

                }
            }
              return Json("null");
         }


        public ActionResult UpdateEmployee([DataSourceRequest] DataSourceRequest request, Employee employee)
        {
            var employeeUpdate = db.Employees.First(emp => emp.EmployeeId == employee.EmployeeId);

            TryUpdateModel(employeeUpdate);

            db.SaveChanges();

            return Json(ModelState.ToDataSourceResult());
        }

        public ActionResult InsertEmployee([DataSourceRequest] DataSourceRequest request, Employee addEmployee)
        {
            
            if (ModelState.IsValid)
            {
                if (TempData["facId"] != null)
                {

                    if (addEmployee.EmployeeId == 0)
                    {
                        string FID = Convert.ToString(TempData["facId"]);
                        var id = int.Parse(FID);

                        addEmployee.FacilityId = id;
                        db.Employees.Add(addEmployee);
                        db.SaveChanges();

                    }
                    else
                    {
                        db.Entry(addEmployee).State = EntityState.Modified;
                        db.Employees.Add(addEmployee);
                        db.SaveChanges();
                    }
                }
            }
            return Json(new[] { addEmployee }.ToDataSourceResult(request));
        }

        public ActionResult DeleteEmployee([DataSourceRequest] DataSourceRequest request, Employee employee)
        {
            var deleteEmployee = db.Employees.First(emp => emp.EmployeeId == employee.EmployeeId);

            if (deleteEmployee != null)
            {
                db.Employees.Remove(deleteEmployee);
                db.SaveChanges();
            }

            return Json(new[] { deleteEmployee }.ToDataSourceResult(request));
        }

        public ActionResult FacilityRemoteData()
        {
            return View();

        }

    }

}