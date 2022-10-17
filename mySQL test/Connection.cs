using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace mySQL_test
{
    class Connection
    {
        static string host = "localhost";
        static string database = "databasetest";
        static string userDB = "root";
        static string password = "Doomslayer69420!";
        public static string strProvider = $@"server={host};userid={userDB};password={password};database={database}";
    }
}
