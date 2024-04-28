using System;
using System.Threading.Tasks;
using education.Tickets;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace education.Tickets
{
    public class TicketService
    {
        private static readonly string connString = "Host=localhost;Username=newuser;Password=password;Database=education";
        public async Task<bool> AddTicketToUserAsync(TicketEntity ticket)
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connString))
                {
                    await conn.OpenAsync();

                    string sql = "INSERT INTO tickets (code, name, description, user_id) VALUES (@Code, @Name, @Description, @UserId)";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("Code", ticket.Code);
                        cmd.Parameters.AddWithValue("Name", ticket.Name);
                        cmd.Parameters.AddWithValue("Description", ticket.Description);
                        cmd.Parameters.AddWithValue("UserId", ticket.UserId);

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding ticket to user: " + ex.Message);
                return false;
            }
        }
        public async Task<TicketEntity> GetTicketAsync(int ticketId)
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connString))
                {
                    await conn.OpenAsync();

                    string sql = "SELECT id, code, name, description, time, user_id FROM tickets WHERE id = @TicketId";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("TicketId", ticketId);

                        using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                return new TicketEntity
                                {
                                    Id = reader.GetInt32(0),
                                    Code = reader.GetString(1),
                                    Name = reader.GetString(2),
                                    Description = reader.GetString(3),
                                    Time = reader.GetDateTime(4),
                                    UserId = reader.GetInt32(5)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching ticket details: " + ex.Message);
            }

            return null;
        }
        public async Task<TicketDTO> GetTicketInfoAsync(int ticketId)
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connString))
                {
                    await conn.OpenAsync();

                    string sql = "SELECT code, name, time, description FROM tickets WHERE id = @TicketId";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("TicketId", ticketId);

                        using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                return new TicketDTO
                                {
                                    Code = reader.GetString(0),
                                    Name = reader.GetString(1),
                                    Time = reader.GetDateTime(2),
                                    Description = reader.GetString(3)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching ticket info: " + ex.Message);
            }

            return null;
        }
    }
}
