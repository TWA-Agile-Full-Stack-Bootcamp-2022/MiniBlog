using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Server;
using MiniBlog.Model;

namespace MiniBlog.Stores
{
    public class UserStoreWillReplaceInFuture
    {
        public List<User> Users { get; private set; } = new List<User>();
    }
}