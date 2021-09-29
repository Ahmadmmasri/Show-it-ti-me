using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Ecommerce.Models;
using System.IO;
using System.Data.Entity;

namespace Ecommerce.Controllers
{
    public class UserController : Controller
    {
        Entities db = new Entities();
        // GET: User

        [HttpGet]
        public ActionResult login()
        {
            Session["user_id"] = null;
            return View();

        }

        [HttpPost]
        public ActionResult login(tbl_user value)
        {
            tbl_user User = db.tbl_user.Where(x => x.u_email == value.u_email && x.u_password == value.u_password).FirstOrDefault();
            if (User != null)
            {
                Session["user_id"] = User.u_id.ToString();
                ViewBag.username = "Welcome" + User.u_name;
                return RedirectToAction("index");
            }
            else
            {
                ViewBag.message = "Invalid user or password";
                return View();
            }
        }

        public ActionResult Index(int? page)
        {
            
            int pagesize = 10, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.tbl_category.Where(x => x.cate_Statu == 1).OrderByDescending(x => x.cate_id).ToList();
            IPagedList<tbl_category> stu = list.ToPagedList(pageindex, pagesize);
            return View(stu);
        }



        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(tbl_user user,HttpPostedFileBase userImage)
        {

            try
            {
                if (userImage != null && userImage.ContentLength > 0)
                {
                    string _path = "";
                    string extension = Path.GetExtension(userImage.FileName);

                    if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".png"))
                    {
                        _path = "~/UsersPhotos" + Path.GetFileName(DateTime.Now.ToString("dd/MM/yy MM") + Session.SessionID + userImage.FileName).Replace(" ", "").Replace(":", "");
                        userImage.SaveAs(Server.MapPath(_path));
                    }

                    tbl_user UserInsert = new tbl_user();
                    UserInsert.u_name = user.u_name;
                    UserInsert.u_email = user.u_email;
                    UserInsert.u_password = user.u_password;
                    UserInsert.u_img = _path;
                    UserInsert.u_contact = user.u_contact;

                    db.tbl_user.Add(UserInsert);
                    db.SaveChanges();
                    ViewBag.Message = "Account Created Successfully!!";

                }
                return View();
            }
            catch (Exception)
            {
                ViewBag.Message = "File upload failed!! " + "information duplicated";
                return View();
            }
        }

        [HttpGet]
        public ActionResult CreateAd()
        {

            if (@Session["user_id"] != null)
                return View();
            else
                return RedirectToAction("login");

        }

        [HttpPost]
        public ActionResult CreateAd(tbl_product prod, HttpPostedFileBase imgfile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (imgfile != null && imgfile.ContentLength > 0)
                    {
                        string _path = "";
                        string extension = Path.GetExtension(imgfile.FileName);

                        if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".png"))
                        {
                            _path = "~/UsersPhotos" + Path.GetFileName(DateTime.Now.ToString("dd/MM/yy MM") + Session.SessionID + imgfile.FileName).Replace(" ", "").Replace(":", "");
                            imgfile.SaveAs(Server.MapPath(_path));
                        }

                        tbl_product ProdInsert = new tbl_product();
                        ProdInsert.prod_fk_cate = Convert.ToInt32(prod.prod_fk_cate);
                        ProdInsert.prod_name = prod.prod_name;
                        ProdInsert.prod_img = _path;
                        ProdInsert.prod_description = prod.prod_description;
                        ProdInsert.prod_price = prod.prod_price;
                        ProdInsert.prod_fk_user = Int32.Parse(Session["user_id"].ToString());

                        db.tbl_product.Add(ProdInsert);
                        db.SaveChanges();
                        ViewBag.Message = "Product Created Successfully!!";
                        return RedirectToAction("Index");

                    }
                    return View();
                }
                return View();
            }
            catch (Exception e)
            {
                ViewBag.Message = "File upload failed!! " + e;
                return View();
            }

        }  
        
        
        [HttpGet]

        public ActionResult EditAdv(int? id)
        {

            if (@Session["user_id"] != null)
            {
                tbl_product prodEdit = db.tbl_product.Find(id);
                if (prodEdit == null)
                {
                    return HttpNotFound();
                }
                return View(prodEdit);

            }
            else
                return RedirectToAction("login");

        }

        [HttpPost]
        public ActionResult EditAdv(tbl_product prod, HttpPostedFileBase imgfile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (imgfile != null && imgfile.ContentLength > 0)
                    {
                        string _path = "";
                        string extension = Path.GetExtension(imgfile.FileName);

                        if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".png"))
                        {
                            _path = "~/UsersPhotos" + Path.GetFileName(DateTime.Now.ToString("dd/MM/yy MM") + Session.SessionID + imgfile.FileName).Replace(" ", "").Replace(":", "");
                            imgfile.SaveAs(Server.MapPath(_path));
                        }

                        tbl_product ProdEdit = new tbl_product();
                        ProdEdit.prod_id = prod.prod_id;
                        ProdEdit.prod_fk_cate = Convert.ToInt32(prod.prod_fk_cate);
                        ProdEdit.prod_name = prod.prod_name;
                        ProdEdit.prod_img = _path;
                        ProdEdit.prod_description = prod.prod_description;
                        ProdEdit.prod_price = prod.prod_price;
                        ProdEdit.prod_fk_user = Int32.Parse(Session["user_id"].ToString());

                        db.Entry(ProdEdit).State = EntityState.Modified;
                        db.SaveChanges();
                        ViewBag.Message = "Product Edited Successfully!!";
                        return RedirectToAction("Index");

                    }
                    return View();
                }
                return View();
            }
            catch (Exception e)
            {
                ViewBag.Message = "File upload failed!! " + e;
                return View();
            }

        }

        [HttpGet]
        public ActionResult Adv(int ?id,int? page) 
        {
            int pagesize = 10, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.tbl_product.Where(x => x.prod_fk_cate == id).OrderByDescending(x => x.prod_id).ToList();
            IPagedList<tbl_product> stu = list.ToPagedList(pageindex, pagesize);
            return View(stu);
        }

        [HttpPost]
        public ActionResult Adv(int? id, int? page,string search)
        {
            int pagesize = 10, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.tbl_product.Where(x => x.prod_name.Contains(search)).OrderByDescending(x => x.prod_id).ToList();
            IPagedList<tbl_product> stu = list.ToPagedList(pageindex, pagesize);
            return View(stu);
        }

        public ActionResult ViewAd(int ? id)
        {
            if(id!=null)
            {
                AdModel ad = new AdModel();
                tbl_product p = db.tbl_product.Where(x => x.prod_id == id).FirstOrDefault();
                ad.prod_id = (id == null ? 0 : p.prod_id);
                ad.prod_name = p.prod_name;
                ad.prod_img = p.prod_img;
                ad.prod_description = p.prod_description;
                ad.prod_price = p.prod_price;
                tbl_category categ = db.tbl_category.Where(x => x.cate_id == p.prod_fk_cate).SingleOrDefault();
                ad.cate_name = categ.cate_name;
                tbl_user User = db.tbl_user.Where(x => x.u_id == p.prod_fk_user).SingleOrDefault();
                ad.u_name = p.prod_name;
                ad.u_img = p.prod_img;
                ad.u_contact = User.u_contact;
                ad.prod_fk_user = User.u_id;
                return View(ad);
            }
            else
            {
                return RedirectToAction("login");
            }
        }

        public ActionResult SignOut()
        {
            Session.RemoveAll();
            Session.Abandon();

           return RedirectToAction("Index");
        }

        public ActionResult DeleteAdv(int? id)
        {
            tbl_product deleteProduct= db.tbl_product.Where(x => x.prod_id == id).SingleOrDefault();
            db.tbl_product.Remove(deleteProduct);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public void CategoryList()
        {
            var CateList = db.tbl_category.ToList();
            Session["CateList"] = new SelectList(CateList, "cate_id", "cate_name");
        }
    }

}

