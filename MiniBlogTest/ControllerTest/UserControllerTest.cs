using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Runtime.CompilerServices;
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
    public class UserControllerTest : TestBase
    {
        public UserControllerTest(CustomWebApplicationFactory<Startup> factory)
            : base(factory)

        {
        }

        [Fact]
        public async Task Should_get_all_users()
        {
            var client = GetClient();

            var response = await client.GetAsync("/user");

            response.EnsureSuccessStatusCode();
            var users = await DeserializeObject<List<User>>(response);
            Assert.Empty(users);
        }

        [Fact]
        public async Task Should_delete_user_and_related_article_success()
        {
            var client = GetClient();
            const string userName = "Tom";
            await PrepareArticle(new Article(userName, string.Empty, string.Empty), client);
            await PrepareArticle(new Article(userName, string.Empty, string.Empty), client);

            var articles = await GetArticles(client);
            Assert.Equal(2, articles.Count);

            var users = await GetUsers(client);
            Assert.Single(users);

            await client.DeleteAsync($"/user?name={userName}");

            var articlesAfterDeleteUser = await GetArticles(client);
            Assert.Empty(articlesAfterDeleteUser);

            var usersAfterDeleteUser = await GetUsers(client);
            Assert.Empty(usersAfterDeleteUser);
        }

        [Fact]
        public async Task Should_register_user_success()
        {
            var client = GetClient();
            var user = new User("Tom", "a@b.com");

            var registerResponse = await client.PostAsync("/user", SerializeToStringContent(user));

            Assert.Equal(HttpStatusCode.Created, registerResponse.StatusCode);
            var users = await GetUsers(client);
            Assert.Single(users);
            Assert.Equal("Tom", users[0].Name);
            Assert.Equal("a@b.com", users[0].Email);
        }

        [Fact]
        public async Task Should_register_user_fail_when_UserStore_unavailable()
        {
            var client = MockUserServiceThrowExceptionWhenAddUser();
            var user = new User("Tom", "a@b.com");

            var registerResponse = await client.PostAsync("/user", SerializeToStringContent(user));

            Assert.Equal(HttpStatusCode.InternalServerError, registerResponse.StatusCode);
        }

        [Fact]
        public async Task Should_update_user_email_success()
        {
            var client = GetClient();
            var originalUser = new User("Tom", "a@b.com");
            var updatedUser = new User("Tom", "tom@b.com");

            await client.PostAsync("/user", SerializeToStringContent(originalUser));
            await client.PutAsync("/user", SerializeToStringContent(updatedUser));

            var users = await GetUsers(client);
            Assert.Single(users);
            Assert.Equal("Tom", users[0].Name);
            Assert.Equal("tom@b.com", users[0].Email);
        }

        private HttpClient MockUserServiceThrowExceptionWhenAddUser()
        {
            var mockUserService = new Mock<UserService>();
            mockUserService.Setup(store => store.AddUser(It.IsAny<User>())).Throws<Exception>();
            var client = Factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddScoped((serviceProvider) => mockUserService.Object);
                });
            }).CreateClient();
            return client;
        }

        private static async Task<List<User>> GetUsers(HttpClient client)
        {
            var response = await client.GetAsync("/user");
            return await DeserializeObject<List<User>>(response);
        }

        private static async Task<List<Article>> GetArticles(HttpClient client)
        {
            var articleResponse = await client.GetAsync("/article");
            return await DeserializeObject<List<Article>>(articleResponse);
        }

        private static async Task PrepareArticle(Article article, HttpClient client)
        {
            await client.PostAsync("/article", SerializeToStringContent(article));
        }
    }
}