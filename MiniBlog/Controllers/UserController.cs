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
            userService.AddUser(user);
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
            return userService.UpdateByUser(user);
        }

        [HttpDelete]
        public User Delete(string name)
        {
            articleService.RemoveArticlesByUserName(name);
            return userService.RemoveUser(name);
        }

        [HttpGet("{name}")]
        public User GetByName(string name)
        {
            return userService.GetUserByName(name);
        }
    }
}