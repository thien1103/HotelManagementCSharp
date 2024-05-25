using HotelManager.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelManager
{
    public partial class fAddRoomType : Form
    {
        public fAddRoomType()
        {
            InitializeComponent();
        }
        private void AddRoomType_Load(object sender, EventArgs e)
        {

        }
        private void btnAddRoomType_Click_1(object sender, EventArgs e)
        {
            if (fCustomer.CheckFillInText(new Control[] { txbName, txbPrice, txtLimitPerson }))
            {
                try
                {
                    string name = txbName.Text; // Get the text value from the Bunifu TextBox control
                    int price = Convert.ToInt32(txbPrice.Text); // Convert the text value to an integer
                    int limitPerson = Convert.ToInt32(txtLimitPerson.Text); // Convert the text value to an integer

                    if (RoomTypeDAO.Instance.InsertRoomType(name, price, limitPerson))
                    {
                        MessageBox.Show("Thêm Thành Công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //       this.LoadFullRoomType();
                    }
                    else
                        MessageBox.Show("Loại phòng này đã tồn tại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch
                {
                    MessageBox.Show("Lỗi loại phòng này đã tồn tại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            else
                MessageBox.Show("Không được để trống", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnClose__Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
