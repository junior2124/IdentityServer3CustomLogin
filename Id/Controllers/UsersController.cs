using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Id.Models;

namespace Id.Controllers
{
    public class UsersController : Controller
    {
        private ISSDBContext db = new ISSDBContext();
        
        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Username,Password,AcceptedEula")] User user)
        {
            if (ModelState.IsValid)
            {
                var plainPW = user.Password;
                //Encrypt PW
                var encryptedPW = General.AesEncrypt(plainPW, ConstantVars.IISAesSalt, ConstantVars.IISAesPassword);
                user.Password = encryptedPW;

                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Username,Password,AcceptedEula")] User user)
        {
            if (ModelState.IsValid)
            {
                var plainPW = user.Password;
                //Encrypt PW
                var encryptedPW = General.AesEncrypt(plainPW, ConstantVars.IISAesSalt, ConstantVars.IISAesPassword);
                user.Password = encryptedPW;

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                List<UserClaim> userClaims = new List<UserClaim>();
                UserClaim userSubjectClaim = new UserClaim() { ClaimType = "Subject", Value = user.Id.ToString(), Id = user.Id };
                UserClaim userRoleClaim = new UserClaim() { ClaimType = "Role", Value = "User", Id = user.Id };
                userClaims.Add(userSubjectClaim);
                userClaims.Add(userRoleClaim);
                db.UserClaims.AddRange(userClaims);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
