using CoffeeManagement.BLL;
using CoffeeManagement.DAL;
using System;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;


namespace CoffeeManagement
{

    public partial class Frm_Phuc_Vu : Form
    {
        const int Ban_Trong = 0;
        const int Ban_Dang_Su_Dung = 1;
        const int Ban_Chua_Don = 2;
        private int thanhtoan = 0;
        private int idBan; // ID của bàn đang được chọn
        private string tenBan;

        KhuVucBLL _khuVucBLL = new KhuVucBLL();
        BanBLL _banBLL = new BanBLL();
        MonBLL _monBLL = new MonBLL();
        DonViBLL _donviBLL = new DonViBLL();
        HoaDonBLL _hoaDonBLL = new HoaDonBLL();
        ThamSoBLL _thamsoBLL = new ThamSoBLL();
        DataTable dtHoaDon = new DataTable();
        public Frm_Phuc_Vu()
        {
            InitializeComponent();
            dt_ThoiGian.DateTime = DateTime.Now;          
        }

        private void LoadDaTaChiTietHoaDon(int idHoaDon)
        {
            List<CHI_TIET_HOA_DON> l = _hoaDonBLL.LayChiTietHoaDonTheoIDHoaDon(idHoaDon);
            dtHoaDon = Utils.Util.ConvertToDataTable(l);
            dtHoaDon.Columns.Add("Don_Vi", typeof(int));
            dtHoaDon.Columns.Add("Don_Gia", typeof(int));
            dtHoaDon.Columns.Add("Thanh_Tien", typeof(int));
            gridControl_HoaDon.DataSource = dtHoaDon;
            for (int i = 0; i < l.Count; i++)
            {
                dtHoaDon.Rows[i]["Don_Vi"] = _monBLL.LayDonViTinh(l[i].ID_Mon);
                dtHoaDon.Rows[i]["Don_Gia"] = _monBLL.LayDonGia(l[i].ID_Mon);
                int t = int.Parse(gridView_HoaDon.GetRowCellValue(i,"So_Luong").ToString()) * (int)_monBLL.LayDonGia(l[i].ID_Mon);
                dtHoaDon.Rows[i]["Thanh_Tien"] = t;
               
            }
            ThanhToan();
            txt_ThanhToan.Text = String.Format("{0:0,0}", ThanhToan());
            tb_VAT.Text = _thamsoBLL.LayVAT().ToString();
            tb_GiamGia.Text = "0";
        }

        private void LoadDataToListView()
        {
            listView1.Clear();
            DataTable dtKhuVuc = Utils.Util.ConvertToDataTable<KHU_VUC>(_khuVucBLL.LayDanhSachKhuVuc());
            for (int i = 0; i < dtKhuVuc.Rows.Count; i++)
            {
                ListViewGroup group = new ListViewGroup(dtKhuVuc.Rows[i][0].ToString(), dtKhuVuc.Rows[i][1].ToString());
                DataTable dtBan = Utils.Util.ConvertToDataTable<BAN>(_banBLL.LayDanhSachBan());
                listView1.Groups.Add(group);
                for (int j = 0; j < dtBan.Rows.Count; j++)
                {
                    if (dtBan.Rows[j]["ID_Khu_Vuc"].ToString() == dtKhuVuc.Rows[i][0].ToString())
                    {
                        if (dtBan.Rows[j]["Trang_Thai"].ToString() == Ban_Trong.ToString())
                        {
                            ListViewItem item = new ListViewItem(dtBan.Rows[j]["Ten_Ban"].ToString(), 0, group);
                            listView1.Items.Add(item);
                        }
                        else
                        {
                            if (dtBan.Rows[i]["Trang_Thai"].ToString() == Ban_Dang_Su_Dung.ToString())
                            {
                                ListViewItem item = new ListViewItem(dtBan.Rows[j]["Ten_Ban"].ToString(), 1, group);
                                listView1.Items.Add(item);
                            }
                            else
                            {
                                ListViewItem item = new ListViewItem(dtBan.Rows[j]["Ten_Ban"].ToString(), 2, group);
                                listView1.Items.Add(item);
                            }
                        }
                    }                   
                }
            }
        }

