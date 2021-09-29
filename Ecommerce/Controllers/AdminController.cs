using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ecommerce.Models;
using System.Web.Mvc;
using System.IO;
using System.Web.UI;
using PagedList.Mvc;
using PagedList;

namespace Ecommerce.Controllers
{
    
    public class AdminController : Controller
    {
        Entities db = new Entities();
        
        // GET: Admin
        [HttpGet]
        public ActionResult login()
        {
            Session["ad_id"] = null;
            return View();

        }

        [HttpPost]
        public ActionResult login(tbl_admin value)
        {
            tbl_admin admin = db.tbl_admin.Where(x => x.ad_user == value.ad_user && x.ad_password == value.ad_password).FirstOrDefault();
            if (admin != null)
            {
                Session["ad_id"] = admin.ad_id.ToString();
                return RedirectToAction("index","Home");
            }
            else
            {
                ViewBag.message = "Invalid user or password";
                return View();
            }
        }


        public ActionResult Create() {
            if (Session["ad_id"]!=null)
            {
                return View();
            }
            else
              return RedirectToAction("login");
        }

        [HttpPost]
        public ActionResult Create(tbl_category cate, HttpPostedFileBase imgfile)
        {
            try
            {
                if (imgfile !=null && imgfile.ContentLength > 0)
                {
                    string _path="";
                    string extension = Path.GetExtension(imgfile.FileName);
                    if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".png")) {

                        _path = "~/UploadedFiles" + Path.GetFileName(DateTime.Now.ToString("dd/MM/yy MM") + Session.SessionID + imgfile.FileName).Replace(" ", "").Replace(":", "");
                        imgfile.SaveAs(Server.MapPath(_path));

                    }
                    tbl_category cateInsert = new tbl_category();
                    cateInsert.cate_name = cate.cate_name;
                    cateInsert.cate_img = _path;
                    cateInsert.cate_Statu =1;
                    cateInsert.cate_fk_ad = Convert.ToInt32( Session["ad_id"].ToString() );

                    db.tbl_category.Add(cateInsert);
                    db.SaveChanges();

                }
                ViewBag.Message = "File Uploaded Successfully!!";
                return View();
            }
            catch(Exception e)
            {
                ViewBag.Message = "File upload failed!! "+e;
                return View();
            }
        }


        public ActionResult ViewProduct(int? page) {
            int pagesize = 10, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.tbl_category.Where(x => x.cate_Statu == 1).OrderByDescending(x=>x.cate_id).ToList();
            IPagedList<tbl_category> stu = list.ToPagedList(pageindex, pagesize);
            return View(stu);
        }
    }
}