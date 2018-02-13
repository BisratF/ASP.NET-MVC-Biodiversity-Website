using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using zefitret.Models;
using System.Net;
using System.IO;

namespace zefitret.Controllers
{
    public class SpeciesController : Controller
    {
        zefitretuserdbEntities db = new zefitretuserdbEntities();

       
        public ActionResult Index(string Viewer , string SearchValue)
        {
            var model = from s in db.SpeciesDataTables
                        select new SpeciesModelClass
                        {
                            MId = s.SId,
                            MCategory = s.Category,
                            MSName = s.SName,
                            MLocalName = s.LocalName,
                            MCommonName = s.CommonName,
                            MDescription = s.Description,
                            MMainPic = s.MainPic,
                            MSecPic = s.SecPic,
                            MThirdPic = s.ThirdPic,
                            MFourthPic = s.FourthPic,
                            MPicTakenBy = s.PicTakenBy,
                            MContentBy = s.ContentBy,
                            MEditedBy = s.EditedBy,
                        };


            var Query = from q in model
                        select q;

            if (SearchValue != null)
            {
                return View(model = model.Where(q => q.MSName.Contains(SearchValue)|| 
                q.MLocalName.Contains(SearchValue) || q.MCommonName.Contains(SearchValue)|| q.MCategory.Contains(SearchValue)||
                q.MPicTakenBy.Contains(SearchValue) || q.MContentBy.Contains(SearchValue) ||
                q.MDescription.Contains(SearchValue) || q.MEditedBy.Contains(SearchValue))); 
               
            }

            ViewBag.Amphibians = string.IsNullOrEmpty(Viewer) ? "Amphibians" : "Amphibians";
            ViewBag.Reptiles = string.IsNullOrEmpty(Viewer) ? "Reptiles" : "Reptiles";
            ViewBag.birds = string.IsNullOrEmpty(Viewer) ? "birds" : "birds";
            ViewBag.bugs = string.IsNullOrEmpty(Viewer) ? "invertebrates" : "invertebrates";
            ViewBag.mammals = string.IsNullOrEmpty(Viewer) ? "Mammals" : "Mammals";
            ViewBag.Flowers = string.IsNullOrEmpty(Viewer) ? "flora" : "flora";
            switch (Viewer)
            {
                case "Amphibians":
                    Query = Query.Where(q => q.MCategory == "Amphibians");

                    ViewBag.Category = "Amphibians";
                    break;

                case "Reptiles":
                    Query = Query.Where(q => q.MCategory == "reptiles");
                    ViewBag.Category = "reptiles";
                    break;
                case "birds":
                    Query = Query.Where(q => q.MCategory == "birds");
                    ViewBag.Category = "birds";
                    break;
                case "invertebrates":
                    Query = Query.Where(q => q.MCategory == "invertebrates");
                    ViewBag.Category = "invertebrates";
                    break;
                case "Mammals":
                    Query = Query.Where(q => q.MCategory == "Mammals");
                    ViewBag.Category = "Mammals";
                    break;
                case "flora":
                    Query = Query.Where(q => q.MCategory == "flora");
                    ViewBag.Category = "flora";
                    break;
                default:
                    Query = Query.Where(q => q.MCategory == "Amphibians");
                    ViewBag.Category = "Amphibians";
                    break;
            }

            
            
            return View(Query);
            
        }


        public ActionResult AddnewSpecies()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("LogIn", "Account");
            }

