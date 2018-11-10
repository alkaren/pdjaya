<%@ Page Title="Master Bills" Language="C#" MasterPageFile="~/Pages/Master/Main.master" AutoEventWireup="true" CodeFile="Bills.aspx.cs" Inherits="Pages_Private_Bills" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:Panel ID="PanelGrid" runat="server">
        <asp:GridView CssClass="table table-bordered table-hover" ID="GvData" runat="server">
            <Columns>
                <asp:TemplateField HeaderText="Actions">
                    <ItemTemplate>
                        <table>
                            <tr>
                                <td>
                                    <asp:LinkButton CssClass="btn btn-info" ID="Edit" runat="server" CommandArgument='<%# Eval("Id") %>' CommandName="Ubah" Text="Ubah"><i class='fa fa-edit'></i></asp:LinkButton></td>
                                <td>
                                    <asp:LinkButton CssClass="btn btn-info" ID="Lihat" runat="server" CommandArgument='<%# Eval("Id") %>' CommandName="Lihat" Text="Lihat"><i class='fa fa-eye'></i></asp:LinkButton></td>
                                <td>
                                    <asp:LinkButton CssClass="btn btn-info" OnClientClick="return confirm('Yakin mau menghapus ?');" ID="Delete" runat="server" CommandArgument='<%# Eval("Id") %>' CommandName="Hapus" Text="Hapus"><i class='fa fa-trash'></i></asp:LinkButton></td>
                            </tr>
                        </table>



                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:Button CssClass="btn btn-info" ID="BtnAdd" runat="server" Text="Add Record" />
    </asp:Panel>
    <asp:Panel ID="PanelInput" Visible="false" runat="server">
        <asp:HiddenField ID="TxtID" runat="server" />
        <div class="form-group">
            <label for="billid">Bill ID:</label>
            <asp:TextBox runat="server" ReadOnly="true" ID="TxtBillID" TextMode="SingleLine" CssClass="form-control" name="billid"></asp:TextBox>
        </div>

        <div class="form-group">
            <label for="marketno">Market No:</label>
            <asp:TextBox runat="server" ValidationGroup="vg1" ID="TxtMarketNo" TextMode="SingleLine" CssClass="form-control" name="marketno"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="TxtMarketNo" ValidationGroup="vg1" ID="RequiredFieldValidator1" runat="server" ErrorMessage="Silakan isi market no"></asp:RequiredFieldValidator>
        </div>
        <div class="form-group">
            <label for="storeno">Store No:</label>
            <asp:TextBox runat="server" ID="TxtStoreNo" TextMode="SingleLine" CssClass="form-control" name="storeno"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="TxtStoreNo" ValidationGroup="vg1" ID="RequiredFieldValidator2" runat="server" ErrorMessage="Silakan isi store no"></asp:RequiredFieldValidator>

        </div>
        <div class="form-group">
            <label for="transcode">Transaction Code:</label>
            <asp:TextBox runat="server" ID="TxtTransactionCode" TextMode="SingleLine" CssClass="form-control" name="transcode"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="TxtTransactionCode" ValidationGroup="vg1" ID="RequiredFieldValidator3" runat="server" ErrorMessage="Silakan isi transaction code"></asp:RequiredFieldValidator>

        </div>

        <div class="form-group">
            <label for="amount">Amount:</label>
            <asp:TextBox runat="server" ID="TxtAmount" TextMode="Number" CssClass="form-control" name="amount"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="TxtAmount" ValidationGroup="vg1" ID="RequiredFieldValidator4" runat="server" ErrorMessage="Silakan isi amount"></asp:RequiredFieldValidator>

        </div>
        <div class="form-group">
            <label for="month">Month:</label>
            <asp:TextBox runat="server" ID="TxtMonth" TextMode="Number" CssClass="form-control" name="month"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="TxtMonth" ValidationGroup="vg1" ID="RequiredFieldValidator5" runat="server" ErrorMessage="Silakan isi month"></asp:RequiredFieldValidator>

        </div>
        <div class="form-group">
            <label for="year">Year:</label>
            <asp:TextBox runat="server" ID="TxtYear" TextMode="SingleLine" CssClass="form-control" name="year"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="TxtYear" ValidationGroup="vg1" ID="RequiredFieldValidator6" runat="server" ErrorMessage="Silakan isi year"></asp:RequiredFieldValidator>

        </div>
        <asp:Button ID="BtnSubmit" ValidationGroup="vg1" CssClass="btn btn-info" runat="server" Text="Save" />
        <asp:Button ID="BtnCancel" CssClass="btn btn-info" runat="server" Text="Cancel" />
        <asp:Label ID="TxtStatus" ForeColor="Red" runat="server" Text=""></asp:Label>
        <asp:ValidationSummary ValidationGroup="vg1" ShowSummary="false" ShowMessageBox="true" ID="ValidationSummary1" runat="server" />
    </asp:Panel>

    <script>
        $(document).ready(function () {
            $('#<%= GvData.ClientID %>').DataTable();
        });
    </script>

</asp:Content>

