using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace mySQL_test
{
    class Program
    {
        static Dictionary<int, string> users = new Dictionary<int, string>();
        static List<string> usernames = new List<string>();
        static List<string> passwords = new List<string>();
        static Dictionary<string, string> lists = new Dictionary<string, string>();
        static int userid = 0;
        static int loop = 0;

        static void Main(string[] args)
        {
            mainProgram();
        }

        static void mainProgram()
        {
            userid = 0;
            var s = Connection.strProvider;
            string cs = $@"{s}";

            using var con = new MySqlConnection(cs);
            con.Open();
            var sql = "SELECT id, username, password1 FROM users";
            using var cmd = new MySqlCommand(sql, con);

            if (loop == 0)
            {
                using MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var converted = Convert.ToInt32(reader.GetValue("id"));
                    users.Add(converted, reader.GetValue("username").ToString());
                    passwords.Add(reader.GetValue("password1").ToString());
                }
                reader.Close();

                foreach (KeyValuePair<int, string> item in users)
                {
                    usernames.Add(item.Value);
                }
                loop = 1;
            }

            Console.WriteLine("\nDo you have an account? y/n");
            var input = Console.ReadLine();

            if (input == "y" || input == "Yes")
            {
                Console.WriteLine("Username:");
                var yesInput1 = Console.ReadLine();

                Console.WriteLine("Password:");
                var yesInput2 = Console.ReadLine();

                if (users.Values.Contains(yesInput1) && passwords.Contains(yesInput2))
                {

                    userid += users.FirstOrDefault(x => x.Value == yesInput1).Key;
                    Console.Clear();
                    Liste();
                }
                else
                {
                    Console.WriteLine("Error: The username or password you entered isn't correct or doesn't exist");
                    mainProgram();
                }
            }

            if (input == "n" || input == "no")
            {
                Console.WriteLine("Write a Username:");
                var noInput1 = Console.ReadLine();
                Console.WriteLine("Write a Password:");
                var noInput2 = Console.ReadLine();

                if (!usernames.Contains(noInput1) || !passwords.Contains(noInput2))
                {
                    cmd.CommandText = $"INSERT INTO users(username, password1) VALUES('{noInput1}', '{noInput2}')";
                    cmd.ExecuteNonQuery();

                    Console.WriteLine("\nAccount Created!");
                    users.Clear();
                    usernames.Clear();
                    passwords.Clear();
                    loop = 0;
                    mainProgram();
                }
                else
                {
                    Console.WriteLine("Error: Username or Password already exists");
                }

            }

            static void Liste()
            {
                Console.WriteLine("Hello!");
                var s = Connection.strProvider;
                string cs = $@"{s}";
                lists.Clear();

                Console.WriteLine("say 'add' to add something to your list \nsay 'remove' to delete something from your list \nsay 'view' to view your list");
                var listInput = Console.ReadLine();

                if (listInput == "view")
                {
                    using var viewCon = new MySqlConnection(cs);
                    viewCon.Open();

                    var viewSql = $"SELECT title, entry FROM lists WHERE user_id = {userid};";

                    using var viewCmd = new MySqlCommand(viewSql, viewCon);
                    using MySqlDataReader reader = viewCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        lists.Add(reader.GetValue("title").ToString(), reader.GetValue("entry").ToString());
                    }
                    reader.Close();

                    foreach(var item in lists)
                    {
                        Console.WriteLine(item);
                    }

                    lists.Clear();
                    Liste();
                }

                if (listInput == "add")
                {
                    using var addCon = new MySqlConnection(cs);
                    addCon.Open();

                    var addSql = $"SELECT * FROM lists;";
                    using var addCmd = new MySqlCommand(addSql, addCon);

                    Console.WriteLine("Add to list (entry name):");
                    var addList1 = Console.ReadLine();
                    Console.WriteLine("\nAdd to list (entry):");
                    var addList2 = Console.ReadLine();
                    addCmd.CommandText = $"INSERT INTO lists (user_id, title, entry) Values ('{userid}', '{addList1}', '{addList2}')";
                    addCmd.ExecuteNonQuery();
                    Console.WriteLine("\nAdded to list!");
                    Liste();
                }

                if (listInput == "remove")
                {

                }

                else
                {
                    Console.WriteLine("Invalid input!");
                    Liste();
                }
            }
        }
    }
}

