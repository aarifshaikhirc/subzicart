using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using subzicart.Models;


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


        public ActionResult productlist()
        {
            if (Request.Cookies["loginID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }


            DataSet ds = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblProductMaster", new SqlParameter("@action", 3));
            //List<> lst = new List<productMaster>();

            List<productMaster> lst = new List<productMaster>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                productMaster list = new productMaster();
                list.productId = Convert.ToInt32(ds.Tables[0].Rows[i]["productId"]);
                list.productName = Convert.ToString(ds.Tables[0].Rows[i]["productName"]);
                list.categoryName = Convert.ToString(ds.Tables[0].Rows[i]["catName"]);
                list.subCatName = Convert.ToString(ds.Tables[0].Rows[i]["subCatName"]);
                list.brandName = Convert.ToString(ds.Tables[0].Rows[i]["brand_name"]);
                list.SKU = Convert.ToString(ds.Tables[0].Rows[i]["SKU"]);
                list.price = Convert.ToInt32(ds.Tables[0].Rows[i]["price"]);
                list.costPrice = Convert.ToInt32(ds.Tables[0].Rows[i]["costPrice"]);
                list.retailPrice = Convert.ToInt32(ds.Tables[0].Rows[i]["retailPrice"]);
                list.salePrice = Convert.ToInt32(ds.Tables[0].Rows[i]["salePrice"]);
                lst.Add(list);
            }

            return View(lst);

        }



        public ActionResult productEdit(int id)
        {
            if (Request.Cookies["loginID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            commonFunctions obj = new commonFunctions();
            ViewBag.category = obj.GetCategory();
            ViewBag.Subcategory = obj.GetSubCategory();
            ViewBag.Brand = obj.GetBrand();


            productMaster list = new productMaster();
            DataSet ds = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblProductMaster", new SqlParameter("@action", 5), new SqlParameter("@productId", id));
            list.productId = Convert.ToInt32(ds.Tables[0].Rows[0]["productId"]);
            list.productName = Convert.ToString(ds.Tables[0].Rows[0]["productName"]);
            list.catId = Convert.ToString(ds.Tables[0].Rows[0]["catId"]);
            list.subCatId = Convert.ToString(ds.Tables[0].Rows[0]["subCatId"]);
            list.brandId = Convert.ToString(ds.Tables[0].Rows[0]["brandId"]);
            list.SKU = Convert.ToString(ds.Tables[0].Rows[0]["SKU"]);
            list.aboutProduct = Convert.ToString(ds.Tables[0].Rows[0]["aboutProduct"]);
            list.description = Convert.ToString(ds.Tables[0].Rows[0]["description"]);
            list.price = Convert.ToInt32(ds.Tables[0].Rows[0]["price"]);
            list.costPrice = Convert.ToInt32(ds.Tables[0].Rows[0]["costPrice"]);
            list.retailPrice = Convert.ToInt32(ds.Tables[0].Rows[0]["retailPrice"]);
            list.salePrice = Convert.ToInt32(ds.Tables[0].Rows[0]["salePrice"]);
            return View(list);

        }
        public ActionResult addproduct()
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
        [HttpPost]
        public ActionResult addproduct(productMaster obj, string ddlCategory, string[] ddlSubCategory, string ddlBrand)
        {
            string subcategory = "";
            string subcategoryName = "";
            foreach (var item in ddlSubCategory)
            {
                subcategory += item.Split('-')[0] + ",";
                subcategoryName += item.Split('-')[1] + ",";
            }

            SqlParameter[] sparameter = new SqlParameter[15];
            sparameter[0] = new SqlParameter("@catId", ddlCategory);
            sparameter[1] = new SqlParameter("@subCatId", subcategory.Substring(0, subcategory.Length - 1));
            sparameter[2] = new SqlParameter("@brandId", ddlBrand);
            sparameter[3] = new SqlParameter("@productName", obj.productName);
            sparameter[4] = new SqlParameter("@SKU", obj.SKU);
            sparameter[5] = new SqlParameter("@aboutProduct", obj.aboutProduct);
            sparameter[6] = new SqlParameter("@description", obj.description);
            sparameter[7] = new SqlParameter("@price", obj.price);
            sparameter[8] = new SqlParameter("@costPrice", obj.costPrice);
            sparameter[9] = new SqlParameter("@retailPrice", obj.retailPrice);
            sparameter[10] = new SqlParameter("@salePrice", obj.salePrice);
            sparameter[11] = new SqlParameter("@userId", Request.Cookies["loginID"].Value.ToString());
            sparameter[12] = new SqlParameter("@action", 1);
            sparameter[13] = new SqlParameter("@remark", SqlDbType.NVarChar, 100);
            sparameter[13].Direction = ParameterDirection.Output;
            sparameter[14] = new SqlParameter("@subCatName", subcategoryName.Substring(0, subcategoryName.Length - 1));
            //sparameter.Parameters["@remark"].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblProductMaster", sparameter);
            ViewBag.msg = Convert.ToString(sparameter[13].Value);
            return View();
        }

        public JsonResult loadCategory(int sID)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            DataSet ds = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.Text, "select catId, catName from tblCategory order by catName");
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow sdr in ds.Tables[0].Rows)
                {
                    items.Add(new SelectListItem
                    {
                        Text = sdr["catName"].ToString(),
                        Value = sdr["catId"].ToString()
                    });
                }
            }
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadSKU(int sID)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            DataSet ds = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.Text, "select top 1 SKU from tblProductMaster where brandId=" + sID + " order by SKU desc");
            //DataSet ds = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblProductMaster", new SqlParameter("@brand_id", sID), new SqlParameter("@action", 6));
            //string skuRange = Convert.ToString(SqlHelper.ExecuteScalar(Connection.ConnectionString, CommandType.StoredProcedure, "sp_tblBrandPrefixes", new SqlParameter("@brand_id", sID), new SqlParameter("@action", 7)));
            string skuRange = Convert.ToString(SqlHelper.ExecuteScalar(Connection.ConnectionString, CommandType.Text, "select brand_prefix + '-' + sku_start_range + '-' +  sku_end_range as prefix from tblBrandPrefixes where brand_id=" + sID + ""));

            if (ds.Tables[0].Rows.Count > 0)
            {
                int sku = Convert.ToInt32(ds.Tables[0].Rows[0]["SKU"].ToString().Replace(skuRange.Split('-')[0], ""));
                sku++;

                string final = sku.ToString();

                if (final.Length == 1)
                    final = "00000" + final;
                if (final.Length == 2)
                    final = "0000" + final;
                if (final.Length == 3)
                    final = "000" + final;
                else if (final.Length == 4)
                    final = "00" + final;
                else if (final.Length == 5)
                    final = "0" + final;

                if (sku > Convert.ToInt32(skuRange.Split('-')[2]))
                {
                    items.Add(new SelectListItem
                    {
                        Text = "SKU Range Exceed",
                        Value = "SKU Range Exceed",
                    });
                }
                else
                {
                    items.Add(new SelectListItem
                    {
                        Text = skuRange.Split('-')[0] + final,
                        Value = skuRange.Split('-')[0] + final
                    });
                }
            }
            else
            {
                items.Add(new SelectListItem
                {
                    Text = skuRange.Split('-')[0] + skuRange.Split('-')[1],
                    Value = skuRange.Split('-')[0] + skuRange.Split('-')[1]
                });
            }
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadSubcategorybyCategory(int sID)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            //DataSet ds = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.Text, "select subCatId,subCatName from tblSubCategory where catId='" + sID + "' order by subCatName");
            DataSet ds = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.Text, "select subCatId,subCatName from tblSubCategory  order by subCatName");
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow sdr in ds.Tables[0].Rows)
                {
                    items.Add(new SelectListItem
                    {
                        Text = sdr["subCatName"].ToString(),
                        Value = sdr["subCatId"].ToString()
                    });
                }
            }
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult loadBrand(int sID)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            DataSet ds = SqlHelper.ExecuteDataset(Connection.ConnectionString, CommandType.Text, "select brand_id,brand_name from tblBrandPrefixes order by brand_name");
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow sdr in ds.Tables[0].Rows)
                {
                    items.Add(new SelectListItem
                    {
                        Text = sdr["brand_name"].ToString(),
                        Value = sdr["brand_id"].ToString()
                    });
                }
            }
            return Json(items, JsonRequestBehavior.AllowGet);
        }


    }
}