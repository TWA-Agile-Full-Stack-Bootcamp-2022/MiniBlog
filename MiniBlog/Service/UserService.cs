using System.Collections.Generic;
using System.Linq;
using MiniBlog.Model;
using MiniBlog.Stores;

namespace MiniBlog.Service
{
    public class UserService
    {
        private readonly UserStoreWillReplaceInFuture userStore;

        public UserService(UserStoreWillReplaceInFuture userStore)
        {
            this.userStore = userStore;
        }

        public virtual void AddUser(User user)
        {
            if (!userStore.Users.Exists(_ => user.Name.ToLower() == _.Name.ToLower()))
            {
                userStore.Users.Add(user);
            }
        }

        public List<User> ListAll()
        {
            return userStore.Users;
        }

        public User Update(User user)
        {
            var foundUser = this.Get(user.Name);
            if (foundUser != null)
            {
                foundUser.Email = user.Email;
            }

            return foundUser;
        }

        public void Del(User user)
        {
            userStore.Users.Remove(user);
        }

        public User Get(string name)
        {
            return userStore.Users.FirstOrDefault(_ => _.Name.ToLower() == name.ToLower());
        }
    }
}