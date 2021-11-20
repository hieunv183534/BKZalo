using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKZalo.Infrastructure.Repositories
{
    public class DatabaseConnection
    {
        /// <summary>
        /// Khởi tạo đối tương connector
        /// </summary>
        public static IDbConnection DbConnection
        {
            get { return new MySqlConnection("Host=sql6.freemysqlhosting.net ;Port=3306 ;Database=sql6451652 ; User Id=sql6451652; Password=5NfGF8z8Kr"); }
        }
    }
}
