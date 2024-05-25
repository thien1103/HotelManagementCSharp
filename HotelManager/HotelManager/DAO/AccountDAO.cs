using HotelManager.DTO;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace HotelManager.DAO
{
    public interface IPasswordHashStrategy
    {
        string HashPassword(string password);
    }

    public class MD5PasswordHashStrategy : IPasswordHashStrategy
    {
        public string HashPassword(string password)
        {
            using (var md5 = MD5.Create())
            {
                byte[] hashData = md5.ComputeHash(Encoding.ASCII.GetBytes(password));
                StringBuilder sb = new StringBuilder();
                foreach (var item in hashData)
                {
                    sb.Append(item.ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
    public class SHA256PasswordHashStrategy : IPasswordHashStrategy
    {
        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashData = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder sb = new StringBuilder();
                foreach (var item in hashData)
                {
                    sb.Append(item.ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
    public class AccountDAO
    {

        private static AccountDAO instance;
        private IPasswordHashStrategy passwordHashStrategy;

        public AccountDAO()
        {
            // By default, use MD5 hashing strategy
            passwordHashStrategy = new MD5PasswordHashStrategy();
        }

        public void SetPasswordHashStrategy(IPasswordHashStrategy strategy)
        {
            passwordHashStrategy = strategy;
        }
        public string GetHashedPassword(string password)
        {
            return passwordHashStrategy.HashPassword(password);
        }
        internal bool Login(string userName, string passWord)
        {
            /*string hashPass = HashPass(passWord);*/
            string hashPass = passwordHashStrategy.HashPassword(passWord);
            string query = "USP_Login @userName , @passWord";
            DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[] { userName, hashPass });
            return data.Rows.Count>0;
        }
        internal Account LoadStaffInforByUserName(string username)
        {
            string query = "USP_GetNameStaffTypeByUserName @username";
            DataTable dataTable = DataProvider.Instance.ExecuteQuery(query, new object[] { username });
            //string query = "select * from Staff where UserName='" + username + "'";
            //DataTable dataTable = DataProvider.Instance.ExecuteQuery(query);
            Account account = new Account(dataTable.Rows[0]);
            return account;
        }
        internal bool IsIdCardExists(string idCard)
        {
            string query = "USP_IsIdCardExistsAcc @idCard";
            return DataProvider.Instance.ExecuteQuery(query, new object[] { idCard }).Rows.Count > 0;
        }
        internal bool UpdateDisplayName(string username,string displayname)
        {
            string query = "USP_UpdateDisplayName @username , @displayname";
            return DataProvider.Instance.ExecuteNoneQuery(query, new object[] { username, displayname }) > 0;
        }
        internal bool UpdatePassword(string username, string password)
        {
            string query = "USP_UpdatePassword @username , @password";
            return DataProvider.Instance.ExecuteNoneQuery(query, new object[] { username, passwordHashStrategy.HashPassword(password) }) > 0;
        }
        internal bool UpdateInfo(string username,string address, int phonenumber,string idCard, DateTime dateOfBirth,string sex)
        {
            string query = "USP_UpdateInfo @username , @address , @phonenumber , @idcard , @dateOfBirth , @sex";
            return DataProvider.Instance.ExecuteNoneQuery(query, new object[] { username, address, phonenumber,idCard,dateOfBirth,sex}) > 0;
        }
        internal Account GetStaffSetUp(int idBill)
        {
            string query = "USP_GetStaffSetUp @idBill";
            Account account = new Account(DataProvider.Instance.ExecuteQuery(query, new object[] { idBill }).Rows[0]);
            return account;
        }
        internal DataTable LoadFullStaff()
        {
            string query = "SELECT * FROM VW_LoadFullStaff";
            return DataProvider.Instance.ExecuteQuery(query);
        }
        internal bool CheckUsername(string username)
        {
            string query = "SELECT dbo.F_CheckSensitiveWords @username";
            object result;

            using (SqlConnection connection = new SqlConnection(DataProvider.Instance.connectionStr))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@username", username);

                result = command.ExecuteScalar();
            }

            if (result != null && result != DBNull.Value)
            {
                // Convert the result to a bool
                return Convert.ToBoolean(result);
            }
            else
            {
                return false;
            }
        }
        internal bool InsertAccount(Account account)
        {
            string query = "EXEC USP_InsertStaff @user , @name , @pass , @idStaffType , @idCard , @dateOfBirth , @sex , @address , @phoneNumber , @startDay";
            object[] parameter = new object[] {account.UserName, account.DisplayName, account.PassWord, account.IdStaffType,
                                                account.IdCard, account.DateOfBirth, account.Sex,
                                                account.Address, account.PhoneNumber, account.StartDay};
            return DataProvider.Instance.ExecuteNoneQuery(query, parameter) > 0;
        }
        internal bool UpdateAccount(Account account)
        {
            string query = "EXEC USP_UpdateStaff @user , @name , @idStaffType , @idCard , @dateOfBirth , @sex , @address , @phoneNumber , @startDay";
            object[] parameter = new object[] {account.UserName, account.DisplayName, account.IdStaffType,
                                               account.IdCard, account.DateOfBirth, account.Sex,
                                                account.Address, account.PhoneNumber, account.StartDay};
            return DataProvider.Instance.ExecuteNoneQuery(query, parameter) > 0;
        }
        internal bool ResetPassword(string user, string hashPass)
        {
            string query = "USP_UpdatePassword @user , @hashPass";
            return DataProvider.Instance.ExecuteNoneQuery(query, new object[] { user, hashPass }) > 0;
        }
        internal DataTable Search(string @string, int phoneNumber)
        {
            string query = "USP_SearchStaff @string , @int";
            return DataProvider.Instance.ExecuteQuery(query, new object[] { @string, phoneNumber });
        }
        internal static AccountDAO Instance {
            get { if (instance == null) instance = new AccountDAO();return instance; }
            private set => instance = value; }
    }
}
