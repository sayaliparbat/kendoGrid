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

        // GET: Default
        public ActionResult Sample()
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

            return Json(Facility.Select(s => new { Facility_ID = s.FacilityId, Facility_Name = s.FacilityName,Facility_Status=s.IsActive }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult InsertData([DataSourceRequest] DataSourceRequest request, string[] facilityId)
        {
            var FacilityData = db.Facilities.AsQueryable();
            List<int> facidlist = new List<int>();

    
            if (facilityId.Count() > 0)
            {
                foreach (var item in facilityId)
                {
                    int i = Int32.Parse(item);

                    facidlist.Add(Int32.Parse(item));
                }

                db.Facilities.Where(x => facidlist.Contains(x.FacilityId)).ToList()
                               .ForEach(a =>
                               {
                                   a.IsActive = true;
                               }
                                       );

                db.SaveChanges();
            }

           var activeUser = FacilityData.Where(x => x.IsActive == true).ToList();

           return Json(activeUser.Select(s => new { Facility_ID = s.FacilityId, Facility_Name = s.FacilityName }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdatedUserList([DataSourceRequest] DataSourceRequest request, string[] facilityIds)
        {

            var FacilityData = db.Facilities.AsQueryable();
            List<int> UpdaetedIdlist = new List<int>();


            if (facilityIds.Count() > 0)
            {
                foreach (var item in facilityIds)
                {
                    int i = Int32.Parse(item);

                    UpdaetedIdlist.Add(Int32.Parse(item));
                }

                db.Facilities.Where(x => UpdaetedIdlist.Contains(x.FacilityId)).ToList()
                               .ForEach(a =>
                               {
                                   a.IsActive = false;
                               }
                                       );

                db.SaveChanges();
            }
            
            var activeUser = FacilityData.Where(x => x.IsActive == true).ToList();
            

            return Json(activeUser.Select(s => new { Facility_ID = s.FacilityId, Facility_Name = s.FacilityName }), JsonRequestBehavior.AllowGet);

         }
    




    }
}