using System.Collections.Generic;
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
    }
}