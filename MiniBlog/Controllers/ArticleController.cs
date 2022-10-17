using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniBlog.Model;
using MiniBlog.Service;
using MiniBlog.Stores;

namespace MiniBlog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArticleController : ControllerBase
    {
        private readonly ArticleService articleService;
        private readonly UserService userService;
        
        public ArticleController(ArticleService articleService, UserService userService)
        {
            this.articleService = articleService;
            this.userService = userService;
        }

        [HttpGet]
        public List<Article> List()
        {
            return articleService.GetArticles();
        }

        [HttpPost]
        public ActionResult<Article> Create(Article article)
        {
            if (article.UserName != null)
            {
                if (!userService.GetUsers().Exists(_ => article.UserName == _.Name))
                {
                    userService.AddUser(new User(article.UserName));
                }

                articleService.AddArticle(article);
            }

            return Created("/article", article);
        }

        [HttpGet("{id}")]
        public Article GetById(Guid id)
        {
            var foundArticle = articleService.GetArticles().FirstOrDefault(article => article.Id == id);
            return foundArticle;
        }
    }
}