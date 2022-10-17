using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
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
            // given
            var client = MockArticleServiceWith2ArticlesExisting();
            // when
            var response = await client.GetAsync("/article");
            // then
            response.EnsureSuccessStatusCode();
            var allArticles = await DeserializeObject<List<Article>>(response);
            Assert.Equal(2, allArticles.Count);
        }

        [Fact]
        public async void Should_create_article_fail_when_ArticleStore_unavailable()
        {
            var client = MockArticleServiceThrowExceptionWhenAddArticle();

            var response = await client.PostAsync("/article",
                SerializeToStringContent(new Article("Tom", "Good day", "What a good day today!")));

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public async void Should_create_article_and_register_user_correct()
        {
            var article = new Article("Tom", "Good day", "What a good day today!");
            var client = GetClient();

            var createArticleResponse = await client.PostAsync("/article", SerializeToStringContent(article));
            Assert.Equal(HttpStatusCode.Created, createArticleResponse.StatusCode);

            var articleResponse = await client.GetAsync("/article");
            var articles = await DeserializeObject<List<Article>>(articleResponse);
            Assert.Single(articles);
            Assert.Equal("Tom", articles[0].UserName);
            Assert.Equal("Good day", articles[0].Title);
            Assert.Equal("What a good day today!", articles[0].Content);

            var userResponse = await client.GetAsync("/user");
            var users = await DeserializeObject<List<User>>(userResponse);
            Assert.Single(users);
            Assert.Equal("Tom", users[0].Name);
            Assert.Equal("anonymous@unknow.com", users[0].Email);
        }

        private HttpClient MockArticleServiceWith2ArticlesExisting()
        {
            var articles = new List<Article>
            {
                new Article(null, "Happy new year", "Happy 2021 new year"),
                new Article(null, "Happy Halloween", "Halloween is coming"),
            };
            var mockArticleService = new Mock<ArticleService>();
            mockArticleService.Setup(service => service.GetArticles()).Returns(articles);
            return GetHttpClientWithMock(mockArticleService);
        }

        private HttpClient MockArticleServiceThrowExceptionWhenAddArticle()
        {
            var mockArticleService = new Mock<ArticleService>();
            mockArticleService.Setup(store => store.AddArticle(It.IsAny<Article>())).Throws<Exception>();
            return GetHttpClientWithMock(mockArticleService);
        }

        private HttpClient GetHttpClientWithMock(Mock<ArticleService> mockArticleService)
        {
            var client = Factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddScoped((serviceProvider) => mockArticleService.Object);
                });
            }).CreateClient();
            return client;
        }

        private async Task<T> DeserializeObject<T>(HttpResponseMessage response)
        {
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }

        private StringContent SerializeToStringContent<T>(T obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, MediaTypeNames.Application.Json);
        }
    }
}