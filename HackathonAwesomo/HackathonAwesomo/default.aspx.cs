using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HackathonAwesomo
{
    public partial class _default : Page
    {
        public static bool DataUpdated = true;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UpdateCharts();
                var patients = DBAccess.GetPatients();
                ddlPatients.DataSource = patients;
                ddlPatients.DataValueField = "id";
                ddlPatients.DataTextField = "name";
                ddlPatients.DataBind();
            }
        }

        protected void Timer1_OnTick(object sender, EventArgs e)
        {
            if (DataUpdated)
            {
                UpdateCharts();
                UpdatePanel1.Update();
                DataUpdated = false;
            }
        }

        protected void UpdateCharts()
        {
            var heartRateData = DBAccess.GetReadings(Config.DeviceId, "HeartRate");
            ctHeartRate.DataSource = heartRateData;
            ctHeartRate.DataBind();
            GridView1.DataSource = heartRateData;
            GridView1.DataBind();

            //var bloodPressureData = DBAccess.GetReadings(Config.DeviceId, "BloodPressure");
            //ctBloodPressure.DataSource = bloodPressureData;
            //ctBloodPressure.DataBind();
            //GridView2.DataSource = bloodPressureData;
            //GridView2.DataBind();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[4].Visible = false;
        }

        protected void ddlPatients_SelectedIndexChanged(object sender, EventArgs e)
        {
            Label1.Text = ddlPatients.SelectedValue;
            Config.DeviceId = ddlPatients.SelectedValue;
            UpdateCharts();
            UpdatePanel1.Update();
        }

        //protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    e.Row.Cells[0].Visible = false;
        //    e.Row.Cells[1].Visible = false;
        //    e.Row.Cells[4].Visible = false;
        //}
    }
}