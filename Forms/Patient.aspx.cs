using ClinicApp.Class;
using ClinicApp.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ClinicApp.Forms
{
    public partial class Patients : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Create Data Table For GridView visits
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[2]
                {
                    new DataColumn("Date"),
                    new DataColumn("Visit Number"),
                });
                ViewState["VisitData"] = dt;
                BindGrid();

                // auto Generate For Code Patient
                var patient = new Patient
                {
                    PatientCode = GetNewCode.GetNewPatienttCode()
                };
                txtPatientCode.Text = patient.PatientCode;
            }

            using (var db = new DBDataContext())
            {
                GridPatient.DataSource = db.Patients;
                GridPatient.DataBind();
            }
        }
        protected void BindGrid()
        {
            GridView1.DataSource = (DataTable)ViewState["VisitData"];
            GridView1.DataBind();
        }
        protected void btnAddVisit_Click(object sender, EventArgs e)
        {
            if (txtvisitN.Text?.Length == 0)
            {
                Messegedanger.Visible = true;
                Messegedanger.InnerText = "Pleas Insert Visit Number";
                txtvisitN.Focus();
                return;
            }
            else if (DtVistDate.Value?.Length == 0)
            {
                Messegedanger.Visible = true;
                Messegedanger.InnerText = "Pleas Insert Visit Date";
                DtVistDate.Focus();
                return;
            }
            else
            {
                DataTable dt = (DataTable)ViewState["VisitData"];
                dt.Rows.Add(DtVistDate.Value.Trim(), txtvisitN.Text.Trim());
                ViewState["VisitData"] = dt;
                txtvisitN.Text = string.Empty;
                DtVistDate.Value = string.Empty;
                BindGrid();
            }
        }
        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            GridViewRow row = (sender as LinkButton)?.NamingContainer as GridViewRow;
            DataTable dt = ViewState["VisitData"] as DataTable;
            dt.Rows.RemoveAt(row.RowIndex);
            ViewState["VisitData"] = dt;
            BindGrid();
        }
        protected void Update_Click(object sender, EventArgs e)
        {
            GridViewRow row = (sender as LinkButton)?.NamingContainer as GridViewRow;
            var VisitNumber = ((TextBox) row.Cells[0].Controls[0]).Text;
            var VisiDate = ((TextBox)row.Cells[1].Controls[0]).Text;

            DataTable dt = ViewState["VisitData"] as DataTable;
            dt.Rows[row.RowIndex]["Date"] = VisiDate;
            dt.Rows[row.RowIndex]["Visit Number"] = VisitNumber;
            ViewState["VisitData"] = dt;
            GridView1.EditIndex = -1;
            BindGrid();
        }
        protected void Cancel_Click(object sender, EventArgs e)
        {
            GridView1.EditIndex = -1;
            BindGrid();
        }
        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            BindGrid();
        }
        private bool Validations()
        {
            if (txtPatientName.Text?.Length == 0)
            {
                Messegedanger.Visible = true;
                Messegedanger.InnerText = "Pleas Insert Patient Name";
                txtPatientName.Focus();
                return false;
            }
            return true;
        }
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            if (Validations())
            {
                var db1 = new DBDataContext();
                var code = db1.Patients.Where(x => x.PatientCode == txtPatientCode.Text).FirstOrDefault();
                // here its make insert Data
                if (code == null)
                {
                    using (var db = new DBDataContext())
                    {
                        var patientSub = new Patient
                        {
                            PatientCode = txtPatientCode.Text,
                            PatientName = txtPatientName.Text
                        };
                        db.Patients.InsertOnSubmit(patientSub);
                        db.SubmitChanges();
                        for (int i = 0; i < GridView1.Rows.Count; i++)
                        {
                            var visitSub = new Visit
                            {
                                PatientID = patientSub.ID,
                                VisitDate = GridView1.Rows[i].Cells[1].Text,
                                VisitNumber = GridView1.Rows[i].Cells[0].Text
                            };
                            db.Visits.InsertOnSubmit(visitSub);
                        }
                        db.SubmitChanges();
                    }
                }

                // Here Make Update Data
                else
                {
                    using (var db = new DBDataContext())
                    {
                        var patientAttach = new Patient
                        {
                            ID = db.Patients.Where(x => x.PatientCode == txtPatientCode.Text).Select(x => x.ID).FirstOrDefault(),
                            PatientCode = txtPatientCode.Text,
                            PatientName = txtPatientName.Text
                        };
                        db.Patients.Attach(patientAttach);
                        for (int i = 0; i < GridView1.Rows.Count; i++)
                        {
                            var visitAttach = new Visit
                            {
                                ID = db.Visits.Where(x => x.VisitNumber == GridView1.Rows[i].Cells[0].Text).Select(x => x.ID).FirstOrDefault(),
                                PatientID = patientAttach.ID,
                                VisitDate = GridView1.Rows[i].Cells[1].Text,
                                VisitNumber = GridView1.Rows[i].Cells[0].Text
                            };
                            db.Visits.Attach(visitAttach);
                        }
                        db.SubmitChanges();
                    }
                }
                MessegeSusuccess.InnerText = "Save Success ...";
                Response.Redirect("Patient.aspx");
            }
        }
        protected void BtnDeletePat_Click(object sender, EventArgs e)
        {
            using (var db = new DBDataContext())
            {
                GridViewRow row = (sender as LinkButton)?.NamingContainer as GridViewRow;
                var PatientID = db.Patients.Where(x => x.PatientCode == row.Cells[1].Text).Select(x=>x.ID).FirstOrDefault();
                var patientDelteing = new Patient
                {
                    ID = PatientID,
                    PatientCode = db.Patients.Where(x => x.ID == PatientID).Select(p => p.PatientCode).FirstOrDefault(),
                    PatientName = db.Patients.Where(x => x.ID == PatientID).Select(p => p.PatientName).FirstOrDefault()
                };
                db.Patients.Attach(patientDelteing);
                db.Patients.DeleteOnSubmit(patientDelteing);
                db.SubmitChanges();
                Response.Redirect("Patient.aspx");
                txtPatientName.Focus();
            }
        }
        protected void UpdatePetient_Click(object sender, EventArgs e)
        {
            using (var db = new DBDataContext())
            {
                GridViewRow row = (sender as LinkButton)?.NamingContainer as GridViewRow;
                var patient = new Patient
                {
                    ID = db.Patients.Where(x => x.PatientCode == row.Cells[1].Text).Select(x => x.ID).FirstOrDefault(),
                    PatientCode = db.Patients.Where(x => x.PatientCode == row.Cells[1].Text).Select(x => x.PatientCode).FirstOrDefault(),
                    PatientName = db.Patients.Where(x => x.PatientCode == row.Cells[1].Text).Select(x => x.PatientName).FirstOrDefault()
                };
                txtPatientCode.Text = patient.PatientCode;
                txtPatientName.Text = patient.PatientName;
                var visit = db.Visits.Where(x => x.PatientID == patient.ID).Select(x => new
                {
                    x.VisitDate,
                    x.VisitNumber
                }).ToArray();

                DataTable dt = (DataTable)ViewState["VisitData"];
               
                int colsCount = dt.Columns.Count;
                for (int i = 0; i < visit.Length; i++)
                {
                    dt.Rows.Add(visit[i].VisitDate.Trim(), visit[i].VisitNumber.Trim());
                }
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }
        protected void GridPatient_RowEditing(object sender, GridViewEditEventArgs e)
        {
            using (var db = new DBDataContext())
            {
                GridPatient.EditIndex = e.NewEditIndex;
                GridPatient.DataSource = db.Patients;
                GridPatient.DataBind();
            }
        }
    }
}