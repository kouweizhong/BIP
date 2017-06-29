using System;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace Wmb.TestWeb {
    public partial class UploadImageToDb : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {

        }

        protected void Button1_Click(object sender, EventArgs e) {
            if (FileUpload1.HasFile) {
                using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["PhotoDbConnectionString"].ConnectionString))
                using (SqlCommand command = new SqlCommand("insert into Photos (FileName, Image) values (@fileName, @imageBytes)", conn)) {
                    SqlParameter fileNameParam = new SqlParameter("@fileName", FileUpload1.FileName);
                    command.Parameters.Add(fileNameParam);

                    SqlParameter imageParam = new SqlParameter("@imageBytes", FileUpload1.FileBytes);
                    command.Parameters.Add(imageParam);

                    conn.Open();
                    command.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }
    }
}
