using System;
using System.Threading.Tasks;
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

                    string sql = "INSERT INTO tickets (code, ticket_id, user_id) VALUES (@Code, @TicketId, @UserId)";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("Code", ticket.Code);
                        cmd.Parameters.AddWithValue("TicketId", ticket.TicketId);
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

                    string sql = "SELECT t.id, t.code, t.time, t.user_id, ti.name, ti.description FROM tickets t INNER JOIN ticket_info ti ON t.ticket_id = ti.ticket_id WHERE t.id = @TicketId";
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
                                    Time = reader.GetDateTime(2),
                                    UserId = reader.GetInt32(3),
                                    Name = reader.GetString(4),
                                    Description = reader.GetString(5)
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

                    string sql = "SELECT ti.code, ti.name, t.time, ti.description FROM tickets t INNER JOIN ticket_info ti ON t.ticket_id = ti.ticket_id WHERE t.id = @TicketId";
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
        public async Task<List<TicketEntity>> GetUserTicketsAsync(string username, string email)
        {
            List<TicketEntity> userTickets = new List<TicketEntity>();

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connString))
                {
                    await conn.OpenAsync();

                    string userIdQuery = "SELECT id FROM public.users WHERE username = @Username AND email = @Email";
                    int userId;
                    using (NpgsqlCommand userIdCmd = new NpgsqlCommand(userIdQuery, conn))
                    {
                        userIdCmd.Parameters.AddWithValue("Username", username);
                        userIdCmd.Parameters.AddWithValue("Email", email);
                        userId = (int)await userIdCmd.ExecuteScalarAsync();
                    }

                    
                    string userTicketsQuery = @"
                SELECT t.id, t.code, t.ticket_id, t.time, t.user_id, ti.name, ti.description 
                FROM public.tickets t 
                INNER JOIN public.ticket_info ti ON t.ticket_id = ti.ticket_id 
                WHERE t.user_id = @UserId";

                    using (NpgsqlCommand userTicketsCmd = new NpgsqlCommand(userTicketsQuery, conn))
                    {
                        userTicketsCmd.Parameters.AddWithValue("UserId", userId);

                        using (NpgsqlDataReader reader = await userTicketsCmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                TicketEntity ticket = new TicketEntity
                                {
                                    Id = reader.GetInt32(0),
                                    Code = reader.GetString(1),
                                    TicketId = reader.GetInt32(2),
                                    Time = reader.GetDateTime(3),
                                    UserId = reader.GetInt32(4),
                                    Name = reader.GetString(5), 
                                    Description = reader.GetString(6) 
                                };
                                userTickets.Add(ticket);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching user tickets: " + ex.Message);
            }

            return userTickets;
        }

    }
}
