using System;
using System.Collections.Generic;
using MiniBlog.Model;

namespace MiniBlog.Stores
{
    public class ArticleStoreWillReplaceInFuture
    {
        public ArticleStoreWillReplaceInFuture()
        {
            Init();
        }

        public List<Article> Articles { get; private set; }

        public void Init()
        {
            Articles = new List<Article>();
            Articles.Add(new Article(null, "Happy new year", "Happy 2021 new year"));
            Articles.Add(new Article(null, "Happy Halloween", "Halloween is coming"));
        }
    }
}