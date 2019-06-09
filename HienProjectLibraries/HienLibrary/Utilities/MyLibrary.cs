using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utilities;
using ShoesStore.MyExtensions;
namespace ShoesStore
{
    public partial class MyLibrary : Page
    {
        private static readonly string MoneyPrefix = "";
        private static readonly string proPath = "/images/products";
        private static readonly string proCatPath = "/images/categories";
        private static readonly string proDetUrl = "/san-pham/";
        private static readonly string slidePath = "/images/slides";
        private static readonly string cusPath = "/images/usrs/cus";
        private static readonly string merPath = "/images/usrs/mer";
        private static readonly string usrPath = "/images/usrs";
        private static readonly string _noImg = "/images/no_img.png";
        private static readonly string _noAvatar = "/images/avatar/no_img.jpg";
        private static readonly string adminPath = "/admin/images/avatar";

        public static SortDirection SortDirection
        {
            get
            {
                if (GetCurrentPageViewState()["SortDirection"] == null)
                    GetCurrentPageViewState()["SortDirection"] = SortDirection.Ascending;
                return (SortDirection)GetCurrentPageViewState()["SortDirection"];
            }
            set => GetCurrentPageViewState()["SortDirection"] = value;
        }

  
        public static StateBag GetCurrentPageViewState()
        {
            var page = HttpContext.Current.Handler as Page;
            var viewStateProp = page?.GetType().GetProperty("ViewState",
                BindingFlags.FlattenHierarchy |
                BindingFlags.Instance |
                BindingFlags.NonPublic);
            return (StateBag)viewStateProp?.GetValue(page);
        }

        public static string GetSortDirection()
        {
            string direction;
            if (SortDirection == SortDirection.Ascending)
            {
                SortDirection = SortDirection.Descending;
                direction = " DESC";
            }
            else
            {
                SortDirection = SortDirection.Ascending;
                direction = " ASC";
            }

            return direction;
        }

        /// <summary>
        ///     <c>Trả về</c> đường dẫn IMG của ProCat
        /// </summary>
        /// <param name="iProCat">Đối tượng thuộc lớp ProCat</param>
        /// <returns>Trả về đường dẫn IMG của ProCat</returns>
      

   

        private static string ReturnUrl(string[] iPath)
        {
            var path = string.Join(@"\", iPath);
            string fullFilePath = Path.Combine(
                HttpContext.Current.Server.MapPath("~")
                    .Substring(0, HttpContext.Current.Server.MapPath("~").Length - 1), path.Substring(1));
            if (File.Exists(fullFilePath))
                return path;

            return _noImg;
        }

        public static string SlidePath(object img)
        {
            return Path.Combine(slidePath, img.ToString()).Replace(@"\", @"/");
        }

        public static void Show(string message, string reUrl = "")
        {
            var cleanMessage = message.Replace("'", "\'");
            var page = HttpContext.Current.CurrentHandler as Page;
            var script = string.Format("alert('{0}');{1}", cleanMessage, reUrl != "" ? "window.location.href ='" + reUrl + "'" : "");
            if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
                page.ClientScript.RegisterClientScriptBlock(page.GetType(), "alert", script, true /* addScriptTags */);
        }
        public static void ShowInUploadPannel(string message, string reUrl = "")
        {
            var cleanMessage = message.Replace("'", "\'");
            string TransferPage = string.Format("<script>alert('{0}');{1}</script>", cleanMessage, reUrl != "" ? "window.location.href ='" + reUrl + "'" : "");
            Page currentPage = HttpContext.Current.Handler as Page;
            ScriptManager.RegisterStartupScript(currentPage, currentPage.GetType(), "temp2", TransferPage, false);
        }
        public static void ShowInUploadPannel(string message)
        {
            string TransferPage = $"<script>alert('{message}')</script>";
            Page currentPage = HttpContext.Current.Handler as Page;
            ScriptManager.RegisterStartupScript(currentPage, currentPage.GetType(), "temp", TransferPage, false);
        }
       public string PropertiesToString()
        {
            PropertyInfo[] _propertyInfos = null;

            if (_propertyInfos == null)
                _propertyInfos = this.GetType().GetProperties();

            var sb = new StringBuilder();

            foreach (var info in _propertyInfos)
            {
                var value = info.GetValue(this, null) ?? "(null)";
                sb.AppendLine(info.Name + ": " + value.ToString());
            }

            return sb.ToString();
        }
        public static Dictionary<string, string> GetProperties(object obj)
        {
            var props = new Dictionary<string, string>();
            if (obj == null)
                return props;

            var type = obj.GetType();
            foreach (var prop in type.GetProperties())
            {
                var val = prop.GetValue(obj, new object[] { });
                var valStr = val == null ? "" : val.ToString();
                props.Add(prop.Name, valStr);
            }

            return props;
        }
        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
        private static MemberExpression GetMemberInfo(Expression method)
        {
            LambdaExpression lambda = method as LambdaExpression;
            if (lambda == null)
                throw new ArgumentNullException("method");

            MemberExpression memberExpr = null;

            if (lambda.Body.NodeType == ExpressionType.Convert)
            {
                memberExpr =
                    ((UnaryExpression)lambda.Body).Operand as MemberExpression;
            }
            else if (lambda.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpr = lambda.Body as MemberExpression;
            }

            if (memberExpr == null)
                throw new ArgumentException("method");

            return memberExpr;
        }
        public static void ExposeProperty<T>(Expression<Func<T>> property)
        {
            var expression = GetMemberInfo(property);
            string path = string.Concat(expression.Member.DeclaringType.FullName,
                ".", expression.Member.Name);
            // Do ExposeProperty work here...
        }

        public static string GetPriceFormat(object orginPrice,object discountPrice)
        {
            return $"<span class='widget-products__new-price'>{double.Parse(discountPrice+"").ToFormatMoney()}</span> <span class='widget-products__old-price'>{orginPrice.ToFormatMoney()}</span>";
                                              
        }
    }
}