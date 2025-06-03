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
using System.Windows.Shapes;

namespace Cuahangxemaynhom8
{
    public partial class Nhanvien : Form
    {
        public Nhanvien()
        {
            InitializeComponent();
        }
        private string currentAction = ""; // thêm, sửa, xóa

        private void Nhanvien_Load(object sender, EventArgs e)
        {
            Function.Connect();
            load_datagrid();
            LoadCV();
            ClearFields();
        }
        private void load_datagrid()
        {
            string sql = "Select * from tblNhanvien";
            DataTable dt = new DataTable();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sql, Function.conn);
            sqlDataAdapter.Fill(dt);
            dgvNV.DataSource = dt;
            dgvNV.Columns[0].HeaderText = "Mã nhân viên";
            dgvNV.Columns[1].HeaderText = "Tên nhân viên";
            dgvNV.Columns[2].HeaderText = "Giới tính";
            dgvNV.Columns[3].HeaderText = "Ngày sinh";
            dgvNV.Columns[4].HeaderText = "Số điện thoại";
            dgvNV.Columns[5].HeaderText = "Địa chỉ";
            dgvNV.Columns[6].HeaderText = "Mã công việc";
            // Ẩn cột mật khẩu
            dgvNV.Columns[7].Visible = false;

            //khóa thao tác thêm và sửa trực tiếp từ người dùng trên DataGridView
            dgvNV.AllowUserToAddRows = false;
            dgvNV.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void dgvNV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaNV.Text = dgvNV.CurrentRow.Cells[0].Value.ToString();     // Mã nhân viên
            txtTenNV.Text = dgvNV.CurrentRow.Cells[1].Value.ToString();
            // Giới tính
            string gioitinh = dgvNV.CurrentRow.Cells[2].Value.ToString();
            radnam.Checked = (gioitinh == "Nam");
            radnu.Checked = (gioitinh == "Nữ");

            // Ngày sinh
            if (DateTime.TryParse(dgvNV.CurrentRow.Cells[3].Value.ToString(), out DateTime ns))
                dtpNS.Value = ns;

            txtDT.Text = dgvNV.CurrentRow.Cells[4].Value.ToString();
            txtDiaChi.Text = dgvNV.CurrentRow.Cells[5].Value.ToString();

            cmbMaCV.SelectedItem = dgvNV.CurrentRow.Cells[6].Value.ToString(); ;                       
            
        }

