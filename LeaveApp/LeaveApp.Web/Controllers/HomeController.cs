using LeaveApp.Service.API;
using LeaveApp.Web.Models;
using Microsoft.Office.Interop.Word;
using System;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LeaveApp.Web.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(IApiService apiService)
        {
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase postedFile)
        {
            object documentFormat = 8;
            string randomName = DateTime.Now.Ticks.ToString();
            object htmlFilePath = Server.MapPath("~/Temp/") + randomName + ".htm";
            string directoryPath = Server.MapPath("~/Temp/") + randomName + "_files";
            object fileSavePath = Server.MapPath("~/Temp/") + Path.GetFileName(postedFile.FileName);

            //If Directory not present, create it.
            if (!Directory.Exists(Server.MapPath("~/Temp/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Temp/"));
            }

            //Upload the word document and save to Temp folder.
            postedFile.SaveAs(fileSavePath.ToString());

            //Open the word document in background.
            _Application applicationclass = new Application();
            applicationclass.Documents.Open(ref fileSavePath);
            applicationclass.Visible = false;
            Document document = applicationclass.ActiveDocument;

            //Save the word document as HTML file.
            document.SaveAs(ref htmlFilePath, ref documentFormat);

            //Close the word document.
            document.Close();

            //Read the saved Html File.
            string wordHTML = System.IO.File.ReadAllText(htmlFilePath.ToString());

            //Loop and replace the Image Path.
            foreach (Match match in Regex.Matches(wordHTML, "<v:imagedata.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase))
            {
                wordHTML = Regex.Replace(wordHTML, match.Groups[1].Value, "Temp/" + match.Groups[1].Value);
            }

            //Delete the Uploaded Word File.
            System.IO.File.Delete(fileSavePath.ToString());

            ViewBag.WordHtml = wordHTML;

            //var docPath = fileSavePath;
            //var app = new Microsoft.Office.Interop.Word.Application();

            ////MessageFilter.Register();

            //app.Visible = true;

            //var doc = app.Documents.Open(docPath);

            //doc.ShowGrammaticalErrors = false;
            //doc.ShowRevisions = false;
            //doc.ShowSpellingErrors = false;

            ////Opens the word document and fetch each page and converts to image
            //foreach (Microsoft.Office.Interop.Word.Window window in doc.Windows)
            //{
            //    foreach (Microsoft.Office.Interop.Word.Pane pane in window.Panes)
            //    {
            //        for (var i = 0; i <= pane.Pages.Count; i++)
            //        {
            //            var page = pane.Pages[i];
            //            var bits = page.EnhMetaFileBits;
            //            var target = fileSavePath;

            //            try
            //            {
            //                using (var ms = new MemoryStream((byte[])(bits)))
            //                {
            //                    var image = System.Drawing.Image.FromStream(ms);
            //                    var pngTarget = Path.ChangeExtension(target.ToString().Split('.')[0], "png");
            //                    image.Save(pngTarget, ImageFormat.Png);
            //                }
            //            }
            //            catch (System.Exception ex)
            //            { 

            //            }
            //        }
            //    }
            //}
            //doc.Close(Type.Missing, Type.Missing, Type.Missing);
            //app.Quit(Type.Missing, Type.Missing, Type.Missing);
            ////MessageFilter.Revoke();
            return View();
        }

        public ActionResult TestDoc()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Error()
        {
            ViewBag.Message = "AvidClan web.";

            return View();
        }
    }
}