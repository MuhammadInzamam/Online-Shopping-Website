using OnlineShoppingStore_2108G1.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShoppingStore_2108G1.Controllers
{
    
    public class AdminController : Controller
    {
        private ecommerce_AUHEntities db = new ecommerce_AUHEntities();
        [HttpPost]
        public ActionResult Login(User user)
        {
            if (ModelState.IsValid)
            {
                var obj = db.Users.Where(x => x.Email.Equals(user.Email)
                            && x.Password.Equals(user.Password)
                            && x.Email_Verified.Equals("Y")
                            && x.Role.Equals("Admin")).FirstOrDefault();
                if (obj != null) //On success
                {
                   
                    Session["LoggedInAdminName"] = obj.Name;
                   
                    return RedirectToAction("Index", "Admin");
                }
            }
            ModelState.AddModelError("InvalidLogin", "Email or Password is incorrect!");
            return View();
        }
       
        // GET: Admin
        public ActionResult Login()
        {

            return View();
        }
        public ActionResult Index()
        {
            if (Session["LoggedInAdminName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.TotalUsers = db.Users.Count();
            ViewBag.TotalOrders = db.Orders.Count();
            ViewBag.TotalEarning = db.Orders.Where(a => a.Order_Status.Equals("Completed")).Select(a => a.Total_Amount).Sum();

            return View();
        }
        public ActionResult addproduct()
        {
            ViewBag.Categorieslist = new SelectList(db.Categories, "Category_ID", "Category_Name");
            if (Session["LoggedInAdminName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.CategoryList = new SelectList(db.Categories, "Category_ID", "Category_Name");
            return View();
        }
        [HttpPost]
        public ActionResult addproduct(Product product, HttpPostedFileBase file)
        {
            ViewBag.Categorieslist = new SelectList(db.Categories, "Category_ID", "Category_Name", product.Category_ID);

            if(ModelState.IsValid)
            {
                string filename = Path.GetFileNameWithoutExtension(file.FileName);
                string extension = Path.GetExtension(file.FileName);
                filename = filename + DateTime.Now.ToString("yymmss") + extension;
                product.Picture = "/External/Main/Products/" + filename;
                filename = Path.Combine(Server.MapPath("/External/Main/Products/"), filename);
                file.SaveAs(filename);

                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(product);

        }
        public ActionResult AddCategory()
        {
            ViewBag.Categorieslist = new SelectList(db.Categories, "Category_ID", "Category_Name");
            if (Session["LoggedInAdminName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.CategoryList = new SelectList(db.Categories, "Category_ID", "Category_Name");
            return View();
        }
        [HttpPost]
        public ActionResult AddCategory(Category Category, HttpPostedFileBase file)
        {
            ViewBag.Categorieslist = new SelectList(db.Categories, "Category_ID", "Category_Name", Category.Category_ID);

            if (ModelState.IsValid)
            {
              

                db.Categories.Add(Category);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(Category);

        }


    }
}