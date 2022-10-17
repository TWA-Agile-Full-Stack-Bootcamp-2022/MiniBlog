using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MiniBlog.Model;
using MiniBlog.Service;

namespace MiniBlog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService userService;
        private readonly ArticleService articleService;

        public UserController(UserService userService, ArticleService articleService)
        {
            this.userService = userService;
            this.articleService = articleService;
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
            return userService.ListAll();
        }

        [HttpPut]
        public User Update(User user)
        {
            var foundUser = userService.Update(user);

            return foundUser;
        }

        [HttpDelete]
        public User Delete(string name)
        {
            var foundUser = userService.Get(name);
            if (foundUser != null)
            {
                userService.Del(foundUser);
                articleService.RemoveAllByUserName(foundUser);
            }

            return foundUser;
        }

        [HttpGet("{name}")]
        public User GetByName(string name)
        {
            return userService.Get(name);
        }
    }
}