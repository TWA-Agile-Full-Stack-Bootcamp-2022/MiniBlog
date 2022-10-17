using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Server;
using MiniBlog.Model;

namespace MiniBlog.Stores
{
    public class UserStoreWillReplaceInFuture
    {
        public UserStoreWillReplaceInFuture()
        {
            Users = new List<User>();
        }

        public List<User> Users { get; private set; }

        /// <summary>
        /// This is for test only, please help resolve!
        /// </summary>
        public void Init()
        {
            Users = new List<User>();
        }
    }
}