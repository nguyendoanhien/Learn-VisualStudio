
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ShoesStore.MyExtensions
{
    public static partial class MyExtensions
    {
   
        public static void ClearTextBoxes(this Control p1)
        {
            foreach (Control ctrl in p1.Controls)
            {
                switch (ctrl)
                {
                    case TextBox txtBox: { if (txtBox != null) txtBox.Text = string.Empty; break; }
                    case HtmlInputText htmlInputText: { if (htmlInputText != null) htmlInputText.Value = string.Empty; break; }
                    case CheckBox chk: { if (chk != null) chk.Checked = false; break; }
                    default:
                        {

                            if (ctrl.Controls.Count > 0)
                            {
                                ClearTextBoxes(ctrl);
                            }
                            break;
                        }
                };


            }
        }
    }

}