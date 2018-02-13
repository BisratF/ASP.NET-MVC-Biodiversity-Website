using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using zefitret.Models;
using Newtonsoft.Json;
using System.Data.Entity;
using System.Web.Security;
using System.Threading.Tasks;

namespace zefitret.Controllers
{
    public class AccountController : Controller
    {
        zefitretuserdbEntities db = new zefitretuserdbEntities();


        // user control panel

        public ActionResult Users()
        {
            return View();


        }

        public ActionResult LogIn(string Rurl)
        {
            ViewBag.ReturnUrl = Rurl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(UsersModel LC)
        {
           

            var logger = db.UserTables.Where(a => a.UserName.Equals(LC.UserName) && a.PassWord.Equals(LC.PassWord));
            int c = logger.Count();
            if (c == 1)
            {
                var X = (from q in logger
                         select q.UserName).Single();
                var Y = (from y in logger
                         select y.FullName).Single();

                ViewBag.user = X;

                Session["user"] = new UsersModel() { FullName = Y, UserName = X };


                return RedirectToAction("Index", "Species");
            }
            else
                return RedirectToAction("LogIn", "Account");
          
           

        }

       
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Species");
        }


        // calles the user lists to Users()
        public JsonResult GetUsersList()
        {


            List<UsersModel> StuList = db.UserTables.Select(x => new UsersModel
            {
                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email,
                UserName = x.UserName,
                PassWord = x.PassWord,
                Privilege = x.Privilege
            }).ToList();

            
           

            return Json(StuList, JsonRequestBehavior.AllowGet);
        }

        // when edit buttun clicked it adds the user detail to the modal
        public JsonResult GetUserByID(int Id)
        {
            UserTable ut = db.UserTables.Where(x => x.Id == Id).SingleOrDefault();

            string somedata = string.Empty;
            somedata = JsonConvert.SerializeObject(ut, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(somedata, JsonRequestBehavior.AllowGet);

        }



        [HttpPost]     // it saves the changes to "UserTable" which is a Database Table 
        public ActionResult SaveData(UserTable umodel)
        {
            bool result = false;
            try
            {
                if ((umodel.Id > 0) && (umodel.Id != 2))
                {

                    // Edit user Details 
                    UserTable ut = db.UserTables.SingleOrDefault(x => x.Id == umodel.Id);
                    ut.FullName = umodel.FullName;
                    ut.Email = umodel.Email;
                    ut.UserName = umodel.UserName;
                    ut.PassWord = umodel.PassWord;
                    ut.Privilege = umodel.Privilege;
                    db.SaveChanges();
                    result = true;

                }
                else
                {    // Add a user 
                    UserTable utbl = new UserTable();
                    utbl.FullName = umodel.FullName;
                    utbl.Email = umodel.Email;
                    utbl.UserName = umodel.UserName;
                    utbl.PassWord = umodel.PassWord;
                    utbl.Privilege = umodel.Privilege;
                    db.UserTables.Add(utbl);
                    db.SaveChanges();
                    result = true;
                }
            }
            catch (Exception exep)
            {
                throw exep;
            }

            return Json(umodel, JsonRequestBehavior.AllowGet);
        }

        // Delete user profile 
        public ActionResult DeleteAuser(int Id)
        {


            bool result = false;
            var utDel = db.UserTables.Where(I => I.Id == Id).FirstOrDefault();

            if ((utDel != null) && (utDel.Id != 2))
            {
                db.UserTables.Remove(utDel);
                db.SaveChanges();

                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }













    }
}