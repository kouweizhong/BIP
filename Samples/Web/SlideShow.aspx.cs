using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace Wmb.TestWeb {
    public partial class SlideShow : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            string imagePath = Server.MapPath("~/Slides");
            DirectoryInfo di = new DirectoryInfo(imagePath);
            var files = from fileInfo in di.GetFiles("*.jpg")
                        select new {
                            Path = "~/Slides/" + fileInfo.Name,
                            ToolTip = fileInfo.Name + "(" + (fileInfo.Length / 1024) + "Kb)"};

            ListView1.DataSource = files;
            ListView1.DataBind();
        }
    }
}
