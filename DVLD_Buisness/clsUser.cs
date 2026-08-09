using DVLD_DataAccess;
using System;
using System.Data;
using static DVLD_Buisness.clsPerson;
using BCrypt.Net;

namespace DVLD_Buisness
{
    public class clsUser
    {
        private enum enMode { AddNew, Update };
        private enMode _Mode = enMode.AddNew;

        public clsPerson PersonInfo => clsPerson.Find(this.PersonID);

        public int UserID { private set; get; }
        public int PersonID { private set; get; }
        public string UserName { set; get; }
        public string Password { set; get; }
        public bool IsActive { set; get; }

        private string _OriginalPassword;

        public clsUser(int PersonID)
        {
            this.UserID = -1;
            this.PersonID = PersonID;
            this.UserName = "";
            this.Password = "";
            this.IsActive = true;

            this._OriginalPassword = "";
            _Mode = enMode.AddNew;
        }

        private clsUser(int UserID, int PersonID, string UserName, string Password, bool IsActive)
        {
            this.UserID = UserID;
            this.PersonID = PersonID;
            this.UserName = UserName;
            this.Password = Password;
            this.IsActive = IsActive;

            this._OriginalPassword = Password;

            _Mode = enMode.Update;
        }

        private bool _AddNewUser()
        {
            try
            {
                this.UserID = clsUserData.AddNewUser(this.PersonID, this.UserName,
                                                     this.Password, this.IsActive);
            }
            catch (Exception ex)
            {
                clsLogger.LogToTextFile(ex);

                return false;
            }

            return (this.UserID != -1);
        }

        private bool _UpdateUser()
        {
            try
            {
                return clsUserData.UpdateUser(this.UserID, this.UserName,
                                              this.Password, this.IsActive);
            }
            catch (Exception ex)
            {
                clsLogger.LogToTextFile(ex);

                return false;
            }
        }

        public static clsUser Find(int UserID)
        {
            string UserName = "", Password = "";

            bool IsActive = true;
            int PersonID = -1;

            try
            {
                if (clsUserData.GetUserInfoByID(UserID, ref PersonID, ref UserName,
                                                ref Password, ref IsActive))
                    return new clsUser(UserID, PersonID, UserName, Password, IsActive);
                else
                    return null;
            }
            catch (Exception ex)
            {
                clsLogger.LogToTextFile(ex);

                return null;
            }
        }

        public static clsUser Find(string UserName)
        {
            int PersonID = -1, UserID = -1;

            string Password = "";
            bool IsActive = true;

            try
            {
                if (clsUserData.GetUserInfoByUsername(UserName, ref UserID, ref PersonID,
                                                      ref Password, ref IsActive))
                    return new clsUser(UserID, PersonID, UserName, Password, IsActive);
                else
                    return null;
            }
            catch (Exception ex)
            {
                clsLogger.LogToTextFile(ex);

                return null;
            }
        }

        public static clsUser FindByUsernameAndPassword(string UserName, string PlainPassword)
        {
            clsUser user = clsUser.Find(UserName);

            if (user == null)
                return null;

            try
            {
                if (BCrypt.Net.BCrypt.Verify(PlainPassword, user.Password))
                    return user;
                else
                    return null;
            }
            catch (Exception ex)
            {
                clsLogger.LogToTextFile(ex);

                return null;
            }
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    this.Password = BCrypt.Net.BCrypt.HashPassword(this.Password);

                    if (_AddNewUser())
                    {
                        this._OriginalPassword = this.Password;
                        _Mode = enMode.Update;
                        return true;
                    }

                    return false;

                case enMode.Update:
                    if (this.Password != this._OriginalPassword)
                    {
                        this.Password = BCrypt.Net.BCrypt.HashPassword(this.Password);
                    }

                    if (_UpdateUser())
                    {
                        this._OriginalPassword = this.Password;
                        return true;
                    }

                    return false;
            }

            return false;
        }

        public static bool DeleteUser(int UserID)
        {
            return clsUserData.DeleteUser(UserID);
        }

        public static DataTable GetAllUsers()
        {
            try
            {
                return clsUserData.GetAllUsers();
            }
            catch (Exception ex)
            {
                clsLogger.LogToTextFile(ex);
                return new DataTable();
            }
        }

        public static bool IsUserExist(int UserID)
        {
            try
            {
                return clsUserData.IsUserExist(UserID);
            }
            catch (Exception ex)
            {
                clsLogger.LogToTextFile(ex);

                return false;
            }
        }

        public static bool IsUserExist(string UserName)
        {
            try
            {
                return clsUserData.IsUserExist(UserName);
            }
            catch (Exception ex)
            {
                clsLogger.LogToTextFile(ex);

                return false;
            }
        }

        public static bool IsUserExistForPersonID(int PersonID)
        {
            try
            {
                return clsUserData.IsUserExistForPersonID(PersonID);
            }
            catch (Exception ex)
            {
                clsLogger.LogToTextFile(ex);

                return false;
            }
        }
    }
}
