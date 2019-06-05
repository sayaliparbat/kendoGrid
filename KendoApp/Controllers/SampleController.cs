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
        public ActionResult Index()
        {
            return View();
           
        }

         public JsonResult GetPharmacyList()
        {
            return Json(db.Pharmacies.Select(x => new { Pharmacy_id = x.PharmacyId, Pharmacy_Name = x.PharmacyName }), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetFacilityList(int? PharmacyId)
        {

            var Facility = db.Facilities.AsQueryable();
            if (PharmacyId != null)
            {
                Facility = Facility.Where(s => s.PharmacyId == PharmacyId);

            }
            return Json(Facility.Select(s => new { Facility_ID = s.FacilityId, Facility_Name = s.FacilityName }), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetEmployeeList([DataSourceRequest] DataSourceRequest request, int? facilityId)
        {
            TempData["facId"] = facilityId;
            var EmpData = db.Employees.AsQueryable();
            if (facilityId > 0)
            {
                EmpData = EmpData.Where(s => s.FacilityId == facilityId);

            }
            return Json(EmpData.Select(e => new { EmployeeId = e.EmployeeId, EmployeeName = e.EmployeeName, EmployeeAddress = e.EmployeeAddress, EmployeeDesignation = e.EmployeeDesignation, FacilityId = e.FacilityId }), JsonRequestBehavior.AllowGet);
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
                if (addEmployee.EmployeeId == 0)
                {
                    var FID = TempData["facId"];
                    addEmployee.FacilityId = (int?)FID;

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




    }

}