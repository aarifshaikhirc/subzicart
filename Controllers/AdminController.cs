using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;

namespace subzicart.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            if (Request.Cookies["loginID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        public ActionResult logout()
        {

            Session["loginID"] = null;
            Session["userName"] = null;

            Response.Cookies["userName"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["loginID"].Expires = DateTime.Now.AddDays(-1);
            return RedirectToAction("Login", "Home");
        }
        public ActionResult Insert_Category(string mid)
        {
            try
            {
                ViewBag.message = Session["Message"];
                Session.RemoveAll();
                Session.Clear();
            }

            catch (Exception ee) { }
            DataSet ds = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblCategory", new SqlParameter("@action", "3"), new SqlParameter("@remark", SqlDbType.Char));
            ViewBag.category = ds.Tables[0];

            if (!string.IsNullOrEmpty(mid))
            {
                ViewBag.update = "1";
                using (var db = new Models.subzicartEntities())
                {
                    //var response = db.Database.SqlQuery<Models.tblCategory>(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblCategory", new SqlParameter("@action", "5"), new SqlParameter("@catId", mid), new SqlParameter("@remark", SqlDbType.Char)).FirstOrDefault();
                    DataSet ds1 = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblCategory", new SqlParameter("@action", "5"), new SqlParameter("@catId", Convert.ToInt32(mid)), new SqlParameter("@remark", SqlDbType.Char));
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        Models.tblCategory md = new Models.tblCategory();
                        md.catId = Convert.ToInt32(ds1.Tables[0].Rows[0]["catId"]);
                        md.catName = ds1.Tables[0].Rows[0]["catName"].ToString();
                        md.description = ds1.Tables[0].Rows[0]["description"].ToString();
                        md.isActive = Convert.ToInt32(ds1.Tables[0].Rows[0]["isActive"]);
                        ViewBag.isActive = Convert.ToInt32(ds1.Tables[0].Rows[0]["isActive"]);

                        ViewBag.data = md;
                        return View(md);
                    }
                    else
                    {
                        ViewBag.message = "Record has an error";
                        //return RedirectToAction("Insert_Category", "Admin");
                    }
                }
            }
            else
            {
                ViewBag.update = "0";
            }

         
            return View();
        }
        [HttpPost]
        public ActionResult Insert_Category(Models.tblCategory modeldata, string command)
        {
            if (!string.IsNullOrEmpty(command) && command == "Save")
            {
            
                if (ModelState.ContainsKey("catid"))
                    ModelState["catid"].Errors.Clear();
                if (ModelState.IsValid)
                {

                    SqlParameter[] sp = new SqlParameter[6];
                    sp[0] = new SqlParameter("@catName", modeldata.catName);
                    sp[1] = new SqlParameter("@description", modeldata.description);
                    sp[2] = new SqlParameter("@isactive", modeldata.isActive);
                    sp[3] = new SqlParameter("@userid", Session["loginID"]);
                    sp[4] = new SqlParameter("@action", "1");
                    sp[5] = new SqlParameter("@remark", SqlDbType.NVarChar, 500);
                    sp[5].Direction = ParameterDirection.Output;
                    int k = SqlHelper.ExecuteNonQuery(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblCategory", sp);
                    if (k > 0)
                    {

                        ViewBag.message = sp[5].Value;
                        ModelState.Clear();
                        ViewBag.update = "0";
                        //return RedirectToAction("Insert_Category", "Admin");
                    }
                    else
                    {
                        ViewBag.message = sp[5].Value;
                        ModelState.Clear();
                        //return RedirectToAction("Insert_Category", "Admin");
                        ViewBag.update = "0";
                    }
                }
                else
                {

                   
                   // ModelState.Clear();
                    //return RedirectToAction("Insert_Category", "Admin");
                    ViewBag.update = "0";
                }
            }
            else if (!string.IsNullOrEmpty(command) && command == "Update")
            {
                if (ModelState.IsValid)
                {

                    SqlParameter[] sp = new SqlParameter[7];
                    sp[0] = new SqlParameter("@catName", modeldata.catName);
                    sp[1] = new SqlParameter("@description", modeldata.description);
                    sp[2] = new SqlParameter("@isactive", modeldata.isActive);
                    sp[3] = new SqlParameter("@userid", Session["loginID"]);
                    sp[4] = new SqlParameter("@action", "2");
                    sp[5] = new SqlParameter("@catid", modeldata.catId);
                    sp[6] = new SqlParameter("@remark", SqlDbType.NVarChar, 500);
                    sp[6].Direction = ParameterDirection.Output;
                    int k = SqlHelper.ExecuteNonQuery(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblCategory", sp);
                    if (k > 0)
                    {

                        ViewBag.message = sp[6].Value;
                        ModelState.Clear();
                        //return RedirectToAction("Insert_Category", "Admin");
                        ViewBag.update = "0";
                    }
                    else
                    {
                        ViewBag.message = sp[6].Value;
                        ModelState.Clear();
                        //return RedirectToAction("Insert_Category", "Admin");
                        ViewBag.update = "0";
                    }
                }
                else
                {

                  
                    //ModelState.Clear();
                    //return RedirectToAction("Insert_Category", "Admin");
                    ViewBag.update = "0";
                }





            }
            if (!string.IsNullOrEmpty(command) && command == "Cancel")
            {
                return RedirectToAction("Insert_Category", "Admin");
            }
            DataSet ds = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblCategory", new SqlParameter("@action", "3"), new SqlParameter("@remark", SqlDbType.Char));
            ViewBag.category = ds.Tables[0];
            return View();
        }

      
        public ActionResult DeleteCategory(int mid)
        {
            if (ModelState.IsValid)
            {

                SqlParameter[] sp = new SqlParameter[3];
             
                sp[0] = new SqlParameter("@action", "6");
                sp[1] = new SqlParameter("@catid", mid);
                sp[2] = new SqlParameter("@remark", SqlDbType.NVarChar, 500);
                sp[2].Direction = ParameterDirection.Output;
                int k = SqlHelper.ExecuteNonQuery(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblCategory", sp);
                if (k > 0)
                {

                    //ViewBag.message = sp[2];
                    Session["Message"] = sp[2].Value;
                    return RedirectToAction("Insert_Category", "Admin");
                }
                else
                {
                    Session["Message"] = sp[2].Value;
                 
                    return RedirectToAction("Insert_Category", "Admin");
                }






            }
            return View();

        }

        //------------------------------------SubCategory Operations-----------------------------

        public ActionResult Insert_SubCategory(string mid)
        {
            try
            {
                ViewBag.message = Session["Message1"];
                Session.RemoveAll();
                Session.Clear();
            }

            catch (Exception ee) { }
            DataSet ds = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblsubCategory", new SqlParameter("@action", "3"), new SqlParameter("@remark", SqlDbType.Char));
            ViewBag.category = ds.Tables[0];

            DataSet ds3 = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblCategory", new SqlParameter("@action", "3"), new SqlParameter("@remark", SqlDbType.Char));
            ViewBag.category1 = ds3.Tables[0];

            if (!string.IsNullOrEmpty(mid))
            {
                ViewBag.update = "1";
                using (var db = new Models.subzicartEntities())
                {
                    //var response = db.Database.SqlQuery<Models.tblCategory>(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblCategory", new SqlParameter("@action", "5"), new SqlParameter("@catId", mid), new SqlParameter("@remark", SqlDbType.Char)).FirstOrDefault();
                   
                    DataSet ds1 = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblsubCategory", new SqlParameter("@action", "5"), new SqlParameter("@subcatId", mid), new SqlParameter("@remark", SqlDbType.Char));

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Models.tblSubCategory md = new Models.tblSubCategory();
                        md.catId = Convert.ToInt32(ds1.Tables[0].Rows[0]["catId"]);
                        md.subCatId = Convert.ToInt32(ds1.Tables[0].Rows[0]["subCatId"]);
                        md.subCatName = ds1.Tables[0].Rows[0]["subCatName"].ToString();

                        md.description = ds1.Tables[0].Rows[0]["description"].ToString();
                        md.isActive = Convert.ToInt32(ds1.Tables[0].Rows[0]["isActive"]);
                        ViewBag.isActive = Convert.ToInt32(ds1.Tables[0].Rows[0]["isActive"]);
                        ViewBag.categoryid = Convert.ToInt32(ds1.Tables[0].Rows[0]["catId"]);


                        ViewBag.data = md;

                        return View(md);
                    }
                    else
                    {
                        ViewBag.message = "Record has an error";
                        //return RedirectToAction("Insert_Category", "Admin");
                    }
                }
            }
            else
            {
                ViewBag.update = "0";
            }

            return View();
        }
        [HttpPost]
        public ActionResult Insert_SubCategory(Models.tblSubCategory modeldata, string command)
        {
            if (!string.IsNullOrEmpty(command) && command == "Save")
            {
                if (ModelState.ContainsKey("subcatid"))
                    ModelState["subcatid"].Errors.Clear();
                if (ModelState.IsValid)
                {
                    SqlParameter[] sp = new SqlParameter[7];
                    sp[0] = new SqlParameter("@subCatName", modeldata.subCatName);
                    sp[1] = new SqlParameter("@catid", modeldata.catId);
                    sp[2] = new SqlParameter("@description", modeldata.description);
                    sp[3] = new SqlParameter("@isactive", modeldata.isActive);
                    sp[4] = new SqlParameter("@userid", Session["loginID"]);
                    sp[5] = new SqlParameter("@action", "1");
                    sp[6] = new SqlParameter("@remark", SqlDbType.NVarChar, 500);
                    sp[6].Direction = ParameterDirection.Output;
                    int k = SqlHelper.ExecuteNonQuery(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblSubCategory", sp);
                    if (k > 0)
                    {

                        ViewBag.message = sp[6].Value;
                        ModelState.Clear();
                        //return RedirectToAction("Insert_Category", "Admin");
                        ViewBag.update = "0";
                    }
                    else
                    {
                        ViewBag.message = sp[6].Value;
                        ModelState.Clear();
                        //return RedirectToAction("Insert_Category", "Admin");
                        ViewBag.update = "0";
                    }
                }
                else
                {
                   // ModelState.Clear();
                    //return RedirectToAction("Insert_Category", "Admin");
                    ViewBag.update = "0";
                }

            }
            else if (!string.IsNullOrEmpty(command) && command == "Update")
            {
                if (ModelState.IsValid)
                {
                    SqlParameter[] sp = new SqlParameter[8];
                    sp[0] = new SqlParameter("@subCatId", modeldata.subCatId);
                    sp[1] = new SqlParameter("@subCatName", modeldata.subCatName);
                    sp[2] = new SqlParameter("@catid", modeldata.catId);
                    sp[3] = new SqlParameter("@description", modeldata.description);
                    sp[4] = new SqlParameter("@isactive", modeldata.isActive);
                    sp[5] = new SqlParameter("@userid", Session["loginID"]);
                    sp[6] = new SqlParameter("@action", "2");
                    sp[7] = new SqlParameter("@remark", SqlDbType.NVarChar, 500);
                    sp[7].Direction = ParameterDirection.Output;
                    int k = SqlHelper.ExecuteNonQuery(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblSubCategory", sp);
                    if (k > 0)
                    {

                        ViewBag.message = sp[7].Value;
                        ModelState.Clear();
                        //return RedirectToAction("Insert_Category", "Admin");
                        ViewBag.update = "0";
                    }
                    else
                    {
                        ViewBag.message = sp[7].Value;
                        ModelState.Clear();
                        //return RedirectToAction("Insert_Category", "Admin");
                        ViewBag.update = "0";
                    }
                }
                else
                {
                    //ModelState.Clear();
                    //return RedirectToAction("Insert_Category", "Admin");
                    ViewBag.update = "0";
                }
            }
            if (!string.IsNullOrEmpty(command) && command == "Cancel")
            {

                return RedirectToAction("Insert_subCategory", "Admin");
            }


            DataSet ds1 = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblsubCategory", new SqlParameter("@action", "3"), new SqlParameter("@remark", SqlDbType.Char));
            ViewBag.category = ds1.Tables[0];
            DataSet ds3 = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblCategory", new SqlParameter("@action", "3"), new SqlParameter("@remark", SqlDbType.Char));
            ViewBag.category1 = ds3.Tables[0];




            return View();
        }

       

        public ActionResult DeleteSubCategory(int mid)
        {
            
            if (ModelState.IsValid)
            {

                SqlParameter[] sp = new SqlParameter[3];

                sp[0] = new SqlParameter("@action", "6");
                sp[1] = new SqlParameter("@subcatid", mid);
                sp[2] = new SqlParameter("@remark", SqlDbType.NVarChar, 500);
                sp[2].Direction = ParameterDirection.Output;
                int k = SqlHelper.ExecuteNonQuery(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblSubCategory", sp);
                if (k > 0)
                {

                    //ViewBag.message = sp[2];
                    Session["Message1"] = sp[2].Value;
                    return RedirectToAction("Insert_SubCategory", "Admin");
                }
                else
                {
                    Session["Message1"] = sp[2].Value;

                    return RedirectToAction("Insert_SubCategory", "Admin");
                }






            }
            return View();

        }

        //---------------------------Currency Master-------------------------


        public ActionResult Insert_Currency(string mid)
        {
            try
            {
                ViewBag.message = Session["Message1"];
                Session.RemoveAll();
                Session.Clear();
            }

            catch (Exception ee) { }

            DataSet ds = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblCurrencymaster", new SqlParameter("@action", "3"), new SqlParameter("@remark", SqlDbType.Char));
            ViewBag.category = ds.Tables[0];

            if (!string.IsNullOrEmpty(mid))
            {
                ViewBag.update = "1";
                using (var db = new Models.subzicartEntities())
                {
                    //var response = db.Database.SqlQuery<Models.tblCategory>(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblCategory", new SqlParameter("@action", "5"), new SqlParameter("@catId", mid), new SqlParameter("@remark", SqlDbType.Char)).FirstOrDefault();

                    DataSet ds1 = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblCurrencymaster", new SqlParameter("@action", "5"), new SqlParameter("@cId", mid), new SqlParameter("@remark", SqlDbType.Char));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Models.tblCurrencyMaster md = new Models.tblCurrencyMaster();
                        md.cid = Convert.ToInt32(ds1.Tables[0].Rows[0]["cid"]);
                        md.currency = ds1.Tables[0].Rows[0]["currency"].ToString();
                        md.value = Convert.ToDouble(ds1.Tables[0].Rows[0]["value"].ToString());
                        md.rateInUSD = Convert.ToDouble(ds1.Tables[0].Rows[0]["rateInUSD"].ToString());
                        md.active = Convert.ToInt32(ds1.Tables[0].Rows[0]["active"]);
                        ViewBag.isActive = Convert.ToInt32(ds1.Tables[0].Rows[0]["active"]);

                        ViewBag.data = md;
                        return View(md);
                    }
                    else
                    {
                        ViewBag.message = "Record has an error";
                        //return RedirectToAction("Insert_Category", "Admin");
                    }
                }
            }
            else
            {
                ViewBag.update = "0";
            }



            return View();
        }
        [HttpPost]
        public ActionResult Insert_Currency(Models.tblCurrencyMaster modeldata, string command)
        {
            if (!string.IsNullOrEmpty(command) && command == "Save")
            {
                if (ModelState.ContainsKey("cid"))
                    ModelState["cid"].Errors.Clear();
                if (ModelState.IsValid)
                {

                    SqlParameter[] sp = new SqlParameter[7];
                    sp[0] = new SqlParameter("@currency", modeldata.currency);
                    sp[1] = new SqlParameter("@value", modeldata.value);
                    sp[2] = new SqlParameter("@rateinUsd", modeldata.rateInUSD);
                    sp[3] = new SqlParameter("@active", modeldata.active);
                    sp[4] = new SqlParameter("@userid", Session["loginID"]);
                    sp[5] = new SqlParameter("@action", "1");
                    sp[6] = new SqlParameter("@remark", SqlDbType.NVarChar, 500);
                    sp[6].Direction = ParameterDirection.Output;
                    int k = SqlHelper.ExecuteNonQuery(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblCurrencymaster", sp);
                    if (k > 0)
                    {

                        ViewBag.message = sp[6].Value;
                        ModelState.Clear();
                        //return RedirectToAction("Insert_Category", "Admin");
                        ViewBag.update = "0";
                    }
                    else
                    {
                        ViewBag.message = sp[6].Value;
                        ModelState.Clear();
                        //return RedirectToAction("Insert_Category", "Admin");
                        ViewBag.update = "0";
                    }
                }
                else
                {
                  
                   // ModelState.Clear();
                    //return RedirectToAction("Insert_Category", "Admin");
                    ViewBag.update = "0";
                }
            }
            else if (!string.IsNullOrEmpty(command) && command == "Update")
            {
                if (ModelState.IsValid)
                {
                    SqlParameter[] sp = new SqlParameter[8];
                    sp[0] = new SqlParameter("@cid", modeldata.cid);
                    sp[1] = new SqlParameter("@currency", modeldata.currency);
                    sp[2] = new SqlParameter("@value", modeldata.value);
                    sp[3] = new SqlParameter("@rateinUsd", modeldata.rateInUSD);
                    sp[4] = new SqlParameter("@active", modeldata.active);
                    sp[5] = new SqlParameter("@userid", Session["loginID"]);
                    sp[6] = new SqlParameter("@action", "2");
                    sp[7] = new SqlParameter("@remark", SqlDbType.NVarChar, 500);
                    sp[7].Direction = ParameterDirection.Output;
                    int k = SqlHelper.ExecuteNonQuery(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblCurrencymaster", sp);
                    if (k > 0)
                    {

                        ViewBag.message = sp[7].Value;
                        ModelState.Clear();
                        //return RedirectToAction("Insert_Category", "Admin");
                        ViewBag.update = "0";
                    }
                    else
                    {
                        ViewBag.message = sp[7].Value;
                        ModelState.Clear();
                        //return RedirectToAction("Insert_Category", "Admin");
                        ViewBag.update = "0";
                    }
                }
                else
                {
                   
                   // ModelState.Clear();
                    //return RedirectToAction("Insert_Category", "Admin");
                    ViewBag.update = "0";
                }

            }
            else if (!string.IsNullOrEmpty(command) && command == "Update")
            {
                return RedirectToAction("Insert_Currency", "Admin");
            }

            DataSet ds = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblCurrencymaster", new SqlParameter("@action", "3"), new SqlParameter("@remark", SqlDbType.Char));
            ViewBag.category = ds.Tables[0];
            return View();
        }


        public ActionResult DeleteCurrency(int mid)
        {
            if (ModelState.IsValid)
            {

                SqlParameter[] sp = new SqlParameter[3];

                sp[0] = new SqlParameter("@action", "6");
                sp[1] = new SqlParameter("@cid", mid);
                sp[2] = new SqlParameter("@remark", SqlDbType.NVarChar, 500);
                sp[2].Direction = ParameterDirection.Output;
                int k = SqlHelper.ExecuteNonQuery(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblCurrencymaster", sp);
                if (k > 0)
                {

                    //ViewBag.message = sp[2];
                    Session["Message1"] = sp[2].Value;
                    return RedirectToAction("Insert_Currency", "Admin");
                }
                else
                {
                    Session["Message1"] = sp[2].Value;

                    return RedirectToAction("Insert_Currency", "Admin");
                }






            }
            return View();

        }





        //---------------------------Mesaurement Master-------------------------


        public ActionResult Insert_mesaurementmaster(string mid)
        {
            try
            {
                ViewBag.message = Session["Message1"];
                Session.RemoveAll();
                Session.Clear();
            }

            catch (Exception ee) { }

            DataSet ds = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblMeasurementMaster", new SqlParameter("@action", "3"), new SqlParameter("@remark", SqlDbType.Char));
            ViewBag.category = ds.Tables[0];
            if (!string.IsNullOrEmpty(mid))
            {
                ViewBag.update = "1";
                using (var db = new Models.subzicartEntities())
                {
                    //var response = db.Database.SqlQuery<Models.tblCategory>(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblCategory", new SqlParameter("@action", "5"), new SqlParameter("@catId", mid), new SqlParameter("@remark", SqlDbType.Char)).FirstOrDefault();

                    DataSet ds1 = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblMeasurementMaster", new SqlParameter("@action", "5"), new SqlParameter("@m_id", mid), new SqlParameter("@remark", SqlDbType.Char));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Models.tblMeasurementMaster md = new Models.tblMeasurementMaster();
                        md.m_id = Convert.ToInt32(ds1.Tables[0].Rows[0]["m_id"]);
                        md.unit_type = ds1.Tables[0].Rows[0]["unit_type"].ToString();
                        md.unit_value = Convert.ToDouble(ds1.Tables[0].Rows[0]["unit_value"].ToString());
                        md.eq_unit_type = ds1.Tables[0].Rows[0]["eq_unit_type"].ToString();
                        md.eq_unit_value = Convert.ToDouble(ds1.Tables[0].Rows[0]["eq_unit_value"].ToString());
                        md.active = Convert.ToInt32(ds1.Tables[0].Rows[0]["active"]);
                        ViewBag.isActive = Convert.ToInt32(ds1.Tables[0].Rows[0]["active"]);

                        ViewBag.data = md;
                        return View(md);
                    }
                    else
                    {
                        ViewBag.message = "Record has an error";
                        //return RedirectToAction("Insert_Category", "Admin");
                    }
                }
            }
            else
            {
                ViewBag.update = "0";
            }




            return View();
        }
        [HttpPost]
        public ActionResult Insert_mesaurementmaster(Models.tblMeasurementMaster modeldata, string command)
        {

            if (!string.IsNullOrEmpty(command) && command == "Save")
            {
                if (ModelState.ContainsKey("m_id"))
                    ModelState["m_id"].Errors.Clear();
                if (ModelState.IsValid)
                {

                    SqlParameter[] sp = new SqlParameter[8];
                    sp[0] = new SqlParameter("@unit_type", modeldata.unit_type);
                    sp[1] = new SqlParameter("@unit_value", modeldata.unit_value);
                    sp[2] = new SqlParameter("@eq_unit_type", modeldata.eq_unit_type);
                    sp[3] = new SqlParameter("@eq_unit_value", modeldata.eq_unit_value);
                    sp[4] = new SqlParameter("@active", modeldata.active);
                    sp[5] = new SqlParameter("@userid", Session["loginID"]);
                    sp[6] = new SqlParameter("@action", "1");
                    sp[7] = new SqlParameter("@remark", SqlDbType.NVarChar, 500);
                    sp[7].Direction = ParameterDirection.Output;
                    int k = SqlHelper.ExecuteNonQuery(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblMeasurementMaster", sp);
                    if (k > 0)
                    {

                        ViewBag.message = sp[7].Value;
                        ModelState.Clear();
                        //return RedirectToAction("Insert_Category", "Admin");
                        ViewBag.update = "0";
                    }
                    else
                    {
                        ViewBag.message = sp[7].Value;
                        ModelState.Clear();
                        //return RedirectToAction("Insert_Category", "Admin");
                        ViewBag.update = "0";
                    }
                }
                else
                {

                    // ModelState.Clear();
                    //return RedirectToAction("Insert_Category", "Admin");
                    ViewBag.update = "0";
                }
            }
            else if (!string.IsNullOrEmpty(command) && command == "Update")
            {
                if (ModelState.IsValid)
                {
                    SqlParameter[] sp = new SqlParameter[9];
                    sp[0] = new SqlParameter("@unit_type", modeldata.unit_type);
                    sp[1] = new SqlParameter("@unit_value", modeldata.unit_value);
                    sp[2] = new SqlParameter("@eq_unit_type", modeldata.eq_unit_type);
                    sp[3] = new SqlParameter("@eq_unit_value", modeldata.eq_unit_value);

                    sp[4] = new SqlParameter("@m_id", modeldata.m_id);

                    sp[5] = new SqlParameter("@active", modeldata.active);
                    sp[6] = new SqlParameter("@userid", Session["loginID"]);
                    sp[7] = new SqlParameter("@action", "2");
                    sp[8] = new SqlParameter("@remark", SqlDbType.NVarChar, 500);
                    sp[8].Direction = ParameterDirection.Output;
                    int k = SqlHelper.ExecuteNonQuery(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblMeasurementMaster", sp);
                    if (k > 0)
                    {

                        ViewBag.message = sp[8].Value;
                        ModelState.Clear();
                        //return RedirectToAction("Insert_Category", "Admin");
                        ViewBag.update = "0";
                    }
                    else
                    {
                        ViewBag.message = sp[8].Value;
                        ModelState.Clear();
                        //return RedirectToAction("Insert_Category", "Admin");
                        ViewBag.update = "0";
                    }
                }
                else
                {

                    // ModelState.Clear();
                    //return RedirectToAction("Insert_Category", "Admin");
                    ViewBag.update = "0";
                }

            }
            else if (!string.IsNullOrEmpty(command) && command == "Update")
            {
                return RedirectToAction("Insert_mesaurementmaster", "Admin");
            }

            DataSet ds = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblMeasurementMaster", new SqlParameter("@action", "3"), new SqlParameter("@remark", SqlDbType.Char));
            ViewBag.category = ds.Tables[0];




            return View();
        }

     
        public ActionResult Deletemesaurementmaster(int mid)
        {
            if (ModelState.IsValid)
            {

                SqlParameter[] sp = new SqlParameter[3];

                sp[0] = new SqlParameter("@action", "6");
                sp[1] = new SqlParameter("@m_id", mid);
                sp[2] = new SqlParameter("@remark", SqlDbType.VarChar, 200);
                sp[2].Direction = ParameterDirection.Output;
                int k = SqlHelper.ExecuteNonQuery(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblMeasurementMaster", sp);
                if (k > 0)
                {

                    //ViewBag.message = sp[2];
                    Session["Message1"] = sp[2].Value;
                    return RedirectToAction("Insert_mesaurementmaster", "Admin");
                }
                else
                {
                    Session["Message1"] = sp[2].Value;

                    return RedirectToAction("Insert_mesaurementmaster", "Admin");
                }






            }
            return View();

        }



        //---------------------------Brand Prefixes-------------------------


        public ActionResult Insert_brandprefix(string mid)
        {
            try
            {
                ViewBag.message = Session["Message1"];
                Session.RemoveAll();
                Session.Clear();
            }

            catch (Exception ee) { }

            DataSet ds = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblBrandPrefixes", new SqlParameter("@action", "3"), new SqlParameter("@remark", SqlDbType.Char));
            ViewBag.category = ds.Tables[0];
            if (!string.IsNullOrEmpty(mid))
            {
                ViewBag.update = "1";
                using (var db = new Models.subzicartEntities())
                {
                    //var response = db.Database.SqlQuery<Models.tblCategory>(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblCategory", new SqlParameter("@action", "5"), new SqlParameter("@catId", mid), new SqlParameter("@remark", SqlDbType.Char)).FirstOrDefault();

                    DataSet ds1 = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblBrandPrefixes", new SqlParameter("@action", "5"), new SqlParameter("@brand_id", mid), new SqlParameter("@remark", SqlDbType.Char));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Models.tblBrandPrefix md = new Models.tblBrandPrefix();
                        md.brand_id = Convert.ToInt32(ds1.Tables[0].Rows[0]["brand_id"]);
                        md.brand_name = ds1.Tables[0].Rows[0]["brand_name"].ToString();
                        md.brand_prefix = ds1.Tables[0].Rows[0]["brand_prefix"].ToString();
                        md.sku_start_range = ds1.Tables[0].Rows[0]["sku_start_range"].ToString();
                        md.sku_end_range = ds1.Tables[0].Rows[0]["sku_end_range"].ToString();
                        md.active = Convert.ToInt32(ds1.Tables[0].Rows[0]["active"]);
                        ViewBag.isActive = Convert.ToInt32(ds1.Tables[0].Rows[0]["active"]);

                        ViewBag.data = md;
                        return View(md);
                    }
                    else
                    {
                        ViewBag.message = "Record has an error";
                        //return RedirectToAction("Insert_Category", "Admin");
                    }
                }
            }
            else
            {
                ViewBag.update = "0";
            }




            return View();
        }
        [HttpPost]
        public ActionResult Insert_brandprefix(Models.tblBrandPrefix modeldata, string command)
        {
            if (!string.IsNullOrEmpty(command) && command == "Save")
            {
                if (ModelState.ContainsKey("brand_id"))
                    ModelState["brand_id"].Errors.Clear();
                if (ModelState.IsValid)
                {


                    SqlParameter[] sp = new SqlParameter[8];
                    sp[0] = new SqlParameter("@brand_name", modeldata.brand_name);
                    sp[1] = new SqlParameter("@brand_prefix", modeldata.brand_prefix);
                    sp[2] = new SqlParameter("@sku_start_range", modeldata.sku_start_range);
                    sp[3] = new SqlParameter("@sku_end_range", modeldata.sku_end_range);
                    sp[4] = new SqlParameter("@active", modeldata.active);
                    sp[5] = new SqlParameter("@userid", Session["loginID"]);
                    sp[6] = new SqlParameter("@action", "1");
                    sp[7] = new SqlParameter("@remark", SqlDbType.NVarChar, 500);
                    sp[7].Direction = ParameterDirection.Output;
                    int k = SqlHelper.ExecuteNonQuery(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblBrandPrefixes", sp);
                    if (k > 0)
                    {

                        ViewBag.message = sp[7].Value;
                        ModelState.Clear();
                        //return RedirectToAction("Insert_Category", "Admin");
                        ViewBag.update = "0";
                    }
                    else
                    {
                        ViewBag.message = sp[7].Value;
                        ModelState.Clear();
                        //return RedirectToAction("Insert_Category", "Admin");
                        ViewBag.update = "0";
                    }
                }
                else
                {

                    // ModelState.Clear();
                    //return RedirectToAction("Insert_Category", "Admin");
                    ViewBag.update = "0";
                }
            }
            else if (!string.IsNullOrEmpty(command) && command == "Update")
            {
                if (ModelState.IsValid)
                {
                    SqlParameter[] sp = new SqlParameter[9];
                    sp[0] = new SqlParameter("@brand_name", modeldata.brand_name);
                    sp[1] = new SqlParameter("@brand_prefix", modeldata.brand_prefix);
                    sp[2] = new SqlParameter("@sku_start_range", modeldata.sku_start_range);
                    sp[3] = new SqlParameter("@sku_end_range", modeldata.sku_end_range);

                    sp[4] = new SqlParameter("@brand_id", modeldata.brand_id);

                    sp[5] = new SqlParameter("@active", modeldata.active);
                    sp[6] = new SqlParameter("@userid", Session["loginID"]);
                    sp[7] = new SqlParameter("@action", "2");
                    sp[8] = new SqlParameter("@remark", SqlDbType.NVarChar, 500);
                    sp[8].Direction = ParameterDirection.Output;
                    int k = SqlHelper.ExecuteNonQuery(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblBrandPrefixes", sp);
                    if (k > 0)
                    {

                        ViewBag.message = sp[8].Value;
                        ModelState.Clear();
                        //return RedirectToAction("Insert_Category", "Admin");
                        ViewBag.update = "0";
                    }
                    else
                    {
                        ViewBag.message = sp[8].Value;
                        ModelState.Clear();
                        //return RedirectToAction("Insert_Category", "Admin");
                        ViewBag.update = "0";
                    }
                }
                else
                {

                    // ModelState.Clear();
                    //return RedirectToAction("Insert_Category", "Admin");
                    ViewBag.update = "0";
                }

            }
            else if (!string.IsNullOrEmpty(command) && command == "Update")
            {
                return RedirectToAction("Insert_brandprefix", "Admin");
            }

            DataSet ds = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblBrandPrefixes", new SqlParameter("@action", "3"), new SqlParameter("@remark", SqlDbType.Char));
            ViewBag.category = ds.Tables[0];


            return View();
        }

        public ActionResult update_brandprefix(long mid)
        {
            using (var db = new Models.subzicartEntities())
            {
                //var response = db.Database.SqlQuery<Models.tblCategory>(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblCategory", new SqlParameter("@action", "5"), new SqlParameter("@catId", mid), new SqlParameter("@remark", SqlDbType.Char)).FirstOrDefault();
                DataSet ds = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblBrandPrefixes", new SqlParameter("@action", "5"), new SqlParameter("@brand_id", mid), new SqlParameter("@remark", SqlDbType.Char));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Models.tblBrandPrefix md = new Models.tblBrandPrefix();
                    md.brand_id = Convert.ToInt32(ds.Tables[0].Rows[0]["brand_id"]);
                    md.brand_name = ds.Tables[0].Rows[0]["brand_name"].ToString();
                    md.brand_prefix = ds.Tables[0].Rows[0]["brand_prefix"].ToString();
                    md.sku_start_range = ds.Tables[0].Rows[0]["sku_start_range"].ToString();
                    md.sku_end_range = ds.Tables[0].Rows[0]["sku_end_range"].ToString();
                    md.active = Convert.ToInt32(ds.Tables[0].Rows[0]["active"]);
                    ViewBag.isActive = Convert.ToInt32(ds.Tables[0].Rows[0]["active"]);

                    ViewBag.data = md;
                    return View(md);
                }
                else
                {
                    Session["Message3"] = "Record has an error";
                    return RedirectToAction("Insert_brandprefix", "Admin");
                }
            }

        }
        [HttpPost]
        public ActionResult update_brandprefix(Models.tblBrandPrefix modeldata)
        {

            if (ModelState.IsValid)
            {

                SqlParameter[] sp = new SqlParameter[9];
                sp[0] = new SqlParameter("@brand_name", modeldata.brand_name);
                sp[1] = new SqlParameter("@brand_prefix", modeldata.brand_prefix);
                sp[2] = new SqlParameter("@sku_start_range", modeldata.sku_start_range);
                sp[3] = new SqlParameter("@sku_end_range", modeldata.sku_end_range);

                sp[4] = new SqlParameter("@brand_id", modeldata.brand_id);

                sp[5] = new SqlParameter("@active", modeldata.active);
                sp[6] = new SqlParameter("@userid", Session["loginID"]);
                sp[7] = new SqlParameter("@action", "2");
                sp[8] = new SqlParameter("@remark", SqlDbType.NVarChar, 500);
                sp[8].Direction = ParameterDirection.Output;
                int k = SqlHelper.ExecuteNonQuery(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblBrandPrefixes", sp);
                if (k > 0)
                {

                    ViewBag.message = sp[8].Value;
                    return RedirectToAction("ViewAllbrandprefix", "Admin");
                }
                else
                {
                    ViewBag.message = sp[8].Value;
                }




            }
            return View();
        }

        public ActionResult ViewAllbrandprefix()
        {
            try
            {
                ViewBag.message = Session["Message3"];
            }
            catch (Exception ee) { }

            DataSet ds = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblBrandPrefixes", new SqlParameter("@action", "3"), new SqlParameter("@remark", SqlDbType.Char));
            ViewBag.category = ds.Tables[0];
            return View();

        }

        public ActionResult Deletebrandprefix(int mid)
        {
            if (ModelState.IsValid)
            {

                SqlParameter[] sp = new SqlParameter[3];

                sp[0] = new SqlParameter("@action", "6");
                sp[1] = new SqlParameter("@brand_id", mid);
                sp[2] = new SqlParameter("@remark", SqlDbType.VarChar, 200);
                sp[2].Direction = ParameterDirection.Output;
                int k = SqlHelper.ExecuteNonQuery(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblBrandPrefixes", sp);
                if (k > 0)
                {

                    //ViewBag.message = sp[2];
                    Session["Message1"] = sp[2].Value;
                    return RedirectToAction("Insert_brandprefix", "Admin");
                }
                else
                {
                    Session["Message1"] = sp[2].Value;

                    return RedirectToAction("Insert_brandprefix", "Admin");
                }






            }
            return View();

        }

        static string ss = "0";
        public ActionResult Insert_CategoryMaster(string mid)
        {
            try
            {
                ViewBag.message = Session["Message1"];
                Session.RemoveAll();
                Session.Clear();
            }

            catch (Exception ee) { }
            Models.MyViewModel catmaster = new Models.MyViewModel();

            catmaster.category = new List<Models.CategoryClass>();
            catmaster.category.Add(new Models.CategoryClass { cat_id = 1, cat_name = "Root", p_cat_id = 0 });
            //List <Models.CategoryClass> catmaster = new List<Models.CategoryClass>();
            //Models.CategoryClass catmaster = new Models.CategoryClass();
            DataSet ds = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblCategoryMaster", new SqlParameter("@action", "5"), new SqlParameter("@p_cat_id", "1"), new SqlParameter("@remark", SqlDbType.Char));
            //ViewBag.category = ds.Tables[0];
            if (ds.Tables[0].Rows.Count > 0)
            {
           
                foreach (DataRow rw in ds.Tables[0].Rows)
                {
                    int cat_id = Convert.ToInt32(rw["cat_id"]);
                    string cat_name ="-" + rw["cat_name"].ToString();
                    int p_cat_id = Convert.ToInt32(rw["p_cat_id"]);
                  

                    //catmaster.cat_id = cat_id;
                    //catmaster.cat_name = cat_name;
                    //catmaster.p_cat_id = p_cat_id;
                    catmaster.category.Add(new Models.CategoryClass { cat_id = cat_id, cat_name = cat_name, p_cat_id = p_cat_id });

                    DataSet ds1 = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblCategoryMaster", new SqlParameter("@action", "5"), new SqlParameter("@p_cat_id", cat_id), new SqlParameter("@remark", SqlDbType.Char));
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow rw1 in ds1.Tables[0].Rows)
                        {
                            int cat_id1 = Convert.ToInt32(rw1["cat_id"]);
                            string cat_name1 = "---" + rw1["cat_name"].ToString();
                            int p_cat_id1 = Convert.ToInt32(rw1["p_cat_id"]);
                            catmaster.category.Add(new Models.CategoryClass { cat_id = cat_id1, cat_name = cat_name1, p_cat_id = p_cat_id1 });
                            DataSet ds11 = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblCategoryMaster", new SqlParameter("@action", "5"), new SqlParameter("@p_cat_id", cat_id1), new SqlParameter("@remark", SqlDbType.Char));
                            if (ds11.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow rw11 in ds11.Tables[0].Rows)
                                {
                                    int cat_id12 = Convert.ToInt32(rw11["cat_id"]);
                                    string cat_name12 = "-----" + rw11["cat_name"].ToString();
                                    int p_cat_id12 = Convert.ToInt32(rw11["p_cat_id"]);
                                    catmaster.category.Add(new Models.CategoryClass { cat_id = cat_id12, cat_name = cat_name12, p_cat_id = p_cat_id12 });

                                    DataSet ds12 = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblCategoryMaster", new SqlParameter("@action", "5"), new SqlParameter("@p_cat_id", cat_id12), new SqlParameter("@remark", SqlDbType.Char));
                                    if (ds12.Tables[0].Rows.Count > 0)
                                    {
                                        foreach (DataRow rw12 in ds12.Tables[0].Rows)
                                        {
                                            int cat_id122 = Convert.ToInt32(rw12["cat_id"]);
                                            string cat_name122 = "-------" + rw12["cat_name"].ToString();
                                            int p_cat_id122 = Convert.ToInt32(rw12["p_cat_id"]);
                                            catmaster.category.Add(new Models.CategoryClass { cat_id = cat_id122, cat_name = cat_name122, p_cat_id = p_cat_id122 });

                                        }
                                    }
                                }
                            }
                        }
                    }
                    catmaster.category.Add(new Models.CategoryClass { cat_id = 0, cat_name = "</br>", p_cat_id = 0 });

                }




            }
            ViewBag.category1 = catmaster.category;





            Models.MyViewModel1 catmastergrid = new Models.MyViewModel1();

            catmastergrid.category = new List<Models.CategoryGrid>();
            //catmastergrid.category.Add(new Models.CategoryGrid { cat_id=0,cat_name="",sub_Cat1="",sub_Cat2=""});
            //List <Models.CategoryClass> catmaster = new List<Models.CategoryClass>();
            //Models.CategoryClass catmaster = new Models.CategoryClass();
            DataSet ds113 = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblCategoryMaster", new SqlParameter("@action", "5"), new SqlParameter("@p_cat_id", "1"), new SqlParameter("@remark", SqlDbType.Char));
            //ViewBag.category = ds.Tables[0];
            if (ds113.Tables[0].Rows.Count > 0)
            {
                int count_3 = 0;
                int count_a = 0;
                foreach (DataRow rw in ds113.Tables[0].Rows)
                {
                    count_3 = count_3 + 1;
                    int cat_id = Convert.ToInt32(rw["cat_id"]);
                    string cat_name = rw["cat_name"].ToString();
                    string description = rw["description"].ToString();
                    int p_cat_id = Convert.ToInt32(rw["p_cat_id"]);
                    string bb = "";
                    if (ss == "0")
                    {
                        ss = cat_name;
                        
                    }
                    if(ss==cat_name)
                    {
                        count_a = count_a + 1;
                    }
                    else
                    {
                        ss = cat_name;
                        count_a = 0;

                    }
                    //catmaster.cat_id = cat_id;
                    //catmaster.cat_name = cat_name;
                    //catmaster.p_cat_id = p_cat_id;
                    //catmaster.category.Add(new Models.CategoryClass { cat_id = cat_id, cat_name = cat_name, p_cat_id = p_cat_id });

                    DataSet ds1 = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblCategoryMaster", new SqlParameter("@action", "5"), new SqlParameter("@p_cat_id", cat_id), new SqlParameter("@remark", SqlDbType.Char));
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        int count_2 = 0;
                        foreach (DataRow rw1 in ds1.Tables[0].Rows)
                        {
                            count_2 = count_2 + 1;
                           
                            int cat_id1 = Convert.ToInt32(rw1["cat_id"]);
                            string cat_name1 =  rw1["cat_name"].ToString();
                            string description1 = rw1["description"].ToString();
                            int p_cat_id1 = Convert.ToInt32(rw1["p_cat_id"]);
                            //catmaster.category.Add(new Models.CategoryClass { cat_id = cat_id1, cat_name = cat_name1, p_cat_id = p_cat_id1 });
                            DataSet ds11 = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblCategoryMaster", new SqlParameter("@action", "5"), new SqlParameter("@p_cat_id", cat_id1), new SqlParameter("@remark", SqlDbType.Char));
                            if (ds11.Tables[0].Rows.Count > 0)
                            {
                                int count_1 = 0;
                                foreach (DataRow rw11 in ds11.Tables[0].Rows)
                                {
                                    count_1 = count_1 + 1;
                                    int cat_id12 = Convert.ToInt32(rw11["cat_id"]);
                                    string cat_name12 =  rw11["cat_name"].ToString();
                                    string description12 = rw11["description"].ToString();
                                    int p_cat_id12 = Convert.ToInt32(rw11["p_cat_id"]);
                                    
                                       // catmastergrid.category.Add(new Models.CategoryGrid { cat_id = Convert.ToInt32(rw["cat_id"]), cat_name = cat_name, sub_Cat1 = cat_name1, sub_Cat2 = cat_name12,description=description12 });


                                    DataSet ds12 = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblCategoryMaster", new SqlParameter("@action", "5"), new SqlParameter("@p_cat_id", cat_id12), new SqlParameter("@remark", SqlDbType.Char));
                                    if (ds12.Tables[0].Rows.Count > 0)
                                    {
                                        int count = 0;
                                        foreach (DataRow rw12 in ds12.Tables[0].Rows)
                                        {
                                            count = count + 1;
                                            int cat_id122 = Convert.ToInt32(rw12["cat_id"]);
                                            string cat_name122 = rw12["cat_name"].ToString();
                                            int p_cat_id122 = Convert.ToInt32(rw12["p_cat_id"]);
                                            //catmaster.category.Add(new Models.CategoryClass { cat_id = cat_id122, cat_name = cat_name122, p_cat_id = p_cat_id122 });
                                            if(count==1)
                                            {
                                                catmastergrid.category.Add(new Models.CategoryGrid { cat_id = Convert.ToInt32(rw["cat_id"]), cat_name = cat_name, sub_Cat1 ="", sub_Cat2 = cat_name12, description = description12, sub_Cat3 = cat_name122 });
                                            }
                                            else
                                            {
                                                if (count_a == 1)
                                                {
                                                    catmastergrid.category.Add(new Models.CategoryGrid { cat_id = Convert.ToInt32(rw["cat_id"]), cat_name = cat_name, sub_Cat1 = "", sub_Cat2 = "", description = description12, sub_Cat3 = cat_name122 });
                                                }
                                                else
                                                {
                                                    catmastergrid.category.Add(new Models.CategoryGrid { cat_id = Convert.ToInt32(rw["cat_id"]), cat_name = cat_name, sub_Cat1 = "", sub_Cat2 = "", description = description12, sub_Cat3 = cat_name122 });
                                                }
                                            }
                                            

                                        }
                                    }
                                    else
                                    {
                                        if (count_1 == 1)
                                        {
                                            if (count_a == 1)
                                            {
                                                catmastergrid.category.Add(new Models.CategoryGrid { cat_id = Convert.ToInt32(rw["cat_id"]), cat_name = cat_name, sub_Cat1 = cat_name1, sub_Cat2 = cat_name12, description = description1, sub_Cat3 = "" });
                                            }
                                            else
                                            {
                                                catmastergrid.category.Add(new Models.CategoryGrid { cat_id = Convert.ToInt32(rw["cat_id"]), cat_name = cat_name, sub_Cat1 = cat_name1, sub_Cat2 = cat_name12, description = description1, sub_Cat3 = "" });
                                            }

                                        }
                                        else
                                        {


                                            catmastergrid.category.Add(new Models.CategoryGrid { cat_id = Convert.ToInt32(rw["cat_id"]), cat_name = cat_name, sub_Cat1 = "", sub_Cat2 = cat_name12, description = description1, sub_Cat3 = "" });

                                        }

                                    }
                                }
                            }
                            else
                            {
                                if (count_2 == 1)
                                {
                                    catmastergrid.category.Add(new Models.CategoryGrid { cat_id = Convert.ToInt32(rw["cat_id"]), cat_name = cat_name, sub_Cat1 = cat_name1, sub_Cat2 = "", description = description1, sub_Cat3 = "" });
                                }
                                else
                                {
                                   
                                        catmastergrid.category.Add(new Models.CategoryGrid { cat_id = Convert.ToInt32(rw["cat_id"]), cat_name = cat_name, sub_Cat1 = cat_name1, sub_Cat2 = "", description = description1, sub_Cat3 = "" });
                                    

                                }
                            }
                        }
                    }
                    else
                    {
                        if (count_a==1)
                        {
                            catmastergrid.category.Add(new Models.CategoryGrid { cat_id = Convert.ToInt32(rw["cat_id"]), cat_name = cat_name, sub_Cat1 = "", sub_Cat2 = "", description = description, sub_Cat3 = "" });
                        }
                        else
                        {
                            catmastergrid.category.Add(new Models.CategoryGrid { cat_id = Convert.ToInt32(rw["cat_id"]), cat_name = cat_name, sub_Cat1 = "", sub_Cat2 = "", description = description, sub_Cat3 = "" });
                        }
                    }
                    

                }




            }
            
            ViewBag.category = catmastergrid.category;










            //DataSet ds3 = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblCategoryMaster", new SqlParameter("@action", "5"), new SqlParameter("@remark", SqlDbType.Char));
            // ViewBag.category1 = ds3.Tables[0];

            if (!string.IsNullOrEmpty(mid))
            {
                ViewBag.update = "1";
                using (var db = new Models.subzicartEntities())
                {
                    //var response = db.Database.SqlQuery<Models.tblCategory>(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblCategory", new SqlParameter("@action", "5"), new SqlParameter("@catId", mid), new SqlParameter("@remark", SqlDbType.Char)).FirstOrDefault();

                    DataSet ds1 = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblCategoryMaster", new SqlParameter("@action", "6"), new SqlParameter("@cat_id", mid), new SqlParameter("@remark", SqlDbType.Char));

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Models.tblSubCategory md = new Models.tblSubCategory();
                        md.catId = Convert.ToInt32(ds1.Tables[0].Rows[0]["catId"]);
                        md.subCatId = Convert.ToInt32(ds1.Tables[0].Rows[0]["subCatId"]);
                        md.subCatName = ds1.Tables[0].Rows[0]["subCatName"].ToString();

                        md.description = ds1.Tables[0].Rows[0]["description"].ToString();
                        md.isActive = Convert.ToInt32(ds1.Tables[0].Rows[0]["isActive"]);
                        ViewBag.isActive = Convert.ToInt32(ds1.Tables[0].Rows[0]["isActive"]);
                        ViewBag.categoryid = Convert.ToInt32(ds1.Tables[0].Rows[0]["catId"]);


                        ViewBag.data = md;

                        return View(md);
                    }
                    else
                    {
                        ViewBag.message = "Record has an error";
                        //return RedirectToAction("Insert_Category", "Admin");
                    }
                }
            }
            else
            {
                ViewBag.update = "0";
            }

            return View();
        }
        [HttpPost]
        public ActionResult Insert_CategoryMaster(Models.tblCategoryMaster modeldata, string command)
        {
            if (!string.IsNullOrEmpty(command) && command == "Save")
            {
                if (ModelState.ContainsKey("cat_id"))
                    ModelState["cat_id"].Errors.Clear();
                if (ModelState.IsValid)
                {
                    SqlParameter[] sp = new SqlParameter[7];
                  
                    sp[0] = new SqlParameter("@cat_name", modeldata.cat_name);
                    sp[1] = new SqlParameter("@description", modeldata.description);
                    sp[2] = new SqlParameter("@p_cat_id", modeldata.p_cat_id);
                    sp[3] = new SqlParameter("@isactive", modeldata.isActive);
                    sp[4] = new SqlParameter("@userid", Session["loginID"]);
                    sp[5] = new SqlParameter("@action", "1");
                    sp[6] = new SqlParameter("@remark", SqlDbType.NVarChar, 500);
                    sp[6].Direction = ParameterDirection.Output;
                    int k = SqlHelper.ExecuteNonQuery(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblCategoryMaster", sp);
                    if (k > 0)
                    {

                        ViewBag.message = sp[6].Value;
                        ModelState.Clear();
                        //return RedirectToAction("Insert_Category", "Admin");
                        ViewBag.update = "0";
                    }
                    else
                    {
                        ViewBag.message = sp[6].Value;
                        ModelState.Clear();
                        //return RedirectToAction("Insert_Category", "Admin");
                        ViewBag.update = "0";
                    }
                }
                else
                {
                    // ModelState.Clear();
                    //return RedirectToAction("Insert_Category", "Admin");
                    ViewBag.update = "0";
                }

            }
            else if (!string.IsNullOrEmpty(command) && command == "Update")
            {
                if (ModelState.IsValid)
                {
                    SqlParameter[] sp = new SqlParameter[8];
                    sp[0] = new SqlParameter("@cat_id", modeldata.cat_id);
                    sp[1] = new SqlParameter("@cat_name", modeldata.cat_name);
                    sp[2] = new SqlParameter("@description", modeldata.description);
                    sp[3] = new SqlParameter("@p_cat_id", modeldata.p_cat_id);
                    sp[4] = new SqlParameter("@isactive", modeldata.isActive);
                    sp[5] = new SqlParameter("@userid", Session["loginID"]);
                    sp[6] = new SqlParameter("@action", "1");
                    sp[7] = new SqlParameter("@remark", SqlDbType.NVarChar, 500);
                    sp[7].Direction = ParameterDirection.Output;
                    int k = SqlHelper.ExecuteNonQuery(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblCategoryMaster", sp);
                    if (k > 0)
                    {

                        ViewBag.message = sp[7].Value;
                        ModelState.Clear();
                        //return RedirectToAction("Insert_Category", "Admin");
                        ViewBag.update = "0";
                    }
                    else
                    {
                        ViewBag.message = sp[7].Value;
                        ModelState.Clear();
                        //return RedirectToAction("Insert_Category", "Admin");
                        ViewBag.update = "0";
                    }
                }
                else
                {
                    //ModelState.Clear();
                    //return RedirectToAction("Insert_Category", "Admin");
                    ViewBag.update = "0";
                }
            }
            if (!string.IsNullOrEmpty(command) && command == "Cancel")
            {

                return RedirectToAction("Insert_CategoryMaster", "Admin");
            }

            Models.MyViewModel catmaster = new Models.MyViewModel();

            catmaster.category = new List<Models.CategoryClass>();

            //List <Models.CategoryClass> catmaster = new List<Models.CategoryClass>();
            //Models.CategoryClass catmaster = new Models.CategoryClass();
            DataSet ds = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblCategoryMaster", new SqlParameter("@action", "5"), new SqlParameter("@p_cat_id", "1"), new SqlParameter("@remark", SqlDbType.Char));
            //ViewBag.category = ds.Tables[0];
            if (ds.Tables[0].Rows.Count > 0)
            {
                catmaster.category.Add(new Models.CategoryClass { cat_id = 1, cat_name = "Root", p_cat_id = 0 });
                foreach (DataRow rw in ds.Tables[0].Rows)
                {
                    int cat_id = Convert.ToInt32(rw["cat_id"]);
                    string cat_name = "-" + rw["cat_name"].ToString();
                    int p_cat_id = Convert.ToInt32(rw["p_cat_id"]);
                    //catmaster.cat_id = cat_id;
                    //catmaster.cat_name = cat_name;
                    //catmaster.p_cat_id = p_cat_id;
                    catmaster.category.Add(new Models.CategoryClass { cat_id = cat_id, cat_name = cat_name, p_cat_id = p_cat_id });

                    DataSet ds1 = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblCategoryMaster", new SqlParameter("@action", "5"), new SqlParameter("@p_cat_id", cat_id), new SqlParameter("@remark", SqlDbType.Char));
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow rw1 in ds1.Tables[0].Rows)
                        {
                            int cat_id1 = Convert.ToInt32(rw1["cat_id"]);
                            string cat_name1 = "---" + rw1["cat_name"].ToString();
                            int p_cat_id1 = Convert.ToInt32(rw1["p_cat_id"]);
                            catmaster.category.Add(new Models.CategoryClass { cat_id = cat_id1, cat_name = cat_name1, p_cat_id = p_cat_id1 });
                            DataSet ds11 = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblCategoryMaster", new SqlParameter("@action", "5"), new SqlParameter("@p_cat_id", cat_id1), new SqlParameter("@remark", SqlDbType.Char));
                            if (ds11.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow rw11 in ds11.Tables[0].Rows)
                                {
                                    int cat_id12 = Convert.ToInt32(rw11["cat_id"]);
                                    string cat_name12 = "-----" + rw11["cat_name"].ToString();
                                    int p_cat_id12 = Convert.ToInt32(rw11["p_cat_id"]);
                                    catmaster.category.Add(new Models.CategoryClass { cat_id = cat_id12, cat_name = cat_name12, p_cat_id = p_cat_id12 });

                                    DataSet ds12 = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblCategoryMaster", new SqlParameter("@action", "5"), new SqlParameter("@p_cat_id", cat_id12), new SqlParameter("@remark", SqlDbType.Char));
                                    if (ds12.Tables[0].Rows.Count > 0)
                                    {
                                        foreach (DataRow rw12 in ds12.Tables[0].Rows)
                                        {
                                            int cat_id122 = Convert.ToInt32(rw12["cat_id"]);
                                            string cat_name122 = "-----" + rw12["cat_name"].ToString();
                                            int p_cat_id122 = Convert.ToInt32(rw12["p_cat_id"]);
                                            catmaster.category.Add(new Models.CategoryClass { cat_id = cat_id122, cat_name = cat_name122, p_cat_id = p_cat_id122 });

                                        }
                                    }

                                }
                            }
                        }
                    }
                    catmaster.category.Add(new Models.CategoryClass { cat_id = 0, cat_name = "</br>", p_cat_id = 0 });

                }




            }
            ViewBag.category1 = catmaster.category;

            Models.MyViewModel1 catmastergrid = new Models.MyViewModel1();

            catmastergrid.category = new List<Models.CategoryGrid>();
            //catmastergrid.category.Add(new Models.CategoryGrid { cat_id = 0, cat_name = "", sub_Cat1 = "", sub_Cat2 = "" });
            //List <Models.CategoryClass> catmaster = new List<Models.CategoryClass>();
            //Models.CategoryClass catmaster = new Models.CategoryClass();
            DataSet ds113 = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblCategoryMaster", new SqlParameter("@action", "5"), new SqlParameter("@p_cat_id", "1"), new SqlParameter("@remark", SqlDbType.Char));
            //ViewBag.category = ds.Tables[0];
            if (ds113.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow rw in ds113.Tables[0].Rows)
                {
                    int cat_id = Convert.ToInt32(rw["cat_id"]);
                    string cat_name = rw["cat_name"].ToString();
                    string description = rw["description"].ToString();
                    int p_cat_id = Convert.ToInt32(rw["p_cat_id"]);
                    //catmaster.cat_id = cat_id;
                    //catmaster.cat_name = cat_name;
                    //catmaster.p_cat_id = p_cat_id;
                    //catmaster.category.Add(new Models.CategoryClass { cat_id = cat_id, cat_name = cat_name, p_cat_id = p_cat_id });

                    DataSet ds1 = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblCategoryMaster", new SqlParameter("@action", "5"), new SqlParameter("@p_cat_id", cat_id), new SqlParameter("@remark", SqlDbType.Char));
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow rw1 in ds1.Tables[0].Rows)
                        {
                            int cat_id1 = Convert.ToInt32(rw1["cat_id"]);
                            string cat_name1 = rw1["cat_name"].ToString();
                            string description1 = rw1["description"].ToString();
                            int p_cat_id1 = Convert.ToInt32(rw1["p_cat_id"]);
                            //catmaster.category.Add(new Models.CategoryClass { cat_id = cat_id1, cat_name = cat_name1, p_cat_id = p_cat_id1 });
                            DataSet ds11 = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblCategoryMaster", new SqlParameter("@action", "5"), new SqlParameter("@p_cat_id", cat_id1), new SqlParameter("@remark", SqlDbType.Char));
                            if (ds11.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow rw11 in ds11.Tables[0].Rows)
                                {
                                    int cat_id12 = Convert.ToInt32(rw11["cat_id"]);
                                    string cat_name12 = rw11["cat_name"].ToString();
                                    string description12 = rw11["description"].ToString();
                                    int p_cat_id12 = Convert.ToInt32(rw11["p_cat_id"]);

                                    //catmastergrid.category.Add(new Models.CategoryGrid { cat_id = Convert.ToInt32(rw["cat_id"]), cat_name = cat_name, sub_Cat1 = cat_name1, sub_Cat2 = cat_name12, description = description12 });


                                    DataSet ds12 = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblCategoryMaster", new SqlParameter("@action", "5"), new SqlParameter("@p_cat_id", cat_id12), new SqlParameter("@remark", SqlDbType.Char));
                                    if (ds12.Tables[0].Rows.Count > 0)
                                    {
                                        foreach (DataRow rw12 in ds12.Tables[0].Rows)
                                        {
                                            int cat_id122 = Convert.ToInt32(rw12["cat_id"]);
                                            string cat_name122 =  rw12["cat_name"].ToString();
                                            int p_cat_id122 = Convert.ToInt32(rw12["p_cat_id"]);
                                            //catmaster.category.Add(new Models.CategoryClass { cat_id = cat_id122, cat_name = cat_name122, p_cat_id = p_cat_id122 });
                                            catmastergrid.category.Add(new Models.CategoryGrid { cat_id = Convert.ToInt32(rw["cat_id"]), cat_name = cat_name, sub_Cat1 = cat_name1, sub_Cat2 = cat_name12, description = description12, sub_Cat3 = cat_name122 });

                                        }
                                    }
                                }
                            }
                            else
                            {
                                catmastergrid.category.Add(new Models.CategoryGrid { cat_id = Convert.ToInt32(rw["cat_id"]), cat_name = cat_name, sub_Cat1 = cat_name1, sub_Cat2 = "", description = description1, sub_Cat3 = "" });
                            }
                        }
                    }
                    else
                    {
                        catmastergrid.category.Add(new Models.CategoryGrid { cat_id = Convert.ToInt32(rw["cat_id"]), cat_name = cat_name, sub_Cat1 = "", sub_Cat2 = "", description = description, sub_Cat3 = "" });
                    }


                }




            }

            ViewBag.category = catmastergrid.category;





            return View();
        }

        public ActionResult insert_ImageLog (string mid)
        {
            try
            {
                ViewBag.message = Session["Message1"];
                Session.RemoveAll();
                Session.Clear();
            }

            catch (Exception ee) { }

            DataSet ds = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblProductMaster", new SqlParameter("@action", "7"), new SqlParameter("@remark", SqlDbType.Char));
            ViewBag.category = ds.Tables[0];

            DataSet ds2 = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblImage_log", new SqlParameter("@action", "2"), new SqlParameter("@remark", SqlDbType.Char));
            ViewBag.category1 = ds2.Tables[0];
            if (!string.IsNullOrEmpty(mid))
            {
                ViewBag.update = "1";
                using (var db = new Models.subzicartEntities())
                {
                    //var response = db.Database.SqlQuery<Models.tblCategory>(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblCategory", new SqlParameter("@action", "5"), new SqlParameter("@catId", mid), new SqlParameter("@remark", SqlDbType.Char)).FirstOrDefault();

                    DataSet ds1 = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblImage_log", new SqlParameter("@action", "2"), new SqlParameter("@brand_id", mid), new SqlParameter("@remark", SqlDbType.Char));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Models.tblImageLog md = new Models.tblImageLog();
                        md.product_id = Convert.ToInt32(ds1.Tables[0].Rows[0]["product_id"]);
                        md.image_id = Convert.ToInt32(ds1.Tables[0].Rows[0]["image_id"]);
                        md.image_name = ds1.Tables[0].Rows[0]["image_name"].ToString();
                        md.image_url = ds1.Tables[0].Rows[0]["image_url"].ToString();
                        md.image_desc = ds1.Tables[0].Rows[0]["image_desc"].ToString();
                        //md.image_binary = "";
                        
                        md.isthumb = ds1.Tables[0].Rows[0]["isthumb"].ToString();
                        md.sort_id = Convert.ToInt32(ds1.Tables[0].Rows[0]["sort_id"]);
                        md.active = Convert.ToInt32(ds1.Tables[0].Rows[0]["active"]);
                        ViewBag.isActive = Convert.ToInt32(ds1.Tables[0].Rows[0]["active"]);

                        ViewBag.data = md;
                        return View(md);
                    }
                    else
                    {
                        ViewBag.message = "Record has an error";
                        //return RedirectToAction("Insert_Category", "Admin");
                    }
                }
            }
            else
            {
                ViewBag.update = "0";
            }

            return View();
        }
        static string filename = string.Empty;
        [HttpPost]
        public ActionResult insert_ImageLog(Models.imageModel imagemodel, HttpPostedFileBase postedFile, string command)
        {
            string filePath = string.Empty;
            if (postedFile != null)
            {
                string path = Server.MapPath("~/product_image/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                filename = path + Path.GetFileName(postedFile.FileName);
                string extension = Path.GetExtension(postedFile.FileName);
                postedFile.SaveAs(filename);




            }
            else if(imagemodel.image_url!="" || imagemodel.image_url != null )
            {

                System.Drawing.Image image = DownloadImageFromUrl(imagemodel.image_url.Trim());

                string rootPath = Server.MapPath("~/product_image/");
                filename = System.IO.Path.Combine(rootPath, imagemodel.image_name + ".jpg");
                image.Save(filename);





            }

            try
            {

                Byte[] bytes = null;

                if (filename!= null)

                {
                    if (postedFile != null)
                    {
                        Stream fs = postedFile.InputStream;

                        BinaryReader br = new BinaryReader(fs);

                        bytes = br.ReadBytes((Int32)fs.Length);
                    }
                    else
                    {

                        byte[] fileContents;
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            using (Bitmap image = new Bitmap(WebRequest.Create(filename).GetResponse().GetResponseStream()))
                                image.Save(memoryStream, ImageFormat.Jpeg);
                            bytes = memoryStream.ToArray();
                        }



                    }

                    if (!string.IsNullOrEmpty(command) && command == "Save")
                    {
                        if (ModelState.ContainsKey("image_id"))
                            ModelState["image_id"].Errors.Clear();
                        if (ModelState.IsValid)
                        {
                            SqlParameter[] sp = new SqlParameter[11];

                            sp[0] = new SqlParameter("@product_id", imagemodel.product_id);
                            sp[1] = new SqlParameter("@image_name", imagemodel.image_name);
                            sp[2] = new SqlParameter("@image_desc", imagemodel.image_desc);
                            sp[3] = new SqlParameter("@image_url", "http://apps.indiaresults.com/subzicart/product_image/" + imagemodel.image_name + ".jpg");
                            sp[4] = new SqlParameter("@image_binary", bytes);
                            sp[5] = new SqlParameter("@isthumb", imagemodel.isthumb);
                            sp[6] = new SqlParameter("@sort_id", imagemodel.sort_id);
                            sp[7] = new SqlParameter("@insdt", imagemodel.insdt);
                            sp[8] = new SqlParameter("@userId", imagemodel.userId);
                            sp[9] = new SqlParameter("@action", "1");
                            sp[10] = new SqlParameter("@remark", SqlDbType.NVarChar, 500);
                            sp[10].Direction = ParameterDirection.Output;
                            int k = SqlHelper.ExecuteNonQuery(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblImage_log", sp);
                            if (k > 0)
                            {

                                ViewBag.message = sp[10].Value;
                                ModelState.Clear();
                                //return RedirectToAction("Insert_Category", "Admin");
                                ViewBag.update = "0";
                            }
                            else
                            {
                                ViewBag.message = sp[10].Value;
                                ModelState.Clear();
                                //return RedirectToAction("Insert_Category", "Admin");
                                ViewBag.update = "0";
                            }
                        }
                        else
                        {
                            // ModelState.Clear();
                            //return RedirectToAction("Insert_Category", "Admin");
                            ViewBag.update = "0";
                        }

                    }

                }

            }

            catch (Exception)
            {

                throw;

            }


            DataSet ds = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblProductMaster", new SqlParameter("@action", "7"), new SqlParameter("@remark", SqlDbType.Char));
            ViewBag.category = ds.Tables[0];

            DataSet ds2 = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblImage_log", new SqlParameter("@action", "2"), new SqlParameter("@remark", SqlDbType.Char));
            ViewBag.category1 = ds2.Tables[0];


            return View();

        }
        public System.Drawing.Image DownloadImageFromUrl(string imageUrl)
        {
            System.Drawing.Image image = null;

            try
            {
                System.Net.HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(imageUrl);
                webRequest.AllowWriteStreamBuffering = true;
                webRequest.Timeout = 30000;

                System.Net.WebResponse webResponse = webRequest.GetResponse();

                System.IO.Stream stream = webResponse.GetResponseStream();

                image = System.Drawing.Image.FromStream(stream);

                webResponse.Close();
            }
            catch (Exception ex)
            {
                return null;
            }

            return image;
        }
        private string ViewImage(byte[] arrayImage)

        {

            string base64String = Convert.ToBase64String(arrayImage, 0, arrayImage.Length);

            return "data:image/png;base64," + base64String;

        }
    }
}