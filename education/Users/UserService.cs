using System;
using System.Threading.Tasks;
using education.Users; 
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace education.Users
{
    public class UserService
    {
        private static readonly string connString = "Host=localhost;Username=newuser;Password=password;Database=education";

        public async Task<bool> RegisterUserAsync(UserEntity newUser)
        {
            try
            {
                using (NpgsqlConnection conn = new(connString))
                {
                    await conn.OpenAsync();

                    string sql = "INSERT INTO users (username, password, email) VALUES (@Username, @Password, @Email)";
                    using (NpgsqlCommand cmd = new(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("Username", newUser.Username);
                        cmd.Parameters.AddWithValue("Password", newUser.Password);
                        cmd.Parameters.AddWithValue("Email", newUser.Email);

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error registering user: " + ex.Message);
                return false;
            }
        }
        public async Task<bool> LoginAsync(string username, string password)
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connString))
                {
                    await conn.OpenAsync();

                    string sql = "SELECT COUNT(*) FROM users WHERE username = @Username AND password = @Password";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("Username", username);
                        cmd.Parameters.AddWithValue("Password", password);

                        int count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error logging in: " + ex.Message);
                return false;
            }
        }
        public async Task<UserDTO> GetUserDetailsAsync(string username)
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connString))
                {
                    await conn.OpenAsync();

                    string sql = "SELECT username, email FROM users WHERE username = @Username";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("Username", username);

                        using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                return new UserDTO
                                {
                                    Name = reader.GetString(0),
                                    Email = reader.GetString(1)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching user details: " + ex.Message);
            }

            return null;
        }
    }
}
