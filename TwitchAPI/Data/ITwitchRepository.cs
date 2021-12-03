using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwitchAPI.Data
{
    internal interface ITwitchRepository
    {
        bool SaveChanges();

        //IEnumerable<Command> GetAllCommands();

        //Command GetCommandById(int id);

        //void CreateCommand(Command cmd);

        //void UpdateCommand(Command cmd);

        //void DeleteCommand(Command cmd);
    }
}
