using System;
using System.Data;
using DVLD_DataAccess;

namespace DVLD_Buisness
{
    public class clsPerson
    {
        private enum enMode { AddNew, Update};
        private enMode _Mode = enMode.AddNew;

        public enum enGender { Male, Female};

        public int PersonID { private set; get; }
        public string NationalNo { set; get; }
        public string FirstName { set; get; }
        public string SecondName { set; get; }
        public string ThirdName { set; get; }
        public string LastName { set; get; }
        public DateTime DateOfBirth { set; get; }
        public enGender Gender { set; get; }
        public string Address { set; get; }
        public string PhoneNumber { set; get; }
        public string Email { set; get; }
        public int CountryID { set; get; }
        public string ImagePath { set; get; }

        public clsPerson()
        {
            this.PersonID = -1;
            this.NationalNo = "";
            this.FirstName = "";
            this.SecondName = "";
            this.ThirdName = "";
            this.LastName = "";
            this.DateOfBirth = DateTime.Now;
            this.Gender = enGender.Male;
            this.Address = "";
            this.PhoneNumber = "";
            this.Email = "";
            this.CountryID = -1;
            this.ImagePath = "";

            _Mode = enMode.AddNew;
        }

        private clsPerson(int PersonID, string NationalNo, string FirstName,
                           string SecondName, string ThirdName, string LastName,
                           DateTime DateOfBirth, enGender Gender, string Address,
                           string PhoneNumber, string Email, int CountryID, string ImagePath)
        {
            this.PersonID = PersonID;
            this.NationalNo = NationalNo;
            this.FirstName = FirstName;
            this.SecondName = SecondName;
            this.ThirdName = ThirdName;
            this.LastName = LastName;
            this.DateOfBirth = DateOfBirth;
            this.Gender = Gender;
            this.Address = Address;
            this.PhoneNumber = PhoneNumber;
            this.Email = Email;
            this.CountryID = CountryID;
            this.ImagePath = ImagePath;

            _Mode = enMode.Update;
        }

        public bool _AddNewPerson()
        {
            try
            {
                this.PersonID = clsPersonData.AddNewPerson(this.NationalNo, this.FirstName,
                                                   this.SecondName, this.ThirdName,
                                                   this.LastName, this.DateOfBirth,
                                                   (byte)this.Gender, this.Address,
                                                   this.PhoneNumber, this.Email,
                                                   this.CountryID, this.ImagePath);
            }
            catch (Exception ex)
            {
                clsLogger.LogToTextFile(ex);

                return false;
            }
        
            return (this.PersonID != -1);
        }

        private bool _UpdatePerson()
        {
            try
            {
                return clsPersonData.UpdatePerson(
                    this.PersonID,
                    this.NationalNo,
                    this.FirstName,
                    this.SecondName,
                    this.ThirdName,
                    this.LastName,
                    this.DateOfBirth,
                    (byte)this.Gender,
                    this.Address,
                    this.PhoneNumber,
                    this.Email,
                    this.CountryID,
                    this.ImagePath
                );
            }
            catch (Exception ex)
            {
                clsLogger.LogToTextFile(ex);

                return false;
            }
        }

        public static clsPerson Find(int PersonID)
        {
            string NationalNo = "", FirstName = "", SecondName = "", ThirdName = "",
                   LastName = "", Address = "", PhoneNumber = "", Email = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;

            byte Gender = 0;
            int CountryID = -1;

            try
            {
                if (clsPersonData.GetPersonInfoByID(PersonID, ref NationalNo, ref FirstName, ref SecondName,
                                            ref ThirdName, ref LastName, ref Address, ref PhoneNumber,
                                            ref Email, ref ImagePath, ref DateOfBirth, ref Gender,
                                            ref CountryID))
                    return new clsPerson(PersonID, NationalNo, FirstName, SecondName,
                                         ThirdName, LastName, DateOfBirth, (enGender)Gender, Address,
                                         PhoneNumber, Email, CountryID, ImagePath);
                else 
                    return null;
            }
            catch (Exception ex)
            {
                clsLogger.LogToTextFile(ex);

                return null;
            }                 
        }

        public static clsPerson Find(string NationalNo)
        {
            if (string.IsNullOrWhiteSpace(NationalNo))
                return null;

            string FirstName = "", SecondName = "", ThirdName = "", LastName = "",
                   Address = "", PhoneNumber = "", Email = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;

            byte Gender = 0;
            int CountryID = -1, PersonID = -1;

            try
            {
                if (clsPersonData.GetPersonInfoByNationalNo(NationalNo, ref PersonID, ref FirstName, ref SecondName,
                                      ref ThirdName, ref LastName, ref Address, ref PhoneNumber,
                                      ref Email, ref ImagePath, ref DateOfBirth, ref Gender,
                                      ref CountryID))
                    return new clsPerson(PersonID, NationalNo, FirstName, SecondName,
                                         ThirdName, LastName, DateOfBirth, (enGender)Gender, Address,
                                         PhoneNumber, Email, CountryID, ImagePath);
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
                    if (_AddNewPerson())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    return false;

                case enMode.Update:
                    return _UpdatePerson();
            }

            return false;
        }

        public static bool DeletePerson(int PersonID)
        {
            return clsPersonData.DeletePerson(PersonID);
        }

        public static DataTable GetAllPeople()
        {
            try
            {
                return clsPersonData.GetAllPeople();
            }
            catch (Exception ex)
            {
                clsLogger.LogToTextFile(ex);
                return new DataTable();
            }
        }

        public static bool IsPersonExist(int PersonID)
        {
            try
            {
                return clsPersonData.IsPersonExist(PersonID);
            }
            catch(Exception ex)
            {
                clsLogger.LogToTextFile(ex);

                return false;
            }
        }

        public static bool IsPersonExist(string NationalNo)
        {
            try
            {
                return clsPersonData.IsPersonExist(NationalNo);
            }
            catch (Exception ex)
            {
                clsLogger.LogToTextFile(ex);

                return false;
            }
        }

        public static bool IsPersonExist(string NationalNo, int PersonID)
        {
            return clsPersonData.IsPersonExist(NationalNo, PersonID);
        }

        public static bool IsEmailExist(string Email, int PersonID = -1)
        {
            return clsPersonData.IsEmailExist(Email, PersonID);
        }
    }
}
