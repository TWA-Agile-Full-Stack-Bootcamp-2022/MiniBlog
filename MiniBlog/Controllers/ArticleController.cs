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
            return articleService.FindArticles();
        }

        [HttpPost]
        public ActionResult<Article> Create(Article article)
        {
            var articleAdded = articleService.AddArticles(article);
            userService.AddUser(new User(article.UserName));
            return Created("/article", articleAdded);
        }

        [HttpGet("{id}")]
        public Article GetById(Guid id)
        {
            var foundArticle = articleService.GetById(id);
            return foundArticle;
        }
    }
}