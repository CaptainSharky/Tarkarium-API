using Npgsql;

namespace education.Comments
{
    public class CommentService
    {
        private static readonly string connString = "Host=localhost;Username=newuser;Password=password;Database=education";
        public async Task<bool> AddCommentToUserAsync(int userId, CommentEntity comment)
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connString))
                {
                    await conn.OpenAsync();

                    string sql = "INSERT INTO comments (text, time, likes, user_id) VALUES (@Text, @Time, @Likes, @UserId)";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("Text", comment.Text);
                        cmd.Parameters.AddWithValue("Time", DateTime.Now); 
                        cmd.Parameters.AddWithValue("Likes", comment.Likes);
                        cmd.Parameters.AddWithValue("UserId", userId);

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding comment to user: " + ex.Message);
                return false;
            }
        }
        public async Task<bool> IncreaseLikesAsync(int commentId)
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connString))
                {
                    await conn.OpenAsync();

                    string sql = "UPDATE comments SET likes = likes + 1 WHERE id = @CommentId";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("CommentId", commentId);

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error increasing likes for comment: " + ex.Message);
                return false;
            }
        }
        public async Task<bool> DeleteCommentAsync(int commentId)
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connString))
                {
                    await conn.OpenAsync();

                    string sql = "DELETE FROM comments WHERE id = @CommentId";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("CommentId", commentId);

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting comment: " + ex.Message);
                return false;
            }
        }
        public async Task<CommentDTO> GetCommentInfoAsync(int commentId)
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connString))
                {
                    await conn.OpenAsync();

                    string sql = "SELECT text, time, likes FROM comments WHERE id = @CommentId";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("CommentId", commentId);

                        using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                return new CommentDTO
                                {
                                    Text = reader.GetString(0),
                                    Time = reader.GetDateTime(1),
                                    Likes = reader.GetInt32(2)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching comment info: " + ex.Message);
            }

            return null;
        }
    }
}
