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
using Microsoft.Reporting.WinForms;

namespace Cuahangxemaynhom8
{
    public partial class BaoCaoNhapHang : Form
    {
        public BaoCaoNhapHang()
        {
            InitializeComponent();
        }

        private void BaoCaoNhapHang_Load(object sender, EventArgs e)
        {
            rpvBCNhapHang.ProcessingMode = ProcessingMode.Local;
            rpvBCNhapHang.LocalReport.ReportPath = "ReportNH.rdlc";  // chỉ đường tới file rdlc
            ReportParameter ngayTao = new ReportParameter("NgayTao", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            rpvBCNhapHang.LocalReport.SetParameters(ngayTao);

            this.rpvBCNhapHang.RefreshReport();
        }

        private void btnTaoBaoCao_Click(object sender, EventArgs e)
        {
            if (radTheoKhoang.Checked && dtpTu.Value.Date > dtpDen.Value.Date)
            {
                MessageBox.Show("Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Properties.Settings.Default.QuanLyCuaHangBanXeMayConnectionString;

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "BCMatHangNhapTheoNgay";
            cmd.CommandType = CommandType.StoredProcedure;

            if (radTheoNgay.Checked)
            {
                cmd.Parameters.Add(new SqlParameter("@Ngaynhap", dtpTheoNgay.Value.Date));
                cmd.Parameters.Add(new SqlParameter("@TuNgay", DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@DenNgay", DBNull.Value));
            }
            else if (radTheoKhoang.Checked)
            {
                cmd.Parameters.Add(new SqlParameter("@Ngaynhap", DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@TuNgay", dtpTu.Value.Date));
                cmd.Parameters.Add(new SqlParameter("@DenNgay", dtpDen.Value.Date));
            }
            DataSet ds = new DataSet();
            SqlDataAdapter dapChiTiet = new SqlDataAdapter();
            SqlDataAdapter dapTong = new SqlDataAdapter();
            try
            {
                con.Open();
                dapChiTiet.SelectCommand = cmd;
                dapChiTiet.Fill(ds, "dsBCNhapHangTheoNgay");
                cmd.CommandText = "TongMHNhapTheoNgay";
                dapTong.SelectCommand = cmd;
                dapTong.Fill(ds, "dsBCTongMHNhapTheoNgay");
                con.Close();

                rpvBCNhapHang.ProcessingMode = ProcessingMode.Local;
                rpvBCNhapHang.LocalReport.ReportPath = "ReportNH.rdlc";

                rpvBCNhapHang.LocalReport.DataSources.Clear();

                if (ds.Tables["dsBCNhapHangTheoNgay"].Rows.Count > 0)
                {
                    var rdsChiTiet = new ReportDataSource("dsBCNhapHangTheoNgay", ds.Tables["dsBCNhapHangTheoNgay"]);
                    rpvBCNhapHang.LocalReport.DataSources.Add(rdsChiTiet);
                }

                if (ds.Tables["dsBCTongMHNhapTheoNgay"].Rows.Count > 0)
                {
                    var rdsTong = new ReportDataSource("dsBCTongMHNhapTheoNgay", ds.Tables["dsBCTongMHNhapTheoNgay"]);
                    rpvBCNhapHang.LocalReport.DataSources.Add(rdsTong);
                }

                if (rpvBCNhapHang.LocalReport.DataSources.Count > 0)
                {
                    rpvBCNhapHang.RefreshReport();
                }
                else
                {
                    MessageBox.Show("Không có dữ liệu để hiển thị.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Bạn chưa chọn loại báo cáo", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }

        }
    }
}
