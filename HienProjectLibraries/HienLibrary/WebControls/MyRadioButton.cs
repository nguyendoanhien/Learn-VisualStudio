using System.Web.UI;
using System.Web.UI.WebControls;

namespace ShoesStore.WebControls
{
    public class MyRadioButton : RadioButton
    {
        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write("<input id=\"" + base.ClientID + "\" ");
            writer.Write("type=\"radio\" ");
            writer.Write("name=\"" + base.ID + "\" ");
            writer.Write("value=\"" + base.ID + "\" />");
            writer.Write("<label for=\"" + base.ClientID + "\">");
            writer.Write(base.Text);
            writer.Write("</label>");
        }
    }
}