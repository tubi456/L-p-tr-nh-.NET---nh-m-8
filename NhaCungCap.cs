using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cuahangxemaynhom8
{
    public partial class Nhacungcap : Form
    {
        public Nhacungcap()
        {
            InitializeComponent();
        }
        private string currentAction = ""; // thêm, sửa, xóa

        private void Nhacungcap_Load(object sender, EventArgs e)
        {
            Function.Connect();
            load_datagrid();
            ClearFields();
        }
        private void load_datagrid()
        {
            string sql = "Select * from tblNhacungcap";
            DataTable dt = new DataTable();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sql, Function.conn);
            sqlDataAdapter.Fill(dt);
            dgvNCC.DataSource = dt;
            dgvNCC.Columns[0].HeaderText = "Mã nhà cung cấp";
            dgvNCC.Columns[1].HeaderText = "Tên nhà cung cấp";
            dgvNCC.Columns[2].HeaderText = "Địa chỉ";
            dgvNCC.Columns[3].HeaderText = "Số điện thoại";
            

            //khóa thao tác thêm và sửa trực tiếp từ người dùng trên DataGridView
            dgvNCC.AllowUserToAddRows = false;
            dgvNCC.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void dgvNCC_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaNCC.Text = dgvNCC.CurrentRow.Cells[0].Value.ToString();     // Mã nhân viên
            txtTenNCC.Text = dgvNCC.CurrentRow.Cells[1].Value.ToString();
            txtDiaChi.Text = dgvNCC.CurrentRow.Cells[2].Value.ToString();     // Mã nhân viên
            txtDT.Text = dgvNCC.CurrentRow.Cells[3].Value.ToString();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tukhoa = txtTK.Text.Trim();
            if (string.IsNullOrEmpty(tukhoa))
            {
                MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm!");
                return;
            }

            string sql = @"SELECT * FROM tblNhacungcap
                   WHERE MaNCC LIKE @tukhoa 
                      OR TenNCC LIKE @tukhoa 
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
                dgvNCC.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Không tìm thấy dữ liệu phù hợp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void ClearFields()
        {
            txtMaNCC.Text = "";
            txtTenNCC.Text = "";
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
            if (string.IsNullOrEmpty(txtMaNCC.Text))
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần sửa!");
                return;
            }
            currentAction = "edit";
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaNCC.Text))
            {
                MessageBox.Show("Vui lòng chọn nhà cung cấp cần xóa!", "Thông báo");
                return;
            }

            currentAction = "delete";
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa nhà cung cấp này?", "Xác nhận", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                btnLuu_Click(sender, e);
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaNCC.Text) || string.IsNullOrWhiteSpace(txtTenNCC.Text))
            {
                MessageBox.Show("Mã và Tên không được để trống!", "Lỗi");
                return;
            }

            if (currentAction == "add")
            {
                SqlCommand checkMaCmd = new SqlCommand("SELECT COUNT(*) FROM tblNhacungcap WHERE MaNCC = @MaNCC", Function.conn);
                checkMaCmd.Parameters.AddWithValue("@MaNCC", txtMaNCC.Text);
                int countMa = (int)checkMaCmd.ExecuteScalar();
                if (countMa > 0)
                {
                    MessageBox.Show("❌ Mã nhà cung cấp đã tồn tại!", "Lỗi");
                    return;
                }

                string sql = @"INSERT INTO tblNhacungcap (MaNCC, TenNCC, Diachi, Dienthoai) 
                   VALUES (@MaNCC, @TenNCC,  @Diachi, @Dienthoai)";
                using (SqlCommand cmd = new SqlCommand(sql, Function.conn))
                {
                    cmd.Parameters.AddWithValue("@MaNCC", txtMaNCC.Text);
                    cmd.Parameters.AddWithValue("@TenNCC", txtTenNCC.Text);
                    cmd.Parameters.AddWithValue("@Dienthoai", txtDT.Text);
                    cmd.Parameters.AddWithValue("@Diachi", txtDiaChi.Text);
                    cmd.ExecuteNonQuery();
                }
            }

            else if (currentAction == "edit")
            {
                string sql = @"UPDATE tblNhacungcap SET TenNCC=@TenNCC, Diachi=@Diachi, Dienthoai=@Dienthoai  WHERE MaNCC=@MaNCC";
                using (SqlCommand cmd = new SqlCommand(sql, Function.conn))
                {
                    cmd.Parameters.AddWithValue("@MaNCC", txtMaNCC.Text);
                    cmd.Parameters.AddWithValue("@TenNCC", txtTenNCC.Text);
                    cmd.Parameters.AddWithValue("@Dienthoai", txtDT.Text);
                    cmd.Parameters.AddWithValue("@Diachi", txtDiaChi.Text);
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Sửa thành công!");
            }

            else if (currentAction == "delete")
            {
                // Gỡ liên kết từ các bảng phụ trước khi xóa (nếu có)
                string updateHoaDonNhap = "UPDATE tblHoadonnhap SET MaNCC = NULL WHERE MaNCC = @MaNCC";
                using (SqlCommand cmdUpdate = new SqlCommand(updateHoaDonNhap, Function.conn))
                {
                    cmdUpdate.Parameters.AddWithValue("@MaNCC", txtMaNCC.Text);
                    cmdUpdate.ExecuteNonQuery();
                }


                string sql = "DELETE FROM tblNhacungcap WHERE MaNCC = @MaNCC";
                using (SqlCommand cmd = new SqlCommand(sql, Function.conn))
                {
                    cmd.Parameters.AddWithValue("@MaNCC", txtMaNCC.Text);
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

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
