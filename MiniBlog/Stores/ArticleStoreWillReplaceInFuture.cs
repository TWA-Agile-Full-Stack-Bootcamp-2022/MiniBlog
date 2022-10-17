using System;
using System.Collections.Generic;
using MiniBlog.Model;

namespace MiniBlog.Stores
{
    public class ArticleStoreWillReplaceInFuture
    {
        public List<Article> Articles { get; private set; } = new List<Article>();
    }
}