        private void LoadDataMon()
        {
            gridControl1.DataSource= Utils.Util.ConvertToDataTable(_hoaDonBLL.LayDanhSachCTHoaDon());
        }
        private void LoadDataToGridControlThucDon()
        {
            // Load lkup Loại Món
            LoaiMonBLL _loaiMonBLL = new LoaiMonBLL();
            lkup_LoaiMon.DataSource = Utils.Util.ConvertToDataTable<LOAI_MON>( _loaiMonBLL.LayDanhSachLoaiMon());
            lkup_LoaiMon.DisplayMember = "Ten_Loai_Mon";
            lkup_LoaiMon.ValueMember = "ID_Loai_Mon";


            DataTable dt = Utils.Util.ConvertToDataTable<MON>(_monBLL.LayDanhSachMon());
            gridControl_ThucDon.DataSource = dt;
        }

        private void Frm_Phuc_Vu_Load(object sender, System.EventArgs e)
        {
            LoadDataToListView();
            LoadDataToGridControlThucDon();
            LoadDataMon();
            LoadDonVi();
            LoadThucDon();
            LoadPhucVu();
        }

        private void LoadDonVi()
        {
            lkupDon_Vi.DataSource = Utils.Util.ConvertToDataTable(_donviBLL.LayDanhSachDonVi());
            lkupDon_Vi.ValueMember = "ID_Don_Vi";
            lkupDon_Vi.DisplayMember = "Ten_Don_Vi";
        }
        private void LoadPhucVu()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("TrangThai", typeof(string));
            DataRow dr1 = dt.NewRow();
            dr1["ID"] = 0;
            dr1["TrangThai"] = "False";
            dt.Rows.Add(dr1);
            DataRow dr2 = dt.NewRow();
            dr2["ID"] = 1;
            dr2["TrangThai"] = "True";
            dt.Rows.Add(dr2);
            lkupPhucVu.DataSource = dt;
            lkupPhucVu.ValueMember = "ID";
            lkupPhucVu.DisplayMember = "TrangThai";
        }
        private void LoadThucDon()
        {
            lkupMon.DataSource = Utils.Util.ConvertToDataTable(_monBLL.LayDanhSachMon());
            lkupMon.ValueMember = "ID_Mon";
            lkupMon.DisplayMember = "Ten_Mon";
            lkup_mon.DataSource = Utils.Util.ConvertToDataTable(_monBLL.LayDanhSachMon());
            lkup_mon.ValueMember = "ID_Mon";
            lkup_mon.DisplayMember = "Ten_Mon";
        }
        #region Custom Context Menu
        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (listView1.FocusedItem.Bounds.Contains(e.Location) == true)
                {
                    contextMenuStrip1.Show(Cursor.Position);
                }
            }
            var b = listView1.SelectedItems[0];
            idBan = _banBLL.LayIDTheoBan(b.SubItems[0].Text);
            tenBan = b.SubItems[0].Text;
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (listView1.SelectedItems.Count <= 0)
            {
                e.Cancel = true;
                return;
            }
            // Custom menu với các loại bàn khác nhau
            var b = listView1.SelectedItems[0];
            switch(b.ImageIndex)
            {
                case 0:
                    // Bàn trống hiện menu: Tạo Hóa Đơn
                    menu_DonBan.Visible = false;
                    menu_ChuyenBan.Visible = false;
                    menu_InHoaDon.Visible = false;
                    menu_InTamTinh.Visible = false;
                    menu_ThanhToan.Visible = false;
                    menu_XemChiTiet.Visible = false;
                    sep1.Visible = false;           // Hide Sperator

                    menu_ToaHD.Visible = true;

                    break;
                case 1:
                    // Bàn đang sử dụng: Chuyển bàn, Thanh toán, In Tạm Tính, In Hóa Đơn
                    menu_DonBan.Visible = false;
                    menu_ToaHD.Visible = false;

                    menu_XemChiTiet.Visible = true;
                    menu_ChuyenBan.Visible = true;
                    menu_InHoaDon.Visible = true;
                    menu_InTamTinh.Visible = true;
                    menu_ThanhToan.Visible = true;


                    sep1.Visible = true;           // Hide Sperator

                    break;
                case 2:
                    // Bàn chưa dọn: Dọn bàn
                    menu_DonBan.Visible = true;

                    menu_ChuyenBan.Visible = false;
                    menu_InHoaDon.Visible = false;
                    menu_InTamTinh.Visible = false;
                    menu_ThanhToan.Visible = false;
                    menu_XemChiTiet.Visible = false;
                    menu_ToaHD.Visible = false;

                    sep1.Visible = false;           // Hide Sperator

                    break;
            }
            
        }
        #endregion

        // Cập nhật dọn bàn.
        private void menu_DonBan_Click(object sender, System.EventArgs e)
        {
            if(idBan != -1)
            {
                _banBLL.CapNhatTinhTrangBan(0, idBan);
            }
            LoadDataToListView();
        }

        // Tạo hóa đơn mới
        private void menu_ToaHD_Click(object sender, EventArgs e)
        {
            HOA_DON h = new HOA_DON();
            h.ID_Ban = idBan;
            // h.ID_Nguoi_Dung =
            h.Ngay_Lap = dt_ThoiGian.DateTime;
            h.Trang_Thai_Thanh_Toan = 1;
          
            XemChiTietHoaDon(_hoaDonBLL.ThemHoaDonMoi(h));

            // Cập nhật tình trạng bàn thành đang sử dụng
            _banBLL.CapNhatTinhTrangBan(1, idBan);
            LoadDataToListView();
        }


        private void XemChiTietHoaDon(int idHoaDon)
        {
            var h = _hoaDonBLL.LayHoaDonTheoID(idHoaDon);

            dt_ThoiGian.DateTime = (DateTime)h.Ngay_Lap;
            txt_Ban.Text = tenBan;
            txt_KhuVuc.Text = _banBLL.LayKhuVucTheoIDBan(idBan);
            txt_SoHoaDon.Text = idHoaDon.ToString();

            LoadDaTaChiTietHoaDon(idHoaDon);
        }


        //Button thêm món mới cho bàn sử dụng
        private void bnt_ThemMon_Click(object sender, EventArgs e)
        {
            if(!String.IsNullOrEmpty(txt_SoHoaDon.Text))
            {
                CHI_TIET_HOA_DON ct = new CHI_TIET_HOA_DON();
                ct.ID_Mon = int.Parse(gridView_ThucDon.GetRowCellValue(gridView_ThucDon.FocusedRowHandle,
                    "ID_Mon").ToString());
                ct.So_Luong = 1;
                int idHoaDon = int.Parse(txt_SoHoaDon.Text);
                ct.ID_Hoa_Don = idHoaDon;
                _hoaDonBLL.ThemChiTietHoaDon(ct);

                LoadDaTaChiTietHoaDon(idHoaDon);
            }
            else
            {
                Utils.Notification.Error("Bạn chưa chọn hóa đơn.");
            }
        }

        private void menu_XemChiTiet_Click(object sender, EventArgs e)
        {
            XemChiTietHoaDon(_hoaDonBLL.LayIDHoaDonTheoBan(idBan));
        }


        private int ThanhToan()
        {
            int t = 0;
            for(int i=0; i< dtHoaDon.Rows.Count;i++)
            {
                t += int.Parse(gridView_HoaDon.GetRowCellValue(i, "Thanh_Tien").ToString());
            }
            txt_TongThanhTien.Text = String.Format("{0:0,0}", t) + "  " + "VNĐ";
            return t;
        }
        private void lkupMon_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            int i = gridView_HoaDon.FocusedRowHandle;
            dtHoaDon.Rows[i]["Don_Vi"] = _monBLL.LayDonViTinh(int.Parse(e.NewValue.ToString()));
            dtHoaDon.Rows[i]["Don_Gia"] = _monBLL.LayDonGia(int.Parse(e.NewValue.ToString()));
            int t = int.Parse(gridView_HoaDon.GetRowCellValue(i, "So_Luong").ToString()) * (int)_monBLL.LayDonGia(int.Parse(e.NewValue.ToString()));
            dtHoaDon.Rows[i]["Thanh_Tien"] = t;
            ThanhToan();
        }

        private void spinSoluong_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            int i = gridView_HoaDon.FocusedRowHandle;
            int t = int.Parse(e.NewValue.ToString()) * int.Parse(gridView_HoaDon.GetFocusedRowCellValue("Don_Gia").ToString());
            dtHoaDon.Rows[i]["Thanh_Tien"] = t;
        }

        private void btn_HuyGoiMon_Click(object sender, EventArgs e)
        {
            if(gridView_HoaDon.GetFocusedRowCellValue("Trang_Thai_Phuc_Vu").ToString()=="0")
            {
                int k = int.Parse(dtHoaDon.Rows[gridView_HoaDon.FocusedRowHandle]["ID_Chi_Tiet_Hoa_Don"].ToString());
                _hoaDonBLL.XoaCTHoaDon(k);
                MessageBox.Show("Xóa Món thành công.");
                LoadDaTaChiTietHoaDon(_hoaDonBLL.LayIDHoaDonTheoBan(idBan));
            }
            else MessageBox.Show("Món đã phục vụ, không được xóa!!!");
        }

        private void btn_HuyHoaDon_Click(object sender, EventArgs e)
        {
            DialogResult result1 = MessageBox.Show("Bạn chắc muốn hủy hóa đơn?","Hủy Hóa Đơn",MessageBoxButtons.YesNo);
            if (result1 == DialogResult.Yes)
            {
                _hoaDonBLL.CapNhatTrangThaiThanhToanHD(-1, _hoaDonBLL.LayIDHoaDonTheoBan(idBan));
            }
        }

        private void tb_KhachDuaTien_EditValueChanged(object sender, EventArgs e)
        {
            tb_TienTraLai.Text = (int.Parse(tb_KhachDuaTien.Text) - thanhtoan).ToString();
        }
        private HOA_DON CapNhatHoaDon()
        {
            HOA_DON h = new HOA_DON();
            h.ID_Hoa_Don = _hoaDonBLL.LayIDHoaDonTheoBan(idBan);
            h.Tien_Khach_Dua = int.Parse(tb_KhachDuaTien.Text);
            h.Tien_Tra_Lai = int.Parse(tb_TienTraLai.Text);
            h.Tong_Tien = ThanhToan();
            h.Tong_Thanh_Toan = thanhtoan;
            h.VAT = int.Parse(tb_VAT.Text);
            return h;
        }
        private void btn_ThanhToan_Click(object sender, EventArgs e)
        {
            try {
                _hoaDonBLL.CapNhatTrangThaiThanhToanHD(1, _hoaDonBLL.LayIDHoaDonTheoBan(idBan));
                _banBLL.CapNhatTinhTrangBan(-1, idBan);
                _hoaDonBLL.CapNhatHoaDon(CapNhatHoaDon());
                MessageBox.Show("Thanh toán thành công.");
            }
            catch(Exception ex)
            {
                MessageBox.Show("Thanh toán không thành công!!!: " + ex.Message);
            }
        }

        private void tb_VAT_EditValueChanged(object sender, EventArgs e)
        {
            if (tb_VAT.Text != "" && tb_GiamGia.Text != "")
            {
                int t = ThanhToan();
                if (check_GiamGiaTien.Checked == true)
                {
                    thanhtoan = t - int.Parse(tb_GiamGia.Text) + (t * int.Parse(tb_VAT.Text)) / 100;
                    txt_ThanhToan.Text = String.Format("{0:0,0}", thanhtoan);
                }
                else
                {
                    thanhtoan= t - (t * int.Parse(tb_GiamGia.Text)) / 100 + (t * int.Parse(tb_VAT.Text)) / 100;
                    txt_ThanhToan.Text = String.Format("{0:0,0}", thanhtoan);
                }
            }
            if(tb_KhachDuaTien.Text!="")
            {
                tb_TienTraLai.Text = (int.Parse(tb_KhachDuaTien.Text) - thanhtoan).ToString();
            }
        }

        private void tb_GiamGia_EditValueChanged(object sender, EventArgs e)
        {
            if (tb_VAT.Text != "" && tb_GiamGia.Text != "")
            {
                int t = ThanhToan();
                if (check_GiamGiaTien.Checked == true)
                {
                    thanhtoan = t - int.Parse(tb_GiamGia.Text) + (t * int.Parse(tb_VAT.Text)) / 100;
                    txt_ThanhToan.Text = String.Format("{0:0,0}", thanhtoan);
                }
                else
                {
                    thanhtoan = t - (t * int.Parse(tb_GiamGia.Text)) / 100 + (t * int.Parse(tb_VAT.Text)) / 100;
                    txt_ThanhToan.Text = String.Format("{0:0,0}", thanhtoan);
                }
            }
            if (tb_KhachDuaTien.Text != "")
            {
                tb_TienTraLai.Text = (int.Parse(tb_KhachDuaTien.Text) - thanhtoan).ToString();
            }
        }
    }
}
