using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Private_UserProfiles : System.Web.UI.Page
{
    PDJayaEntities context;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (context == null)
            context = new PDJayaEntities();
        if (!IsPostBack)
            RefreshGrid();
        BtnAdd.Click += BtnAdd_Click;
        //BtnSubmit.Click += BtnSubmit_Click;
        //BtnCancel.Click += BtnCancel_Click;
        GvData.RowCommand += GvData_RowCommand;
    }
    private void GvData_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        var IDS = Convert.ToInt32(e.CommandArgument);
        switch (e.CommandName)
        {
            case "Ubah":
                SetLayout(ModeForm.EditData);
                LoadDetail(IDS);
                break;
            case "Hapus":
                var datas = from x in context.bills
                            where x.Id == IDS
                            select x;
                foreach (var item in datas)
                {
                    context.bills.Remove(item);
                }
                context.SaveChanges();
                RefreshGrid();
                break;
            case "Lihat":
                SetLayout(ModeForm.LihatData);
                LoadDetail(IDS);

                break;
        }
    }

    enum ModeForm { ViewGrid, EditData, LihatData }
    void SetLayout(ModeForm mode)
    {
        switch (mode)
        {
            case ModeForm.EditData:
                PanelGrid.Visible = false;
                PanelInput.Visible = true;
                BtnSubmit.Visible = true;
                break;
            case ModeForm.LihatData:
                PanelGrid.Visible = false;
                PanelInput.Visible = true;
                BtnSubmit.Visible = false;
                break;
            case ModeForm.ViewGrid:
                PanelGrid.Visible = true;
                PanelInput.Visible = false;
                break;
        }

    }
    void LoadDetail(int fid)
    {
        var seldata = from x in context.userprofiles
                      where x.Id == fid
                      select x;
        foreach (var item in seldata)
        {
            TxtUserID.Text = item.UserId;
            TxtUsername.Text = item.UserName;
            TxtFullname.Text = item.FullName;
            TxtPassword.Text = item.Password;
            TxtDepartment.Text = item.Department;
            TxtBranch.Text = item.Branch;
            ChkActive.Checked = item.IsActive;
            TxtFirstName.Text = item.Firstname;
            TxtLastName.Text = item.Lastname;
            TxtEmail.Text = item.Email;
            TxtRole.Text = item.Role;
            TxtPhone.Text = item.Phone;
            break;

            //TxtAmount.Text = item.Amount.ToString();
            //TxtBillID.Text = item.BillID;
            //TxtMarketNo.Text = item.MarketNo;
            //TxtMonth.Text = item.Month.ToString();
            //TxtStatus.Text = "";
            //TxtStoreNo.Text = item.StoreNo;
            //TxtTransactionCode.Text = item.TransactionCode;
            //TxtYear.Text = item.Year.ToString();
            //TxtID.Value = item.Id.ToString();
            //break;
        }
    }

    void RefreshGrid()
    {
        var datas = from x in context.userprofiles
                    select x;
        GvData.DataSource = datas.ToList();
        GvData.DataBind();

        if (GvData.Rows.Count > 0)
        {
            //This replaces <td> with <th>    
            GvData.UseAccessibleHeader = true;
            //This will add the <thead> and <tbody> elements    
            GvData.HeaderRow.TableSection = TableRowSection.TableHeader;
            //This adds the <tfoot> element. Remove if you don't have a footer row    
            GvData.FooterRow.TableSection = TableRowSection.TableFooter;
        }
    }
    private void BtnAdd_Click(object sender, EventArgs e)
    {
        PanelGrid.Visible = false;
       // ClearFields();
        PanelInput.Visible = true;
    }

}