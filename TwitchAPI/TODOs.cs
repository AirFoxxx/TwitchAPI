using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwitchAPI
{
    // TODO: Create database access and save objects App:{App Token, Refresh token, Client secret, Client id}
    // User {UserID, User access token, refresh token, Fkey-SCOPES, ... }
    // Scopes {Enum of scopes}

    // TODO: Abstract away the Client Factory, send it to the New abstraction nd make it Revalidate users and the app?

    // TODO: Separate stuff into a new controller and create a path through the GUI and rough gui frame.

    // TODO: Maybe connect together UserToken and RevalidatedUserToken if it works

    // TODO: Create a socket service for user authentication.

    // TODO: Create a socket service for callbacks.

    //TODO: Put strings into config file and reference them from there.
    //TODO: Fix userInfo for single user
    //TODO: Change names of homecontroller
}
