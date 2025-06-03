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
    public partial class Khachhang: Form
    {
        public Khachhang()
        {
            InitializeComponent();
        }
        private string currentAction = ""; // thêm, sửa, xóa

        private void Khachhang_Load(object sender, EventArgs e)
        {
            Function.Connect();
            load_datagrid();
            ClearFields();
        }
        private void load_datagrid()
        {
            string sql = "Select * from tblKhachhang";
            DataTable dt = new DataTable();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sql, Function.conn);
            sqlDataAdapter.Fill(dt);
            dgvKH.DataSource = dt;
            dgvKH.Columns[0].HeaderText = "Mã khách hàng";
            dgvKH.Columns[1].HeaderText = "Tên khách hàng";
            dgvKH.Columns[2].HeaderText = "Địa chỉ";
            dgvKH.Columns[3].HeaderText = "Số điện thoại";


            //khóa thao tác thêm và sửa trực tiếp từ người dùng trên DataGridView
            dgvKH.AllowUserToAddRows = false;
            dgvKH.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void dgvKH_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaKhach.Text = dgvKH.CurrentRow.Cells[0].Value.ToString();     // Mã nhân viên
            txtTenKhach.Text = dgvKH.CurrentRow.Cells[1].Value.ToString();
            txtDiaChi.Text = dgvKH.CurrentRow.Cells[2].Value.ToString();     // Mã nhân viên
            txtDT.Text = dgvKH.CurrentRow.Cells[3].Value.ToString();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tukhoa = txtTK.Text.Trim();
            if (string.IsNullOrEmpty(tukhoa))
            {
                MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm!");
                return;
            }

            string sql = @"SELECT * FROM tblKhachhang
                   WHERE Makhach LIKE @tukhoa 
                      OR Tenkhach LIKE @tukhoa 
                      OR Dienthoai LIKE @tukhoa 
                      OR Diachi LIKE @tukhoa";

            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sql, Function.conn))
            {
                cmd.Parameters.AddWithValue("@tukhoa", "%" + tukhoa + "%");
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }

            if (dt.Rows.Count > 0)
            {
                dgvKH.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Không tìm thấy dữ liệu phù hợp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void ClearFields()
        {
            txtMaKhach.Text = "";
            txtTenKhach.Text = "";
            txtDT.Text = "";
            txtDiaChi.Text = "";
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            ClearFields();
            currentAction = "add";
            MessageBox.Show("Nhập thông rồi bấm 'Lưu'", "Thông báo");
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaKhach.Text))
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần sửa!");
                return;
            }
            currentAction = "edit";
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaKhach.Text))
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa!", "Thông báo");
                return;
            }

            currentAction = "delete";
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa khách hàng này?", "Xác nhận", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                btnLuu_Click(sender, e);
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaKhach.Text) || string.IsNullOrWhiteSpace(txtTenKhach.Text))
            {
                MessageBox.Show("Mã và Tên không được để trống!", "Lỗi");
                return;
            }
            if (currentAction == "add")
            {
                SqlCommand checkMaCmd = new SqlCommand("SELECT COUNT(*) FROM tblKhachhang WHERE Makhach = @Makhach", Function.conn);
                checkMaCmd.Parameters.AddWithValue("@Makhach", txtMaKhach.Text);
                int countMa = (int)checkMaCmd.ExecuteScalar();
                if (countMa > 0)
                {
                    MessageBox.Show("❌ Mã khách đã tồn tại!", "Lỗi");
                    return;
                }

                string sql = @"INSERT INTO tblKhachhang (Makhach, Tenkhach, Diachi, Dienthoai) 
                   VALUES (@Makhach, @Tenkhach,  @Diachi, @Dienthoai)";
                using (SqlCommand cmd = new SqlCommand(sql, Function.conn))
                {
                    cmd.Parameters.AddWithValue("@Makhach", txtMaKhach.Text);
                    cmd.Parameters.AddWithValue("@Tenkhach", txtTenKhach.Text);
                    cmd.Parameters.AddWithValue("@Dienthoai", txtDT.Text);
                    cmd.Parameters.AddWithValue("@Diachi", txtDiaChi.Text);
                    cmd.ExecuteNonQuery();
                }
            }
            else if (currentAction == "edit")
            {
                string sql = @"UPDATE tblKhachhang SET Tenkhach=@Tenkhach, Diachi=@Diachi, Dienthoai=@Dienthoai  WHERE Makhach=@Makhach";
                using (SqlCommand cmd = new SqlCommand(sql, Function.conn))
                {
                    cmd.Parameters.AddWithValue("@Makhach", txtMaKhach.Text);
                    cmd.Parameters.AddWithValue("@Tenkhach", txtTenKhach.Text);
                    cmd.Parameters.AddWithValue("@Dienthoai", txtDT.Text);
                    cmd.Parameters.AddWithValue("@Diachi", txtDiaChi.Text);
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Sửa thành công!");
            }

            else if (currentAction == "delete")
            {
                // Gỡ liên kết từ các bảng phụ trước khi xóa (nếu có)
                string updateDonDatHang = "UPDATE tblDondathang SET Makhach = NULL WHERE Makhach = @Makhach";
                using (SqlCommand cmdUpdate = new SqlCommand(updateDonDatHang, Function.conn))
                {
                    cmdUpdate.Parameters.AddWithValue("@Makhach", txtMaKhach.Text);
                    cmdUpdate.ExecuteNonQuery();
                }


                string sql = "DELETE FROM tblKhachhang WHERE Makhach = @Makhach";
                using (SqlCommand cmd = new SqlCommand(sql, Function.conn))
                {
                    cmd.Parameters.AddWithValue("@Makhach", txtMaKhach.Text);
                    cmd.ExecuteNonQuery();
                }
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn hành động Thêm, Sửa hoặc Xóa!", "Thông báo");
                return;
            }
            // Sau khi lưu xong
            load_datagrid();
            ClearFields();
            currentAction = "";
        }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
            ClearFields();
            // Làm mới lại DataGridView
            load_datagrid();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
