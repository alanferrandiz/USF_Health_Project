using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Data;

static class Globals
{
    public static String connection;
    public static int currentUserId;
    public static String currentUserName;

    public static T Iif<T>(bool cond, T left, T right)
    {
        return cond ? left : right;
    }

}
