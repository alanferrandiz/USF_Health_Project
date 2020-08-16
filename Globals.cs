using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

static class Globals
{
    public static String connection;

    public static T Iif<T>(bool cond, T left, T right)
    {
        return cond ? left : right;
    }

}
