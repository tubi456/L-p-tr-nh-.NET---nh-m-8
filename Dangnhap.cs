using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cuahangxemaynhom8
{
    public partial class Dangnhap : Form
    {
        private bool isPasswordHidden = true;

        public Dangnhap()
        {
            InitializeComponent();
            txtMK.PasswordChar = '*';
            icondong.IconChar = FontAwesome.Sharp.IconChar.EyeSlash; // icon mắt đóng
     
        }
        public static string manv;
        public static string matkhau;
        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            manv = txtMaNV.Text.Trim();
            matkhau = txtMK.Text.Trim();

            if (string.IsNullOrEmpty(manv) || string.IsNullOrEmpty(matkhau))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo");
                return;
            }

            try
            {
                Function.Connect();

                string sql = "SELECT Matkhau, TenNV FROM tblNhanvien WHERE MaNV = @MaNV";
                SqlCommand cmd = new SqlCommand(sql, Function.conn);
                cmd.Parameters.AddWithValue("@MaNV", manv);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string matkhauTrongDB = reader["Matkhau"].ToString();
                        string tenNV = reader["TenNV"].ToString();

                        // Nếu mật khẩu lưu dưới dạng mã hóa/băm, bạn cần xử lý ở đây.
                        if (matkhau == matkhauTrongDB)
                        {
                            MessageBox.Show("Đăng nhập thành công!", "Thông báo");

                            reader.Close();
                            Function.Disconnect();

                            // Mở form giao diện chính
                            Trangchu trangchu = new Trangchu(tenNV);
                            this.Hide();
                            trangchu.ShowDialog();
                            this.Show();
                        }
                        else
                        {
                            MessageBox.Show("Mật khẩu không đúng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sai mã nhân viên!", "Lỗi đăng nhập");
                    }
                }

                Function.Disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đăng nhập: " + ex.Message);
                Function.Disconnect();
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void icondong_Click(object sender, EventArgs e)
        {
            if (isPasswordHidden)
            {
                txtMK.PasswordChar = '\0'; // Hiện mật khẩu
                icondong.IconChar = FontAwesome.Sharp.IconChar.Eye; // Đổi sang icon mắt mở
                isPasswordHidden = false;
            }
            else
            {
                txtMK.PasswordChar = '*'; // Ẩn mật khẩu
                icondong.IconChar = FontAwesome.Sharp.IconChar.EyeSlash; // Đổi sang icon mắt đóng
                isPasswordHidden = true;
            }
        }
    }
}
