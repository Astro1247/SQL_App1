using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SQL_App1
{
    class SqlManager
    {
        //private static string connString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Astro\\Source\\Repos\\SQL_App1\\SQL_App1\\testingbd.mdf;Integrated Security=True";
        private static readonly string ConnString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;

        public int RegisterUser(string username, string password)
        {
            //SqlConnection connection = new SqlConnection(connString);
            string sqlExpression = "RegisterUser";
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandType = CommandType.StoredProcedure;
                SqlParameter usernameParam = new SqlParameter
                {
                    ParameterName = "@Username",
                    Value = username
                };
                command.Parameters.Add(usernameParam);
                SqlParameter passParam = new SqlParameter
                {
                    ParameterName = "@Password",
                    Value = password
                };
                command.Parameters.Add(passParam);

                var result = command.ExecuteScalar();
                //var result = command.ExecuteNonQuery();

                //Console.WriteLine("Registered user ID: {0}", result);
                return Convert.ToInt32(result);
            }
        }


        public void printTable(string tableName)
        {

            string sqlExpression = "SELECT * FROM " + tableName;
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();


                    if (reader.HasRows)
                    {

                        int a1 = reader.FieldCount;
                        int a2 = 0;
                        while (a2 < a1)
                        {
                            Console.Write(reader.GetName(a2));
                            for (int ctr = 0; ctr < 20 - reader.GetName(a2).Length; ctr++)
                            {
                                Console.Write(" ");
                            }
                            a2++;
                        }
                        Console.WriteLine();

                        while (reader.Read())
                        {
                            a2 = 0;
                            while (a2 < a1)
                            {
                                Console.Write("{0,-20}", reader.GetValue(a2));
                                a2++;
                            }
                            Console.WriteLine();
                        }

                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }
            }
        }

        public void Transaction()
        {
            SqlConnection connection = new SqlConnection(ConnString);
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();
            SqlCommand cmd = connection.CreateCommand();
            cmd.Transaction = transaction;
            try
            {
                cmd.CommandText = "INSERT INTO accounts(username, password) VALUES(@Username1, @Password1)";
                cmd.Parameters.AddWithValue("@Username1", Core.GetInput("Username"));
                cmd.Parameters.AddWithValue("@Password1", Core.GetInput("Password"));
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO accounts(username, password) VALUES(@Username2, @Password2)";
                cmd.Parameters.AddWithValue("@Username2", Core.GetInput("Username"));
                cmd.Parameters.AddWithValue("@Password2", Core.GetInput("Password"));
                cmd.ExecuteNonQuery();

                transaction.Commit();
                Console.WriteLine("Users registered!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                transaction.Rollback();
            }
        }

        public void DataSet()
        {
            int i;
            SqlConnection connection = new SqlConnection(ConnString);
            connection.Open();

            SqlCommand cmd1 = new SqlCommand("SELECT * FROM accounts", connection);
            SqlDataAdapter adapter1 = new SqlDataAdapter(cmd1);

            SqlCommand cmd2 = new SqlCommand("SELECT * FROM Keys", connection);
            SqlDataAdapter adapter2 = new SqlDataAdapter(cmd2);
            DataSet data = new DataSet();
            adapter1.Fill(data, "accounts");
            adapter2.Fill(data, "Keys");

            data.Relations.Add("vs", data.Tables["accounts"].Columns["Id"], data.Tables["Keys"].Columns["ownerId"]);
            Console.Write("{0,-20}", "Id");
            Console.Write("{0,-20}", "Username");
            Console.WriteLine("{0,-20}", "Key ID");
            foreach (DataRow custRow in data.Tables["accounts"].Rows)
            {
                i = 0;
                Console.Write("{0,-20}", custRow["Id"]);
                Console.Write("{0,-20}", custRow["username"]);

                foreach (DataRow orderRow in custRow.GetChildRows("vs"))
                {
                    Console.WriteLine("{0,-20}", orderRow["Id"]);
                    i++;
                }
                if (i == 0) Console.WriteLine();
            }
            connection.Close();
        }

        public void GetUsers()
        {
            // название процедуры
            string sqlExpression = "GetUsers";
            // string connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                SqlCommand sqlCmd = new SqlCommand(sqlExpression, conn);
                // указываем, что команда представляет хранимую процедуру
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader reader = sqlCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    Console.WriteLine("{0}\t{1}\t{2}", reader.GetName(0), reader.GetName(1), reader.GetName(2));
                    while (reader.Read())
                    {
                        int ID = reader.GetInt32(0);
                        string username = reader.GetString(1);
                        string pass = reader.GetString(2);
                        Console.WriteLine("{0}\t{1}\t{2}", ID, username, pass);
                    }
                }
                reader.Close();
            }
        }
    }
}
