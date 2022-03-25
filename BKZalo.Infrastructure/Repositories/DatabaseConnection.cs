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
            get { return new MySqlConnection("Host=MYSQL5025.site4now.net ;Port=3306 ;Database=db_a845ec_hieunv ; User Id=a845ec_hieunv; Password=Vnvd8788"); }
        }
    }
}
