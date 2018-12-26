using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace permission.service
{
    interface IPermissionService
    {
        string GetUserPermissionList(int page, int rows);
        string EditUserPermission(string json);
        string DeleteUserPermission(string username);
    }
}
