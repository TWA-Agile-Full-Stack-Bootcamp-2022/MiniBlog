using System;
using System.Collections.Generic;
using System.Linq;
using MiniBlog.Model;
using MiniBlog.Stores;

namespace MiniBlog.Service
{
    public class ArticleService 
    {
        private readonly ArticleStoreWillReplaceInFuture articleStore;
        private readonly UserStoreWillReplaceInFuture userStore;

        public ArticleService(ArticleStoreWillReplaceInFuture articleStore, UserStoreWillReplaceInFuture userStore)
        {
            this.articleStore = articleStore;
            this.userStore = userStore;
        }

        public List<Article> FindArticles()
        {
            return articleStore.Articles.ToList();
        }

        public virtual Article AddArticles(Article article)
        {
            if (!userStore.Users.Exists(_ => article.UserName == _.Name))
            {
                userStore.Users.Add(new User(article.UserName));
            }

            articleStore.Articles.Add(article);
            return article;
        }

        public Article GetById(Guid id)
        {
            return articleStore.Articles.FirstOrDefault(article => article.Id == id);
        }

        public void RemoveAllByUserName(User foundUser)
        {
            articleStore.Articles.RemoveAll(a => a.UserName == foundUser.Name);
        }
    }
}