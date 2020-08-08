using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.Remoting.Messaging;

namespace SqlLoginWebPage
{
    public partial class Dashboard : System.Web.UI.Page
    {
        SqlConnection sqlCon = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = |DataDirectory|\LoginDB.mdf; Integrated Security = True; Connect Timeout = 30");
        protected void Page_Load(object sender, EventArgs e)
        {
            FillGridView();
            if (Session["UserName"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            lblUserDetails.Text = "UserName: " + Session["UserName"];
            if (!IsPostBack)
            {
                btnDelete.Enabled = false;
            }
        }

        protected void bntLogout_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }
        public void Clear()
        {
            hfContactID.Value = "";
            txtName.Text = txtMobile.Text = txtAddress.Text = "";
            btnSave.Text = "Save";
            btnDelete.Enabled = false;
            lblErrorMessage.Text = lblSuccessMessage.Text = "";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();
            SqlCommand sqlcmd = new SqlCommand("ContactCreateOrUpdate", sqlCon);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.Parameters.AddWithValue("@ContactID",(hfContactID.Value==""?0:Convert.ToInt32(hfContactID.Value)));
            sqlcmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@Mobile", txtMobile.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
            sqlcmd.ExecuteNonQuery();
            sqlCon.Close();
            Clear();
            if (hfContactID.Value == "")
            {
                lblSuccessMessage.Text = "Saved Successfully";
            }
            else
            {
                lblSuccessMessage.Text = "Updated Successfully";
            }
            FillGridView();
        }
        void FillGridView()
        {
            if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();
            SqlDataAdapter sqlda = new SqlDataAdapter("ContactViewAll", sqlCon);
            sqlda.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dtbl = new DataTable();
            sqlda.Fill(dtbl);
            sqlCon.Close();
            gvContact.DataSource = dtbl;
            gvContact.DataBind();
        }
        protected void lnk_OnClick(object sender, EventArgs e)
        {
            int ContactID = Convert.ToInt32((sender as LinkButton).CommandArgument);
            if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();
            SqlDataAdapter sqlda = new SqlDataAdapter("ContactViewByID", sqlCon);
            sqlda.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlda.SelectCommand.Parameters.AddWithValue("@ContactID",ContactID);
            DataTable dtbl = new DataTable();
            sqlda.Fill(dtbl);
            sqlCon.Close();
            hfContactID.Value = ContactID.ToString();
            txtName.Text = dtbl.Rows[0]["Name"].ToString();
            txtMobile.Text = dtbl.Rows[0]["Mobile"].ToString();
            txtAddress.Text = dtbl.Rows[0]["Address"].ToString();
            btnSave.Text = "Update";
            btnDelete.Enabled = true;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();
            SqlCommand sqlcmd = new SqlCommand("ContactDeleteByID", sqlCon);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.Parameters.AddWithValue("@ContactID",Convert.ToInt32(hfContactID.Value));
            sqlcmd.ExecuteNonQuery();
            sqlCon.Close();
            Clear();
            FillGridView();
            lblSuccessMessage.Text = "Delete Successfully";
        }
    }
}