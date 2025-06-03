using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cuahangxemaynhom8
{
    public partial class Sanpham: Form
    {
        public Sanpham()
        {
            InitializeComponent();
        }

        private string currentAction = ""; // Thêm, Sửa, Xóa
        private void Sanpham_Load(object sender, EventArgs e)
        {
            Function.Connect();
            load_datagrid();
            LoadCombobox();
            ClearFields();
        }
        private void load_datagrid()
        {
            string sql = "SELECT * FROM tblDMhang";
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(sql, Function.conn);
            da.Fill(dt);
            dgvSP.DataSource = dt;

            // Tùy chỉnh cột tiêu đề
            dgvSP.Columns[0].HeaderText = "Mã hàng";
            dgvSP.Columns[1].HeaderText = "Tên hàng";
            dgvSP.Columns[2].HeaderText = "Mã loại";
            dgvSP.Columns[3].HeaderText = "Hãng sản xuất";
            dgvSP.Columns[4].HeaderText = "Màu sắc";
            dgvSP.Columns[5].HeaderText = "Năm sản xuất";
            dgvSP.Columns[6].HeaderText = "Phanh xe";
            dgvSP.Columns[7].HeaderText = "Động cơ";
            dgvSP.Columns[8].HeaderText = "Dung tích bình xăng";
            dgvSP.Columns[9].HeaderText = "Nước sản xuất";
            dgvSP.Columns[10].HeaderText = "Tình trạng";
            dgvSP.Columns[11].HeaderText = "Ảnh";
            dgvSP.Columns[12].HeaderText = "Số lượng";
            dgvSP.Columns[13].HeaderText = "Đơn giá nhập";
            dgvSP.Columns[14].HeaderText = "Đơn giá bán";
            dgvSP.Columns[15].HeaderText = "Thời gian bảo hành (tháng)";

            dgvSP.AllowUserToAddRows = false;
            dgvSP.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void dgvSP_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaSP.Text = dgvSP.CurrentRow.Cells[0].Value.ToString();
            txtTenSP.Text = dgvSP.CurrentRow.Cells[1].Value.ToString();

            cboMaLoai.SelectedItem = dgvSP.CurrentRow.Cells[2].Value.ToString();
            cboHangSX.SelectedItem = dgvSP.CurrentRow.Cells[3].Value.ToString();
            cboMau.SelectedItem = dgvSP.CurrentRow.Cells[4].Value.ToString();

            txtNamSX.Text = dgvSP.CurrentRow.Cells[5].Value.ToString();

            cboPhanh.SelectedItem = dgvSP.CurrentRow.Cells[6].Value.ToString();
            cboDongCo.SelectedItem = dgvSP.CurrentRow.Cells[7].Value.ToString();

            txtBinhXang.Text = dgvSP.CurrentRow.Cells[8].Value.ToString();

            cboNuocSX.SelectedItem = dgvSP.CurrentRow.Cells[9].Value.ToString();
            cboTinhTrang.SelectedItem = dgvSP.CurrentRow.Cells[10].Value.ToString();

            txtAnh.Text = dgvSP.CurrentRow.Cells[11].Value.ToString();
            txtSoLuong.Text = dgvSP.CurrentRow.Cells[12].Value.ToString();
            txtGiaNhap.Text = dgvSP.CurrentRow.Cells[13].Value.ToString();
            txtGiaBan.Text = dgvSP.CurrentRow.Cells[14].Value.ToString();
            txtBaoHanh.Text = dgvSP.CurrentRow.Cells[15].Value.ToString();

            // Khi double-click, tự động load ảnh từ txtAnh
            string fileName = txtAnh.Text.Trim();

            // Nếu trong ô chỉ là tên file, nối thêm đường dẫn thư mục ảnh
            string fullPath = fileName;
            if (!File.Exists(fullPath))
            {
                fullPath = Path.Combine(targetFolder, fileName);
            }

            if (File.Exists(fullPath))
            {
                if (picAnh.Image != null)
                {
                    picAnh.Image.Dispose();
                    picAnh.Image = null;
                }

                picAnh.Image = Image.FromFile(fullPath);
                picAnh.SizeMode = PictureBoxSizeMode.Zoom;

                MessageBox.Show("Đã load ảnh từ: " + Path.GetFileName(fullPath), "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Không tìm thấy ảnh tại đường dẫn: " + fullPath, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void LoadCombobox()
        {
            string sql1 = "SELECT Maloai FROM tblTheloai";
            SqlCommand cmd = new SqlCommand(sql1, Function.conn); // Function.conn là SqlConnection mở sẵn
            SqlDataReader reader = cmd.ExecuteReader();
            cboMaLoai.Items.Clear();
            while (reader.Read())
            {
                cboMaLoai.Items.Add(reader["Maloai"].ToString());
            }
            reader.Close();

            string sql2 = "SELECT Mahangsx FROM tblHangsanxuat";
            SqlCommand cmd2 = new SqlCommand(sql2, Function.conn); // Function.conn là SqlConnection mở sẵn
            SqlDataReader reader2 = cmd2.ExecuteReader();
            cboHangSX.Items.Clear();
            while (reader2.Read())
            {
                cboHangSX.Items.Add(reader2["Mahangsx"].ToString());
            }
            reader2.Close();

            string sql3 = "SELECT Mamau FROM tblMausac";
            SqlCommand cmd3 = new SqlCommand(sql3, Function.conn); // Function.conn là SqlConnection mở sẵn
            SqlDataReader reader3 = cmd3.ExecuteReader();
            cboMau.Items.Clear();
            while (reader3.Read())
            {
                cboMau.Items.Add(reader3["Mamau"].ToString());
            }
            reader3.Close();

            string sql4 = "SELECT Maphanh FROM tblPhanhxe";
            SqlCommand cmd4 = new SqlCommand(sql4, Function.conn); // Function.conn là SqlConnection mở sẵn
            SqlDataReader reader4 = cmd4.ExecuteReader();
            cboPhanh.Items.Clear();
            while (reader4.Read())
            {
                cboPhanh.Items.Add(reader4["Maphanh"].ToString());
            }
            reader4.Close();

            string sql5 = "SELECT Matinhtrang FROM tblTinhtrang";
            SqlCommand cmd5 = new SqlCommand(sql5, Function.conn); // Function.conn là SqlConnection mở sẵn
            SqlDataReader reader5 = cmd5.ExecuteReader();
            cboTinhTrang.Items.Clear();
            while (reader5.Read())
            {
                cboTinhTrang.Items.Add(reader5["Matinhtrang"].ToString());
            }
            reader5.Close();

            string sql6 = "SELECT Manuocsx FROM tblNuocsanxuat";
            SqlCommand cmd6 = new SqlCommand(sql6, Function.conn); // Function.conn là SqlConnection mở sẵn
            SqlDataReader reader6 = cmd6.ExecuteReader();
            cboNuocSX.Items.Clear();
            while (reader6.Read())
            {
                cboNuocSX.Items.Add(reader6["Manuocsx"].ToString());
            }
            reader6.Close();

            string sql7 = "SELECT Madongco FROM tblDongco";
            SqlCommand cmd7 = new SqlCommand(sql7, Function.conn); // Function.conn là SqlConnection mở sẵn
            SqlDataReader reader7 = cmd7.ExecuteReader();
            cboDongCo.Items.Clear();
            while (reader7.Read())
            {
                cboDongCo.Items.Add(reader7["Madongco"].ToString());
            }
            reader7.Close();


        }
        private void ClearFields()
        {
            // Xóa text box
            txtMaSP.Text = "";
            txtTenSP.Text = "";
            txtNamSX.Text = "";
            txtBinhXang.Text = "";
            txtAnh.Text = "";
            txtSoLuong.Text = "";
            txtGiaNhap.Text = "";
            txtGiaBan.Text = "";
            txtBaoHanh.Text = "";

            // Reset combobox nếu có DataSource
            cboMaLoai.SelectedIndex = -1;
            cboHangSX.SelectedIndex = -1;
            cboMau.SelectedIndex = -1;
            cboPhanh.SelectedIndex = -1;
            cboDongCo.SelectedIndex = -1;
            cboNuocSX.SelectedIndex = -1;
            cboTinhTrang.SelectedIndex = -1;

            picAnh.Image = null;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            ClearFields();
            currentAction = "add";
            MessageBox.Show("Nhập thông tin rồi bấm 'Lưu'", "Thông báo");
        }


        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaSP.Text))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                currentAction = "delete";
                btnLuu_Click(sender, e);
            }
        }

        private void btnSua_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaSP.Text))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            currentAction = "edit";
            MessageBox.Show("Chỉnh sửa thông tin rồi bấm 'Lưu'", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(txtMaSP.Text) || string.IsNullOrWhiteSpace(txtTenSP.Text) ||
                   string.IsNullOrWhiteSpace(cboMaLoai.Text) || string.IsNullOrWhiteSpace(cboHangSX.Text) ||
                   string.IsNullOrWhiteSpace(cboMau.Text) || string.IsNullOrWhiteSpace(cboPhanh.Text) ||
                   string.IsNullOrWhiteSpace(txtNamSX.Text) || string.IsNullOrWhiteSpace(cboDongCo.Text) ||
                   string.IsNullOrWhiteSpace(txtBinhXang.Text) || string.IsNullOrWhiteSpace(cboNuocSX.Text) ||
                   string.IsNullOrWhiteSpace(cboTinhTrang.Text) || string.IsNullOrWhiteSpace(txtSoLuong.Text) ||
                   string.IsNullOrWhiteSpace(txtGiaNhap.Text) || string.IsNullOrWhiteSpace(txtGiaBan.Text) ||
                   string.IsNullOrWhiteSpace(txtBaoHanh.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra định dạng số
                if (!int.TryParse(txtNamSX.Text, out int namSX) ||
                    !float.TryParse(txtBinhXang.Text, out float binhXang) ||
                    !int.TryParse(txtSoLuong.Text, out int soLuong) ||
                    !float.TryParse(txtGiaNhap.Text, out float giaNhap) ||
                    !float.TryParse(txtGiaBan.Text, out float giaBan) ||
                    !int.TryParse(txtBaoHanh.Text, out int baoHanh))
                {
                    MessageBox.Show("Vui lòng nhập đúng định dạng số cho các trường: Năm sản xuất, Dung tích bình xăng, Số lượng, Đơn giá nhập, Đơn giá bán, Thời gian bảo hành!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (currentAction == "add")
                {
                    string checkSql = "SELECT COUNT(*) FROM tblDMhang WHERE Mahang = @Mahang";
                    using (SqlCommand checkCmd = new SqlCommand(checkSql, Function.conn))
                    {
                        checkCmd.Parameters.AddWithValue("@Mahang", txtMaSP.Text);
                        int count = (int)checkCmd.ExecuteScalar();
                        if (count > 0)
                        {
                            MessageBox.Show("Mã sản phẩm đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    string sql = @"INSERT INTO tblDMhang VALUES (@Mahang, @Tenhang, @Maloai, @Mahangsx, @Mamau, @Namsx, @Maphanh, @Madongco,
                                @Binhxang, @Manuocsx, @Matinhtrang, @Anh, 0, 0, 0, @Baohanh)";
                    using (SqlCommand cmd = new SqlCommand(sql, Function.conn))
                    {
                        AddParameters(cmd);
                        cmd.ExecuteNonQuery();
                    }
                }
                else if (currentAction == "edit")
                {
                    string sql = @"UPDATE tblDMhang SET Tenhang=@Tenhang, Maloai=@Maloai, Mahangsx=@Mahangsx, Mamau=@Mamau,
                                Namsanxuat=@Namsx, Maphanh=@Maphanh, Madongco=@Madongco, Dungtichbinhxang=@Binhxang,
                                Manuocsx=@Manuocsx, Matinhtrang=@Matinhtrang, Anh=@Anh, Soluong=@Soluong,
                                Dongianhap=@Dongianhap, Dongiaban=@Dongiaban, Thoigianbaohanh=@Baohanh
                                WHERE Mahang=@Mahang";
                    using (SqlCommand cmd = new SqlCommand(sql, Function.conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                else if (currentAction == "delete")
                {
                    string sql = "DELETE FROM tblDMhang WHERE Mahang = @Mahang";
                    using (SqlCommand cmd = new SqlCommand(sql, Function.conn))
                    {
                        cmd.Parameters.AddWithValue("@Mahang", txtMaSP.Text);
                        cmd.ExecuteNonQuery();
                    }
                }

                load_datagrid();
                ClearFields();
                currentAction = "";
                MessageBox.Show("Thao tác thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void AddParameters(SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@Mahang", txtMaSP.Text);
            cmd.Parameters.AddWithValue("@Tenhang", txtTenSP.Text);
            cmd.Parameters.AddWithValue("@Maloai", cboMaLoai.Text);
            cmd.Parameters.AddWithValue("@Mahangsx", cboHangSX.Text);
            cmd.Parameters.AddWithValue("@Mamau", cboMau.Text);
            cmd.Parameters.AddWithValue("@Namsx", int.Parse(txtNamSX.Text));
            cmd.Parameters.AddWithValue("@Maphanh", cboPhanh.Text);
            cmd.Parameters.AddWithValue("@Madongco", cboDongCo.Text);
            cmd.Parameters.AddWithValue("@Binhxang", float.Parse(txtBinhXang.Text));
            cmd.Parameters.AddWithValue("@Manuocsx", cboNuocSX.Text);
            cmd.Parameters.AddWithValue("@Matinhtrang", cboTinhTrang.Text);
            cmd.Parameters.AddWithValue("@Anh", txtAnh.Text);
            cmd.Parameters.AddWithValue("@Soluong", int.Parse(txtSoLuong.Text));
            cmd.Parameters.AddWithValue("@Dongianhap", float.Parse(txtGiaNhap.Text));
            cmd.Parameters.AddWithValue("@Dongiaban", float.Parse(txtGiaBan.Text));
            cmd.Parameters.AddWithValue("@Baohanh", int.Parse(txtBaoHanh.Text));
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            ClearFields();
            load_datagrid();
        }

        private void guna2Button9_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtTK.Text.Trim();
                if (string.IsNullOrEmpty(keyword))
                {
                    MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    load_datagrid(); // Tải lại toàn bộ dữ liệu nếu không có từ khóa
                    return;
                }

                string sql = "SELECT * FROM tblDMhang WHERE Mahang LIKE @Keyword OR Tenhang LIKE @Keyword";
                using (SqlCommand cmd = new SqlCommand(sql, Function.conn))
                {
                    cmd.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("Không tìm thấy sản phẩm nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        load_datagrid(); // Tải lại toàn bộ dữ liệu nếu không tìm thấy
                        return;
                    }

                    dgvSP.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

     
        string targetFolder = @"C:\Users\DELL\Downloads\images";

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (currentAction == "edit")
            {
                // Sửa: Cho chọn ảnh mới và dán đường dẫn
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                ofd.Title = "Chọn ảnh mới";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string selectedFile = ofd.FileName;

                    // Giải phóng ảnh cũ nếu có
                    if (picAnh.Image != null)
                    {
                        picAnh.Image.Dispose();
                        picAnh.Image = null;
                    }
                    picAnh.Image = Image.FromFile(selectedFile);
                    picAnh.SizeMode = PictureBoxSizeMode.Zoom;

                    txtAnh.Text = selectedFile;  // Lưu đường dẫn đầy đủ
                    MessageBox.Show("Đã chọn ảnh mới: " + Path.GetFileName(selectedFile), "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                // Thêm hoặc xem: Load ảnh từ đường dẫn trong txtAnh
                string filePath = txtAnh.Text.Trim();

                // Nếu chỉ là tên file, nối thêm thư mục
                string fullPath = filePath;
                if (!File.Exists(filePath))
                {
                    fullPath = Path.Combine(targetFolder, filePath);
                }

                if (File.Exists(fullPath))
                {
                    // Giải phóng ảnh cũ nếu có
                    if (picAnh.Image != null)
                    {
                        picAnh.Image.Dispose();
                        picAnh.Image = null;
                    }

                    picAnh.Image = Image.FromFile(fullPath);
                    picAnh.SizeMode = PictureBoxSizeMode.Zoom;

                    MessageBox.Show("Đã load ảnh từ: " + Path.GetFileName(fullPath), "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy ảnh tại đường dẫn: " + fullPath, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}
