using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public class clsCountryData
    {
        public static int AddNewCountry(string CountryName)
        {
            int CountryID = -1;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"INSERT INTO Countries (CountryName)
                                 VALUES (@CountryName);
                                 SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CountryName", CountryName);

                    try
                    {
                        connection.Open();

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int insertedCountryID))
                        {
                            CountryID = insertedCountryID;
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

            return CountryID;
        }

        public static bool UpdateCountry(int CountryID, string CountryName)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"UPDATE Countries  
                                SET CountryName = @CountryName                  
                                WHERE CountryID = @CountryID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CountryID", CountryID);
                    command.Parameters.AddWithValue("@CountryName", CountryName);

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

        public static bool GetCountryInfoByID(int CountryID, ref string CountryName)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SELECT * FROM Countries
                                 WHERE CountryID = @CountryID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CountryID", CountryID);

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;

                                CountryName = (string)reader["CountryName"];     
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

        public static bool DeleteCountry(int CountryID)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"DELETE Countries 
                                 WHERE CountryID = @CountryID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CountryID", CountryID);

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

        public static DataTable GetAllCountries()
        {
            DataTable dtCountries = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SELECT 
                                   CountryID, 
                                   CountryName 
                                 FROM Countries"; 
                                
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            dtCountries.Load(reader);
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

            return dtCountries;
        }

        public static bool IsCountryExist(int CountryID)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SELECT TOP 1 1
                                 FROM Countries
                                 WHERE CountryID = @CountryID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CountryID", CountryID);

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

        public static bool IsCountryExist(string CountryName)
        {
            if (string.IsNullOrWhiteSpace(CountryName))
                return false;

            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SELECT TOP 1 1
                                 FROM Countries
                                 WHERE CountryName = @CountryName";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CountryName", CountryName);

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
    }
}
