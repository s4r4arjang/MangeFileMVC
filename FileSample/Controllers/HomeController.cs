using FileSample.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;

namespace FileSample.Controllers
{
    public class HomeController : Controller
    {
        public IPLFContext _dbContext = new IPLFContext();
        public ActionResult Index()
        {
            var model = _dbContext.VehicleFiles.Select(p => new AttachmentListViewModel()
            {
                Id = p.Id,
                path = p.FilePath
            }).ToList();
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        [HttpPost]
        public ActionResult CreateVehicleattachment(HttpPostedFileBase fileV)
        {

            string[] imageExtention = { ".bmp", ".jpeg", ".png", ".jpg", ".gif", ".tiff", ".png" };
            var VFile = new VehicleFile();

            if (fileV != null)
            {
                Guid guid_name = Guid.NewGuid();


                string extension = Path.GetExtension(fileV.FileName);
                if (!imageExtention.Contains(extension.ToLower()))
                {

                    return Json(2, JsonRequestBehavior.AllowGet);
                }
                //System.Web.HttpContext.Current.
                var destinationDirectory = Server.MapPath("~/Content/Najm_temp/");
                if (!System.IO.Directory.Exists(destinationDirectory))
                {
                    System.IO.Directory.CreateDirectory(destinationDirectory);
                }
                string fname = "Vehicle" + Guid.NewGuid().ToString();
                string p = destinationDirectory + fname + extension;
                fileV.SaveAs(p);
                VFile.Name = fname + extension;
                VFile.FilePath = "/Content/Najm_temp/" + fname + extension;
                _dbContext.VehicleFiles.Add(VFile);
                var d = _dbContext.SaveChanges();
                return Json(new { Id = VFile.Id , Path= VFile.FilePath}, JsonRequestBehavior.AllowGet);

            }

            return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        public ActionResult DeleteVehicleattachment(int Id)
        {
            var message = "";
            var status = 0; 
           var file =  _dbContext.VehicleFiles.Find(Id);
            var destinationDirectory = Server.MapPath("~/Content/Najm_temp/");
            string extension = Path.GetExtension(file.Name);
            string p = destinationDirectory + file.Name;
            var path = p;
            FileInfo fileinf = new FileInfo(p);
            if (fileinf.Exists)//check file exsit or not  
            {
                fileinf.Delete();
                _dbContext.VehicleFiles.Remove(file);
                _dbContext.SaveChanges();
                message =  " file deleted successfully";
                status = 1; 


            }
            else
            {
                message = " This file does not exists ";
                status = 0;
            }

            return Json(new { message = message, status= status }, JsonRequestBehavior.AllowGet);   
        }


    }
}