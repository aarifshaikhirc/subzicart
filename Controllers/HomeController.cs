using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace subzicart.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return RedirectToAction("login", "Home");
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string userName, string Password)
        {
            SqlDataReader rd;
            rd = SqlHelper.ExecuteReader(Connection.ConnectionString, CommandType.StoredProcedure, "sp_login ",new SqlParameter("@username",userName),new SqlParameter("@pswd", Password));
            if (rd.Read())
            {
                Session["loginID"] = rd["id"].ToString();
                Session["userName"] = rd["uname"].ToString();
                Response.Cookies["loginID"].Value = rd["id"].ToString();
                Response.Cookies["userName"].Value = rd["uname"].ToString();
                return RedirectToAction("Index", "Admin"); 

            }
            else
            {
                ViewBag.message = "Invalid UserId or Password!!!!!";
            }

            return View();
        }
    }
}