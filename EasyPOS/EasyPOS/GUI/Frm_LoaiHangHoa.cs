using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace EasyPOS
{
    public partial class Frm_LoaiHangHoa : Form
    {
        public Frm_LoaiHangHoa()
        {
            InitializeComponent();
        }

        private void Frm_LoaiHangHoa_Load(object sender, EventArgs e)
        {
            gridView1.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
        }
    }
}
