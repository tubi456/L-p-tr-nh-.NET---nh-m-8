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
    public partial class TimkiemHDB : Form
    {
        public TimkiemHDB()
        {
            InitializeComponent();
        }

        private void TimkiemHDB_Load(object sender, EventArgs e)
        {
            Function.Connect();
            load_datagrid();
        }
        private void load_datagrid()
        {
            string sql = "Select SoDDH, n.MaNV, TenNV, k.MaKhach, TenKhach, Ngaymua, Thue, Datcoc, Tongtien FROM tblDondathang d JOIN tblNhanvien n ON d.MaNV = n.MaNV JOIN tblKhachhang k ON d.MaKhach = k.MaKhach";
            DataTable data = new DataTable();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sql, Function.conn);
            sqlDataAdapter.Fill(data);
            dgvHDB.DataSource = data;
            dgvHDB.Columns[0].HeaderText = "Số đơn đặt hàng";
            dgvHDB.Columns[1].HeaderText = "Mã nhân viên";
            dgvHDB.Columns[2].HeaderText = "Tên nhân viên";
            dgvHDB.Columns[3].HeaderText = "Mã khách";
            dgvHDB.Columns[4].HeaderText = "Tên khách";
            dgvHDB.Columns[5].HeaderText = "Ngày mua";
            dgvHDB.Columns[6].HeaderText = "Thuế";
            dgvHDB.Columns[7].HeaderText = "Đặt cọc";
            dgvHDB.Columns[8].HeaderText = "Tổng tiền";

            //khóa thao tác thêm và sửa trực tiếp từ người dùng trên DataGridView
            dgvHDB.AllowUserToAddRows = false;
            dgvHDB.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void dgvHDB_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtSoDDH.Text = dgvHDB.CurrentRow.Cells[0].Value.ToString();
            txtMaNV.Text = dgvHDB.CurrentRow.Cells[1].Value.ToString();
            txtTenNV.Text = dgvHDB.CurrentRow.Cells[2].Value.ToString();
            txtMaKH.Text = dgvHDB.CurrentRow.Cells[3].Value.ToString();
            txtTenKH.Text = dgvHDB.CurrentRow.Cells[4].Value.ToString();
            if (DateTime.TryParse(dgvHDB.CurrentRow.Cells[5].Value.ToString(), out DateTime ngayMua))
                dtpNgayMua.Value = ngayMua;           
            txtThue.Text = dgvHDB.CurrentRow.Cells[6].Value.ToString();
            txtDatCoc.Text = dgvHDB.CurrentRow.Cells[7].Value.ToString();
            txtTongTien.Text = dgvHDB.CurrentRow.Cells[8].Value.ToString();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tukhoa = txtTK.Text.Trim();
            if (string.IsNullOrEmpty(tukhoa))
            {
                MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm!");
                return;
            }
            string sql = "Select SoDDH, n.MaNV, TenNV, k.MaKhach, TenKhach, Ngaymua, Thue, Datcoc, Tongtien FROM tblDondathang d JOIN tblNhanvien n ON d.MaNV = n.MaNV JOIN tblKhachhang k ON d.MaKhach = k.MaKhach Where SoDDH LIKE @tukhoa OR n.MaNV LIKE @tukhoa OR k.MaKhach LIKE @tukhoa";
            DataTable data = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sql, Function.conn))
            {
                cmd.Parameters.AddWithValue("@tukhoa", "%" + tukhoa + "%");

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(data);
            }

            if (data.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy kết quả nào.");
            }

            dgvHDB.DataSource = data;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            // Xóa ô tìm kiếm
            txtTK.Clear();

            // Xóa các textbox thông tin
            txtSoDDH.Clear();
            txtMaNV.Clear();
            txtTenNV.Clear();
            txtMaKH.Clear();
            txtTenKH.Clear();
            txtThue.Clear();
            txtDatCoc.Clear();
            txtTongTien.Clear();

            // Reset DateTimePicker về ngày hiện tại
            dtpNgayMua.Value = DateTime.Now;

            // Load lại toàn bộ dữ liệu vào DataGridView
            load_datagrid();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn thoát?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }

      
    }
}
