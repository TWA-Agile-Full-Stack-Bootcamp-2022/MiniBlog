using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using MiniBlog;
using MiniBlog.Model;
using MiniBlog.Service;
using MiniBlog.Stores;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace MiniBlogTest.ControllerTest
{
    [Collection("IntegrationTest")]
    public class ArticleControllerTest : TestBase
    {
        public ArticleControllerTest(CustomWebApplicationFactory<Startup> factory)
            : base(factory)
        {
        }

        [Fact]
        public async void Should_get_all_Article()
        {
            var articles = new List<Article>
            {
                new Article(null, "Happy new year", "Happy 2021 new year"),
                new Article(null, "Happy Halloween", "Halloween is coming"),
            };
            var mockArticleService = new Mock<ArticleService>();
            mockArticleService.Setup(store => store.GetArticles()).Returns(articles);
            var client = Factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddScoped((serviceProvider) => mockArticleService.Object);
                });
            }).CreateClient();

            var response = await client.GetAsync("/article");
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<Article>>(body);
            Assert.Equal(2, users.Count);
        }

        [Fact]
        public async void Should_create_article_fail_when_ArticleStore_unavailable()
        {
            var mockArticleService = new Mock<ArticleService>();
            mockArticleService.Setup(store => store.AddArticle(It.IsAny<Article>())).Throws<Exception>();
            var client = Factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddScoped((serviceProvider) => mockArticleService.Object);
                });
            }).CreateClient();

            Article article = new Article("Tom", "Good day", "What a good day today!");
            var httpContent = JsonConvert.SerializeObject(article);
            StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);

            var response = await client.PostAsync("/article", content);
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public async void Should_create_article_and_register_user_correct()
        {
            var client = GetClient();
            string userNameWhoWillAdd = "Tom";
            string articleContent = "What a good day today!";
            string articleTitle = "Good day";
            Article article = new Article(userNameWhoWillAdd, articleTitle, articleContent);

            var httpContent = JsonConvert.SerializeObject(article);
            StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            var createArticleResponse = await client.PostAsync("/article", content);

            // It fail, please help
            Assert.Equal(HttpStatusCode.Created, createArticleResponse.StatusCode);

            var articleResponse = await client.GetAsync("/article");
            var body = await articleResponse.Content.ReadAsStringAsync();
            var articles = JsonConvert.DeserializeObject<List<Article>>(body);
            Assert.Equal(1, articles.Count);
            Assert.Equal(articleTitle, articles[0].Title);
            Assert.Equal(articleContent, articles[0].Content);
            Assert.Equal(userNameWhoWillAdd, articles[0].UserName);

            var userResponse = await client.GetAsync("/user");
            var usersJson = await userResponse.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<User>>(usersJson);

            Assert.Equal(1, users.Count);
            Assert.Equal(userNameWhoWillAdd, users[0].Name);
            Assert.Equal("anonymous@unknow.com", users[0].Email);
        }
    }
}