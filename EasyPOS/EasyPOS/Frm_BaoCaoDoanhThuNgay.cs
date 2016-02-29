using CoffeeManagement.BLL;
using CoffeeManagement.DAL;
using System;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using DevExpress.XtraCharts;

namespace CoffeeManagement
{
    public partial class Frm_BaoCaoDoanhThuNgay : Form
    {
        HoaDonBLL _hoadonBLL = new HoaDonBLL();
        public Frm_BaoCaoDoanhThuNgay()
        {
            InitializeComponent();
            SetChart();
        }
        private void SetChart()
        {
            chart_DoanhThuTheoNgay.SeriesTemplate.ArgumentDataMember = "Ngay_Lap";
            chart_DoanhThuTheoNgay.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "Tong_Thanh_Toan" });
            chart_DoanhThuTheoNgay.SeriesDataMember = "So_Luong_HD";
            chart_DoanhThuTheoNgay.SeriesTemplate.View = new StackedBarSeriesView();
            chart_DoanhThuTheoNgay.SeriesNameTemplate.BeginText = "Số Lượng: ";
            // Dock the chart into its parent, and add it to the current form.
            chart_DoanhThuTheoNgay.Dock = DockStyle.Fill;

        }
        private void LoadData()
        {
            DateTime d1 = DateTime.Parse(dateEdit1.Text);
            DateTime d2 = DateTime.Parse(dateEdit2.Text);
            grid_DoanhThuTheoNgay.DataSource = _hoadonBLL.ThongKeTheoNgay(d1, d2);
            chart_DoanhThuTheoNgay.DataSource = _hoadonBLL.ThongKeTheoNgay(d1, d2);           
        }

        private void btn_ThongKe_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
