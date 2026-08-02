using System;
using System.Data;
using System.Data.SqlClient;


namespace DVLD_DataAccess
{
    public class clsPersonData
    {
        public static int AddNewPerson(string NationalNo, string FirstName,
                                       string SecondName, string ThirdName,
                                       string LastName, DateTime DateOfBirth,
                                       byte Gender, string Address,
                                       string PhoneNumber, string Email,
                                       int CountryID, string ImagePath)
        {
            int PersonID = -1;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"INSERT INTO People (NationalNo, FirstName, SecondName, ThirdName, LastName,
                                                     DateOfBirth, Gendor, Address, Phone, Email,
                                                     NationalityCountryID, ImagePath)
                                 VALUES (@NationalNo, @FirstName, @SecondName, @ThirdName, @LastName, @DateOfBirth,
                                         @Gender, @Address, @PhoneNumber, @Email, @CountryID, @ImagePath);
                                 SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NationalNo", NationalNo);
                    command.Parameters.AddWithValue("@FirstName", FirstName);
                    command.Parameters.AddWithValue("@SecondName", SecondName);
                    command.Parameters.AddWithValue("@LastName", LastName);
                    command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                    command.Parameters.AddWithValue("@Gender", Gender);
                    command.Parameters.AddWithValue("@Address", Address);
                    command.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
                    command.Parameters.AddWithValue("@CountryID", CountryID);

                    command.Parameters.AddWithValue("@ThirdName",
                        !string.IsNullOrWhiteSpace(ThirdName) ? (object)ThirdName : DBNull.Value);

                    command.Parameters.AddWithValue("@Email",
                        !string.IsNullOrWhiteSpace(Email) ? (object)Email : DBNull.Value);

                    command.Parameters.AddWithValue("@ImagePath",
                        !string.IsNullOrWhiteSpace(ImagePath) ? (object)ImagePath : DBNull.Value);

