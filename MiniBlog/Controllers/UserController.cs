using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using MiniBlog.Model;
using MiniBlog.Service;
using MiniBlog.Stores;

namespace MiniBlog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private ArticleService articleService;

        private UserService userService;

        public UserController(ArticleService articleService, UserService userService)
        {
            this.articleService = articleService;
            this.userService = userService;
        }

        [HttpPost]
        public ActionResult<User> Register(User user)
        {
            if (!userService.GetUsers().Exists(_ => user.Name.ToLower() == _.Name.ToLower()))
            {
                userService.AddUser(user);
            }

            return Created("/user", user);
        }

        [HttpGet]
        public List<User> GetAll()
        {
            return userService.GetUsers();
        }

        [HttpPut]
        public User Update(User user)
        {
            var foundUser = userService.GetUsers().FirstOrDefault(_ => _.Name == user.Name);
            if (foundUser != null)
            {
                foundUser.Email = user.Email;
            }

            return foundUser;
        }

        [HttpDelete]
        public User Delete(string name)
        {
            var foundUser = userService.GetUsers().FirstOrDefault(_ => _.Name == name);
            if (foundUser != null)
            {
                userService.GetUsers().Remove(foundUser);
                articleService.GetArticles().RemoveAll(a => a.UserName == foundUser.Name);
            }

            return foundUser;
        }

        [HttpGet("{name}")]
        public User GetByName(string name)
        {
            return userService.GetUsers().FirstOrDefault(_ => _.Name.ToLower() == name.ToLower());
        }
    }
}