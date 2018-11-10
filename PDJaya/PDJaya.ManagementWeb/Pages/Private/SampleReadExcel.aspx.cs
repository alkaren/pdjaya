using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Private_SampleReadExcel : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        BtnProcess.Click += BtnProcess_Click;
    }

    private void BtnProcess_Click(object sender, EventArgs e)
    {
        if (FileUpload1.HasFile)
        {
            var data = FileUpload1.FileBytes;
            var dt = ExcelReader.ReadExcelFile(data);
            if (dt != null)
            {
                foreach(DataRow dr in dt.Rows)
                {
                    var xx = Convert.ToString( dr[0]);
                }
                GvData.DataSource = dt;
                GvData.DataBind();
            }
        }
    }
}