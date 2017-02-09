using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using A_Song_of_Ice_and_Fire.Models;

namespace A_Song_of_Ice_and_Fire.Controllers
{
    public class HomeController : Controller
    {
        private NewDatabaseEntities db = new NewDatabaseEntities();

        public ActionResult Index(String house, String searchString)
        {
            var HouseList = new List<String>();
            var houseQuery = from houseData in db.ASOIAFs
                             orderby houseData.Allegiance
                             select houseData.Allegiance;

            HouseList.AddRange(houseQuery.Distinct());
            ViewBag.house = new SelectList(HouseList);


            var search = from s in db.ASOIAFs
                         select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                search = search.Where(s => s.Name.Contains(searchString) || s.Alias.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(house))
            {
                search = search.Where(x => x.Allegiance == house);
            }

            return View(search);

        }

        // GET: Home
        //public ActionResult Index()
        //{
        //    return View(db.ASOIAFs.ToList());
        //}

        // GET: Home/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ASOIAF aSOIAF = db.ASOIAFs.Find(id);
            if (aSOIAF == null)
            {
                return HttpNotFound();
            }
            return View(aSOIAF);
        }

        // GET: Home/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Name,Allegiance,Alias,Image,Profile,Are_They_Dead_")] ASOIAF aSOIAF)
        {
            if (ModelState.IsValid)
            {
                db.ASOIAFs.Add(aSOIAF);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (aSOIAF.Image == null)
            {
                aSOIAF.Image = "http://tinyurl.com/zvktb55";
            }
            if (aSOIAF.Alias == null)
            {
                aSOIAF.Allegiance = "Unknown";
            }
            if (aSOIAF.Profile == null)
            {
                aSOIAF.Profile = "Unknown";
            }

            return View(aSOIAF);
        }

        // GET: Home/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ASOIAF aSOIAF = db.ASOIAFs.Find(id);
            if (aSOIAF == null)
            {
                return HttpNotFound();
            }
            return View(aSOIAF);
        }

        // POST: Home/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Name,Allegiance,Alias,Image,Profile,Are_They_Dead_")] ASOIAF aSOIAF)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aSOIAF).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aSOIAF);
        }

        // GET: Home/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ASOIAF aSOIAF = db.ASOIAFs.Find(id);
            if (aSOIAF == null)
            {
                return HttpNotFound();
            }
            return View(aSOIAF);
        }

        // POST: Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ASOIAF aSOIAF = db.ASOIAFs.Find(id);
            db.ASOIAFs.Remove(aSOIAF);
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
