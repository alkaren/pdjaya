<%@ Page Title="Master User Profiles" Language="C#" MasterPageFile="~/Pages/Master/Main.master" AutoEventWireup="true" CodeFile="UserProfiles.aspx.cs" Inherits="Pages_Private_UserProfiles" %>

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
            <label for="userid">User ID:</label>
            <asp:TextBox runat="server" ReadOnly="true" ID="TxtUserID" TextMode="SingleLine" CssClass="form-control" name="userid"></asp:TextBox>
        </div>

        <div class="form-group">
            <label for="username">Username:</label>
            <asp:TextBox runat="server" ID="TxtUsername" TextMode="SingleLine" CssClass="form-control" name="username"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="TxtUsername" ValidationGroup="vg1" ID="RequiredFieldValidator1" runat="server" ErrorMessage="Silakan isi market no"></asp:RequiredFieldValidator>
        </div>
        <div class="form-group">
            <label for="fullname">Fullname:</label>
            <asp:TextBox runat="server" ID="TxtFullname" TextMode="SingleLine" CssClass="form-control" name="fullname"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="TxtFullname" ValidationGroup="vg1" ID="RequiredFieldValidator2" runat="server" ErrorMessage="Silakan isi store no"></asp:RequiredFieldValidator>

        </div>
        <div class="form-group">
            <label for="password">Password:</label>
            <asp:TextBox runat="server" ID="TxtPassword" TextMode="SingleLine" CssClass="form-control" name="password"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="TxtPassword" ValidationGroup="vg1" ID="RequiredFieldValidator3" runat="server" ErrorMessage="Silakan isi transaction code"></asp:RequiredFieldValidator>

        </div>
        <div class="form-group">
            <label for="department">Department:</label>
            <asp:TextBox runat="server" ID="TxtDepartment" TextMode="SingleLine" CssClass="form-control" name="department"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="TxtDepartment" ValidationGroup="vg1" ID="RequiredFieldValidator4" runat="server" ErrorMessage="Silakan isi transaction code"></asp:RequiredFieldValidator>

        </div>
        <div class="form-group">
            <label for="branch">Branch:</label>
            <asp:TextBox runat="server" ID="TxtBranch" TextMode="SingleLine" CssClass="form-control" name="branch"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="TxtBranch" ValidationGroup="vg1" ID="RequiredFieldValidator5" runat="server" ErrorMessage="Silakan isi transaction code"></asp:RequiredFieldValidator>

        </div>
        <div class="form-group">
            <label for="isactive">Is Active:</label>
            <asp:CheckBox ID="ChkActive" runat="server" name="isactive" />
            <asp:RequiredFieldValidator ControlToValidate="ChkActive" ValidationGroup="vg1" ID="RequiredFieldValidator6" runat="server" ErrorMessage="Silakan isi transaction code"></asp:RequiredFieldValidator>
        </div>

        <div class="form-group">
            <label for="firstname">First Name:</label>
            <asp:TextBox runat="server" ID="TxtFirstName" TextMode="SingleLine" CssClass="form-control" name="firstname"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="TxtFirstName" ValidationGroup="vg1" ID="RequiredFieldValidator7" runat="server" ErrorMessage="Silakan isi transaction code"></asp:RequiredFieldValidator>

        </div>
        <div class="form-group">
            <label for="lastname">Last Name:</label>
            <asp:TextBox runat="server" ID="TxtLastName" TextMode="SingleLine" CssClass="form-control" name="lastname"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="TxtLastName" ValidationGroup="vg1" ID="RequiredFieldValidator8" runat="server" ErrorMessage="Silakan isi transaction code"></asp:RequiredFieldValidator>

        </div>
        <div class="form-group">
            <label for="email">Email:</label>
            <asp:TextBox runat="server" ID="TxtEmail" TextMode="SingleLine" CssClass="form-control" name="email"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="TxtEmail" ValidationGroup="vg1" ID="RequiredFieldValidator9" runat="server" ErrorMessage="Silakan isi transaction code"></asp:RequiredFieldValidator>

        </div>
        <div class="form-group">
            <label for="role">Role:</label>
            <asp:TextBox runat="server" ID="TxtRole" TextMode="SingleLine" CssClass="form-control" name="role"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="TxtRole" ValidationGroup="vg1" ID="RequiredFieldValidator10" runat="server" ErrorMessage="Silakan isi transaction code"></asp:RequiredFieldValidator>

        </div>
        <div class="form-group">
            <label for="phone">Phone:</label>
            <asp:TextBox runat="server" ID="TxtPhone" TextMode="SingleLine" CssClass="form-control" name="phone"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="TxtPhone" ValidationGroup="vg1" ID="RequiredFieldValidator11" runat="server" ErrorMessage="Silakan isi transaction code"></asp:RequiredFieldValidator>

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
