using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.DAO
{
    public interface IAccessDAO
    {
        DataTable GetFullAccessNow(int idStaffType);
        DataTable GetFullAccessRest(int idStaffType);
        void Insert(object idJob, int idStaffType);
        void Delete(int idJob, int idStaffType);
        bool CheckAccess(string username, string formName);
    }

    public class AccessDAO : IAccessDAO
    {
        private readonly DataProvider dataProvider;
        private static AccessDAO instance;

        private AccessDAO()
        {
            dataProvider = new DataProvider();
        }

        public static AccessDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AccessDAO();
                }
                return instance;
            }
        }


        /*public DataTable GetFullAccessNow(int idStaffType)
        {
            string query = "USP_LoadFullAccessNow @idStaffType";
            return dataProvider.ExecuteQuery(query, new object[] { idStaffType });
        }*/
        public DataTable GetFullAccessNow(int idStaffType)
        {
            string query = "SELECT Name, ID FROM VW_LoadFullAccessNow WHERE IDStaffType = @idStaffType";
            return dataProvider.ExecuteQuery(query, new object[] { idStaffType });
        }

        public DataTable GetFullAccessRest(int idStaffType)
        {
            string query = "USP_LoadFullAccessRest @idStaffType";
            return dataProvider.ExecuteQuery(query, new object[] { idStaffType });
        }

        public void Insert(object idJob, int idStaffType)
        {
            string query = "USP_InsertAccess @idjob , @idStafftype";
            dataProvider.ExecuteNoneQuery(query, new object[] { idJob, idStaffType });
        }

        public void Delete(int idJob, int idStaffType)
        {
            if (idJob == 6 && idStaffType == 1) return;
            string query = "USP_DeleteAccess @idjob , @idStafftype";
            dataProvider.ExecuteNoneQuery(query, new object[] { idJob, idStaffType });
        }

        public bool CheckAccess(string username, string formName)
        {
            string query = "USP_ChekcAccess @username , @formname";
            return !(dataProvider.ExecuteScalar(query, new object[] { username, formName }) is null);
        }
    }

    public class AccessDAOWrapper : IAccessDAO
    {
        private readonly IAccessDAO accessDAO;

        public AccessDAOWrapper(IAccessDAO accessDAO)
        {
            this.accessDAO = accessDAO;
        }

        public DataTable GetFullAccessNow(int idStaffType)
        {
            // Additional logic before calling the original method
            Console.WriteLine("Performing additional logic before calling GetFullAccessNow");

            // Call the original method
            return accessDAO.GetFullAccessNow(idStaffType);
        }

        public DataTable GetFullAccessRest(int idStaffType)
        {
            // Additional logic before calling the original method
            Console.WriteLine("Performing additional logic before calling GetFullAccessRest");

            // Call the original method
            return accessDAO.GetFullAccessRest(idStaffType);
        }

        public void Insert(object idJob, int idStaffType)
        {
            // Additional logic before calling the original method
            Console.WriteLine("Performing additional logic before calling Insert");

            // Call the original method
            accessDAO.Insert(idJob, idStaffType);
        }

        public void Delete(int idJob, int idStaffType)
        {
            // Additional logic before calling the original method
            Console.WriteLine("Performing additional logic before calling Delete");

            // Call the original method
            accessDAO.Delete(idJob, idStaffType);
        }

        public bool CheckAccess(string username, string formName)
        {
            // Additional logic before calling the original method
            Console.WriteLine("Performing additional logic before calling CheckAccess");

            // Call the original method
            return accessDAO.CheckAccess(username, formName);
        }
    }
}