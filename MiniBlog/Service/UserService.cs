using System;
using System.Collections.Generic;
using System.Linq;
using MiniBlog.Model;
using MiniBlog.Stores;

namespace MiniBlog.Service
{
    public class UserService
    {
        private readonly UserStoreWillReplaceInFuture userStore;

        public UserService()
        {
        }

        public UserService(UserStoreWillReplaceInFuture userStore)
        {
            this.userStore = userStore;
        }

        public virtual void AddUser(User user)
        {
            userStore.Users.Add(user);
        }

        public virtual List<User> GetUsers()
        {
            return userStore.Users;
        }

        public User GetUserByName(string name)
        {
            return userStore.Users.FirstOrDefault(usr =>
                string.Equals(usr.Name, name, StringComparison.CurrentCultureIgnoreCase));
        }

        public bool IsUserExists(string name)
        {
            return userStore.Users.Exists(user =>
                string.Equals(name, user.Name, StringComparison.CurrentCultureIgnoreCase));
        }

        public void RemoveUser(User foundUser)
        {
            userStore.Users.Remove(foundUser);
        }
    }
}