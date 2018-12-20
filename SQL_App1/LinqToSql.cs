using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL_App1
{
    public class LinqToSql : DataContext
    {
        public LinqToSql(string connectionString) : base(connectionString)
        {
            // this.dataSet = new DataSetLoader();
        }
        private static LinqTdsDataContext db = new LinqTdsDataContext();
        private static readonly string ConnString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;

        public DataSet ds = new DataSet();
        public SqlConnection connection = new SqlConnection(ConnString);
        public SqlCommand cmd;
        public SqlDataAdapter adapter;

        public void Init()
        {
            cmd = new SqlCommand("SELECT * FROM accounts", connection);
            adapter = new SqlDataAdapter(cmd);
            connection.Open();
            adapter.Fill(ds, "accounts");
            cmd = new SqlCommand("SELECT * FROM Keys", connection);
            adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds, "Keys");
            connection.Close();
            Console.WriteLine("DataSet initialised!\r\nPress any key to continue. . .");
            Console.ReadKey(true);
            LinqTdsDataContext context = new LinqTdsDataContext();
            //IEnumerable<accounts> accs = new

            var result = from accounts in ds.Tables["accounts"].AsEnumerable()
                select accounts;
            Console.WriteLine(result);
        }
    }
}
