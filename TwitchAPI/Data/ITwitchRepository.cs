using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitchAPI.Models;

namespace TwitchAPI.Data
{
    public interface ITwitchRepository
    {
        bool SaveChanges();

        IEnumerable<User> GetAllUsers();

        User GetUserByUserId(int id);

        User GetUserByDBId(int id);

        void CreateUser(User user);

        void DeleteUser(User user);

        public App GetApp();

        void CreateApp(App app);

        void DeleteApp(App app);
    }
}
