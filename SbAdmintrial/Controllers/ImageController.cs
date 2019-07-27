using MySql.Data.MySqlClient;
using SbAdmintrial.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SbAdmintrial.Controllers
{
    public class ImageController : Controller
    {
      [HttpGet]
        public ActionResult Add(int id)
        {
            return View(id);
        }

        [HttpPost]
        public ActionResult Add(HttpPostedFileBase file,int id)
        {

            if(file !=null && file.ContentLength > 0)
            {
                string filename = Path.GetFileName(file.FileName);
                string fileext = Path.GetExtension(filename);
                if(fileext==".jpg" || fileext == ".png"||fileext==".docx" ||fileext==".pdf"||fileext==".mp4")
                {
                    string filepath = Path.Combine(Server.MapPath("~/Image"), filename);
                    string mainconn = ConfigurationManager.ConnectionStrings["sbsclatest"].ConnectionString.ToString();
                    MySqlConnection mysqlconn = new MySqlConnection(mainconn);                  

                    MySqlCommand mysqlcon = new MySqlCommand("Insertimgsp", mysqlconn);
                    mysqlcon.CommandType = CommandType.StoredProcedure;
                    mysqlconn.Open();

                    mysqlcon.Parameters.AddWithValue("@p_name", filename);
                    mysqlcon.Parameters.AddWithValue("@p_ext", fileext);
                    mysqlcon.Parameters.AddWithValue("@p_path", filepath);
                    mysqlcon.Parameters.AddWithValue("@p_mid", id);
                    mysqlcon.ExecuteNonQuery();
                    file.SaveAs(filepath);

                    mysqlconn.Close();                  

                   
                }
            }
            return RedirectToAction("Add");
            
        }

        [HttpGet]
        public ActionResult View(int id)
        {
            meterial imageModel = new meterial();


            DataTable dtblproduct = new DataTable();

            try
            {
                string conString = ConfigurationManager.ConnectionStrings["sbsclatest"].ConnectionString.ToString();
                MySqlConnection con = new MySqlConnection(conString);
                /// con. connectionString = "server=localhost;user id=root;database=mydb;persistsecurityinfo=True";
                /// 
                /// open the connection
                con.Open();
                ViewBag.successMessage = "connection was established";
                string theSql = "SELECT * FROM material WHERE idmaterial=@idmaterial";
                MySqlDataAdapter command = new MySqlDataAdapter(theSql, con);
                command.SelectCommand.Parameters.AddWithValue("@idmaterial", id);

                command.Fill(dtblproduct);
                return View(imageModel);
            }  
                    
            catch (Exception e)
            {
                ViewBag.errorMessage = e.Message;
            }
            return View();
}

        public ActionResult Download()
        {
            return View();
        }
    }
}