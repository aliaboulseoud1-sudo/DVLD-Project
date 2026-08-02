using DVLD_DataAccess;
using System;
using System.Data;

namespace DVLD_Buisness
{
    public class clsCountry
    {
        private enum enMode { AddNew, Update };
        private enMode _Mode = enMode.AddNew;

        public int CountryID { private set; get; }
        public string CountryName { set; get; }

        public clsCountry()
        {
            this.CountryID = -1;
            this.CountryName = "";

            _Mode = enMode.AddNew;
        }

        private clsCountry(int CountryID, string CountryName)
        {
            this.CountryID = CountryID;
            this.CountryName = CountryName;

            _Mode = enMode.Update;
        }

        public bool _AddNewCountry()
        {
            try
            {
                this.CountryID = clsCountryData.AddNewCountry(this.CountryName);
            }
            catch (Exception ex)
            {
                clsLogger.LogToTextFile(ex);

                return false;
            }

            return (this.CountryID != -1);
        }

        private bool _UpdateCountry()
        {
            try
            {
                return clsCountryData.UpdateCountry(this.CountryID, this.CountryName);
            }
            catch (Exception ex)
            {
                clsLogger.LogToTextFile(ex);

                return false;
            }
        }

        public static clsCountry Find(int CountryID)
        {
            string CountryName = "";

            try
            {
                if (clsCountryData.GetCountryInfoByID(CountryID, ref CountryName))
                    return new clsCountry(CountryID, CountryName);
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
                    if (_AddNewCountry())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    return false;

                case enMode.Update:
                    return _UpdateCountry();
            }

            return false;
        }

        public static bool DeleteCountry(int CountryID)
        {
            return clsCountryData.DeleteCountry(CountryID);
        }

        public static DataTable GetAllCountries()
        {
            try
            {
                return clsCountryData.GetAllCountries();
            }
            catch (Exception ex)
            {
                clsLogger.LogToTextFile(ex);
                return new DataTable();
            }
        }

        public static bool IsCountryExist(int CountryID)
        {
            try
            {
                return clsCountryData.IsCountryExist(CountryID);
            }
            catch (Exception ex)
            {
                clsLogger.LogToTextFile(ex);

                return false;
            }
        }

        public static bool IsCountryExist(string CountryName)
        {
            try
            {
                return clsCountryData.IsCountryExist(CountryName);
            }
            catch (Exception ex)
            {
                clsLogger.LogToTextFile(ex);

                return false;
            }
        }
    }
}
