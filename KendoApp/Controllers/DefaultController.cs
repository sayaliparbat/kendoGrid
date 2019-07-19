using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using KendoApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KendoApp.Controllers
{
    public class DefaultController : Controller
    {
        private TrainingEntities db = new TrainingEntities();
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }
        
        public JsonResult GetPharmacyList()
        {
            return Json(db.Pharmacies.Select(x => new { Pharmacy_id = x.PharmacyId, Pharmacy_Name = x.PharmacyName }), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetFacilityList([DataSourceRequest] DataSourceRequest request, int? PharmacyId)
        {

            var Facility = db.Facilities.AsQueryable();
            if (PharmacyId != null)
            {
                Facility = Facility.Where(s => s.PharmacyId == PharmacyId);

            }

            return Json(Facility.Select(s => new { Facility_ID = s.FacilityId, Facility_Name = s.FacilityName }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult InsertData([DataSourceRequest] DataSourceRequest request, string[] facilityId,Facility facility)
        {
            //var FacilityUpdate = db.Facilities.First(fac => fac.FacilityId == facility.FacilityId);
            var FacilityData = db.Facilities.AsQueryable();
            List<int> facidlist = new List<int>();
           
            List<Facility> facilitylist = new List<Facility>();
            if (facilityId.Count() > 0)
            {
                foreach (var item in facilityId)
                {
                    int i = Int32.Parse(item);

                    foreach (var data in FacilityData)
                    {
                        if (data.FacilityId == i)
                        {
                            facilitylist.Add(data);

                        }
                    }
                   facidlist.Add(Int32.Parse(item));
                
                }
               TryUpdateModel(FacilityUpdate);

              db.SaveChanges();
                       
            return Json(ModelState.ToDataSourceResult());
        }



    }
}