                    try
                    {
                        connection.Open();

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int insertedPersonID))
                        {
                            PersonID = insertedPersonID;
                        }
                    }
                    catch (SqlException sqlEx)
                    {
                        clsDataAccessLogger.LogException(sqlEx);
                    }
                    catch (Exception ex)
                    {
                        clsDataAccessLogger.LogException(ex);
                    }
                }
            }

            return PersonID;
        }

        public static bool UpdatePerson(int PersonID, string NationalNo, string FirstName,
                                string SecondName, string ThirdName, string LastName,
                                DateTime DateOfBirth, byte Gender, string Address,
                                string PhoneNumber, string Email, int CountryID, string ImagePath)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"UPDATE People  
                         SET NationalNo = @NationalNo,
                             FirstName = @FirstName, 
                             SecondName = @SecondName,
                             ThirdName = @ThirdName,
                             LastName = @LastName,
                             DateOfBirth = @DateOfBirth,
                             Gendor = @Gender,
                             Address = @Address,
                             Phone = @PhoneNumber,                               
                             Email = @Email,
                             NationalityCountryID = @CountryID,
                             ImagePath = @ImagePath                  
                         WHERE PersonID = @PersonID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@NationalNo", NationalNo);
                    command.Parameters.AddWithValue("@FirstName", FirstName);
                    command.Parameters.AddWithValue("@SecondName", SecondName);
                    command.Parameters.AddWithValue("@LastName", LastName);
                    command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                    command.Parameters.AddWithValue("@Gender", Gender);
                    command.Parameters.AddWithValue("@Address", Address);
                    command.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
                    command.Parameters.AddWithValue("@CountryID", CountryID);

                    command.Parameters.AddWithValue("@ThirdName",
                       !string.IsNullOrWhiteSpace(ThirdName) ? (object)ThirdName : DBNull.Value);
                    command.Parameters.AddWithValue("@Email",
                       !string.IsNullOrWhiteSpace(Email) ? (object)Email : DBNull.Value);
                    command.Parameters.AddWithValue("@ImagePath",
                        !string.IsNullOrWhiteSpace(ImagePath) ? (object)ImagePath : DBNull.Value);

                    try
                    {
                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                    catch (SqlException sqlEx)
                    {
                        clsDataAccessLogger.LogException(sqlEx);
                    }
                    catch (Exception ex)
                    {
                        clsDataAccessLogger.LogException(ex);
                    }
                }
            }

            return (rowsAffected > 0);
        }

        public static bool GetPersonInfoByID(int PersonID, ref string NationalNo, ref string FirstName, ref string SecondName,
                                                ref string  ThirdName, ref string  LastName, ref string  Address, ref string PhoneNumber,
                                                ref string Email, ref string  ImagePath, ref DateTime DateOfBirth, ref byte Gender,
                                                ref int CountryID)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SELECT * FROM People
                                 WHERE PersonID = @PersonID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;

                                NationalNo = (string)reader["NationalNo"];
                                FirstName = (string)reader["FirstName"];
                                SecondName = (string)reader["SecondName"];
                                LastName = (string)reader["LastName"];
                                DateOfBirth = (DateTime)reader["DateOfBirth"];
                                Gender = (byte)reader["Gendor"];
                                Address = (string)reader["Address"];
                                PhoneNumber = (string)reader["Phone"];
                                CountryID = (int)reader["NationalityCountryID"];

                                if (reader["ThirdName"] != DBNull.Value)
                                    ThirdName = (string)reader["ThirdName"];
                                else
                                    ThirdName = "";

                                if (reader["Email"] != DBNull.Value)
                                    Email = (string)reader["Email"];
                                else
                                    Email = "";

                                if (reader["ImagePath"] != DBNull.Value)
                                    ImagePath = (string)reader["ImagePath"];
                                else
                                    ImagePath = "";
                            }
                            else
                            {
                                isFound = false;
                            }
                        }                          
                    }
                    catch(SqlException sqlEx)
                    {
                        clsDataAccessLogger.LogException(sqlEx);

                        isFound = false;
                    }
                    catch (Exception ex)
                    {
                       clsDataAccessLogger.LogException(ex);

                        isFound = false;
                    }
                }
            }

            return isFound;
        }

        public static bool GetPersonInfoByNationalNo(string NationalNo, ref int PersonID, ref string FirstName, ref string SecondName,
                                                ref string ThirdName, ref string LastName, ref string Address, ref string PhoneNumber,
                                                ref string Email, ref string ImagePath, ref DateTime DateOfBirth, ref byte Gender,
                                                ref int CountryID)
        {
            if (string.IsNullOrWhiteSpace(NationalNo))
                return false;

            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SELECT * FROM People
                                 WHERE NationalNo = @NationalNo";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NationalNo", NationalNo);

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;

                                PersonID = (int)reader["PersonID"];
                                FirstName = (string)reader["FirstName"];
                                SecondName = (string)reader["SecondName"];
                                LastName = (string)reader["LastName"];
                                DateOfBirth = (DateTime)reader["DateOfBirth"];
                                Gender = (byte)reader["Gendor"];
                                Address = (string)reader["Address"];
                                PhoneNumber = (string)reader["Phone"];
                                CountryID = (int)reader["NationalityCountryID"];

                                if (reader["ThirdName"] != DBNull.Value)
                                    ThirdName = (string)reader["ThirdName"];
                                else
                                    ThirdName = "";

                                if (reader["Email"] != DBNull.Value)
                                    Email = (string)reader["Email"];
                                else
                                    Email = "";

                                if (reader["ImagePath"] != DBNull.Value)
                                    ImagePath = (string)reader["ImagePath"];
                                else
                                    ImagePath = "";
                            }
                            else
                            {
                                isFound = false;
                            }
                        }
                    }
                    catch (SqlException sqlEx)
                    {
                        clsDataAccessLogger.LogException(sqlEx);

                        isFound = false;
                    }
                    catch (Exception ex)
                    {
                        clsDataAccessLogger.LogException(ex);

                        isFound = false;
                    }
                }
            }

            return isFound;
        }

        public static bool DeletePerson(int PersonID)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"DELETE People 
                                 WHERE PersonID = @PersonID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);

                    try
                    {
                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                    catch (SqlException sqlEx)
                    {
                        clsDataAccessLogger.LogException(sqlEx);
                    }
                    catch (Exception ex)
                    {
                        clsDataAccessLogger.LogException(ex);
                    }
                }                 
            }

            return (rowsAffected > 0);
        }

        public static DataTable GetAllPeople()
        {
            DataTable dtPeople = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SELECT 
                                    People.PersonID AS [Person ID], 
                                    People.NationalNo AS [National No.], 
                                    People.FirstName AS [First Name], 
                                    People.SecondName AS [Second Name], 
                                    People.ThirdName AS [Third Name], 
                                    People.LastName AS [Last Name],
                                    CASE 
                                        WHEN People.Gendor = 0 THEN 'Male' 
                                        ELSE 'Female' 
                                    END AS Gender,
                                    People.Address,
                                    People.DateOfBirth AS [Date Of Birth], 
                                    Countries.CountryName AS Nationality,
                                    People.Phone, 
                                    People.Email,
                                    People.ImagePath
                                 FROM People 
                                 INNER JOIN Countries ON People.NationalityCountryID = Countries.CountryID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                                dtPeople.Load(reader);
                        }
                    }
                    catch (SqlException sqlEx)
                    {
                        clsDataAccessLogger.LogException(sqlEx);
                    }
                    catch (Exception ex)
                    {
                        clsDataAccessLogger.LogException(ex);
                    }
                }       
            }

            return dtPeople;
        }

        public static bool IsPersonExist(int PersonID)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SELECT TOP 1 1
                                 FROM People
                                 WHERE PersonID = @PersonID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);

                    try
                    {
                        connection.Open();

                        object result = command.ExecuteScalar();

                        if (result != null)
                            isFound = true;
                    }
                    catch (SqlException sqlEx)
                    {
                        clsDataAccessLogger.LogException(sqlEx);

                        isFound = false;
                    }
                    catch (Exception ex)
                    {
                        clsDataAccessLogger.LogException(ex);

                        isFound = false;
                    }
                }
            }

            return isFound;
        }

        public static bool IsPersonExist(string NationalNo)
        {
            if (string.IsNullOrWhiteSpace(NationalNo))
                return false;

            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SELECT TOP 1 1
                                 FROM People
                                 WHERE NationalNo = @NationalNo";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NationalNo", NationalNo);

                    try
                    {
                        connection.Open();

                        object result = command.ExecuteScalar();

                        if (result != null)
                            isFound = true;
                    }
                    catch (SqlException sqlEx)
                    {
                        clsDataAccessLogger.LogException(sqlEx);

                        isFound = false;
                    }
                    catch (Exception ex)
                    {
                        clsDataAccessLogger.LogException(ex);

                        isFound = false;
                    }
                }
            }

            return isFound;
        }

        public static bool IsPersonExist(string NationalNo, int PersonID)
        {
            bool isFound = false;

            string query = @"SELECT Found = 1 FROM People 
                         WHERE NationalNo = @NationalNo AND PersonID <> @PersonID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NationalNo", NationalNo);
                    command.Parameters.AddWithValue("@PersonID", PersonID);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            isFound = true;
                        }
                    }
                    catch (SqlException sqlEx)
                    {
                        clsDataAccessLogger.LogException(sqlEx);
                        isFound = false;
                    }
                    catch (Exception ex)
                    {
                        clsDataAccessLogger.LogException(ex);
                        isFound = false;
                    }
                }
            }

            return isFound;
        }

        public static bool IsEmailExist(string Email, int PersonID = -1)
        {
            bool isFound = false;

            string query = @"SELECT Found = 1 FROM People 
                             WHERE Email = @Email AND (@PersonID = -1 OR PersonID <> @PersonID)";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", Email);
                    command.Parameters.AddWithValue("@PersonID", PersonID);

                    try
                    {
                        connection.Open();

                        object result = command.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            isFound = true;
                        }
                    }
                    catch (SqlException sqlEx)
                    {
                        clsDataAccessLogger.LogException(sqlEx);
                        isFound = false;
                    }
                    catch (Exception ex)
                    {
                        clsDataAccessLogger.LogException(ex);
                        isFound = false;
                    }
                }
            }

            return isFound;
        }
    }
}
