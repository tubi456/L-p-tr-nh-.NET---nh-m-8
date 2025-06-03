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
    public partial class Hoadonban : Form
    {
        public Hoadonban()
        {
            InitializeComponent();
        }
        bool isAdding = false;
        bool isEditing = false;
        private void Hoadonban_Load(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Function.connString))
                {
                    conn.Open();
                    // Thêm cột Dongia vào truy vấn
                    string query = "SELECT SoDDH, Mahang, Soluong, Dongia, Giamgia, Thanhtien FROM dbo.tblChitietDDH";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvCTHDB.DataSource = dt;

                    // Đặt tiêu đề cột
                    if (dgvCTHDB.Columns.Contains("SoDDH")) dgvCTHDB.Columns["SoDDH"].HeaderText = "Số Đơn Đặt Hàng";
                    if (dgvCTHDB.Columns.Contains("Mahang")) dgvCTHDB.Columns["MaHang"].HeaderText = "Mã Hàng";
                    if (dgvCTHDB.Columns.Contains("Soluong")) dgvCTHDB.Columns["Soluong"].HeaderText = "Số Lượng";
                    if (dgvCTHDB.Columns.Contains("Dongia")) dgvCTHDB.Columns["Dongia"].HeaderText = "Đơn Giá";
                    if (dgvCTHDB.Columns.Contains("Giamgia")) dgvCTHDB.Columns["Giamgia"].HeaderText = "Giảm Giá";
                    if (dgvCTHDB.Columns.Contains("Thanhtien")) dgvCTHDB.Columns["Thanhtien"].HeaderText = "Thành Tiền";

                    // Định dạng số tiền
                    dgvCTHDB.Columns["Dongia"].DefaultCellStyle.Format = "#,##0";
                    dgvCTHDB.Columns["Giamgia"].DefaultCellStyle.Format = "#,##0";
                    dgvCTHDB.Columns["Thanhtien"].DefaultCellStyle.Format = "#,##0";

                    // Căn chỉnh cột
                    dgvCTHDB.Columns["Soluong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvCTHDB.Columns["Dongia"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvCTHDB.Columns["Giamgia"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvCTHDB.Columns["Thanhtien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    // Điều chỉnh độ rộng cột
                    //  dgvHDB.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;//

                    // Định dạng DateTimePicker
                    dtpNgayMua.Format = DateTimePickerFormat.Custom;
                    dtpNgayMua.CustomFormat = "dd/MM/yyyy";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            }
        }

        private void dgvCTHDB_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Đảm bảo click vào dòng hợp lệ
            {
                // Lấy dữ liệu từ dòng được chọn trong DataGridView
                DataGridViewRow row = dgvCTHDB.Rows[e.RowIndex];
                string soDDH = row.Cells["SoDDH"].Value?.ToString() ?? "";
                string maHang = row.Cells["Mahang"].Value?.ToString() ?? "";

                // **Phần Chi tiết Đơn đặt hàng**: Hiển thị dữ liệu từ DataGridView (tblChiTietDDH)
                txtMaHang.Text = row.Cells["Mahang"].Value?.ToString() ?? "";
                txtSoLuong.Text = row.Cells["Soluong"].Value?.ToString() ?? "";
                txtDonGia.Text = row.Cells["Dongia"].Value != null ? Convert.ToDecimal(row.Cells["Dongia"].Value).ToString("#,##0") : "0";
                txtGiamGia.Text = row.Cells["Giamgia"].Value != null ? Convert.ToDecimal(row.Cells["Giamgia"].Value).ToString("#,##0") : "0";
                txtThanhTien.Text = row.Cells["Thanhtien"].Value != null ? Convert.ToDecimal(row.Cells["Thanhtien"].Value).ToString("#,##0") : "0";

                // **Phần Thông tin chung**: Lấy dữ liệu từ tblDonDatHang và tblChiTietDDH
                try
                {
                    using (SqlConnection conn = new SqlConnection(Function.connString))
                    {
                        conn.Open();
                        string query = @"
                    SELECT 
                        d.SoDDH, d.MaNV, d.MaKhach, d.NgayMua, d.DatCoc, d.Thue, d.TongTien ROM dbo.tblDondathang d
                    INNER JOIN 
                        dbo.tblChiTietDDH c ON d.SoDDH = c.SoDDH
                    WHERE 
                        d.SoDDH = @SoDDH AND c.Mahang = @Mahang";

                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@SoDDH", soDDH);
                        cmd.Parameters.AddWithValue("@Mahang", maHang);

                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            // Hiển thị dữ liệu từ tblDonDatHang
                            txtSoDDH.Text = reader["SoDDH"].ToString();
                            txtMaNV.Text = reader["MaNV"].ToString();
                            txtMaKH.Text = reader["MaKhach"].ToString();
                            dtpNgayMua.Value = reader["Ngaymua"] != DBNull.Value ? Convert.ToDateTime(reader["Ngaymua"]) : DateTime.Now;
                            txtDatCoc.Text = reader["Datcoc"] != DBNull.Value ? Convert.ToDecimal(reader["Datcoc"]).ToString("#,##0") : "0";
                            txtThue.Text = reader["Thue"] != DBNull.Value ? Convert.ToDecimal(reader["Thue"]).ToString("#,##0") : "0";
                            txtTongTien.Text = reader["Tongtien"] != DBNull.Value ? Convert.ToDecimal(reader["Tongtien"]).ToString("#,##0") : "0";
                        }
                        else
                        {
                            // Xóa dữ liệu nếu không tìm thấy
                            txtSoDDH.Text = "";
                            txtMaNV.Text = "";
                            txtMaKH.Text = "";
                            dtpNgayMua.Value = DateTime.Now;
                            txtDatCoc.Text = "";
                            txtThue.Text = "";
                            txtTongTien.Text = "";
                        }
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lấy dữ liệu Thông tin chung: " + ex.Message);
                }
            }
        }

        private void btnTK_Click(object sender, EventArgs e)
        {
            string tukhoa = txtTK.Text.Trim();
            if (string.IsNullOrEmpty(tukhoa))
            {
                MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm!");
                return;
            }

            string sql = @"SELECT c.SoDDH, MaNV, MaKhach, Ngaymua,Datcoc, Thue, Tongtien, Mahang, Soluong, Dongia, Giamgia, Thanhtien
            FROM tblDondathang h JOIN tblChitietDDH c ON h.SoDDH = c.SoDDH WHERE h.SoDDH LIKE @tukhoa";

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
                txtSoDDH.Text = row["SoDDH"].ToString();
                txtMaNV.Text = row["MaNV"].ToString();
                txtMaKH.Text = row["MaKhach"].ToString();
                if (DateTime.TryParse(row["Ngaymua"].ToString(), out DateTime ngaymua))
                    dtpNgayMua.Value = ngaymua;
                txtDatCoc.Text = row["Datcoc"].ToString();
                txtThue.Text = row["Thue"].ToString();
                txtTongTien.Text = row["Tongtien"].ToString();

                // Tạo DataTable chỉ chứa chi tiết cần hiển thị trên DataGridView
                DataTable dtChiTiet = dt.DefaultView.ToTable(false, "Mahang", "Soluong", "Dongia", "Giamgia", "Thanhtien");

                dgvCTHDB.DataSource = dtChiTiet;

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

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            isAdding = true;
            isEditing = false;

            // Mở khóa các textbox
            txtSoDDH.Enabled = true;
            txtMaNV.Enabled = true;
            txtMaKH.Enabled = true;
            txtThue.Enabled = true;
            txtDatCoc.Enabled = true;
            dtpNgayMua.Enabled = true;
            txtMaHang.Enabled = true;
            txtSoLuong.Enabled = true;

            // Xóa các ô nhập liệu
            txtSoDDH.Clear();
            txtMaNV.Clear();
            txtMaKH.Clear();
            txtDatCoc.Clear();
            txtThue.Clear();
            txtMaHang.Clear();
            txtSoLuong.Clear();
            txtDonGia.Clear();
            txtGiamGia.Clear();
            txtThanhTien.Clear();
            txtTongTien.Clear();
            txtTK.Clear();  
            txtSoDDH.Focus();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            isAdding = false;
            isEditing = true;

            txtSoDDH.Enabled = false; // không cho sửa mã đơn
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Function.connString))
                {
                    conn.Open();

                    string soDDH = txtSoDDH.Text.Trim();
                    string maHang = txtMaHang.Text.Trim();
                    int soLuong = int.TryParse(txtSoLuong.Text, out var sl) ? sl : 0;
                    decimal donGia = decimal.TryParse(txtDonGia.Text.Replace(",", ""), out var dg) ? dg : 0;
                    decimal giamGia = decimal.TryParse(txtGiamGia.Text.Replace(",", ""), out var gg) ? gg : 0;
                    decimal thanhTien = decimal.TryParse(txtThanhTien.Text.Replace(",", ""), out var tt) ? tt : 0;

                    string maNV = txtMaNV.Text.Trim();
                    string maKH = txtMaKH.Text.Trim();
                    decimal datCoc = decimal.TryParse(txtDatCoc.Text.Replace(",", ""), out var dc) ? dc : 0;
                    decimal thue = decimal.TryParse(txtThue.Text.Replace(",", ""), out var th) ? th : 0;
                    decimal tongTien = decimal.TryParse(txtTongTien.Text.Replace(",", ""), out var tong) ? tong : 0;
                    DateTime ngayMua = dtpNgayMua.Value;

                    if (isAdding)
                    {
                        // Kiểm tra đơn hàng có tồn tại chưa
                        string checkDon = "SELECT COUNT(*) FROM tblDonDatHang WHERE SoDDH = @SoDDH";
                        SqlCommand cmdCheck = new SqlCommand(checkDon, conn);
                        cmdCheck.Parameters.AddWithValue("@SoDDH", soDDH);
                        int count = (int)cmdCheck.ExecuteScalar();

                        if (count == 0)
                        {
                            // Thêm vào bảng tblDonDatHang
                            string insertDon = @"
                        INSERT INTO tblDonDatHang (SoDDH, MaNV, MaKhach, NgayMua, DatCoc, Thue, TongTien)
                        VALUES (@SoDDH, @MaNV, @MaKH, @NgayMua, @DatCoc, @Thue, @TongTien)";
                            SqlCommand cmdInsertDon = new SqlCommand(insertDon, conn);
                            cmdInsertDon.Parameters.AddWithValue("@SoDDH", soDDH);
                            cmdInsertDon.Parameters.AddWithValue("@MaNV", maNV);
                            cmdInsertDon.Parameters.AddWithValue("@MaKH", maKH);
                            cmdInsertDon.Parameters.AddWithValue("@NgayMua", ngayMua);
                            cmdInsertDon.Parameters.AddWithValue("@DatCoc", datCoc);
                            cmdInsertDon.Parameters.AddWithValue("@Thue", thue);
                            cmdInsertDon.Parameters.AddWithValue("@TongTien", tongTien);
                            cmdInsertDon.ExecuteNonQuery();
                        }
                        // Giảm số lượng tồn kho
                        string updateSoLuong = @"UPDATE tblDMHang SET Soluong = Soluong - @Soluong WHERE Mahang = @MaHang";
                        SqlCommand cmdUpdateSL = new SqlCommand(updateSoLuong, conn);
                        cmdUpdateSL.Parameters.AddWithValue("@Soluong", soLuong);
                        cmdUpdateSL.Parameters.AddWithValue("@MaHang", maHang);
                        cmdUpdateSL.ExecuteNonQuery();

                        // Thêm vào bảng tblChiTietDDH
                        string insertCT = @"
                    INSERT INTO tblChiTietDDH (SoDDH, MaHang, Soluong, Dongia, Giamgia, Thanhtien)
                    VALUES (@SoDDH, @MaHang, @Soluong, @Dongia, @Giamgia, @Thanhtien)";
                        SqlCommand cmdInsertCT = new SqlCommand(insertCT, conn);
                        cmdInsertCT.Parameters.AddWithValue("@SoDDH", soDDH);
                        cmdInsertCT.Parameters.AddWithValue("@MaHang", maHang);
                        cmdInsertCT.Parameters.AddWithValue("@Soluong", soLuong);
                        cmdInsertCT.Parameters.AddWithValue("@Dongia", donGia);
                        cmdInsertCT.Parameters.AddWithValue("@Giamgia", giamGia);
                        cmdInsertCT.Parameters.AddWithValue("@Thanhtien", thanhTien);
                        cmdInsertCT.ExecuteNonQuery();

                        MessageBox.Show("Thêm mới hóa đơn thành công!");
                    }
                    else if (isEditing)
                    {
                        // Lấy số lượng cũ từ chi tiết đơn hàng (TRƯỚC KHI update chi tiết)
                        string getOldSoluong = "SELECT Soluong FROM tblChiTietDDH WHERE SoDDH = @SoDDH AND MaHang = @MaHang";
                        SqlCommand cmdGetOld = new SqlCommand(getOldSoluong, conn);
                        cmdGetOld.Parameters.AddWithValue("@SoDDH", soDDH);
                        cmdGetOld.Parameters.AddWithValue("@MaHang", maHang);
                        int oldSoluong = (int)cmdGetOld.ExecuteScalar();

                        // Tính delta
                        int delta = soLuong - oldSoluong;

                        // Lấy số lượng tồn kho hiện tại
                        string getKhoSoluong = "SELECT Soluong FROM tblDMHang WHERE MaHang = @MaHang";
                        SqlCommand cmdGetKho = new SqlCommand(getKhoSoluong, conn);
                        cmdGetKho.Parameters.AddWithValue("@MaHang", maHang);
                        int khoSoluong = (int)cmdGetKho.ExecuteScalar();

                        // Tính số lượng mới trong kho
                        int newKhoSoluong = khoSoluong - delta;
                        if (newKhoSoluong < 0)
                        {
                            MessageBox.Show("❌ Không đủ hàng tồn kho!", "Lỗi");
                            return;
                        }

                        // Update số lượng kho
                        string updateKho = "UPDATE tblDMHang SET Soluong = @Soluong WHERE MaHang = @MaHang";
                        SqlCommand cmdUpdateKho = new SqlCommand(updateKho, conn);
                        cmdUpdateKho.Parameters.AddWithValue("@Soluong", newKhoSoluong);
                        cmdUpdateKho.Parameters.AddWithValue("@MaHang", maHang);
                        cmdUpdateKho.ExecuteNonQuery();

                        // Cập nhật bảng tblChiTietDDH
                        string updateCT = @"UPDATE tblChiTietDDH SET Soluong = @Soluong, Dongia = @Dongia, Giamgia = @Giamgia, Thanhtien = @Thanhtien WHERE SoDDH = @SoDDH AND MaHang = @MaHang";
                        SqlCommand cmdUpdateCT = new SqlCommand(updateCT, conn);
                        cmdUpdateCT.Parameters.AddWithValue("@SoDDH", soDDH);
                        cmdUpdateCT.Parameters.AddWithValue("@MaHang", maHang);
                        cmdUpdateCT.Parameters.AddWithValue("@Soluong", soLuong);
                        cmdUpdateCT.Parameters.AddWithValue("@Dongia", donGia);
                        cmdUpdateCT.Parameters.AddWithValue("@Giamgia", giamGia);
                        cmdUpdateCT.Parameters.AddWithValue("@Thanhtien", thanhTien);
                        cmdUpdateCT.ExecuteNonQuery();

                        // Cập nhật bảng tblDonDatHang (nếu cần)
                        string updateDon = @"UPDATE tblDonDatHang SET MaNV = @MaNV, MaKhach = @MaKH, NgayMua = @NgayMua, DatCoc = @DatCoc, Thue = @Thue, TongTien = @TongTien WHERE SoDDH = @SoDDH";
                        SqlCommand cmdUpdateDon = new SqlCommand(updateDon, conn);
                        cmdUpdateDon.Parameters.AddWithValue("@SoDDH", soDDH);
                        cmdUpdateDon.Parameters.AddWithValue("@MaNV", maNV);
                        cmdUpdateDon.Parameters.AddWithValue("@MaKH", maKH);
                        cmdUpdateDon.Parameters.AddWithValue("@NgayMua", ngayMua);
                        cmdUpdateDon.Parameters.AddWithValue("@DatCoc", datCoc);
                        cmdUpdateDon.Parameters.AddWithValue("@Thue", thue);
                        cmdUpdateDon.Parameters.AddWithValue("@TongTien", tongTien);
                        cmdUpdateDon.ExecuteNonQuery();

                        MessageBox.Show("Cập nhật hóa đơn thành công!");
                    }
                    CapNhatTongTien();
                    // Reset trạng thái
                    isAdding = false;
                    isEditing = false;

                    // Hiển thị lại dữ liệu
                    LoadData(); // Viết hàm LoadData để nạp lại dgvHDB

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message);
            }
        }

        private void LoadData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Function.connString))
                {
                    conn.Open();
                    string query = "SELECT SoDDH, MaHang, Soluong, Dongia, Giamgia, Thanhtien FROM dbo.tblChiTietDDH";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvCTHDB.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            }
        }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
            // Khóa tất cả các textbox
            txtSoDDH.Enabled = false;
            txtMaNV.Enabled = false;
            txtMaKH.Enabled = false;
            txtThue.Enabled = false;
            txtDatCoc.Enabled = false;
            dtpNgayMua.Enabled = false;
            txtMaHang.Enabled = false;
            txtSoLuong.Enabled = false;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn thoát?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void btnInHD_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSoDDH.Text))
            {
                MessageBox.Show("Vui lòng chọn đơn đặt hàng để in!");
                return;
            }

            // Giả lập in hóa đơn – bạn có thể thay bằng xuất PDF hoặc in thực tế
            MessageBox.Show("In hóa đơn: " + txtSoDDH.Text, "In Hóa Đơn", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void TinhThanhTien()
        {
            try
            {
                int soluong = int.Parse(txtSoLuong.Text);
                long dongia = long.Parse(txtDonGia.Text.Replace(",", ""));
                string sqlGetDonGia = "SELECT Dongianhap FROM tblDMHang WHERE Mahang = @Mahang";
                using (SqlCommand cmdDonGia = new SqlCommand(sqlGetDonGia, Function.conn))
                {
                    cmdDonGia.Parameters.AddWithValue("@Mahang", txtMaHang.Text);
                    object result = cmdDonGia.ExecuteScalar();
                    if (result == null)
                    {
                        MessageBox.Show("❌ Mã hàng không tồn tại!", "Lỗi");
                        return;
                    }
                    dongia = Convert.ToInt64(result);
                }

                long giamgia = long.Parse(txtGiamGia.Text.Replace(",", ""));

                long thanhtien = (dongia * soluong) - giamgia;
                txtThanhTien.Text = thanhtien.ToString("N0"); // Hiện định dạng 1,000,000
            }
            catch
            {
                txtThanhTien.Text = "0";
            }
        }
        private void txtSoluong_Leave(object sender, EventArgs e)
        {
            TinhThanhTien();
        }
        private void CapNhatTongTien()
        {
            string soDDH = txtSoDDH.Text;

            // Lấy giá trị thuế và đặt cọc từ textbox
            decimal thue = !string.IsNullOrEmpty(txtThue.Text) ? Convert.ToDecimal(txtThue.Text) : 0;
            decimal datCoc = !string.IsNullOrEmpty(txtDatCoc.Text) ? Convert.ToDecimal(txtDatCoc.Text) : 0;
            decimal tongChiTiet = 0;

            string sql = "SELECT SUM(Thanhtien) FROM tblChiTietDDH WHERE SoDDH = @SoDDH";

            using (SqlConnection conn = new SqlConnection(Function.connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@SoDDH", soDDH);
                    object result = cmd.ExecuteScalar();
                    if (result != DBNull.Value && result != null)
                    {
                        tongChiTiet = Convert.ToDecimal(result);
                    }
                }
            }

            decimal tongTien = tongChiTiet + thue - datCoc;
            txtTongTien.Text = string.Format("{0:N0}", tongTien);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSoDDH.Text) || string.IsNullOrEmpty(txtMaHang.Text))
            {
                MessageBox.Show("❌ Vui lòng chọn chi tiết đơn hàng cần xóa!", "Lỗi");
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa chi tiết này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(Function.connString))
                    {
                        conn.Open();

                        string soDDH = txtSoDDH.Text.Trim();
                        string maHang = txtMaHang.Text.Trim();

                        // Lấy số lượng từ chi tiết đơn hàng để cộng lại kho
                        string getSoluong = "SELECT Soluong FROM tblChiTietDDH WHERE SoDDH = @SoDDH AND MaHang = @MaHang";
                        SqlCommand cmdGet = new SqlCommand(getSoluong, conn);
                        cmdGet.Parameters.AddWithValue("@SoDDH", soDDH);
                        cmdGet.Parameters.AddWithValue("@MaHang", maHang);
                        int soluong = (int)cmdGet.ExecuteScalar();

                        // Cộng lại số lượng kho
                        string updateKho = "UPDATE tblDMHang SET Soluong = Soluong + @Soluong WHERE MaHang = @MaHang";
                        SqlCommand cmdUpdateKho = new SqlCommand(updateKho, conn);
                        cmdUpdateKho.Parameters.AddWithValue("@Soluong", soluong);
                        cmdUpdateKho.Parameters.AddWithValue("@MaHang", maHang);
                        cmdUpdateKho.ExecuteNonQuery();

                        // Xóa chi tiết đơn hàng
                        string deleteCT = "DELETE FROM tblChiTietDDH WHERE SoDDH = @SoDDH AND MaHang = @MaHang";
                        SqlCommand cmdDeleteCT = new SqlCommand(deleteCT, conn);
                        cmdDeleteCT.Parameters.AddWithValue("@SoDDH", soDDH);
                        cmdDeleteCT.Parameters.AddWithValue("@MaHang", maHang);
                        cmdDeleteCT.ExecuteNonQuery();

                        // Kiểm tra nếu không còn chi tiết nào, xóa luôn đơn đặt hàng chính
                        string checkCT = "SELECT COUNT(*) FROM tblChiTietDDH WHERE SoDDH = @SoDDH";
                        SqlCommand cmdCheck = new SqlCommand(checkCT, conn);
                        cmdCheck.Parameters.AddWithValue("@SoDDH", soDDH);
                        int countCT = (int)cmdCheck.ExecuteScalar();

                        if (countCT == 0)
                        {
                            string deleteDon = "DELETE FROM tblDonDatHang WHERE SoDDH = @SoDDH";
                            SqlCommand cmdDeleteDon = new SqlCommand(deleteDon, conn);
                            cmdDeleteDon.Parameters.AddWithValue("@SoDDH", soDDH);
                            cmdDeleteDon.ExecuteNonQuery();
                        }
                        else
                        {
                            // Nếu vẫn còn chi tiết, cập nhật lại tổng tiền
                            CapNhatTongTien();
                        }

                        MessageBox.Show("✅ Đã xóa chi tiết đơn hàng!");

                        // Làm mới dữ liệu
                        LoadData();

                        // Xóa form
                        txtMaHang.Clear();
                        txtSoLuong.Clear();
                        txtDonGia.Clear();
                        txtGiamGia.Clear();
                        txtThanhTien.Clear();
                        txtTongTien.Clear();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("❌ Lỗi khi xóa: " + ex.Message, "Lỗi");
                }
            }
        }

     
    }
}
