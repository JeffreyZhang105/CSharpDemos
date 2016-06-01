using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebDemos
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var a = 1;
            var b = 2;
            Response.Redirect("http://www.baidu.com");
        }

        protected override void Render(HtmlTextWriter writer)
        {
            var test = 1;
            var test2 = 2;
        }
    }
}