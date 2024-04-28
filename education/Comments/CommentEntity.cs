using System;

namespace education.Comments
{
    public class CommentEntity
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }
        public int Likes { get; set; }
        public int UserId { get; set; } 
    }
}
