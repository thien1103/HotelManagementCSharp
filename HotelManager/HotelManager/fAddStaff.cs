using HotelManager.DAO;
using HotelManager.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace HotelManager
{
    public partial class fAddStaff : Form
    {
        public fAddStaff()
        {
            InitializeComponent();
            LoadFullStaffType();
        }

        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thêm nhân viên mới không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.OK)
            {

                if (CheckDate())
                {
                    InsertStaff();
                }
            }

        }
        private void TxbPhoneNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }
        /*private void TxbUserName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsLetter(e.KeyChar) || char.IsNumber(e.KeyChar) || e.KeyChar == '\b' || e.KeyChar == '.' || e.KeyChar == '-' ||
       e.KeyChar == '_' || e.KeyChar == '@' || e.KeyChar == (char)Keys.Space))
                e.Handled= true;
                // ^ ([a - zA - Z0 - 9\.\-_ ?@] +)$

        }*/
        private void LoadFullStaffType()
        {
            comboBoxSex.SelectedIndex = 0;
            DataTable table = GetFullStaffType();
            comboBoxStaffType.DataSource = table;
            comboBoxStaffType.DisplayMember = "Name";
            if (table.Rows.Count > 0)
                comboBoxStaffType.SelectedIndex = 0;
        }
        private DataTable GetFullStaffType()
        {
            return AccountTypeDAO.Instance.LoadFullStaffType();
        }

        private void InsertStaff()
        {
            //string usernameVerify = txbName.Text;
            string usernameVerify = TxbUserName.Text;
            bool isFill = fCustomer.CheckFillInText(new Control[] { TxbUserName, comboBoxStaffType, txbFullName ,
                                     txbIDcard , comboBoxSex , txbPhoneNumber, txbAddress});
            if (isFill)
            {
                try
                {
                    // Check if the username is acceptable
                    bool isUsernameValid = AccountDAO.Instance.CheckUsername(usernameVerify);

                    if (!isUsernameValid)
                    {
                        Account accountNow = GetStaffNow();
                        accountNow.PassWord = fStaff.HassPass;

                        if (AccountDAO.Instance.InsertAccount(accountNow))
                        {
                            MessageBox.Show("Thêm Thành Công\n Mật khẩu mặc đinh cho tài khoản " + TxbUserName.Text +
                                ": 123456", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Nhân Viên Đã Tồn Tại(Trùng tên đăng nhập hoặc Số CMND)", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Tên đăng nhập chứa từ không phù hợp. Vui lòng chọn tên khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Không được để trống", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private Account GetStaffNow()
        {
            Account account = new Account();

            #region Format
            fStaff.Trim(new Bunifu.Framework.UI.BunifuMetroTextbox[] { TxbUserName, txbIDcard, txbAddress });
            #endregion

            account.UserName = TxbUserName.Text;
            int index = comboBoxStaffType.SelectedIndex;
            account.IdStaffType = (int)((DataTable)comboBoxStaffType.DataSource).Rows[index]["id"];
            account.DisplayName = txbFullName.Text;
            account.IdCard = txbIDcard.Text;
            account.Sex = comboBoxSex.Text;
            account.DateOfBirth = datepickerDateOfBirth.Value;
            account.PhoneNumber = int.Parse(txbPhoneNumber.Text);
            account.Address = txbAddress.Text;
            account.StartDay = datePickerStartDay.Value;
            return account;
        }

        private bool CheckDate()
        {
            if (!CheckTrueDate(datepickerDateOfBirth.Value, DateTime.Now))
            {
                MessageBox.Show("Ngày sinh không hợp lệ (Tuổi phải lớn hơn 18)", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
                if (!CheckTrueDate(datepickerDateOfBirth.Value, datePickerStartDay.Value))
            {
                MessageBox.Show("Ngày vào làm không hợp lệ (Lớn hơn 18 tuổi)", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        private bool CheckTrueDate(DateTime date1, DateTime date2)
        {
            if (date2.Subtract(date1).Days < 6574)
                return false;
            return true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void fAddStaff_Load(object sender, EventArgs e)
        {
            datePickerStartDay.Value = DateTime.Now;
            comboBoxSex.SelectedIndex = 1;
        }

        private void TxbUserName_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
    }

}
