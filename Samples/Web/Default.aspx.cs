using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;

namespace WebApplication1
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) {
                string imagePath = Server.MapPath("~/Slides");
                DirectoryInfo di = new DirectoryInfo(imagePath);
                var files = from fileInfo in di.GetFiles("*.jpg")
                            select new {
                                Path = "~/Slides/" + fileInfo.Name,
                                ToolTip = fileInfo.Name + "(" + (fileInfo.Length / 1024) + "Kb)"
                            };

                CycleList1.DataSource = files;
                CycleList1.DataBind();

                CycleList2.DataSource = files;
                CycleList2.DataBind();

                CycleList3.DataSource = files;
                CycleList3.DataBind();
            }
        }



        protected void BetterImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            BetterImage1.ImageSettings.Grayscale = !BetterImage1.ImageSettings.Grayscale;
        }
    }
}
