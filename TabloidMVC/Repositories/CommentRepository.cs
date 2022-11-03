using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public class CommentRepository : BaseRepository, ICommentRepository
    {
        public CommentRepository(IConfiguration config) : base(config) { }
        public List<Comment> GetAllComments(int postId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    SELECT c.Id, c.PostId, c.UserProfileId, c.Subject, c.Content, c.CreateDateTime, Post.Title, u.DisplayName
                        FROM Comment c
                        join Post on c.PostId = Post.Id
                        join UserProfile u on c.UserProfileId = u.Id
                    WHERE PostId = @postId";
                    cmd.Parameters.AddWithValue("@postId", postId);

                    var reader = cmd.ExecuteReader();

                    var comments = new List<Comment>();

                    while (reader.Read())
                    {
                        comments.Add(NewComment(reader));
                    }

                    reader.Close();

                    return comments;
                }
            }
        }
        private Comment NewComment(SqlDataReader reader)
        {
            Comment comment = new Comment()
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                Subject = reader.GetString(reader.GetOrdinal("Subject")),
                Content = reader.GetString(reader.GetOrdinal("Content")),
                CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                Post = new Post(),
                DisplayName = reader.GetString(reader.GetOrdinal("DisplayName"))
            };

            comment.Post.Title = reader.GetString(reader.GetOrdinal("Title"));

            return comment;
        }

        public Comment GetCommentById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT c.Id, c.PostId, c.UserProfileId, c.Subject, c.Content, c.CreateDateTime, Post.Title, u.DisplayName
                        FROM Comment c
                        join Post on c.PostId = Post.Id
                        join UserProfile u on c.UserProfileId = u.Id
                    WHERE c.Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();

                    Comment comment = null;

                    if (reader.Read())
                    {
                        comment = NewComment(reader);
                    }

                    reader.Close();

                    return comment;
                }
            }
        }
    }
}
