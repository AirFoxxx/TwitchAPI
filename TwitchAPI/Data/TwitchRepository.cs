using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitchAPI.Models;

namespace TwitchAPI.Data
{
    public class TwitchRepository : ITwitchRepository
    {
        private readonly TwitchContext _context;

        public TwitchRepository(TwitchContext context)
        {
            _context = context;
        }

        public void CreateApp(App app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            _context.Apps.Add(app);
        }

        public void CreateUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            _context.Users.Add(user);
        }

        public void DeleteApp(App app)
        {
            _context.Apps.Remove(app);
        }

        public void DeleteUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            _context.Users.Remove(user);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public App GetApp()
        {
            return _context.Apps.FirstOrDefault();
        }

        public User GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(user => user.Id == id);
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
