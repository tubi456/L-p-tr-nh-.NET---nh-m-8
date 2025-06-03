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
    public partial class BaoCaoDoanhThu : Form
    {
        public BaoCaoDoanhThu()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void BaoCaoDoanhThu_Load(object sender, EventArgs e)
        {
            rpvBCDoanhTHu.ProcessingMode = ProcessingMode.Local;
            rpvBCDoanhTHu.LocalReport.ReportPath = "ReportDT.rdlc";  // chỉ đường tới file rdlc
            ReportParameter ngayTao = new ReportParameter("NgayTao", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            rpvBCDoanhTHu.LocalReport.SetParameters(ngayTao);
            this.rpvBCDoanhTHu.RefreshReport();
        }

        private void btnTaoBaoCao_Click(object sender, EventArgs e)
        {
            if (radTheoKhoang.Checked && dtpTu.Value.Date > dtpDen.Value.Date)
            {
                MessageBox.Show("Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            SqlConnection con1 = new SqlConnection();
            con1.ConnectionString = Properties.Settings.Default.QuanLyCuaHangBanXeMayConnectionString;

            SqlCommand cmd1 = new SqlCommand();
            cmd1.Connection = con1;
            cmd1.CommandText = "BCDoanhThu";
            cmd1.CommandType = CommandType.StoredProcedure;

            if (radTheoNgay.Checked)
            {
                cmd1.Parameters.Add(new SqlParameter("@Ngayban", dtpTheoNgay.Value.Date));
                cmd1.Parameters.Add(new SqlParameter("@TuNgay", DBNull.Value));
                cmd1.Parameters.Add(new SqlParameter("@DenNgay", DBNull.Value));
            }
            else if (radTheoKhoang.Checked)
            {
                cmd1.Parameters.Add(new SqlParameter("@Ngayban", DBNull.Value));
                cmd1.Parameters.Add(new SqlParameter("@TuNgay", dtpTu.Value.Date));
                cmd1.Parameters.Add(new SqlParameter("@DenNgay", dtpDen.Value.Date));
            }

            DataSet ds1 = new DataSet();
            SqlDataAdapter dapDoanhThu = new SqlDataAdapter();

            try
            {
                con1.Open();
                dapDoanhThu.SelectCommand = cmd1;
                dapDoanhThu.Fill(ds1, "dsBCDoanhThu");
                con1.Close();

                rpvBCDoanhTHu.ProcessingMode = ProcessingMode.Local;
                rpvBCDoanhTHu.LocalReport.ReportPath = "ReportDT.rdlc";

                if (ds1.Tables["dsBCDoanhThu"].Rows.Count > 0)
                {
                    ReportDataSource rdsDoanhThu = new ReportDataSource();
                    rdsDoanhThu.Name = "dsBCDoanhThu";
                    rdsDoanhThu.Value = ds1.Tables["dsBCDoanhThu"];

                    rpvBCDoanhTHu.LocalReport.DataSources.Clear();
                    rpvBCDoanhTHu.LocalReport.DataSources.Add(rdsDoanhThu);
                    rpvBCDoanhTHu.RefreshReport();
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
                if (con1.State == ConnectionState.Open)
                {
                    con1.Close();
                }
            }

        }
    }
}
