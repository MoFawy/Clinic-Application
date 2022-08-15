<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Patient.aspx.cs" Inherits="ClinicApp.Forms.Patients" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Clinic Application</title>
    <link href="../Bootstrap/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <nav class="navbar navbar-expand-md navbar-dark fixed-top bg-dark">
        <a class="navbar-brand" href="FRM_Patient.aspx">MyClinic.com</a>
        <button class="navbar-toggler collapsed" type="button" data-toggle="collapse" data-target="#navbar" aria-controls="navbar" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>
        <div id="navbar" class="navbar-collapse collapse">
            <ul class="navbar-nav mr-auto">
                <li class="nav-item active"><a class="nav-link" href="FRM_Patient.aspx">Home</a></li>
                <li class="nav-item"><a class="nav-link" href="#about">About</a></li>
                <li class="nav-item"><a class="nav-link" href="#contact">Contact</a></li>
                <li class="nav-item dropdown">
                    <a class="nav-link dropdown-toggle" href="#" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" id="dropdown04">Drop down <span class="caret"></span></a>
                    <div class="dropdown-menu" aria-labelledby="dropdown04">
                        <a class="dropdown-item" href="#">Action</a>
                        <a class="dropdown-item" href="#">Another action</a>
                        <a class="dropdown-item" href="#">Something else here</a>
                    </div>
                </li>
            </ul>
        </div>
    </nav>

    <form id="form2" runat="server">
        <div class="container mt-5">
            <div class="row">
                <div class="card mt-5">
                    <div class="card-header">
                        <div class="d-flex justify-content-between">
                            <h2>Create New Patient</h2>
                            <label runat="server" id="Messegedanger" visible="false" class="alert alert-danger"></label>
                            <label runat="server" id="MessegeSusuccess" visible="false" class="alert alert-success"></label>
                        </div>
                    </div>
                    <div class="card-body">
                        <div class="d-flex justify-content-between">
                            <div class="input-group">
                                <label class="input-group-text">Patient Name</label>
                                <asp:TextBox ID="txtPatientName" runat="server" CssClass="form-control" placeholder="Patient Name"></asp:TextBox>
                            </div>

                            <div class="input-group">
                                <label class="input-group-text">Patient Code</label>
                                <asp:TextBox ID="txtPatientCode" runat="server" CssClass="form-control" ReadOnly="True" Enabled="False"></asp:TextBox>
                            </div>
                        </div>
                        <hr />
                        <div class="d-flex justify-content-between">
                            <div class="input-group">
                                <label class="input-group-text">Visit Number</label>
                                <asp:TextBox ID="txtvisitN" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="input-group" data-provide="datepicker">
                                <label class="input-group-text">Visit Date</label>
                                <input type="date" runat="server" id="DtVistDate" class="form-control" />
                            </div>
                        </div>
                        <br />
                        <asp:Button ID="btnAddVisit" runat="server" Text="Add"
                            CssClass="btn btn-dark" OnClick="btnAddVisit_Click" />
                        <br />
                        <br />
                        <asp:GridView ID="GridView1" HeaderStyle-BackColor="#3AC0F2" CssClass="table table-dark" runat="server" AutoGenerateColumns="false" OnRowEditing="GridView1_RowEditing">
                            <Columns>
                                <asp:BoundField DataField="Visit Number" HeaderText="Visit Number" />
                                <asp:BoundField DataField="Date" HeaderText="Date" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="BtnEdit" Text="Edit" runat="server" CommandName="Edit" CssClass="btn btn-primary" />
                                        <asp:LinkButton ID="BtnDelete" Text="Delete" runat="server" OnClick="BtnDelete_Click" CssClass="btn btn-danger" OnClientClick="return confirm('Are you Sure you want to delete this product?');" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton Text="Update" ID="Update" runat="server" OnClick="Update_Click" CssClass="btn btn-success" />
                                        <asp:LinkButton Text="Cancel" ID="Cancel" runat="server" OnClick="Cancel_Click" CssClass="btn btn-primary" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                        <br />
                        <hr />
                        <asp:Button ID="btnSave" runat="server" Text="Save"
                            CssClass="btn btn-success " OnClick="BtnSave_Click" />
                        <br />
                    </div>
                    <div class="card-footer">

                        <asp:GridView ID="GridPatient" runat="server" AutoGenerateColumns="False"
                            CssClass="table table-dark" OnRowEditing="GridPatient_RowEditing">
                            <Columns>
                                <asp:BoundField DataField="PatientName" HeaderText="PatientName" SortExpression="PatientName"></asp:BoundField>
                                <asp:BoundField DataField="PatientCode" HeaderText="PatientCode" SortExpression="PatientCode"></asp:BoundField>

                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="BtnEdit" Text="Edit" runat="server" CommandName="Edit" CssClass="btn btn-primary" OnClick="UpdatePetient_Click" />
                                        <asp:LinkButton ID="BtnDeletePat" Text="Delete" runat="server" CssClass="btn btn-danger" OnClientClick="return confirm('Are you Sure you want to delete this product?');" OnClick="BtnDeletePat_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <script src="../Bootstrap/bootstrap/js/bootstrap.bundle.js"></script>
    <script src="../Bootstrap/bootstrap/js/bootstrap.js"></script>
</body>
</html>
