using System;
using System.Globalization;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using System.Linq;
namespace Utilities
{
    public class TextHelper
    {
        public static string FindPhoneNumberVietNamFromText(string inputText)
        {
            var exp = new Regex(@"0(1\d{9}|9\d{8})", RegexOptions.IgnoreCase);

            var text = inputText.Replace(".", "").Replace(" ", "");

            var matchList = exp.Matches(text).Cast<Match>()
              .Select(m => m.Groups[0].Value)
              .ToArray();
            if (matchList.Length > 0)
            {
                return matchList[0];
            }
            else
            {
                return string.Empty;
            }
        }
        public static string RandomString(int length)
        {
            Random random = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            return new string(Enumerable.Repeat(chars, length)
      .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string RemapInternationalCharToAscii(char c)
        {
            var s = c.ToString().ToLowerInvariant();
            if ("àåáâäãåą".Contains(s))
                return "a";
            if ("èéêëę".Contains(s))
                return "e";
            if ("ìíîïı".Contains(s))
                return "i";
            if ("òóôõöøőð".Contains(s))
                return "o";
            if ("ùúûüŭů".Contains(s))
                return "u";
            if ("çćčĉ".Contains(s))
                return "c";
            if ("żźž".Contains(s))
                return "z";
            if ("śşšŝ".Contains(s))
                return "s";
            if ("ñń".Contains(s))
                return "n";
            if ("ýÿ".Contains(s))
                return "y";
            if ("ğĝ".Contains(s))
                return "g";
            if (c == 'ř')
                return "r";
            if (c == 'ł')
                return "l";
            if (c == 'đ')
                return "d";
            if (c == 'ß')
                return "ss";
            if (c == 'þ')
                return "th";
            if (c == 'ĥ')
                return "h";
            if (c == 'ĵ')
                return "j";
            return "";
        }

        public static string UrlFriendly(string text, int maxLength = 0)
        {
            // Return empty value if text is null
            if (text == null) return "";
            var normalizedString = text
                // Make lowercase
                .ToLowerInvariant()
                // Normalize the text
                .Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();
            var stringLength = normalizedString.Length;
            var prevdash = false;
            var trueLength = 0;
            char c;
            for (var i = 0; i < stringLength; i++)
            {
                c = normalizedString[i];
                switch (CharUnicodeInfo.GetUnicodeCategory(c))
                {
                    // Check if the character is a letter or a digit if the character is a
                    // international character remap it to an ascii valid character
                    case UnicodeCategory.LowercaseLetter:
                    case UnicodeCategory.UppercaseLetter:
                    case UnicodeCategory.DecimalDigitNumber:
                        if (c < 128)
                            stringBuilder.Append(c);
                        else
                            stringBuilder.Append(RemapInternationalCharToAscii(c));
                        prevdash = false;
                        trueLength = stringBuilder.Length;
                        break;
                    // Check if the character is to be replaced by a hyphen but only if the last character wasn't
                    case UnicodeCategory.SpaceSeparator:
                    case UnicodeCategory.ConnectorPunctuation:
                    case UnicodeCategory.DashPunctuation:
                    case UnicodeCategory.OtherPunctuation:
                    case UnicodeCategory.MathSymbol:
                        if (!prevdash)
                        {
                            stringBuilder.Append('-');
                            prevdash = true;
                            trueLength = stringBuilder.Length;
                        }

                        break;
                }

                // If we are at max length, stop parsing
                if (maxLength > 0 && trueLength >= maxLength)
                    break;
            }

            // Trim excess hyphens
            var result = stringBuilder.ToString().Trim('-');
            // Remove any excess character to meet maxlength criteria
            return maxLength <= 0 || result.Length <= maxLength ? result : result.Substring(0, maxLength);
        }

        public static string ConvertLongMoney(string text)
        {
            text = string.Format(CultureInfo.CreateSpecificCulture("vi-VN"), "{0:#,##}", decimal.Parse(text));
            return text;
        }

        public static string ConvertMoney(string text)
        {
            if (text.Length == 11)
                text = string.Format(CultureInfo.CreateSpecificCulture("vi-VN"), "{0:#,##}", float.Parse(text))
                    .Substring(0, 6);
            if (text.Length == 10)
                text = string.Format(CultureInfo.CreateSpecificCulture("vi-VN"), "{0:#,##}", float.Parse(text))
                    .Substring(0, 5);
            if (text.Length == 9)
                text = string.Format(CultureInfo.CreateSpecificCulture("vi-VN"), "{0:#,##}", float.Parse(text))
                    .Substring(0, 3);
            if (text.Length == 8)
                text = string.Format(CultureInfo.CreateSpecificCulture("vi-VN"), "{0:#,##}", float.Parse(text))
                    .Substring(0, 2);
            if (text.Length == 7)
                text = string.Format(CultureInfo.CreateSpecificCulture("vi-VN"), "{0:#,##}", float.Parse(text))
                    .Substring(0, 1);
            return text;
        }

        public static string ConvertText(string text)
        {
            text = text.Replace("'", "");
            text = text.Replace("(", "[");
            text = text.Replace(")", "]");
            text = text.Replace(" OR ", "");
            text = text.Replace(" AND ", "");
            text = text.Replace("--", "");
            text = text.Replace("drop", "");
            text = text.Replace("select", "");
            text = text.Replace("delete", "");
            text = text.Replace("infomation_schema", "");
            text = text.Replace("insert into", "");
            text = text.Replace("update set", "");
            text = text.Replace("from", "");
            text = text.Replace("union", "");
            text = text.Replace("xp_", "");
            text = text.Replace("where", "");
            text = text.Replace("field name", "");
            return text;
        }

        public static string ConvertToUnSign(string text)
        {
            if (text.Trim() != "" || text != null)
            {
                text = text.Replace("'", "");
                text = text.Replace(".", "");
                for (var i = 32; i < 48; i++) text = text.Replace(((char)i).ToString(), " ");
                text = text.Trim();
                text = text.Replace(" ", "-");
                text = text.Replace(",", "-");
                text = text.Replace(";", "-");
                text = text.Replace(":", "-");
                text = text.Replace("/", "-");
                text = text.Replace("%", "-");
                text = text.Replace("?", "");
                text = text.Replace("!", "");
                text = text.Replace("---", "-");
                text = text.Replace("--", "-");
                text = text.Replace("_", "-");
                var regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
                var strFormD = text.Normalize(NormalizationForm.FormD);
                return regex.Replace(strFormD, string.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
            }

            return "";
        }

        public static string ConvertUnSign(string text)
        {
            if (text.Trim() != "" || text != null)
            {
                var regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
                var strFormD = text.Normalize(NormalizationForm.FormD).Replace("", " ").Replace("&", "")
                    .Replace(".", "");
                return regex.Replace(strFormD, string.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
            }

            return "";
        }

        public static string DateText(string text)
        {
            var day = DateTime.Parse(text).ToString("ddd");
            var buoi = DateTime.Parse(text).ToString("tt");
            var ngay = System.Convert.ToInt32(DateTime.Parse(text).ToString("dd"));
            var thang = System.Convert.ToInt32(DateTime.Parse(text).ToString("MM"));
            var nam = System.Convert.ToInt32(DateTime.Parse(text).ToString("yyyy"));
            var gio = System.Convert.ToInt32(DateTime.Parse(text).ToString("hh"));
            var phut = System.Convert.ToInt32(DateTime.Parse(text).ToString("mm"));
            var buoix = DateTime.Now.ToString("tt");
            var ngayx = System.Convert.ToInt32(DateTime.Now.ToString("dd"));
            var thangx = System.Convert.ToInt32(DateTime.Now.ToString("MM"));
            var namx = System.Convert.ToInt32(DateTime.Now.ToString("yyyy"));
            var giox = System.Convert.ToInt32(DateTime.Now.ToString("hh"));
            var phutx = System.Convert.ToInt32(DateTime.Now.ToString("mm"));
            if (buoix == "PM") giox = giox + 12;
            if (buoi == "AM")
            {
                buoi = " sáng";
            }
            else
            {
                buoi = " chiều";
                gio = gio + 12;
            }

            if (nam == namx)
            {
                if (thang == thangx)
                {
                    if (ngay == ngayx)
                    {
                        if (gio == giox)
                            text = phutx - phut + " phút trước";
                        else
                            text = giox - gio + " giờ trước";
                    }
                    else
                    {
                        if (ngayx - ngay == 1)
                            text = "Hôm qua lúc " + gio + "h" + phut + buoi;
                        else if (ngayx - ngay == 2)
                            text = "Hôm kia lúc " + gio + "h" + phut + buoi;
                        else
                            text = ngayx - ngay + " ngày trước lúc " + gio + "h" + phut + buoi;
                    }
                }
                else if (thangx - thang == 1)
                {
                    text = "Tháng trước";
                }
                else if (thangx - thang == 2)
                {
                    text = "2 tháng trước";
                }
                else
                {
                    if (day == "Mon")
                        text = "Thứ hai " + DateTime.Parse(text).ToString("dd-MM-yyyy hh:mm") + buoi;
                    else if (day == "Tue")
                        text = "Thứ ba " + DateTime.Parse(text).ToString("dd-MM-yyyy hh:mm") + buoi;
                    else if (day == "Wes")
                        text = "Thứ tư " + DateTime.Parse(text).ToString("dd-MM-yyyy hh:mm") + buoi;
                    else if (day == "Thu")
                        text = "Thứ năm " + DateTime.Parse(text).ToString("dd-MM-yyyy hh:mm") + buoi;
                    else if (day == "Fri")
                        text = "Thứ sáu " + DateTime.Parse(text).ToString("dd-MM-yyyy hh:mm") + buoi;
                    else if (day == "Sat")
                        text = "Thứ bảy " + DateTime.Parse(text).ToString("dd-MM-yyyy hh:mm") + buoi;
                    else if (day == "Sun")
                        text = "Chủ nhật " + DateTime.Parse(text).ToString("dd-MM-yyyy hh:mm") + buoi;
                }
            }
            else if (namx - nam == 1)
            {
                text = DateTime.Parse(text).ToString("dd-MM-yyyy hh:mm") + buoi;
            }
            else
            {
                text = namx - nam + " năm trước";
            }

            return text;
        }

        public static string GetBuyCode(string num)
        {
            if (HttpContext.Current.Request.Cookies["cart"] != null)
                return HttpContext.Current.Request.Cookies["cart"].Values["buycode"];
            var val = "";
            var codes = "FANY" + num + "Y" + DateTime.Now.ToString("yyyy") + "M" + DateTime.Now.ToString("MM") + "D" +
                        DateTime.Now.ToString("dd") + "AT" + DateTime.Now.ToString("HHmmss");
            HttpContext.Current.Response.Cookies["cart"].Expires = DateTime.Now.AddDays(-1);
            var cookieLang = new HttpCookie("cart");
            cookieLang.Values.Add("buycode", codes);
            cookieLang.Expires = DateTime.Now.AddDays(1);
            HttpContext.Current.Response.SetCookie(cookieLang);
            return val;
        }

        public static string getImage_ChangeFileName(string savePath, string extraName, HttpPostedFile imgAdv)
        {
            var strFileName = "";
            try
            {
                var th = new Thumbnail();
                var rootPath = HttpContext.Current.Request.PhysicalApplicationPath + savePath;
                if (imgAdv != null && imgAdv.FileName.Length != 0) //Checking for valid file
                {
                    // Since the PostedFile.FileNameFileName gives the entire path we use Substring function to rip of the filename alone.
                    var strFileNameType = imgAdv.FileName.Substring(imgAdv.FileName.LastIndexOf(".") + 1);
                    strFileName = "ewebvietnam-hcm-" + extraName + "-" + DateTime.Now.Year + DateTime.Now.Month +
                                  DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second +
                                  "." + strFileNameType;
                    var intFileSize = imgAdv.ContentLength;
                    if (intFileSize <= 0 || intFileSize > 3145728) return strFileName;
                    imgAdv.SaveAs(rootPath + "\\" + strFileName);
                    return strFileName;
                }

                return strFileName;
            }
            catch (Exception ex)
            {
                
                return strFileName;
            }
        }

        public static string GetIpAddress()
        {
            var val = "";
            try
            {
                val = new WebClient().DownloadString("http://checkip.dyndns.org/");
                val = new Regex(@"\d{1,3}\.\d{1,3}.\d{1,3}.\d{1,3}").Matches(val)[0].ToString();
            }
            catch (Exception)
            {
                val = HttpContext.Current.Request.UserHostAddress;
            }

            return val;
        }

        public static string GetRoot()
        {
            return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
        }

        public static bool IsSpecialCharacters(string s)
        {
            var regexItem = new Regex("^[a-zA-Z0-9 ]*$");
            if (regexItem.IsMatch(s)) return false;
            return true;
        }

        public static void LoadMeta(HtmlHead headTag, string title, string description, string keyws, string category,
            string url, string img, string dateup, string datemf, string datecre)
        {
            headTag.Title = title;
            var pagemetaTag = new HtmlMeta
            {
                Name = "Description",
                Content = description
            };
            headTag.Controls.Add(pagemetaTag);
            pagemetaTag = new HtmlMeta
            {
                Name = "Keywords",
                Content = keyws
            };
            headTag.Controls.Add(pagemetaTag);
            pagemetaTag = new HtmlMeta();
            pagemetaTag.Attributes.Add("property", "og:url");
            pagemetaTag.Attributes.Add("itemprop", "url");
            pagemetaTag.Content = url;
            headTag.Controls.Add(pagemetaTag);
            pagemetaTag = new HtmlMeta();
            pagemetaTag.Attributes.Add("property", "og:image");
            pagemetaTag.Attributes.Add("itemprop", "thumbnailUrl");
            pagemetaTag.Content = img;
            headTag.Controls.Add(pagemetaTag);
            pagemetaTag = new HtmlMeta();
            pagemetaTag.Attributes.Add("property", "og:title");
            pagemetaTag.Attributes.Add("itemprop", "headline");
            pagemetaTag.Content = title;
            headTag.Controls.Add(pagemetaTag);
            pagemetaTag = new HtmlMeta();
            pagemetaTag.Attributes.Add("itemprop", "articleSection");
            pagemetaTag.Content = category;
            headTag.Controls.Add(pagemetaTag);
            pagemetaTag = new HtmlMeta();
            pagemetaTag.Attributes.Add("itemprop", "sourceOrganization");
            pagemetaTag.Attributes.Add("name", "source");
            pagemetaTag.Content = GetRoot();
            headTag.Controls.Add(pagemetaTag);
            pagemetaTag = new HtmlMeta();
            pagemetaTag.Attributes.Add("itemprop", "dateCreated");
            pagemetaTag.Content = DateTime.Parse(datecre).ToString("yyyy-MM-dd HH:mm + 07:00");
            headTag.Controls.Add(pagemetaTag);
            var editdate = datemf;
            if (editdate == "")
                editdate = datecre;
            pagemetaTag = new HtmlMeta();
            pagemetaTag.Attributes.Add("itemprop", "dateModified");
            pagemetaTag.Attributes.Add("name", "lastmod");
            pagemetaTag.Content = DateTime.Parse(editdate).ToString("yyyy-MM-dd HH:mm + 07:00");
            headTag.Controls.Add(pagemetaTag);
            var upddate = dateup;
            if (upddate == "")
                upddate = datecre;
            pagemetaTag = new HtmlMeta();
            pagemetaTag.Attributes.Add("itemprop", "datePublished");
            pagemetaTag.Attributes.Add("name", "pubdate");
            pagemetaTag.Content = DateTime.Parse(upddate).ToString("yyyy-MM-dd HH:mm + 07:00");
            headTag.Controls.Add(pagemetaTag);
            pagemetaTag = new HtmlMeta();
            pagemetaTag.Attributes.Add("property", "og:type");
            pagemetaTag.Content = "article";
            headTag.Controls.Add(pagemetaTag);
            pagemetaTag = new HtmlMeta();
            pagemetaTag.Attributes.Add("property", "og:site_name");
            pagemetaTag.Content = "Botyenmachtphcm.com";
            headTag.Controls.Add(pagemetaTag);
            pagemetaTag.Attributes.Add("property", "og:description");
            pagemetaTag.Attributes.Add("itemprop", "description");
            pagemetaTag.Content = description;
            headTag.Controls.Add(pagemetaTag);
        }

        public static void MoveSelectedValue(ListBox lbScr, ListBox lbDest)
        {
            foreach (ListItem listItem1 in lbScr.Items)
                if (listItem1.Selected)
                    lbDest.Items.Add(listItem1);
            for (var i = lbScr.Items.Count - 1; i >= 0; i--)
                if (lbScr.Items[i].Selected)
                    lbScr.Items.RemoveAt(i);
            foreach (ListItem listItem2 in lbDest.Items)
                if (listItem2.Selected)
                    listItem2.Selected = false;
        }

        public static string RandomNumber(int length)
        {
            var s = "";
            var random = new Random();
            for (var i = 1; i <= length; i++) s += random.Next(0, 9);
            return s;
        }

        public static string ShortDateText(string text)
        {
            var day = DateTime.Parse(text).ToString("ddd");
            var buoi = DateTime.Parse(text).ToString("tt");
            var ngay = System.Convert.ToInt32(DateTime.Parse(text).ToString("dd"));
            var thang = System.Convert.ToInt32(DateTime.Parse(text).ToString("MM"));
            var nam = System.Convert.ToInt32(DateTime.Parse(text).ToString("yyyy"));
            var gio = System.Convert.ToInt32(DateTime.Parse(text).ToString("hh"));
            var phut = System.Convert.ToInt32(DateTime.Parse(text).ToString("mm"));
            var buoix = DateTime.Now.ToString("tt");
            var ngayx = System.Convert.ToInt32(DateTime.Now.ToString("dd"));
            var thangx = System.Convert.ToInt32(DateTime.Now.ToString("MM"));
            var namx = System.Convert.ToInt32(DateTime.Now.ToString("yyyy"));
            var giox = System.Convert.ToInt32(DateTime.Now.ToString("hh"));
            var phutx = System.Convert.ToInt32(DateTime.Now.ToString("mm"));
            if (buoix == "PM") giox = giox + 12;
            if (buoi == "AM")
            {
                buoi = " sáng";
            }
            else
            {
                buoi = " chiều";
                gio = gio + 12;
            }

            if (nam == namx)
            {
                if (thang == thangx)
                {
                    if (ngay == ngayx)
                    {
                        if (gio == giox)
                            text = phutx - phut + " phút trước";
                        else
                            text = giox - gio + " giờ trước";
                    }
                    else
                    {
                        if (ngayx - ngay == 1)
                            text = "Hôm qua";
                        else if (ngayx - ngay == 2)
                            text = "Hôm kia";
                        else
                            text = ngayx - ngay + " ngày trước";
                    }
                }
                else if (thangx - thang == 1)
                {
                    text = "Tháng trước";
                }
                else if (thangx - thang == 2)
                {
                    text = "2 tháng trước";
                }
                else
                {
                    if (day == "Mon")
                        text = "T.hai " + DateTime.Parse(text).ToString("dd-MM-yyyy");
                    else if (day == "Tue")
                        text = "T.ba " + DateTime.Parse(text).ToString("dd-MM-yyyy");
                    else if (day == "Wes")
                        text = "T.tư " + DateTime.Parse(text).ToString("dd-MM-yyyy hh:mm");
                    else if (day == "Thu")
                        text = "T.năm " + DateTime.Parse(text).ToString("dd-MM-yyyy hh:mm");
                    else if (day == "Fri")
                        text = "T.sáu " + DateTime.Parse(text).ToString("dd-MM-yyyy hh:mm");
                    else if (day == "Sat")
                        text = "T.bảy " + DateTime.Parse(text).ToString("dd-MM-yyyy hh:mm");
                    else if (day == "Sun") text = "C.nhật " + DateTime.Parse(text).ToString("dd-MM-yyyy hh:mm");
                }
            }
            else if (namx - nam == 1)
            {
                text = DateTime.Parse(text).ToString("dd-MM-yyyy hh:mm") + buoi;
            }
            else
            {
                text = namx - nam + " năm trước";
            }

            return text;
        }

        public static string TrimText(string textTrim, int length)
        {
            if (!string.IsNullOrEmpty(textTrim))
                if (textTrim.Length > length)
                {
                    var arrWord = textTrim.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    var sb = new StringBuilder();
                    foreach (var word in arrWord)
                    {
                        if ((sb + word).Length > length)
                            return string.Format("{0} ...", sb.ToString().TrimEnd(' '));
                        sb.Append(word + " ");
                    }
                }

            return textTrim;
        }
    }
}