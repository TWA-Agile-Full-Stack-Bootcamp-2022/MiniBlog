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
            if (!IsUserExists(user.Name))
            {
                userStore.Users.Add(user);
            }
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

        public User UpdateByUser(User user)
        {
            var foundUser = GetUserByName(user.Name);
            if (foundUser != null)
            {
                foundUser.Email = user.Email;
            }

            return foundUser;
        }

        public User RemoveUser(string name)
        {
            var foundUser = GetUserByName(name);
            if (foundUser != null)
            {
                userStore.Users.Remove(foundUser);
            }

            return foundUser;
        }

        private bool IsUserExists(string name)
        {
            return userStore.Users.Exists(user =>
                string.Equals(name, user.Name, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}