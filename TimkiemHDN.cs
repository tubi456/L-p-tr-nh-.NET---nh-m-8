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
    public partial class TimkiemHDN : Form
    {
        public TimkiemHDN()
        {
            InitializeComponent();
        }

        private void TimkiemHDN_Load(object sender, EventArgs e)
        {
            Function.Connect();
            load_datagrid();
        }
        private void load_datagrid()
        {
            string sql = "Select SoHDN, Ngaynhap, n.MaNV, TenNV, c.MaNCC, TenNCC, Tongtien FROM tblHoadonnhap h JOIN tblNhanvien n ON h.MaNV = n.MaNV JOIN tblNhacungcap c ON h.MaNCC = c.MaNCC";
            DataTable data = new DataTable();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sql, Function.conn);
            sqlDataAdapter.Fill(data);
            dgvHDN.DataSource = data;
            dgvHDN.Columns[0].HeaderText = "Số hóa đơn nhập";
            dgvHDN.Columns[1].HeaderText = "Ngày nhập";
            dgvHDN.Columns[2].HeaderText = "Mã nhân viên";
            dgvHDN.Columns[3].HeaderText = "Tên nhân viên";
            dgvHDN.Columns[4].HeaderText = "Mã NCC";
            dgvHDN.Columns[5].HeaderText = "Tên NCC";
            dgvHDN.Columns[6].HeaderText = "Tổng tiền";

            //khóa thao tác thêm và sửa trực tiếp từ người dùng trên DataGridView
            dgvHDN.AllowUserToAddRows = false;
            dgvHDN.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void dgvHDN_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtSoHDN.Text = dgvHDN.CurrentRow.Cells[0].Value.ToString();
            if (DateTime.TryParse(dgvHDN.CurrentRow.Cells[1].Value.ToString(), out DateTime ngayNhap))           
            dtpNgayNhap.Value = ngayNhap;
            
            //dtpNgayNhap.Text = dgvHDN.CurrentRow.Cells[1].Value.ToString();
            txtMaNV.Text = dgvHDN.CurrentRow.Cells[2].Value.ToString();
            txtTenNV.Text = dgvHDN.CurrentRow.Cells[3].Value.ToString();
            txtMaNCC.Text = dgvHDN.CurrentRow.Cells[4].Value.ToString();
            txtTenNCC.Text = dgvHDN.CurrentRow.Cells[5].Value.ToString();
            txtTongTien.Text = dgvHDN.CurrentRow.Cells[6].Value.ToString();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tukhoa = txtTK.Text.Trim();
            if (string.IsNullOrEmpty(tukhoa))
            {
                MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm!");
                return;
            }
            string sql = "Select SoHDN, Ngaynhap, n.MaNV, TenNV, c.MaNCC, TenNCC, Tongtien FROM tblHoadonnhap h JOIN tblNhanvien n ON h.MaNV = n.MaNV JOIN tblNhacungcap c ON h.MaNCC = c.MaNCC WHERE SoHDN LIKE @tukhoa OR n.MaNV LIKE @tukhoa OR c.MaNCC LIKE @tukhoa";
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

            dgvHDN.DataSource = data;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            // Xóa ô tìm kiếm
            txtTK.Clear();

            // Xóa các textbox thông tin
            txtSoHDN.Clear();
            txtMaNV.Clear();
            txtTenNV.Clear();
            txtMaNCC.Clear();
            txtTenNCC.Clear();
            txtTongTien.Clear();

            // Reset DateTimePicker về ngày hiện tại
            dtpNgayNhap.Value = DateTime.Now;

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
