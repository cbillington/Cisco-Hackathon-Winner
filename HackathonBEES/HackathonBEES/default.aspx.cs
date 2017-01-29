using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HackathonBEES
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            Member member = new Member();
            member.name = txtName.Text;
            member.personEmail = txtEmail.Text;
            member.phone = txtphone.Text;
            member.role = ddlRole.SelectedValue;
            SparkBot.AddTeamMember(member);
        }

        protected void btnEmergency_Click(object sender, EventArgs e)
        {
            var emergs = DBAccess.GetEmergencies();
            gvData.DataSource = emergs;
            gvData.DataBind();
        }

        protected void btnSensor_Click(object sender, EventArgs e)
        {
            var sensorData = DBAccess.GetSensorData();
            gvData.DataSource = sensorData;
            gvData.DataBind();
        }

        protected void btnMembers_Click(object sender, EventArgs e)
        {
            var memberData = DBAccess.GetMembers();
            gvData.DataSource = memberData;
            gvData.DataBind();
        }
    }
}