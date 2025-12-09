using CaseManagementSystem.Models;
using CaseManagementSystem.Settings;
using System;
using System.Data;
using System.Data.SqlClient;
using BCrypt.Net;

namespace CaseManagementSystem.Services
{
    public class AuthService
    {
        private readonly string _conString = AppValues.ConString;

        // LOGIN
        public User Login(string username, string password)
        {
            using (SqlConnection conn = new SqlConnection(_conString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Users WHERE Username=@Username", conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string storedHash = reader["PasswordHash"].ToString();
                            if (BCrypt.Net.BCrypt.Verify(password, storedHash))
                            {
                                return new User
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    Username = reader["Username"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    Mobile = reader["Mobile"].ToString(),
                                    Role = reader["Role"].ToString(),
                                    Gender = reader["Gender"].ToString(),
                                    DateOfJoining = reader["DateOfJoining"] as DateTime?,
                                    Address = reader["Address"].ToString(),
                                    Department = reader["Department"].ToString(),
                                    Designation = reader["Designation"].ToString(),
                                    IsActive = Convert.ToBoolean(reader["IsActive"])
                                };
                            }
                        }
                    }
                }
            }
            return null; // invalid username/password
        }

        // VERIFY SECURITY ANSWER
        public bool VerifySecurityAnswer(string username, string answer)
        {
            using (SqlConnection conn = new SqlConnection(_conString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT SecurityAnswerHash FROM Users WHERE Username=@Username", conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    var result = cmd.ExecuteScalar();
                    if (result == null) return false;

                    string storedHash = result.ToString();
                    return BCrypt.Net.BCrypt.Verify(answer, storedHash);
                }
            }
        }

        // RESET PASSWORD
        public bool ResetPassword(string username, string newPassword)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);

            using (SqlConnection conn = new SqlConnection(_conString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("UPDATE Users SET PasswordHash=@PasswordHash WHERE Username=@Username", conn))
                {
                    cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                    cmd.Parameters.AddWithValue("@Username", username);

                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }

        // CREATE USER (SIGNUP)
        public bool CreateUser(User user)
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            user.SecurityAnswer = BCrypt.Net.BCrypt.HashPassword(user.SecurityAnswer); // hash security answer

            using (SqlConnection conn = new SqlConnection(_conString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_InsertUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Mobile", user.Mobile);
                    cmd.Parameters.AddWithValue("@Role", user.Role);
                    cmd.Parameters.AddWithValue("@Gender", user.Gender);
                    cmd.Parameters.AddWithValue("@DateOfJoining", user.DateOfJoining);
                    cmd.Parameters.AddWithValue("@Address", user.Address);
                    cmd.Parameters.AddWithValue("@Department", (object)user.Department ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Designation", (object)user.Designation ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@SecurityQuestion", (object)user.SecurityQuestion ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@SecurityAnswer", (object)user.SecurityAnswer ?? DBNull.Value);
                    cmd.Parameters.Add("@UserImage", SqlDbType.VarBinary).Value = (object)user.UserImage ?? DBNull.Value;

                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }
    }
}

//using CaseManagementSystem.Models;
//using CaseManagementSystem.Settings;
//using System;
//using System.Data;
//using System.Data.SqlClient;
//using System.Security.Cryptography;
//using System.Text;

//namespace CaseManagementSystem.Services
//{
//    public class AuthService
//    {
//        private readonly string _connectionString = AppValues.ConString;

//        // Signup using stored procedure
//        public void CreateUser(SignupModel signup)
//        {
//            string hashedPassword = ComputeSha256Hash(signup.Password);

//            using (SqlConnection conn = new SqlConnection(_connectionString))
//            {
//                conn.Open();
//                using (SqlCommand cmd = new SqlCommand("sp_InsertUser", conn))
//                {
//                    cmd.CommandType = CommandType.StoredProcedure;
//                    cmd.Parameters.AddWithValue("@Username", signup.Username);
//                    cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);
//                    cmd.Parameters.AddWithValue("@Email", signup.Email);
//                    cmd.Parameters.AddWithValue("@Mobile", signup.Mobile);
//                    cmd.Parameters.AddWithValue("@Role", signup.Role);
//                    cmd.Parameters.AddWithValue("@Gender", signup.Gender ?? (object)DBNull.Value);
//                    cmd.Parameters.AddWithValue("@DateOfJoining", signup.DateOfJoining ?? (object)DBNull.Value);
//                    cmd.Parameters.AddWithValue("@Address", signup.Address ?? (object)DBNull.Value);
//                    cmd.Parameters.AddWithValue("@Department", signup.Department ?? (object)DBNull.Value);
//                    cmd.Parameters.AddWithValue("@Designation", signup.Designation ?? (object)DBNull.Value);
//                    cmd.Parameters.AddWithValue("@SecurityQuestion", signup.SecurityQuestion ?? (object)DBNull.Value);
//                    cmd.Parameters.AddWithValue("@SecurityAnswer", signup.SecurityAnswer ?? (object)DBNull.Value);
//                    cmd.Parameters.AddWithValue("@UserImage", signup.UserImage ?? (object)DBNull.Value);

//                    cmd.ExecuteNonQuery();
//                }
//            }
//        }

//        // Login using stored procedure
//        public User Login(string username, string password)
//        {
//            string hashedPassword = ComputeSha256Hash(password);

//            using (SqlConnection conn = new SqlConnection(_connectionString))
//            {
//                conn.Open();
//                using (SqlCommand cmd = new SqlCommand("sp_LoginUser", conn))
//                {
//                    cmd.CommandType = CommandType.StoredProcedure;
//                    cmd.Parameters.AddWithValue("@Username", username);
//                    cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);

//                    using (SqlDataReader reader = cmd.ExecuteReader())
//                    {
//                        if (reader.Read())
//                        {
//                            return new User
//                            {
//                                Id = Convert.ToInt32(reader["Id"]),
//                                Username = reader["Username"].ToString(),
//                                Role = reader["Role"].ToString(),
//                                IsActive = Convert.ToBoolean(reader["IsActive"])
//                            };
//                        }
//                    }
//                }
//            }

//            return null;
//        }

//        private string ComputeSha256Hash(string rawData)
//        {
//            using (SHA256 sha256Hash = SHA256.Create())
//            {
//                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
//                StringBuilder builder = new StringBuilder();
//                foreach (var b in bytes)
//                    builder.Append(b.ToString("x2"));
//                return builder.ToString();
//            }
//        }
//    }
//}
