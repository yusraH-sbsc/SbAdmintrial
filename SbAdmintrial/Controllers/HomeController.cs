using MySql.Data.MySqlClient;
using System.Data;
using SbAdmintrial.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SbAdmintrial.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult landingpage()
        {
            return View();
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


        [HttpGet]
        public ActionResult Register()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        public ActionResult Register(User theUser)
        {
            string defaultrole = "EMPLOYEE";

            
            if (theUser.Password != theUser.password_confirm || theUser.Password.Length != 5)
            {

                ViewBag.errorMessage = "Password missmatch or pass word lengths has to be 5 didgits";
                return View();


            }

            else
            {
                ///store i in the db
                /// establish the connection
                try
                {
                    /// string connectionString = "server=localhost;user id=root;database=mydb;persistsecurityinfo=True";

                    string conString = ConfigurationManager.ConnectionStrings["sbsclatest"].ConnectionString.ToString();
                    MySqlConnection con = new MySqlConnection(conString);
                    /// con. connectionString = "server=localhost;user id=root;database=mydb;persistsecurityinfo=True";
                    /// 
                    /// open the connection
                    con.Open();
                    ViewBag.successMessage = "connection was established";
                    string theSql = "insert user(firstname,lastname,username,email,role,password)values(@firstname,@lastname,@username,@email,@role,@password)";
                    MySqlCommand command = new MySqlCommand(theSql, con);

                    command.Parameters.Add("@firstname", MySqlDbType.String);
                    command.Parameters["@firstname"].Value = theUser.FirstName;

                    command.Parameters.Add("@lastname", MySqlDbType.String);
                    command.Parameters["@lastname"].Value = theUser.LastName;

                    command.Parameters.Add("@username", MySqlDbType.String);
                    command.Parameters["@username"].Value = theUser.username;

                    command.Parameters.Add("@email", MySqlDbType.String);
                    command.Parameters["@email"].Value = theUser.Email;

                    command.Parameters.Add("@role", MySqlDbType.String);
                    command.Parameters["@role"].Value = defaultrole;

                    command.Parameters.Add("@password", MySqlDbType.String);
                    command.Parameters["@password"].Value = theUser.Password;

                    command.ExecuteNonQuery();
                    ViewBag.success = "Registration Successful";
                    con.Close();


                }

                catch (Exception e)
                {
                    ViewBag.errorMessage = e.Message;
                    return View();
                }





            }
            return RedirectToAction("login");
        }
    
//---------------------------------------------------------------------------------------------------------------------------------------------------------------//        

        [HttpGet]
        public ActionResult logIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult logIn(User p)
        {
            try
            {
                string conString = ConfigurationManager.ConnectionStrings["sbsclatest"].ConnectionString.ToString();
                MySqlConnection con = new MySqlConnection(conString);

                MySqlCommand cmd = new MySqlCommand("Select * from user where username=@username and password=@password", con);
                cmd.Parameters.AddWithValue(("@username"), p.username);
                cmd.Parameters.AddWithValue(("@password"), p.Password);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable DT = new DataTable();
                da.Fill(DT);
                if (DT.Rows.Count > 0)
                {
                    string e = (DT.Rows[0]["role"]).ToString();
                    p.idUser = Convert.ToInt32((DT.Rows[0]["iduser"]).ToString());
                    //return RedirectTousernameAction("AfterLogin");

                    if (e == "IT_STAFF")
                    {

                        Session["isLogged"] = true;
                        Session["username"] = p.username;
                        Session["iduser"] = p.idUser;
                        return RedirectToAction("landingpage");
                    }
                    else if (e == "HR_MANAGEMENT")
                    {
                        ViewBag.successMessage = "Welcome";
                        Session["isLogged"] = true;
                        Session["username"] = p.username;
                        Session["iduser"] = p.idUser;
                        return RedirectToAction("About");

                    }

                    else if (e == "EMPLOYEE")
                    {
                        ViewBag.successMessage = "Welcome";
                        Session["isLogged"] = true;
                        Session["username"] = p.username;
                        Session["iduser"] = p.idUser;
                        return RedirectToAction("Contact");

                    }
                }
                else
                {
                    ViewBag.Message = "invalid username or password";

                }

            }
            catch (Exception e)
            {
                ViewBag.Message = "oop! wrong Email or Password";
                return View();
            }
            return View();

        }
        public ActionResult logout()
        {


            Session["firstname"] = null;
            Session["iduser"] = null;
            return RedirectToAction("login", "Home");
            //Response.DisableUserCache();
            //Session.Abandon();
            //return RedirectPermanent("homepage");
        }
        [HttpGet]
        public ActionResult userprofile(retrievuser user)
        {
            DataTable dtblproduct = new DataTable();
            user.idUser= (int)Session["iduser"];

            try
            {
                string conString = ConfigurationManager.ConnectionStrings["sbsclatest"].ConnectionString.ToString();
                MySqlConnection con = new MySqlConnection(conString);
                /// con. connectionString = "server=localhost;user id=root;database=mydb;persistsecurityinfo=True";
                /// 
                /// open the connection
                con.Open();
                ViewBag.successMessage = "connection was established";
                string theSql = "SELECT * FROM user WHERE iduser=@iduser";
                MySqlDataAdapter command = new MySqlDataAdapter(theSql, con);
                command.SelectCommand.Parameters.AddWithValue("@iduser", user.idUser);

                command.Fill(dtblproduct);

                if (dtblproduct.Rows.Count == 1)
                {
                    
                    user.idUser= Convert.ToInt32(dtblproduct.Rows[0][0].ToString());
                    user.FirstName = dtblproduct.Rows[0][1].ToString();
                    user.LastName = dtblproduct.Rows[0][2].ToString();
                    user.username = dtblproduct.Rows[0][3].ToString();
                    user.Email = dtblproduct.Rows[0][4].ToString();
                    user.Role= dtblproduct.Rows[0][5].ToString();
                    user.Password = dtblproduct.Rows[0][6].ToString();
                   

                    return View(user);
                }
                else
                {
                    return RedirectToAction("courselist");
                }
                return View(user);
            }

            catch (Exception e)
            {
                ViewBag.errorMessage = e.Message;
            }
            return View();
        }



        [HttpGet]
        public ActionResult editprofile(int id)
        {

            retrievuser user = new retrievuser();

            DataTable dtblproduct = new DataTable();
            user.idUser = (int)Session["iduser"];

            try
            {
                string conString = ConfigurationManager.ConnectionStrings["sbsclatest"].ConnectionString.ToString();
                MySqlConnection con = new MySqlConnection(conString);
                /// con. connectionString = "server=localhost;user id=root;database=mydb;persistsecurityinfo=True";
                /// 
                /// open the connection
                con.Open();
                ViewBag.successMessage = "connection was established";
                string theSql = "SELECT * FROM user WHERE iduser=@iduser";
                MySqlDataAdapter command = new MySqlDataAdapter(theSql, con);
                command.SelectCommand.Parameters.AddWithValue("@iduser", user.idUser);

                command.Fill(dtblproduct);

                if (dtblproduct.Rows.Count == 1)
                {

                    user.idUser = Convert.ToInt32(dtblproduct.Rows[0][0].ToString());
                    user.FirstName = dtblproduct.Rows[0][1].ToString();
                    user.LastName = dtblproduct.Rows[0][2].ToString();
                    user.username = dtblproduct.Rows[0][3].ToString();
                    user.Email = dtblproduct.Rows[0][4].ToString();
                    user.Role = dtblproduct.Rows[0][5].ToString();
                    user.Password = dtblproduct.Rows[0][6].ToString();


                    return View(user);
                }
                else
                {
                    return RedirectToAction("courselist");
                }
            }
            catch (Exception e)
            {
                ViewBag.errorMessage = e.Message;
            }
            return View(user);

            }
            


        [HttpPost]
        public ActionResult editprofile(retrievuser user)
        {
            /// string connectionString = "server=localhost;user id=root;database=mydb;persistsecurityinfo=True";

            string conString = ConfigurationManager.ConnectionStrings["sbsclatest"].ConnectionString.ToString();
            MySqlConnection con = new MySqlConnection(conString);
            /// con. connectionString = "server=localhost;user id=root;database=mydb;persistsecurityinfo=True";
            /// 
            /// open the connection
            con.Open();
            ViewBag.successMessage = "connection was established";
            string theSql = "UPDATE user SET iduser=@iduser,firstname=@firstname,lastname=@lastname,Username=@username,email=@email, role=@role, password=@password WHERE iduser=@iduser";
            MySqlCommand command = new MySqlCommand(theSql, con);

            command.Parameters.AddWithValue(("@iduser"), user.idUser);
            command.Parameters.AddWithValue(("@firstname"), user.FirstName);
            command.Parameters.AddWithValue(("@lastname"), user.LastName);
            command.Parameters.AddWithValue(("@Username"), user.username);
            command.Parameters.AddWithValue(("@email"), user.Email);

            command.Parameters.AddWithValue(("@role"), user.Role);
            command.Parameters.AddWithValue(("@password"), user.Password);



            command.ExecuteNonQuery();
            ViewBag.success = "Registration Successful";
            con.Close();





            return RedirectToAction("landingpage");
        }

        [HttpGet]
        public ActionResult UserRole(retrievuser user)
        {

            DataTable dtblproduct = new DataTable();

            /* user.idUser = (int)Session["iduser"]*/

            try
            {
                string conString = ConfigurationManager.ConnectionStrings["sbsclatest"].ConnectionString.ToString();
                MySqlConnection con = new MySqlConnection(conString);
                
                con.Open();
                ViewBag.successMessage = "connection was established";
                string theSql = "SELECT iduser, firstname,lastname,username,role FROM user";
                MySqlDataAdapter command = new MySqlDataAdapter(theSql, con);                
                command.Fill(dtblproduct);

                if (dtblproduct.Rows.Count == 1)
                {
                 
                    return View(dtblproduct);
                }
                else
                {
                    return RedirectToAction("courselist");
                }
            }
            catch (Exception e)
            {
                ViewBag.errorMessage = e.Message;
            }


            return View(dtblproduct);
        }

        [HttpGet]
        public ActionResult changeRole(int id)
        {
            
            DataTable dtblproduct = new DataTable();
            User user = new User();
            user.idUser = id;
            return View(user);


            //try
            //{
            //    string conString = ConfigurationManager.ConnectionStrings["sbsclatest"].ConnectionString.ToString();
            //    MySqlConnection con = new MySqlConnection(conString);
            //    /// con. connectionString = "server=localhost;user id=root;database=mydb;persistsecurityinfo=True";
            //    /// 
            //    /// open the connection
            //    con.Open();
            //    ViewBag.successMessage = "connection was established";
            //    string theSql = "SELECT role FROM user WHERE iduser=@iduser";
            //    MySqlDataAdapter command = new MySqlDataAdapter(theSql, con);
            //    command.SelectCommand.Parameters.AddWithValue("@iduser", user.idUser);

            //    command.Fill(dtblproduct);

            //    if (dtblproduct.Rows.Count == 1)
            //    {


            //        user.Role = dtblproduct.Rows[0][5].ToString();


            //        return View(user);
            //    }
            //    else
            //    {
            //        return RedirectToAction("courselist");
            //    }
            //}
            //catch (Exception e)
            //{
            //    ViewBag.errorMessage = e.Message;
            //}
            //return View(user);


           
        }

        [HttpPost]
        public ActionResult changeRole(retrievuser user)
        {


            string conString = ConfigurationManager.ConnectionStrings["sbsclatest"].ConnectionString.ToString();
            MySqlConnection con = new MySqlConnection(conString);
            /// con. connectionString = "server=localhost;user id=root;database=mydb;persistsecurityinfo=True";
            /// 
            /// open the connection
            con.Open();
            ViewBag.successMessage = "connection was established";
            string theSql = "UPDATE user SET role=@role WHERE iduser=@iduser";
            MySqlCommand command = new MySqlCommand(theSql, con);

            command.Parameters.AddWithValue(("@iduser"), user.idUser);

            command.Parameters.AddWithValue(("@role"), user.Role);
            command.ExecuteNonQuery();
            ViewBag.success = "Registration Successful";
            con.Close();
            return RedirectToAction("UserRole");
        }


        [HttpGet]
        public ActionResult courseReg()
        {
            //course theUser = new course();
            //theUser.authorid = id;
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        public ActionResult courseReg(course theUser)
        {
          
                ///store i in the db
                /// establish the connection
                try
                {
                    /// string connectionString = "server=localhost;user id=root;database=mydb;persistsecurityinfo=True";

                    string conString = ConfigurationManager.ConnectionStrings["sbsclatest"].ConnectionString.ToString();
                    MySqlConnection con = new MySqlConnection(conString);
                    /// con. connectionString = "server=localhost;user id=root;database=mydb;persistsecurityinfo=True";
                    /// 
                    /// open the connection
                    con.Open();
                    ViewBag.successMessage = "connection was established";
                    string theSql = "insert course(coursename,corsecode,courseAuthor,description,datecreated,duration)values(@coursename,@corsecode,@courseAuthor,@description,@datecreated,@duration)";
                    MySqlCommand command = new MySqlCommand(theSql, con);

                    command.Parameters.Add("@coursename", MySqlDbType.String);
                    command.Parameters["@coursename"].Value = theUser.coursename;

                    command.Parameters.Add("@corsecode", MySqlDbType.String);
                    command.Parameters["@corsecode"].Value = theUser.coursecode;

                    command.Parameters.Add("@courseAuthor", MySqlDbType.String);
                    command.Parameters["@courseAuthor"].Value = theUser.authorid;

                    command.Parameters.Add("@description", MySqlDbType.String);
                    command.Parameters["@description"].Value = theUser.description;

                    command.Parameters.Add("@datecreated", MySqlDbType.DateTime);
                    command.Parameters["@datecreated"].Value = theUser.date;

                command.Parameters.Add("@duration", MySqlDbType.String);
                command.Parameters["@duration"].Value = theUser.duration;

                command.ExecuteNonQuery();
                    ViewBag.success = "Registration Successful";
                    con.Close();


                }

                catch (Exception e)
                {
                    ViewBag.errorMessage = e.Message;
                    return View();
                }





            
            return RedirectToAction("Login");
        }

       


       

        public ActionResult courselist()
        {
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

                string theSql = "SELECT * FROM course";
                // MySqlCommand command = new MySqlCommand(theSql, con);

                //creat reader
                MySqlDataAdapter sqda = new MySqlDataAdapter(theSql, con);
                sqda.Fill(dtblproduct);
                if (dtblproduct.Rows.Count != 0)
                { return View(dtblproduct) ; }

                else
                {
                    return RedirectToAction("courseReg");
                }
            }

            catch (Exception e)
            {
                ViewBag.errorMessage = e.Message;
                return RedirectToAction("courseReg");
            }



            return View(dtblproduct);
        }


        [HttpGet]
        public ActionResult courseListHR()
        {
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

                string theSql = "SELECT * FROM course";
                // MySqlCommand command = new MySqlCommand(theSql, con);

                //creat reader
                MySqlDataAdapter sqda = new MySqlDataAdapter(theSql, con);
                sqda.Fill(dtblproduct);
                if (dtblproduct.Rows.Count != 0)
                { return View(dtblproduct); }

                else
                {
                    return RedirectToAction("courseReg");
                }
            }

            catch (Exception e)
            {
                ViewBag.errorMessage = e.Message;
                return RedirectToAction("courseReg");
            }



            return View(dtblproduct);

        }

        [HttpGet]
        public ActionResult courseListEM()
        {
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

                string theSql = "SELECT * FROM course";
                // MySqlCommand command = new MySqlCommand(theSql, con);

                //creat reader
                MySqlDataAdapter sqda = new MySqlDataAdapter(theSql, con);
                sqda.Fill(dtblproduct);
                if (dtblproduct.Rows.Count != 0)
                { return View(dtblproduct); }

                else
                {
                    return RedirectToAction("courseReg");
                }
            }

            catch (Exception e)
            {
                ViewBag.errorMessage = e.Message;
                return RedirectToAction("courseReg");
            }



            return View(dtblproduct);

        }

        [HttpGet]
        public ActionResult editcourses(int id)
        {

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
                string theSql = "SELECT idcourse,coursename,corsecode,courseAuthor,description,duration,isActive FROM course WHERE idcourse=@idcourse";
                MySqlDataAdapter command = new MySqlDataAdapter(theSql, con);
                command.SelectCommand.Parameters.AddWithValue("@idcourse", id);

                command.Fill(dtblproduct);
                if (dtblproduct.Rows.Count == 1)
                {
                    course cus = new course();
                    cus.courseid = Convert.ToInt32(dtblproduct.Rows[0][0].ToString());
                    cus.coursename = dtblproduct.Rows[0][1].ToString();
                    cus.coursecode = dtblproduct.Rows[0][2].ToString();
                  //  cus.date = Convert.ToDateTime(dtblproduct.Rows[0][3]);
                   // cus.authorid = dtblproduct.Rows[0][3].ToString();
                    cus.description = dtblproduct.Rows[0][4].ToString();
                    cus.duration = dtblproduct.Rows[0][5].ToString();
                    cus.isActive = dtblproduct.Rows[0][6].ToString();                 

                    return View(cus);
                }
                else
                {
                    return RedirectToAction("courselist");
                }

            }
            catch (Exception e)
            {
                ViewBag.errorMessage = e.Message;
            }
            return View();
        }


        [HttpPost]
        public ActionResult editcourses(course cus)
        {


            /// string connectionString = "server=localhost;user id=root;database=mydb;persistsecurityinfo=True";

            string conString = ConfigurationManager.ConnectionStrings["sbsclatest"].ConnectionString.ToString();
            MySqlConnection con = new MySqlConnection(conString);
            /// con. connectionString = "server=localhost;user id=root;database=mydb;persistsecurityinfo=True";
            /// 
            /// open the connection
            con.Open();
            ViewBag.successMessage = "connection was established";
            string theSql = "UPDATE course SET coursename=@coursename,corsecode=@corsecode,courseAuthor=@courseAuthor,description=@description,duration=@duration,isActive=@isActive WHERE idcourse=@idcourse";
            MySqlCommand command = new MySqlCommand(theSql, con);

            command.Parameters.AddWithValue(("@idcourse"), cus.courseid);
            command.Parameters.AddWithValue(("@coursename"), cus.coursename);
            command.Parameters.AddWithValue(("@coursecode"), cus.coursecode);
            command.Parameters.AddWithValue(("@courseAuthor"), cus.authorid);
            command.Parameters.AddWithValue(("@description"), cus.description);
            
            command.Parameters.AddWithValue(("@duration"), cus.duration);
            command.Parameters.AddWithValue(("@isActive"), cus.isActive);



            command.ExecuteNonQuery();
            ViewBag.success = "Registration Successful";
            con.Close();





            return RedirectToAction("courselist");

        }
        public ActionResult deletecourse(int id)
        {
            /// string connectionString = "server=localhost;user id=root;database=mydb;persistsecurityinfo=True";

            string conString = ConfigurationManager.ConnectionStrings["sbsclatest"].ConnectionString.ToString();
            MySqlConnection con = new MySqlConnection(conString);
            /// con. connectionString = "server=localhost;user id=root;database=mydb;persistsecurityinfo=True";
            /// 
            /// open the connection
            con.Open();
            ViewBag.successMessage = "connection was established";
            string theSql = "Delete FROM course Where idcourse=@idcourse";
            MySqlCommand command = new MySqlCommand(theSql, con);
            command.Parameters.AddWithValue("@idcourse", id);
            command.ExecuteNonQuery();
            return RedirectToAction("courselist");
        }


        public ActionResult Listofusers(User theuser)
        {
            
        
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

                string theSql = "SELECT iduser,firstname,lastname,username,role  FROM user Where role=HR_MANAGEMENT";
                // MySqlCommand command = new MySqlCommand(theSql, con);

                //creat reader
                MySqlDataAdapter sqda = new MySqlDataAdapter(theSql, con);
                sqda.Fill(dtblproduct);
                if (dtblproduct.Rows.Count != 0)
                { return View(dtblproduct); }

                else
                {
                    return RedirectToAction("courselist");
                }
            }

            catch (Exception e)
            {
                ViewBag.errorMessage = e.Message;
                return RedirectToAction("courselist");
            }



            return View(dtblproduct);
        
    }

        [HttpGet]
        public ActionResult courseauthors(int id)
        {
            courseXuser cua = new courseXuser();

            try {

                string conString = ConfigurationManager.ConnectionStrings["sbsclatest"].ConnectionString.ToString();
                MySqlConnection con = new MySqlConnection(conString);
                /// con. connectionString = "server=localhost;user id=root;database=mydb;persistsecurityinfo=True";
                /// 
                /// open the connection
                //con.Open();
                ViewBag.successMessage = "connection was established";
                string theSql = "SELECT iduser,username FROM sbsclatest.user";
                MySqlCommand command = new MySqlCommand(theSql, con);
                con.Open();

                MySqlDataAdapter sda = new MySqlDataAdapter(command);
                DataSet ds = new DataSet();
                sda.Fill(ds);

               
                ViewBag.usernames = ds.Tables[0];
                List<SelectListItem> getusername = new List<SelectListItem>();

                foreach (System.Data.DataRow dr in ViewBag.usernames.Rows)
                {

                    getusername.Add(new SelectListItem { Text = @dr["username"].ToString(), Value = @dr["username"].ToString() });
                }

                ViewBag.usernames = getusername;

                cua.courseid = id;
               

                con.Close();

                return View(cua);




            }
            catch (Exception e)
            {

            }

            return View(cua);
        }

        [HttpPost]
        public ActionResult courseauthors(courseXuser cua)
        {
            ///store i in the db
            /// establish the connection
            try
            {
                /// string connectionString = "server=localhost;user id=root;database=mydb;persistsecurityinfo=True";

                string conString = ConfigurationManager.ConnectionStrings["sbsclatest"].ConnectionString.ToString();
                MySqlConnection con = new MySqlConnection(conString);
                /// con. connectionString = "server=localhost;user id=root;database=mydb;persistsecurityinfo=True";
                /// 
                /// open the connection
                con.Open();
                ViewBag.successMessage = "connection was established";
                string theSql = "insert courseschedule (course_idcourse,user_iduser) values(@course_idcourse,@user_iduser) where username=@username ";
                MySqlCommand command = new MySqlCommand(theSql, con);

                command.Parameters.AddWithValue(("@username"), cua.username);
                command.Parameters.AddWithValue(("@course_idcourse"), cua.courseid);
                command.Parameters.AddWithValue(("@user_iduser"), cua.userid);




                //command.Parameters.Add("@coursename", MySqlDbType.String);
                //command.Parameters["@coursename"].Value = theUser.coursename;

                //command.Parameters.Add("@corsecode", MySqlDbType.String);
                //command.Parameters["@corsecode"].Value = theUser.coursecode;

                //command.Parameters.Add("@courseAuthor", MySqlDbType.String);
                //command.Parameters["@courseAuthor"].Value = theUser.author;

                //command.Parameters.Add("@description", MySqlDbType.String);
                //command.Parameters["@description"].Value = theUser.description;

                //command.Parameters.Add("@datecreated", MySqlDbType.DateTime);
                //command.Parameters["@datecreated"].Value = theUser.date;

                //command.Parameters.Add("@duration", MySqlDbType.String);
                //command.Parameters["@duration"].Value = theUser.duration;

                command.ExecuteNonQuery();
                ViewBag.success = "Registration Successful";
                con.Close();


            }

            catch (Exception e)
            {
                ViewBag.errorMessage = e.Message;
                return View();
            }






            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Addmodule(int id)
        {
            Module cus = new Module();
            cus.courseid = id;
            return View(cus);        
       
        }

        [HttpPost]
        public ActionResult Addmodule(Module cus)
        {
                        ///store i in the db
            /// establish the connection
            try
            {
                /// string connectionString = "server=localhost;user id=root;database=mydb;persistsecurityinfo=True";

                string conString = ConfigurationManager.ConnectionStrings["sbsclatest"].ConnectionString.ToString();
                MySqlConnection con = new MySqlConnection(conString);
                /// con. connectionString = "server=localhost;user id=root;database=mydb;persistsecurityinfo=True";
                /// 
                /// open the connection
                con.Open();
                ViewBag.successMessage = "connection was established";
                string theSql = "insert module(mname,mdescription,idcourse)values(@mname,@mdescription,@idcourse)";
                MySqlCommand command = new MySqlCommand(theSql, con);



                command.Parameters.Add("@mname", MySqlDbType.String);
                command.Parameters["@mname"].Value = cus.modulename;

                command.Parameters.Add("@mname", MySqlDbType.String);
                command.Parameters["@mname"].Value = cus.modulename;

                command.Parameters.Add("@mdescription", MySqlDbType.String);
                command.Parameters["@mdescription"].Value = cus.moduledesc;

                command.ExecuteNonQuery();
                ViewBag.success = "Registration Successful";
                con.Close();


            }

            catch (Exception e)
            {
                ViewBag.errorMessage = e.Message;
                return View();
            }
            return RedirectToAction("courselist");
        }

        public ActionResult ModuleList(int id)
        {
            
            DataTable dtblproduct = new DataTable();

            try
            {

                string conString = ConfigurationManager.ConnectionStrings["sbsclatest"].ConnectionString.ToString();
                MySqlConnection con = new MySqlConnection(conString);
               
                con.Open();
                ViewBag.successMessage = "connection was established";

                string theSql = "SELECT idmodule,mname,mdescription FROM user Where course_moduleid=@course_moduleid";

                MySqlDataAdapter sqda = new MySqlDataAdapter(theSql, con);
                sqda.SelectCommand.Parameters.AddWithValue("@course_moduleid", id);

                sqda.Fill(dtblproduct);
                if (dtblproduct.Rows.Count != 0)
                { return View(dtblproduct); }

                else
                {
                    return RedirectToAction("courselistHR");
                }
            }

            catch (Exception e)
            {
                ViewBag.errorMessage = e.Message;
                return RedirectToAction("courselistHR");
            }           

        }

        public ActionResult CreateExams(int id)
        {
            Exam theUser = new Exam();
                ///store i in the db
                /// establish the connection
                try
                {
                    /// string connectionString = "server=localhost;user id=root;database=mydb;persistsecurityinfo=True";

                    string conString = ConfigurationManager.ConnectionStrings["sbsclatest"].ConnectionString.ToString();
                    MySqlConnection con = new MySqlConnection(conString);
                    /// con. connectionString = "server=localhost;user id=root;database=mydb;persistsecurityinfo=True";
                    /// 
                    /// open the connection
                    con.Open();
                    ViewBag.successMessage = "connection was established";
                    string theSql = "insert exams(examdate,examdesc,courseid,isActive)values(@examdate,@examdesc,@courseid,@isActive)";
                    MySqlCommand command = new MySqlCommand(theSql, con);

                    command.Parameters.Add("@examdate", MySqlDbType.String);
                    command.Parameters["@examdate"].Value = theUser.date;

                    command.Parameters.Add("@examdesc", MySqlDbType.String);
                    command.Parameters["@examdesc"].Value = theUser.examdesc;

                    command.Parameters.Add("@courseid", MySqlDbType.String);
                    command.Parameters["@courseid"].Value =id;

                    command.Parameters.Add("@isActive", MySqlDbType.String);
                    command.Parameters["@isActive"].Value = theUser.isActive;                  

                    command.ExecuteNonQuery();
                    ViewBag.success = "Registration Successful";
                    con.Close();
                }

                catch (Exception e)
                {
                    ViewBag.errorMessage = e.Message;
                    return View("courseListHR");
                }
            return View("ExamList");

        }

        public ActionResult AddQuestions(int id)
        {
            Exam theUser = new Exam();
            ///store i in the db
            /// establish the connection
            try
            {
                /// string connectionString = "server=localhost;user id=root;database=mydb;persistsecurityinfo=True";

                string conString = ConfigurationManager.ConnectionStrings["sbsclatest"].ConnectionString.ToString();
                MySqlConnection con = new MySqlConnection(conString);
                /// con. connectionString = "server=localhost;user id=root;database=mydb;persistsecurityinfo=True";
                /// 
                /// open the connection
                con.Open();
                ViewBag.successMessage = "connection was established";
                string theSql = "insert exams(examdate,examdesc,courseid,isActive)values(@examdate,@examdesc,@courseid,@isActive)";
                MySqlCommand command = new MySqlCommand(theSql, con);

                command.Parameters.Add("@examdate", MySqlDbType.String);
                command.Parameters["@examdate"].Value = theUser.date;

                command.Parameters.Add("@examdesc", MySqlDbType.String);
                command.Parameters["@examdesc"].Value = theUser.examdesc;

                command.Parameters.Add("@courseid", MySqlDbType.String);
                command.Parameters["@courseid"].Value = id;

                command.Parameters.Add("@isActive", MySqlDbType.String);
                command.Parameters["@isActive"].Value = theUser.isActive;

                command.ExecuteNonQuery();
                ViewBag.success = "Registration Successful";
                con.Close();
            }

            catch (Exception e)
            {
                ViewBag.errorMessage = e.Message;
                return View("courseListHR");
            }
            return View("ExamList");

        }

        public ActionResult AddOptions(int id)
        {
            options theUser = new options();
            ///store i in the db
            /// establish the connection
            try
            {
                /// string connectionString = "server=localhost;user id=root;database=mydb;persistsecurityinfo=True";

                string conString = ConfigurationManager.ConnectionStrings["sbsclatest"].ConnectionString.ToString();
                MySqlConnection con = new MySqlConnection(conString);
                /// con. connectionString = "server=localhost;user id=root;database=mydb;persistsecurityinfo=True";
                /// 
                /// open the connection
                con.Open();
                ViewBag.successMessage = "connection was established";
                string theSql = "insert options(questionid,optA,optB,optC,optD,isAnswer)values(@questionid,@optA,@optB,@optC,@optD,@isanswer)";
                MySqlCommand command = new MySqlCommand(theSql, con);


                command.Parameters.Add("@questionid", MySqlDbType.String);
                command.Parameters["@questionid"].Value = theUser.questionid;

                command.Parameters.Add("@optA", MySqlDbType.String);
                command.Parameters["@optA"].Value = theUser.optA;

                command.Parameters.Add("@optB", MySqlDbType.String);
                command.Parameters["@optB"].Value = theUser.optB;

                command.Parameters.Add("@optC", MySqlDbType.String);
                command.Parameters["@optC"].Value = theUser.optC;

                command.Parameters.Add("@optD", MySqlDbType.String);
                command.Parameters["@optD"].Value = theUser.optD;

                command.Parameters.Add("@isactive", MySqlDbType.String);
                command.Parameters["@isactive"].Value = theUser.isAnswer;

                command.ExecuteNonQuery();
                ViewBag.success = "Registration Successful";
                con.Close();
            }

            catch (Exception e)
            {
                ViewBag.errorMessage = e.Message;
                return View("courseListHR");
            }
            return View("ExamList");

        }

    }
}