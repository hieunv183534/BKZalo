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
            get { return new MySqlConnection("Host=MYSQL5037.site4now.net ;Port=3306 ;Database=db_a7ca4c_bkzalo ; User Id=a7ca4c_bkzalo; Password=Vnvd8788"); }
        }
    }
}
