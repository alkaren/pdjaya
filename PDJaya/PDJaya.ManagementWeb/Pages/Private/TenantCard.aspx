<%@ Page Title="Master Tenant Card" Language="C#" MasterPageFile="~/Pages/Master/Main.master" AutoEventWireup="true" CodeFile="TenantCard.aspx.cs" Inherits="Pages_Private_TenantCard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
      <h1>Master Data Tenant Card</h1>
<p>
    <div class="text-right">
        <asp:FormView ID="FormView1" runat="server" DataKeyNames="Id" DataSourceID="EntityDataSource1" DefaultMode="Insert">
            <EditItemTemplate>
                CreatedBy:
                <asp:TextBox ID="CreatedByTextBox" runat="server" Text='<%# Bind("CreatedBy") %>' />
                <br />
                UpdatedBy:
                <asp:TextBox ID="UpdatedByTextBox" runat="server" Text='<%# Bind("UpdatedBy") %>' />
                <br />
                Created:
                <asp:TextBox ID="CreatedTextBox" runat="server" Text='<%# Bind("Created") %>' />
                <br />
                Updated:
                <asp:TextBox ID="UpdatedTextBox" runat="server" Text='<%# Bind("Updated") %>' />
                <br />
                Id:
                <asp:Label ID="IdLabel1" runat="server" Text='<%# Eval("Id") %>' />
                <br />
                CardID:
                <asp:TextBox ID="CardIDTextBox" runat="server" Text='<%# Bind("CardID") %>' />
                <br />
                StoreNo:
                <asp:TextBox ID="StoreNoTextBox" runat="server" Text='<%# Bind("StoreNo") %>' />
                <br />
                CardNo:
                <asp:TextBox ID="CardNoTextBox" runat="server" Text='<%# Bind("CardNo") %>' />
                <br />
                CardType:
                <asp:TextBox ID="CardTypeTextBox" runat="server" Text='<%# Bind("CardType") %>' />
                <br />
                Status:
                <asp:TextBox ID="StatusTextBox" runat="server" Text='<%# Bind("Status") %>' />
                <br />
                <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" CommandName="Update" Text="Update" />
                &nbsp;<asp:LinkButton ID="UpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" />
            </EditItemTemplate>
            <InsertItemTemplate>
                <form class="form-horizontal" role="form"> 
                    <div class="jumbotron">
                        <div class="form-group">
                            <label class="control-label col-sm-3">CreatedBy:</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="CreatedByTextBox" runat="server" Text='<%# Bind("CreatedBy") %>' CssClass="form-control"/>
                                </div>
                        </div>
                    <br /><br />
                        <div class="form-group">
                            <label class="control-label col-sm-3">UpdatedBy:</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="UpdatedByTextBox" runat="server" Text='<%# Bind("UpdatedBy") %>' CssClass="form-control"/>
                                </div>
                            </div>
                         <br /><br />
                            <div class="form-group">
                                <label class="control-label col-sm-3">Created:</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="CreatedTextBox" runat="server" Text='<%# Bind("Created") %>' CssClass="form-control"/>
                                    </div>
                            </div>
                         <br /><br />
                        <div class="form-group">
                            <label class="control-label col-sm-3">Updated:</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="UpdatedTextBox" runat="server" Text='<%# Bind("Updated") %>' CssClass="form-control"/>
                                </div>
                            </div>
                       <br /><br />
                       <div class="form-group">
                            <label class="control-label col-sm-3">Id:</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="IdTextBox" runat="server" Text='<%# Bind("Id") %>' CssClass="form-control"/>
                                </div>
                           </div>
                       <br /><br />
                       <div class="form-group">
                            <label class="control-label col-sm-3">CardID:</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="CardIDTextBox" runat="server" Text='<%# Bind("CardID") %>' CssClass="form-control" />
                                </div>
                           </div>
                      <br /><br />
                      <div class="form-group">
                            <label class="control-label col-sm-3">StoreNo:</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="StoreNoTextBox" runat="server" Text='<%# Bind("StoreNo") %>' CssClass="form-control" />
                                </div>
                            </div>
                      <br /><br />
                      <div class="form-group">
                            <label class="control-label col-sm-3">CardNo:</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="CardNoTextBox" runat="server" Text='<%# Bind("CardNo") %>' CssClass="form-control" />
                                </div>
                           </div>
                      <br /><br />
                      <div class="form-group">
                            <label class="control-label col-sm-3">CardType:</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="CardTypeTextBox" runat="server" Text='<%# Bind("CardType") %>' CssClass="form-control" />
                                </div>
                           </div>
                      <br /><br />
                      <div class="form-group">
                            <label class="control-label col-sm-3">Status:</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="StatusTextBox" runat="server" Text='<%# Bind("Status") %>' CssClass="form-control" />
                                </div>
                           </div>
                      <br /><br />
                      <div class="col-sm-offset-2 col-sm-7">
                            <asp:LinkButton ID="InsertButton" runat="server" CausesValidation="True" CommandName="Insert" Text="Add" CssClass="btn btn-success" />
                                &nbsp;<asp:LinkButton ID="InsertCancelButton" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" CssClass="btn btn-danger" />
                            </div>
                        </div>
                    </form>
            </InsertItemTemplate>
            <ItemTemplate>
                CreatedBy:
                <asp:Label ID="CreatedByLabel" runat="server" Text='<%# Bind("CreatedBy") %>' />
                <br />
                UpdatedBy:
                <asp:Label ID="UpdatedByLabel" runat="server" Text='<%# Bind("UpdatedBy") %>' />
                <br />
                Created:
                <asp:Label ID="CreatedLabel" runat="server" Text='<%# Bind("Created") %>' />
                <br />
                Updated:
                <asp:Label ID="UpdatedLabel" runat="server" Text='<%# Bind("Updated") %>' />
                <br />
                Id:
                <asp:Label ID="IdLabel" runat="server" Text='<%# Eval("Id") %>' />
                <br />
                CardID:
                <asp:Label ID="CardIDLabel" runat="server" Text='<%# Bind("CardID") %>' />
                <br />
                StoreNo:
                <asp:Label ID="StoreNoLabel" runat="server" Text='<%# Bind("StoreNo") %>' />
                <br />
                CardNo:
                <asp:Label ID="CardNoLabel" runat="server" Text='<%# Bind("CardNo") %>' />
                <br />
                CardType:
                <asp:Label ID="CardTypeLabel" runat="server" Text='<%# Bind("CardType") %>' />
                <br />
                Status:
                <asp:Label ID="StatusLabel" runat="server" Text='<%# Bind("Status") %>' />
                <br />
                <asp:LinkButton ID="EditButton" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit" />
                &nbsp;<asp:LinkButton ID="DeleteButton" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete" />
                &nbsp;<asp:LinkButton ID="NewButton" runat="server" CausesValidation="False" CommandName="New" Text="New" />
            </ItemTemplate>
        </asp:FormView>
    </div>
    <asp:EntityDataSource ID="EntityDataSource1" runat="server" ConnectionString="name=PDJayaEntities" DefaultContainerName="PDJayaEntities" EnableDelete="True" EnableFlattening="False" EnableInsert="True" EnableUpdate="True" EntitySetName="tenantcards">
    </asp:EntityDataSource>
</p>
</asp:Content>

