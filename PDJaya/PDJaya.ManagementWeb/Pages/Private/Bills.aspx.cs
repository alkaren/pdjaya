using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Private_Bills : System.Web.UI.Page
{
    PDJayaEntities context;
    protected void Page_Load(object sender, EventArgs e)
    {
        if(context==null)
            context = new PDJayaEntities();
        if (!IsPostBack)
            RefreshGrid();
        BtnAdd.Click += BtnAdd_Click;
        BtnSubmit.Click += BtnSubmit_Click;
        BtnCancel.Click += BtnCancel_Click;
        GvData.RowCommand += GvData_RowCommand;
       
    }

    private void GvData_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        var IDS = Convert.ToInt32( e.CommandArgument);
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
                foreach(var item in datas)
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

    enum ModeForm { ViewGrid, EditData, LihatData}
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

    private void BtnCancel_Click(object sender, EventArgs e)
    {
        SetLayout(ModeForm.ViewGrid);
        RefreshGrid();
    }

    void LoadDetail(int fid)
    {
        var seldata = from x in context.bills
                      where x.Id == fid
                      select x;
        foreach(var item in seldata)
        {
            TxtAmount.Text = item.Amount.ToString();
            TxtBillID.Text = item.BillID;
            TxtMarketNo.Text = item.MarketNo;
            TxtMonth.Text = item.Month.ToString();
            TxtStatus.Text = "";
            TxtStoreNo.Text = item.StoreNo;
            TxtTransactionCode.Text = item.TransactionCode;
            TxtYear.Text = item.Year.ToString();
            TxtID.Value = item.Id.ToString();
            break;
        }
    }

    private void BtnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            //update
            if (!string.IsNullOrEmpty(TxtID.Value)) {
                var IDS = int.Parse(TxtID.Value);
                var NewItem = (from x in context.bills
                              where x.Id == IDS
                              select x).FirstOrDefault();
                //NewItem.BillID = $"TX-{shortid.ShortId.Generate(true, false, 10)}";
                NewItem.Amount = decimal.Parse(TxtAmount.Text);
                //NewItem.CreatedDate = DateTime.Now;
                //NewItem.IsPaid = false;
                NewItem.MarketNo = TxtMarketNo.Text;
                NewItem.Month = int.Parse(TxtMonth.Text);
                NewItem.Year = int.Parse(TxtYear.Text);
                NewItem.TransactionCode = TxtTransactionCode.Text;
                NewItem.StoreNo = TxtStoreNo.Text;
              
               
            }
            else //add new
            {
                var NewItem = new bill() { };
               // NewItem.BillID = $"TX-{shortid.ShortId.Generate(true, false, 10)}";
                NewItem.Amount = decimal.Parse(TxtAmount.Text);
                NewItem.CreatedDate = DateTime.Now;
                NewItem.IsPaid = false;
                NewItem.MarketNo = TxtMarketNo.Text;
                NewItem.Month = int.Parse(TxtMonth.Text);
                NewItem.Year = int.Parse(TxtYear.Text);
                NewItem.TransactionCode = TxtTransactionCode.Text;
                NewItem.StoreNo = TxtStoreNo.Text;
                context.bills.Add(NewItem);
               
               
            }
            context.SaveChanges();
            PanelGrid.Visible = true;
            PanelInput.Visible = false;
            RefreshGrid();
        }
        catch(Exception ex)
        {
            TxtStatus.Text = "gagal save --> "+ex.Message;
        }
    }

    private void BtnAdd_Click(object sender, EventArgs e)
    {
        PanelGrid.Visible = false;
        ClearFields();
        PanelInput.Visible = true;
    }

    void ClearFields()
    {
        //clear all field input
        TxtID.Value = "";
    }

    void RefreshGrid()
    {
        var datas = from x in context.bills
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
}