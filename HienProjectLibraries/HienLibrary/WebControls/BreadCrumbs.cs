/* ================================================================================================
 * BreadCrumbs.cs
 * 
 * This file contains the code that creates the breadcrumb navigation on my Web site.For an 
 * article describing this control's functionality, please see
 * http://www.codeproject.com/aspnet/breadcrumbs.asp.
 * 
 * This file is provided "as is" with no expressed or implied warranty.The author accepts no 
 * liability for any damage/loss of business that this product may cause.
 * 
 * Author:		Jon Sagara (jonsagara@hotmail.com)
 * History:
 *		1.0.0
 *			08/23/2003	- Released as an include file.
 * 
 *		2.0.0
 *			09/23/2003	- Converted to a custom control.
 * 
 * Compile: csc /out:..\bin\BreadCrumbs.dll /target:library BreadCrumbs.cs
 * ============================================================================================= */

using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ShoesStore
{
    namespace BreadCrumbs
    {
        public class BreadCrumbControl : WebControl
        {
            private readonly HybridDictionary labels = new HybridDictionary(); // Holds the "friendly" directory names.

// RootName."Friendly" name of the root directory.Default is "Home".
// RootUrl.URL of the root directory.Default is "/".
            private readonly StringBuilder
                sbBcUrl =
                    new StringBuilder(); // Holds the URL of the breadcrumb.Directories are appended in succession to the root.

            private readonly StringBuilder sbResult = new StringBuilder(); // Holds the Breadcrumb HTML.

// Separator.Contains the character(s) that separate each directory and/or file name
// in the breadcrumb HTML.Default is ">".
// PROPERTIES
// ShowFileName.Set to true if you want to append an extra separator character and
// the current file's name; false if not.Default is false.
/*
			 *	Constructor
			 *	Hook up the Control_Load event handler.
			 */
            public BreadCrumbControl()
            {
                Load += Control_Load;
//
// Initialize properties to default values.
//
                ShowFileName = false;
                Separator = ">";
                RootUrl = "/";
                RootName = "Trang chủ";
//
// Give the directories "friendly" names.Given the dynamic nature of directory structures, list them 
// alphabetically by key.
//
                labels.Add("subdir", "Sub Directory");
                labels.Add("subsubdir", "Sub Sub Directory");
            }

            public bool ShowFileName { get; set; }
            public string Separator { get; set; }
            public string RootUrl { get; set; }

            public string RootName { get; set; }

// EVENT HANDLERS
            protected override void Render(HtmlTextWriter output)
            {
                output.Write(sbResult + "<br>");
            }

/*
			 *	EventHandler: Control_Load
			 *	This event handler contains the meat of the control's functionality.
			 */
            protected void Control_Load(object sender, EventArgs e)
            {
//
// Create the root breadcrumb (corresponds to the root directory).
//
                sbResult.Append("<a href=\"" + RootUrl + "\">" + RootName + "</a>");
//
// Get the site URL.Use a StringBuilder to hold the URL so that we can append 
// directory names in succession.
//
                var strHostUrl = "http://" + Page.Request.ServerVariables["HTTP_HOST"] + "/";
                sbBcUrl.Append(strHostUrl);
//
// Break up the path parts into an array (directory name(s) and/or file name).
//
                var scriptName = Page.Request.ServerVariables["SCRIPT_NAME"];
                var strScriptDir = Path.GetDirectoryName(scriptName);
                var bHasExtension = Path.HasExtension(scriptName);
//
// Create breadcrumb HTML for the directory name(s).
// *** 	Note: Remove the first "\"; otherwise, when you split the string, the first item in the
// 		array is an empty string.
//
                strScriptDir = strScriptDir.Substring(1);
                var strDirs = strScriptDir.Split('\\');
                var nNumDirs = strDirs.Length;
//
// Splitting the string "\" (root directory) produces an array with an empty string as its only element.If there is
// only one element and it is the empty string, then we are in the root directory.If the user has chosen
// to display the file name, then the separator and the file name will be appended to the Web site root
// Name/URL.
//
// If, however, there is one element and it is NOT the empty string, then we are one directory deep.
// The else statement is executed and the separator character is inserted.
//
                var strSeparator = "";
                if (1 == nNumDirs && strDirs[0] == "")
                    strSeparator = "";
                else
                    strSeparator = string.Format(" {0} ", Separator);
                foreach (var strDirName in strDirs)
                {
                    sbResult.Append(strSeparator + "<a href=\"" + sbBcUrl + strDirName + "/\">" + labels[strDirName] +
                                    "</a>");
                    sbBcUrl.Append(strDirName + "/");
                }

//
// If the user wants to display file names, do it now.
//
                if (ShowFileName) sbResult.Append(string.Format(" {0} {1}", Separator, Path.GetFileName(scriptName)));
            }
        }
    }
}