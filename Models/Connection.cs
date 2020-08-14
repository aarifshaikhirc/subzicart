
using System.Web;

namespace bseb
{
    /// <summary>
    /// Summary description for Connection
    /// </summary>
    public class Connection
    {

        #region connnectionString_C
        public static string ConnectionString_C
        {


            get
            {
                object userSettings = "";
                HttpCookie aCookie = HttpContext.Current.Request.Cookies["class"];
                if (aCookie != null)
                {
                     userSettings = aCookie.Value;
                }
                else
                {
                     userSettings = "subzicart"; 
                    //userSettings = "";
                }


                //return "Data Source=216.158.238.98; Initial Catalog = '" + userSettings.ToString() + "'; Persist Security Info=True;Connection Timeout=1000;User ID = gk; Password=Admin@123";
                //return "Data Source=35.154.78.52; Initial Catalog = '" + userSettings.ToString() + "'; Persist Security Info=True;Connection Timeout=1000;User ID = hbse; Password=irc@2016!@#"; 
                return "Data Source=35.154.78.52; Initial Catalog = 'subzicart'; Persist Security Info=True;Connection Timeout=1000;User ID = hbse; Password=irc@2016!@#"; 

            }
        }
        #endregion


    }
}
