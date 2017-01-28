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
            SparkBot.AddTeamMember(member);
        }
    }
}