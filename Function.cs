using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuahangxemaynhom8
{
    internal class Function
    {
        public static SqlConnection conn;
        public static string connString;
        public static void Connect()
        {
            connString = "Data Source=DESKTOP-2405QGG\\SQLEXPRESS;Initial Catalog=QuanLyCuaHangBanXeMay;Integrated Security=True;Encrypt=True;Encrypt=False";
            conn = new SqlConnection(connString);
            conn.Open();
        }
        public static void Disconnect()
        {
            if (conn != null && conn.State == System.Data.ConnectionState.Open)
            {
                conn.Close();
            }
        }
        public static DataTable Getdatatotable(string sql)
        {
            // Gọi hàm Connect() để khởi tạo và mở kết nối
            Connect();

            SqlDataAdapter mydata = new SqlDataAdapter(sql, conn);
            DataTable table = new DataTable();
            mydata.Fill(table);
            return table;
        }
    }
}
