using System.Collections.Generic;
using MiniBlog.Model;
using MiniBlog.Stores;

namespace MiniBlog.Service
{
    public class ArticleService
    {
        private readonly ArticleStoreWillReplaceInFuture articleStore;

        public ArticleService()
        {
        }

        public ArticleService(ArticleStoreWillReplaceInFuture articleStore)
        {
            this.articleStore = articleStore;
        }

        public virtual List<Article> GetArticles()
        {
            return articleStore.Articles;
        }

        public virtual void AddArticle(Article article)
        {
            articleStore.Articles.Add(article);
        }
    }
}