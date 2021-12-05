using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitchAPI.Models;

namespace TwitchAPI.ViewModels
{
    public class UserScope
    {
        public Scope Scope { get; set; }
        public bool IsSelected { get; set; }
    }

    public class UserScopes
    {
        public List<UserScope> ScopeList { get; set; }
    }
}
