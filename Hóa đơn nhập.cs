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
    public partial class Hoadonnhap : Form
    {
        public Hoadonnhap()
        {
            InitializeComponent();
        }
        private string currentAction = ""; // thêm, sửa, xóa

        private void Hoadonnhap_Load(object sender, EventArgs e)
        {
            Function.Connect();
            load_datagrid();
            LoadThongTinHoaDon();
        }
        private void load_datagrid()
        {

            string sql1 = "Select Mahang, Soluong, Dongia, Giamgia, Thanhtien from tblChitietHDN";
            DataTable data = new DataTable();
            SqlDataAdapter sqlDataAdapter2 = new SqlDataAdapter(sql1, Function.conn);
            sqlDataAdapter2.Fill(data);
            dgvCTHDN.DataSource = data;
            dgvCTHDN.Columns[0].HeaderText = "Mã hàng";
            dgvCTHDN.Columns[1].HeaderText = "Số lượng";
            dgvCTHDN.Columns[2].HeaderText = "Đơn giá nhập";
            dgvCTHDN.Columns[3].HeaderText = "Giảm giá";
            dgvCTHDN.Columns[4].HeaderText = "Thành tiền";

            //khóa thao tác thêm và sửa trực tiếp từ người dùng trên DataGridView
            dgvCTHDN.AllowUserToAddRows = false;
            dgvCTHDN.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void dgvCTHDN_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaHang.Text = dgvCTHDN.CurrentRow.Cells[0].Value.ToString();     // Mã nhân viên
            txtSoLuong.Text = dgvCTHDN.CurrentRow.Cells[1].Value.ToString();
            txtDonGia.Text = dgvCTHDN.CurrentRow.Cells[2].Value.ToString();
            txtGiamGia.Text = dgvCTHDN.CurrentRow.Cells[3].Value.ToString();
            txtThanhTien.Text = dgvCTHDN.CurrentRow.Cells[4].Value.ToString();
        }
        private void LoadThongTinHoaDon()
        {
            string soHDN = txtSoHDN.Text;

            string sql = "SELECT SoHDN, MaNV, MaNCC, Ngaynhap, Tongtien FROM tblHoadonnhap WHERE SoHDN = @SoHDN";
            using (SqlCommand cmd = new SqlCommand(sql, Function.conn))
            {
                cmd.Parameters.AddWithValue("@SoHDN", soHDN);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        txtSoHDN.Text = reader["SoHDN"].ToString();
                        txtMaNV.Text = reader["MaNV"].ToString();
                        txtMaNCC.Text = reader["MaNCC"].ToString();

                        if (DateTime.TryParse(reader["Ngaynhap"].ToString(), out DateTime nn))
                            dtpNgayNhap.Value = nn;

                        txtTongTien.Text = reader["Tongtien"].ToString();
                    }
                }
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tukhoa = txtTK.Text.Trim();
            if (string.IsNullOrEmpty(tukhoa))
            {
                MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm!");
                return;
            }

            string sql = @"SELECT c.SoHDN, MaNV, Ngaynhap, MaNCC, Tongtien, Mahang, Soluong, Dongia, Giamgia, Thanhtien 
                   FROM tblHoadonnhap h
                   JOIN tblChiTietHDN c ON h.SoHDN = c.SoHDN
                   WHERE h.SoHDN LIKE @tukhoa";

            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sql, Function.conn))
            {
                cmd.Parameters.AddWithValue("@tukhoa", "%" + tukhoa + "%");
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }

            if (dt.Rows.Count > 0)
            {
                // Lấy thông tin hóa đơn (
                DataRow row = dt.Rows[0];
                txtSoHDN.Text = row["SoHDN"].ToString();
                txtMaNV.Text = row["MaNV"].ToString();
                txtMaNCC.Text = row["MaNCC"].ToString();
                if (DateTime.TryParse(row["Ngaynhap"].ToString(), out DateTime ngaynhap))
                    dtpNgayNhap.Value = ngaynhap;
                txtTongTien.Text = row["Tongtien"].ToString();

                // Tạo DataTable chỉ chứa chi tiết cần hiển thị trên DataGridView
                DataTable dtChiTiet = dt.DefaultView.ToTable(false, "Mahang", "Soluong", "Dongia", "Giamgia", "Thanhtien");

                dgvCTHDN.DataSource = dtChiTiet;

                // Hiển thị dòng đầu tiên vào các textbox chi tiết (nếu cần)
                txtMaHang.Text = dt.Rows[0]["Mahang"].ToString();
                txtSoLuong.Text = dt.Rows[0]["Soluong"].ToString();
                txtDonGia.Text = dt.Rows[0]["Dongia"].ToString();
                txtGiamGia.Text = dt.Rows[0]["Giamgia"].ToString();
                txtThanhTien.Text = dt.Rows[0]["Thanhtien"].ToString();
            }
            else
            {
                MessageBox.Show("Không tìm thấy dữ liệu phù hợp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            ClearFields();
            currentAction = "add";
            MessageBox.Show("Nhập thông rồi bấm 'Lưu'", "Thông báo");
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaHang.Text))
            {
                MessageBox.Show("❗ Vui lòng chọn chi tiết hóa đơn cần sửa!", "Thông báo");
                return;
            }
            currentAction = "edit";
            MessageBox.Show("🔔 Chỉnh sửa thông tin rồi bấm 'Lưu'", "Thông báo");

        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSoHDN.Text) || string.IsNullOrEmpty(txtMaNV.Text) ||
        string.IsNullOrEmpty(txtMaNCC.Text) || string.IsNullOrEmpty(txtMaHang.Text))
            {
                MessageBox.Show("❌ Số HĐN, Mã NV, Mã NCC, Mã hàng không được để trống!", "Lỗi");
                return;
            }

            try
            {
                decimal soLuong = decimal.TryParse(txtSoLuong.Text.Replace(",", ""), out var sl) ? sl : 0;
                decimal giamGia = decimal.TryParse(txtGiamGia.Text.Replace(",", ""), out var gg) ? gg : 0;
                decimal donGiaNhap = 0;

                // Kiểm tra mã nhân viên
                string sqlCheckNV = "SELECT COUNT(*) FROM tblNhanvien WHERE MaNV = @MaNV";
                using (SqlCommand cmdNV = new SqlCommand(sqlCheckNV, Function.conn))
                {
                    cmdNV.Parameters.AddWithValue("@MaNV", txtMaNV.Text);
                    int existsNV = (int)cmdNV.ExecuteScalar();
                    if (existsNV == 0)
                    {
                        MessageBox.Show("❌ Mã nhân viên không tồn tại!", "Lỗi");
                        return;
                    }
                }

                // Kiểm tra mã nhà cung cấp
                string sqlCheckNCC = "SELECT COUNT(*) FROM tblNhacungcap WHERE MaNCC = @MaNCC";
                using (SqlCommand cmdNCC = new SqlCommand(sqlCheckNCC, Function.conn))
                {
                    cmdNCC.Parameters.AddWithValue("@MaNCC", txtMaNCC.Text);
                    int existsNCC = (int)cmdNCC.ExecuteScalar();
                    if (existsNCC == 0)
                    {
                        MessageBox.Show("❌ Mã nhà cung cấp không tồn tại!", "Lỗi");
                        return;
                    }
                }

                // Kiểm tra mã hàng phải có trong DMHang
                string sqlCheckHang = "SELECT Dongianhap FROM tblDMHang WHERE Mahang = @Mahang";
                using (SqlCommand cmdCheckHang = new SqlCommand(sqlCheckHang, Function.conn))
                {
                    cmdCheckHang.Parameters.AddWithValue("@Mahang", txtMaHang.Text);
                    object result = cmdCheckHang.ExecuteScalar();
                    if (result == null)
                    {
                        MessageBox.Show("❌ Mã hàng không tồn tại trong danh mục!", "Lỗi");
                        return;
                    }

                    donGiaNhap = Convert.ToDecimal(result);
                    if (donGiaNhap == 0)
                    {
                        donGiaNhap = decimal.TryParse(txtDonGia.Text.Replace(",", ""), out var inputGia) ? inputGia : 0;
                        if (donGiaNhap == 0)
                        {
                            MessageBox.Show("❌ Đơn giá nhập chưa có, vui lòng nhập!", "Lỗi");
                            return;
                        }

                        // Cập nhật đơn giá nhập mới vào DMHang
                        string sqlUpdateGia = "UPDATE tblDMHang SET Dongianhap = @Dongianhap, Dongiaban = @Dongiaban WHERE Mahang = @Mahang";
                        using (SqlCommand cmdUpdateGia = new SqlCommand(sqlUpdateGia, Function.conn))
                        {
                            cmdUpdateGia.Parameters.AddWithValue("@Dongianhap", donGiaNhap);
                            cmdUpdateGia.Parameters.AddWithValue("@Dongiaban", donGiaNhap * 1.1m); 
                            cmdUpdateGia.Parameters.AddWithValue("@Mahang", txtMaHang.Text);
                            cmdUpdateGia.ExecuteNonQuery();
                        }
                    }
                }

                decimal thanhTien = (soLuong * donGiaNhap) - giamGia;



                if (currentAction == "add")
                {
                    // Kiểm tra hóa đơn đã tồn tại chưa
                    string sqlCheckHDN = "SELECT COUNT(*) FROM tblHoadonnhap WHERE SoHDN = @SoHDN";
                    using (SqlCommand cmdCheck = new SqlCommand(sqlCheckHDN, Function.conn))
                    {
                        cmdCheck.Parameters.AddWithValue("@SoHDN", txtSoHDN.Text);
                        int count = (int)cmdCheck.ExecuteScalar();
                        if (count == 0)
                        {
                            string sqlInsert = @"INSERT INTO tblHoadonnhap (SoHDN, MaNV, Ngaynhap, MaNCC, Tongtien)
                                 VALUES (@SoHDN, @MaNV, @Ngaynhap, @MaNCC, 0)";
                            using (SqlCommand cmdInsert = new SqlCommand(sqlInsert, Function.conn))
                            {
                                cmdInsert.Parameters.AddWithValue("@SoHDN", txtSoHDN.Text);
                                cmdInsert.Parameters.AddWithValue("@MaNV", txtMaNV.Text);
                                cmdInsert.Parameters.AddWithValue("@Ngaynhap", dtpNgayNhap.Value);
                                cmdInsert.Parameters.AddWithValue("@MaNCC", txtMaNCC.Text);
                                cmdInsert.ExecuteNonQuery();
                            }
                        }
                    }

                    // Thêm chi tiết
                    string sqlInsertCT = @"INSERT INTO tblChiTietHDN (SoHDN, Mahang, Soluong, Dongia, Giamgia, Thanhtien)
                                   VALUES (@SoHDN, @Mahang, @Soluong, @Dongia, @Giamgia, @Thanhtien)";
                    using (SqlCommand cmdInsertCT = new SqlCommand(sqlInsertCT, Function.conn))
                    {
                        cmdInsertCT.Parameters.AddWithValue("@SoHDN", txtSoHDN.Text);
                        cmdInsertCT.Parameters.AddWithValue("@Mahang", txtMaHang.Text);
                        cmdInsertCT.Parameters.AddWithValue("@Soluong", soLuong);
                        cmdInsertCT.Parameters.AddWithValue("@Dongia", donGiaNhap);
                        cmdInsertCT.Parameters.AddWithValue("@Giamgia", giamGia);
                        cmdInsertCT.Parameters.AddWithValue("@Thanhtien", thanhTien);
                        cmdInsertCT.ExecuteNonQuery();
                    }

                    // Cộng kho
                    string updateKho = "UPDATE tblDMHang SET Soluong = Soluong + @Soluong WHERE Mahang = @Mahang";
                    using (SqlCommand cmdUpdate = new SqlCommand(updateKho, Function.conn))
                    {
                        cmdUpdate.Parameters.AddWithValue("@Soluong", soLuong);
                        cmdUpdate.Parameters.AddWithValue("@Mahang", txtMaHang.Text);
                        cmdUpdate.ExecuteNonQuery();
                    }

                    MessageBox.Show("✅ Đã thêm chi tiết hóa đơn!");
                }
                else if (currentAction == "edit")
                {
                    // Lấy số lượng cũ
                    int oldSoluong = 0;
                    string getOldSql = "SELECT Soluong FROM tblChiTietHDN WHERE SoHDN = @SoHDN AND Mahang = @Mahang";
                    using (SqlCommand cmdGetOld = new SqlCommand(getOldSql, Function.conn))
                    {
                        cmdGetOld.Parameters.AddWithValue("@SoHDN", txtSoHDN.Text);
                        cmdGetOld.Parameters.AddWithValue("@Mahang", txtMaHang.Text);
                        object result = cmdGetOld.ExecuteScalar();
                        if (result != null)
                            oldSoluong = Convert.ToInt32(result);
                    }

                    // Tính chênh lệch
                    int delta = (int)soLuong - oldSoluong;

                    // Update chi tiết
                    string sqlUpdate = @"UPDATE tblChiTietHDN 
                                 SET Soluong = @Soluong, Dongia = @Dongia, Giamgia = @Giamgia, Thanhtien = @Thanhtien
                                 WHERE SoHDN = @SoHDN AND Mahang = @Mahang";
                    using (SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, Function.conn))
                    {
                        cmdUpdate.Parameters.AddWithValue("@Soluong", soLuong);
                        cmdUpdate.Parameters.AddWithValue("@Dongia", donGiaNhap);
                        cmdUpdate.Parameters.AddWithValue("@Giamgia", giamGia);
                        cmdUpdate.Parameters.AddWithValue("@Thanhtien", thanhTien);
                        cmdUpdate.Parameters.AddWithValue("@SoHDN", txtSoHDN.Text);
                        cmdUpdate.Parameters.AddWithValue("@Mahang", txtMaHang.Text);
                        cmdUpdate.ExecuteNonQuery();
                    }

                    // Cập nhật kho
                    string updateKho = "UPDATE tblDMHang SET Soluong = Soluong + @Delta WHERE Mahang = @Mahang";
                    using (SqlCommand cmdUpdateKho = new SqlCommand(updateKho, Function.conn))
                    {
                        cmdUpdateKho.Parameters.AddWithValue("@Delta", delta);
                        cmdUpdateKho.Parameters.AddWithValue("@Mahang", txtMaHang.Text);
                        cmdUpdateKho.ExecuteNonQuery();
                    }

                    MessageBox.Show("✅ Đã sửa chi tiết hóa đơn!");
                }

                UpdateTongTien();
                load_datagrid();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi: {ex.Message}", "Lỗi");
            }
        }
        private void UpdateTongTien()
        {
            string sql = "SELECT SUM(Thanhtien) FROM tblChiTietHDN WHERE SoHDN = @SoHDN";
            using (SqlCommand cmd = new SqlCommand(sql, Function.conn))
            {
                cmd.Parameters.AddWithValue("@SoHDN", txtSoHDN.Text);
                object result = cmd.ExecuteScalar();
                decimal tongTien = result != DBNull.Value ? Convert.ToDecimal(result) : 0;

                string sqlUpdate = "UPDATE tblHoadonnhap SET Tongtien = @Tongtien WHERE SoHDN = @SoHDN";
                using (SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, Function.conn))
                {
                    cmdUpdate.Parameters.AddWithValue("@Tongtien", tongTien);
                    cmdUpdate.Parameters.AddWithValue("@SoHDN", txtSoHDN.Text);
                    cmdUpdate.ExecuteNonQuery();
                }

                txtTongTien.Text = tongTien.ToString("N0");
            }
        }
        private void ClearFields()
        {
            // Xóa phần thông tin chung
            txtSoHDN.Clear();
            txtMaNV.Clear();
            txtMaNCC.Clear();
            dtpNgayNhap.Value = DateTime.Now;
            txtTongTien.Clear();

            // Xóa phần chi tiết hóa đơn
            txtMaHang.Clear();
            txtSoLuong.Clear();
            txtDonGia.Clear();
            txtGiamGia.Clear();
            txtThanhTien.Clear();

            // Nếu có ô tìm kiếm, xóa luôn
            txtTK.Clear();

            // Nếu có bảng dữ liệu, clear luôn (nếu cần)
            if (dgvCTHDN.DataSource != null)
            {
                dgvCTHDN.DataSource = null;
            }
        }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
            ClearFields();
            // Làm mới lại DataGridView
            load_datagrid();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn thoát?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSoHDN.Text))
            {
                MessageBox.Show("Vui lòng chọn đơn nhập hàng để in");
                return;
            }

            // Giả lập in hóa đơn – bạn có thể thay bằng xuất PDF hoặc in thực tế
            MessageBox.Show("In hóa đơn: " + txtSoHDN.Text, "In Hóa Đơn", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void guna2Button9_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSoHDN.Text) || string.IsNullOrEmpty(txtMaHang.Text))
            {
                MessageBox.Show("❌ Vui lòng chọn chi tiết hóa đơn cần xóa!", "Lỗi");
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa chi tiết này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    // Giảm số lượng trong kho
                    string getSoluong = "SELECT Soluong FROM tblChiTietHDN WHERE SoHDN = @SoHDN AND Mahang = @Mahang";
                    int soluong = 0;
                    using (SqlCommand cmdGet = new SqlCommand(getSoluong, Function.conn))
                    {
                        cmdGet.Parameters.AddWithValue("@SoHDN", txtSoHDN.Text);
                        cmdGet.Parameters.AddWithValue("@Mahang", txtMaHang.Text);
                        object result = cmdGet.ExecuteScalar();
                        soluong = result != null ? Convert.ToInt32(result) : 0;
                    }

                    string updateKho = "UPDATE tblDMHang SET Soluong = Soluong - @Soluong WHERE Mahang = @Mahang";
                    using (SqlCommand cmdUpdate = new SqlCommand(updateKho, Function.conn))
                    {
                        cmdUpdate.Parameters.AddWithValue("@Soluong", soluong);
                        cmdUpdate.Parameters.AddWithValue("@Mahang", txtMaHang.Text);
                        cmdUpdate.ExecuteNonQuery();
                    }

                    // Xóa chi tiết HDN
                    string sqlDeleteCT = "DELETE FROM tblChiTietHDN WHERE SoHDN = @SoHDN AND Mahang = @Mahang";
                    using (SqlCommand cmdDelete = new SqlCommand(sqlDeleteCT, Function.conn))
                    {
                        cmdDelete.Parameters.AddWithValue("@SoHDN", txtSoHDN.Text);
                        cmdDelete.Parameters.AddWithValue("@Mahang", txtMaHang.Text);
                        cmdDelete.ExecuteNonQuery();
                    }

                    // Cập nhật tổng tiền
                    UpdateTongTien();

                    // Kiểm tra nếu không còn chi tiết nào, xóa luôn hóa đơn
                    string checkCT = "SELECT COUNT(*) FROM tblChiTietHDN WHERE SoHDN = @SoHDN";
                    int countCT = 0;
                    using (SqlCommand cmdCheck = new SqlCommand(checkCT, Function.conn))
                    {
                        cmdCheck.Parameters.AddWithValue("@SoHDN", txtSoHDN.Text);
                        countCT = (int)cmdCheck.ExecuteScalar();
                    }

                    if (countCT == 0)
                    {
                        string deleteHDN = "DELETE FROM tblHoadonnhap WHERE SoHDN = @SoHDN";
                        using (SqlCommand cmdDelHDN = new SqlCommand(deleteHDN, Function.conn))
                        {
                            cmdDelHDN.Parameters.AddWithValue("@SoHDN", txtSoHDN.Text);
                            cmdDelHDN.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("✅ Đã xóa chi tiết hóa đơn!");
                    ClearFields();
                    load_datagrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"❌ Lỗi khi xóa: {ex.Message}", "Lỗi");
                }
            }
        }
    }
}
