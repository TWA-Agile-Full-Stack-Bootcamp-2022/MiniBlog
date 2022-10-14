using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniBlog.Model;
using MiniBlog.Stores;

namespace MiniBlog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArticleController : ControllerBase
    {
        [HttpGet]
        public List<Article> List()
        {
            return ArticleStoreWillReplaceInFuture.Articles.ToList();
        }

        [HttpPost]
        public Article Create(Article article)
        {
            if (article.UserName != null)
            {
                if (!UserStoreWillReplaceInFuture.Users.Exists(_ => article.UserName == _.Name))
                {
                    UserStoreWillReplaceInFuture.Users.Add(new User(article.UserName));
                }

                ArticleStoreWillReplaceInFuture.Articles.Add(article);
            }

            return article;
        }

        [HttpGet("{id}")]
        public Article GetById(Guid id)
        {
            var foundArticle = ArticleStoreWillReplaceInFuture.Articles.FirstOrDefault(article => article.Id == id);
            return foundArticle;
        }
    }
}