using Coronavirus_Information_v3.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coronavirus_Information_v3.Controllers
{
    public class AccueilController : Controller
    {
        // GET: AccueilController
        [HttpGet]
        public ActionResult Index()
        {
            ViewData["googleKey"] = Admin.GetKeyGoogle();
            return View();
        }

        [HttpGet]
        public ActionResult Actualite([FromQuery]Dictionary<string, string> data)
        {
            ViewData["nbpage"] = Article.GetNbPages(4);
            ViewData["pageNum"] = Convert.ToInt32(data["page"]);
            ViewData["articles"] = Article.GetArticles(Convert.ToInt32(data["page"]), 4);
            return View();
        }

        [HttpGet]
        public ActionResult SingleActualite([FromQuery] Dictionary<string, string> data)
        {
            ViewData["article"] = Article.GetArticleParId(Convert.ToInt32(data["idArticle"]));
            return View();
        }

        [Route("/robots.txt")]
        public ContentResult RobotsTxt()
        {
            var sb = new StringBuilder();
            sb.AppendLine("User-agent: *")
                .AppendLine("Allow: /* .css?*")
                .AppendLine("Allow: /* .js?*")
                .AppendLine("Disallow: /admin/")
                .AppendLine("Disallow: /admin/*")
                .Append("sitemap: ")
                .Append(this.Request.Scheme)
                .Append("://")
                .Append(this.Request.Host)
                .AppendLine("/sitemap.xml");

            return this.Content(sb.ToString(), "text/plain", Encoding.UTF8);
        }

        [Route("/sitemap.xml")]
        public ContentResult SitemapXml()
        {
            string xml = System.IO.File.ReadAllText("~/sitemap.xml");
            return Content(xml);
        }

        [Route("/.well-known/pki-validation/179C5AD63609E8D9DFB52279F546BA2A.txt")]
        public ContentResult SslCertificate()
        {
            string sllc = System.IO.File.ReadAllText("wwwroot/179C5AD63609E8D9DFB52279F546BA2A.txt");
            return Content(sllc);
        }
    }
}
