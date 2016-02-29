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
    public partial class Frm_ThongKeMatHangBan : Form
    {
        HoaDonBLL _hoadonBLL = new HoaDonBLL();
        public Frm_ThongKeMatHangBan()
        {
            InitializeComponent();
            SetChart();
        }
        private void SetChart()
        {
            chartControl1.SeriesTemplate.ArgumentDataMember = "Ten_Mon";
            chartControl1.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "Tong_Thanh_Toan" });
            chartControl1.SeriesDataMember = "So_Luong";
            chartControl1.SeriesTemplate.View = new StackedBarSeriesView();
            chartControl1.SeriesNameTemplate.BeginText = "So_Luong: ";
            // Dock the chart into its parent, and add it to the current form.
            chartControl1.Dock = DockStyle.Fill;
        }
        private void LoadData()
        {
            DateTime d1 = DateTime.Parse(dateEdit1.Text);
            DateTime d2 = DateTime.Parse(dateEdit2.Text);
            gridControl1.DataSource = _hoadonBLL.ThongKeTheoMatHang(d1, d2);
            chartControl1.DataSource = _hoadonBLL.ThongKeTheoMatHang(d1, d2);    
        }

        private void btn_ThongKe_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}