            return View();
        }



       

        [HttpPost, ActionName("AddnewSpecies")]
        public ActionResult SaveDataAdded([Bind(Include = "SId,Category,SName,LocalName,CommonName,Description,PicTakenBy,ContentBy,EditedBy")]SpeciesDataTable ARow, HttpPostedFileBase file, HttpPostedFileBase file2, HttpPostedFileBase file3, HttpPostedFileBase file4)
        {

            if (ModelState.IsValid)
            {

                if (file != null && file.ContentLength > 0)
                {
                    using (var Bnryreader = new System.IO.BinaryReader(file.InputStream))
                    {
                        ARow.MainPic = Bnryreader.ReadBytes(file.ContentLength);
                    }

                }

                if (file2 != null && file2.ContentLength > 0)
                {
                    using (var reader = new System.IO.BinaryReader(file2.InputStream))
                    {
                        ARow.SecPic = reader.ReadBytes(file2.ContentLength);
                    }

                }
                if (file3 != null && file3.ContentLength > 0)
                {
                    using (var reader = new System.IO.BinaryReader(file3.InputStream))
                    {
                        ARow.ThirdPic = reader.ReadBytes(file3.ContentLength);
                    }

                }
                if (file4 != null && file4.ContentLength > 0)
                {
                    using (var reader = new System.IO.BinaryReader(file4.InputStream))
                    {
                        ARow.FourthPic = reader.ReadBytes(file4.ContentLength);
                    }

                }


                db.SpeciesDataTables.Add(ARow);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ARow);

        }



        // EditSpecies Get
        public ActionResult EditSpecies(int EId)  // this EId is coming from Index EditSpecies link
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("LogIn", "Account");
            }



            var e = db.SpeciesDataTables.Find(EId);
            var vm = new SpeciesModelClass() { MId = EId, MSName = e.SName };
            vm.MCategory = e.Category;
            vm.MLocalName = e.LocalName;
            vm.MCommonName = e.CommonName;
            vm.MDescription = e.Description;
            vm.MPicTakenBy = e.PicTakenBy;
            vm.MContentBy = e.ContentBy;
            vm.MEditedBy = e.EditedBy;

            if (e.MainPic != null)
            {
                vm.MainSrc = $"data:image/jpg;base64,{Convert.ToBase64String(e.MainPic)}";
            }
            if (e.SecPic != null)
            {
                vm.SecondSrc = $"data:image/jpg;base64,{Convert.ToBase64String(e.SecPic)}";
            }
            if (e.ThirdPic != null)
            {
                vm.ThirdSrc = $"data:image/jpg;base64,{Convert.ToBase64String(e.ThirdPic)}";
            }
            if (e.FourthPic != null)
            {
                vm.FourthSrc = $"data:image/jpg;base64,{Convert.ToBase64String(e.FourthPic)}";
            }
            return View(vm);


        }

        [HttpPost]

        public ActionResult EditSpecies(SpeciesModelClass model, int MId)
        {
            var e = db.SpeciesDataTables.Find(MId);
            e.Category = model.MCategory;
            e.SName = model.MSName;
            e.LocalName = model.MLocalName;
            e.CommonName = model.MCommonName;
            e.Description = model.MDescription;
            e.PicTakenBy = model.MPicTakenBy;
            e.ContentBy = model.MContentBy;
            e.EditedBy = model.MEditedBy;

            //Update the image properties only if it was send from the form
            if (model.MainImg != null)
            {
                e.MainPic = GetByteArrayFromImage(model.MainImg);
            }
            if (model.SecondImg != null)
            {
                e.SecPic = GetByteArrayFromImage(model.SecondImg);
            }
            if (model.ThirdImg != null)
            {
                e.ThirdPic = GetByteArrayFromImage(model.ThirdImg);
            }
            if (model.FourthImg != null)
            {
                e.FourthPic = GetByteArrayFromImage(model.FourthImg);
            }

            db.SaveChanges();
            return RedirectToAction("Index");

        }
        private byte[] GetByteArrayFromImage(HttpPostedFileBase file)
        {
            if (file == null)
                return null;
            var target = new MemoryStream();
            file.InputStream.CopyTo(target);
            return target.ToArray();
        }




        public ActionResult SpeciesDetail(int? DetailId)
        {
            if (DetailId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            SpeciesDataTable DetailTable = db.SpeciesDataTables.Find(DetailId);
            if (DetailTable == null)
            {
                return HttpNotFound();
            }
            return View(DetailTable);

        }




        // delete get Request
        public ActionResult DeleteSpecies(int? DeleteId)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("LogIn", "Account");
            }


            if (DeleteId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            SpeciesDataTable SMCForDelete = db.SpeciesDataTables.Find(DeleteId);
            if (SMCForDelete == null)
            {
                return HttpNotFound();
            }
            return View(SMCForDelete);
        }

        [HttpPost, ActionName("DeleteSpecies")]
        public ActionResult ConfirmDelete(int? DeleteId)
        {
            SpeciesDataTable RowToDelete = db.SpeciesDataTables.Find(DeleteId);
            db.SpeciesDataTables.Remove(RowToDelete);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}