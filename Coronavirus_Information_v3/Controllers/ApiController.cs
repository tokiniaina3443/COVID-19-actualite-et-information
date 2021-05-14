using Coronavirus_Information_v3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Coronavirus_Information_v3.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ApiController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string GenerateJwtToken(Admin admin)
        {
            var securityKey = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]);

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, "0"),
                new Claim(ClaimTypes.Name, admin.Username)
            };

            var credentials = new SigningCredentials(new SymmetricSecurityKey(securityKey), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] Admin admin)
        {
            if (admin.Username.Equals("admin") && (admin.Password.Equals("123456")))
            {
                var token = this.GenerateJwtToken(admin);
                return Ok(new { status = "success", data = token });
            }
            else
            {
                return BadRequest("Invalid User");
            }
        }

        [HttpPost]
        [Authorize]
        [Route("AjouterArticle")]
        public IActionResult AjouterArticle([FromBody] Dictionary<string, string> data)
        {
            try
            {
                Base64 base64 = new Base64("illustration_" + Illustration.GetLastId(), data["base64"]);
                Illustration illustration = base64.ToIllustration();
                Article article = new Article(data["titre"], data["auteur"], DateTime.Parse(data["date"]), data["description"], illustration, data["objet"]);
                article.Save();
                return Ok(new { status = "success", data = "" });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("AvoirArticles")]
        public IActionResult AvoirArticles([FromQuery]Dictionary<string, string> query)
        {
            try
            {
                List<Article> articles = Article.GetArticles();
                return Ok(new { status = "success", data = articles });
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("EffacerArticle")]
        public IActionResult DeleteArticle([FromQuery] Dictionary<string, string> data)
        {
            if (Article.DeleteArticle(Int32.Parse(data["idArticle"])))
            {
                return Ok(new { status = "success", data = "" });
            }
            else
            {
                return Ok(new { status = "error", data = "erreur du serveur" });
            }
        }
    }
}
