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
    public partial class Frm_ThongKeTonKho : Form
    {
        HoaDonBLL _hoadonBLL = new HoaDonBLL();
        public Frm_ThongKeTonKho()
        {
            InitializeComponent();
            LoadData();
        }
        private void SetChart()
        {
            chart_BaoCaoTon.SeriesDataMember = "So_Luong_Nhap";
            chart_BaoCaoTon.SeriesTemplate.ArgumentDataMember = "Ten_Nguyen_Lieu";
            chart_BaoCaoTon.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "So_Luong_Ton" });
            // Specify the template's series view.
            chart_BaoCaoTon.SeriesTemplate.View = new StackedBarSeriesView();

            // Specify the template's name prefix.
            chart_BaoCaoTon.SeriesNameTemplate.BeginText = "Nhập vào: ";

            // Dock the chart into its parent, and add it to the current form.
            chart_BaoCaoTon.Dock = DockStyle.Fill;

        }
        private void LoadData()
        {
            dateEdit1.Text = DateTime.Today.Date.ToShortDateString();
            gridControl1.DataSource = _hoadonBLL.ThongKeTheoTonKho();
            SetChart();
            chart_BaoCaoTon.DataSource = _hoadonBLL.ThongKeTheoTonKho();
            
        }


    }
}

