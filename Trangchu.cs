using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cuahangxemaynhom8
{
    public partial class Trangchu : Form
    {
        public Trangchu( string tenNV)
        {
            InitializeComponent();
            lblnguoidung.Text = tenNV;
        }

        private Form currentFormChild;
        private void OpenChildForm(Form childForm)
        {
            // Đóng form con hiện tại nếu có
            if (currentFormChild != null)
            {
                currentFormChild.Close();
            }

            // Cập nhật form con hiện tại
            currentFormChild = childForm;

            // Cấu hình form con để nhúng vào panel
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            // Thêm form con vào panel
            panelgiua.Controls.Add(childForm);
            panelgiua.Tag = childForm;

            // Đưa form con ra phía trước và hiển thị
            childForm.BringToFront();
            childForm.Show();
        }
        private void btnDanhMuc_Click(object sender, EventArgs e)
        {
            paneldanhmuc.Visible = !paneldanhmuc.Visible;
        }

        private void btnHoaDon_Click(object sender, EventArgs e)
        {
            panelhoadon.Visible = !panelhoadon.Visible;
        }

        private void btnBaoCao_Click(object sender, EventArgs e)
        {
            panelbaocao.Visible = !panelbaocao.Visible;
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            Dangnhap formDN = new Dangnhap();

            // Hiện form mới
            formDN.Show();

            // Đóng form hiện tại
            this.Close();
        }

        private void btnNhaCungCap_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Nhacungcap());
            lbltenform.Text = btnNhaCungCap.Text;
        }

        private void btnNhanVien_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Nhanvien());
            lbltenform.Text = btnNhanVien.Text;
        }

        private void btnSanPham_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Sanpham());
            lbltenform.Text = btnSanPham.Text;
        }

        private void btnKhachHang_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Khachhang());
            lbltenform.Text = btnKhachHang.Text;
        }

        private void btnHDB_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Hoadonban());
            lbltenform.Text = btnHDB.Text;
        }

        private void btnHDN_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Hoadonnhap());
            lbltenform.Text = btnHDN.Text;
        }

        private void btnTimHDB_Click(object sender, EventArgs e)
        {
            OpenChildForm(new TimkiemHDB());
            lbltenform.Text = btnTimHDB.Text;
        }

        private void btnTimHDN_Click(object sender, EventArgs e)
        {
            OpenChildForm(new TimkiemHDN());
            lbltenform.Text = btnTimHDN.Text;
        }

        private void btnBCN_Click(object sender, EventArgs e)
        {
            OpenChildForm(new BaoCaoNhapHang());
            lbltenform.Text = btnBCN.Text;
        }

        private void btnBCB_Click(object sender, EventArgs e)
        {
            OpenChildForm(new BaoCaoBanHang());
            lbltenform.Text = btnBCB.Text;
        }

        private void btnBCDT_Click(object sender, EventArgs e)
        {
            OpenChildForm(new BaoCaoDoanhThu());
            lbltenform.Text = btnBCDT.Text;
        }

        private void btnDB_Click(object sender, EventArgs e)
        {
            // Đóng forrm
            if (currentFormChild != null)
            {
                currentFormChild.Close();
                currentFormChild = null;
            }

            // Cập nhật lại label tên form nếu có
            lbltenform.Text = "MOTO VIỆT";
        }
    }
}