        private void LoadCV()
        {
            string sql = "SELECT MaCV FROM tblCongviec";
            SqlCommand cmd = new SqlCommand(sql, Function.conn); // Function.conn là SqlConnection mở sẵn
            SqlDataReader reader = cmd.ExecuteReader();

            cmbMaCV.Items.Clear();

            while (reader.Read())
            {
                cmbMaCV.Items.Add(reader["MaCV"].ToString());
            }

            reader.Close();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tukhoa = txtTK.Text.Trim();
            if (string.IsNullOrEmpty(tukhoa))
            {
                MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm!");
                return;
            }

            string sql = @"SELECT * FROM tblNhanvien 
                   WHERE MaNV LIKE @tukhoa 
                      OR TenNV LIKE @tukhoa 
                      OR Dienthoai LIKE @tukhoa 
                      OR Ngaysinh LIKE @tukhoa 
                      OR Diachi LIKE @tukhoa 
                      OR Gioitinh LIKE @tukhoa 
                      OR MaCV LIKE @tukhoa";

            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sql, Function.conn))
            {
                cmd.Parameters.AddWithValue("@tukhoa", "%" + tukhoa + "%");
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }

            if (dt.Rows.Count > 0)
            {
                dgvNV.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Không tìm thấy dữ liệu phù hợp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void ClearFields()
        {
            txtMaNV.Text = "";
            txtTenNV.Text = "";
            radnam.Checked = false;
            radnu.Checked = false;
            dtpNS.Value = DateTime.Today;
            txtDT.Text = "";
            txtDiaChi.Text = "";
            cmbMaCV.SelectedIndex = -1;
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            ClearFields();
            currentAction = "add";
            MessageBox.Show("Nhập thông rồi bấm 'Lưu'", "Thông báo");
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaNV.Text))
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần sửa!");
                return;
            }
            currentAction = "edit";
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaNV.Text))
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần xóa!", "Thông báo");
                return;
            }

            currentAction = "delete";
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa nhân viên này?", "Xác nhận", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                btnLuu_Click(sender, e);
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string gioitinh = radnam.Checked ? "Nam" : "Nữ";

            if (string.IsNullOrWhiteSpace(txtMaNV.Text) || string.IsNullOrWhiteSpace(txtTenNV.Text))
            {
                MessageBox.Show("Mã và Tên không được để trống!", "Lỗi");
                return;
            }


            if (currentAction == "add")
            {
                SqlCommand checkMaCmd = new SqlCommand("SELECT COUNT(*) FROM tblNhanvien WHERE MaNV = @MaNV", Function.conn);
                checkMaCmd.Parameters.AddWithValue("@MaNV", txtMaNV.Text);
                int countMa = (int)checkMaCmd.ExecuteScalar();
                if (countMa > 0)
                {
                    MessageBox.Show("❌ Mã nhân viên đã tồn tại!", "Lỗi");
                    return;
                }

                string sql = @"INSERT INTO tblNhanvien (MaNV, TenNV, Gioitinh, Ngaysinh, Dienthoai, Diachi, MaCV, Matkhau) 
                   VALUES (@MaNV, @TenNV, @Gioitinh, @Ngaysinh, @Dienthoai, @Diachi, @MaCV, @Matkhau)";
                using (SqlCommand cmd = new SqlCommand(sql, Function.conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", txtMaNV.Text);
                    cmd.Parameters.AddWithValue("@TenNV", txtTenNV.Text);
                    cmd.Parameters.AddWithValue("@Gioitinh", gioitinh);
                    cmd.Parameters.AddWithValue("@Ngaysinh", dtpNS.Value);
                    cmd.Parameters.AddWithValue("@Dienthoai", txtDT.Text);
                    cmd.Parameters.AddWithValue("@Diachi", txtDiaChi.Text);
                    cmd.Parameters.AddWithValue("@MaCV", cmbMaCV.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@Matkhau", "123456");
                    cmd.ExecuteNonQuery();
                }
            }
            else if (currentAction == "edit")
            {
                string sql = @"UPDATE tblNhanvien SET TenNV=@TenNV, Gioitinh=@Gioitinh, Ngaysinh=@Ngaysinh, 
                       Dienthoai=@Dienthoai, Diachi=@Diachi, MaCV=@MaCV WHERE MaNV=@MaNV";
                using (SqlCommand cmd = new SqlCommand(sql, Function.conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", txtMaNV.Text);
                    cmd.Parameters.AddWithValue("@TenNV", txtTenNV.Text);
                    cmd.Parameters.AddWithValue("@Gioitinh", gioitinh);
                    cmd.Parameters.AddWithValue("@Ngaysinh", dtpNS.Value);
                    cmd.Parameters.AddWithValue("@Dienthoai", txtDT.Text);
                    cmd.Parameters.AddWithValue("@Diachi", txtDiaChi.Text);
                    cmd.Parameters.AddWithValue("@MaCV", cmbMaCV.SelectedItem.ToString());
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Sửa thành công!");
            }
            else if (currentAction == "delete")
            {
                // Gỡ liên kết từ các bảng phụ trước khi xóa (nếu có)
                string updateDonDatHang = "UPDATE tblDondathang SET MaNV = NULL WHERE MaNV = @MaNV";
                using (SqlCommand cmdUpdate = new SqlCommand(updateDonDatHang, Function.conn))
                {
                    cmdUpdate.Parameters.AddWithValue("@MaNV", txtMaNV.Text);
                    cmdUpdate.ExecuteNonQuery();
                }

                string updateHoadonnhap = "UPDATE tblHoadonnhap SET MaNV = NULL WHERE MaNV = @MaNV";
                using (SqlCommand cmdUpdate2 = new SqlCommand(updateHoadonnhap, Function.conn))
                {
                    cmdUpdate2.Parameters.AddWithValue("@MaNV", txtMaNV.Text);
                    cmdUpdate2.ExecuteNonQuery();
                }

                string sql = "DELETE FROM tblNhanvien WHERE MaNV = @MaNV";
                using (SqlCommand cmd = new SqlCommand(sql, Function.conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", txtMaNV.Text);
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

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
