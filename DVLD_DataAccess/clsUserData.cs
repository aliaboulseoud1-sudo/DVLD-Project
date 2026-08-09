using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public class clsUserData
    {
        public static int AddNewUser(int PersonID, string UserName,
                                     string Password, bool IsActive)
        {
            int UserID = -1;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"INSERT INTO Users (PersonID, UserName, Password, IsActive)
                                 VALUES (@PersonID, @UserName, @Password, @IsActive);
                                 SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@UserName", UserName);
                    command.Parameters.AddWithValue("@Password", Password);
                    command.Parameters.AddWithValue("@IsActive", IsActive);

                    try
                    {
                        connection.Open();

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int insertedUserID))
                        {
                            UserID = insertedUserID;
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

            return UserID;
        }

        public static bool UpdateUser(int UserID, string UserName,
                                      string Password, bool IsActive)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"UPDATE Users  
                                 SET UserName = @UserName,
                                     Password = @Password, 
                                     IsActive = @IsActive                                                                             
                                 WHERE UserID = @UserID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);
                    command.Parameters.AddWithValue("@UserName", UserName);
                    command.Parameters.AddWithValue("@Password", Password);
                    command.Parameters.AddWithValue("@IsActive", IsActive);

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

        public static bool GetUserInfoByID(int UserID, ref int PersonID, ref string UserName,
                                           ref string Password, ref bool IsActive)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SELECT PersonID, UserName, Password, IsActive 
                                 FROM Users
                                 WHERE UserID = @UserID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;

                                PersonID = (int)reader["PersonID"];
                                UserName = (string)reader["UserName"];
                                Password = (string)reader["Password"];
                                IsActive = (bool)reader["IsActive"];
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

        public static bool GetUserInfoByUsername(string UserName, ref int UserID, ref int PersonID,
                                                 ref string Password, ref bool IsActive)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SELECT UserID, PersonID, Password, IsActive 
                                 FROM Users
                                 WHERE UserName = @UserName";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", UserName);

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;

                                UserID = (int)reader["UserID"];
                                PersonID = (int)reader["PersonID"];
                                Password = (string)reader["Password"];
                                IsActive = (bool)reader["IsActive"];
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

        public static bool DeleteUser(int UserID)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"DELETE FROM Users 
                                 WHERE UserID = @UserID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);

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

        public static DataTable GetAllUsers()
        {
            DataTable dtUsers = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SELECT
                                    Users.UserID AS [User ID],
                                    Users.PersonID AS [Person ID],
                                    CONCAT_WS(' ', People.FirstName, People.SecondName, People.ThirdName, People.LastName) AS [Full Name],
                                    Users.UserName AS [Username],
                                    Users.IsActive AS [Is Active]
                                FROM Users 
                                INNER JOIN People ON People.PersonID = Users.PersonID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            dtUsers.Load(reader);
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

            return dtUsers;
        }

        public static bool IsUserExist(int UserID)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SELECT TOP 1 1
                                 FROM Users
                                 WHERE UserID = @UserID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);

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

        public static bool IsUserExist(string UserName)
        {
            if (string.IsNullOrWhiteSpace(UserName))
                return false;

            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SELECT TOP 1 1
                                 FROM Users
                                 WHERE UserName = @UserName";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", UserName);

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

        public static bool IsUserExistForPersonID(int PersonID)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SELECT TOP 1 1
                                 FROM Users
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
    }
}
