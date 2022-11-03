using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public class PostTagRepository: BaseRepository , IPostTagRepository
    {
        public PostTagRepository(IConfiguration config) : base(config) { }

        
        //creat a postTag
        public void AddPostTag(PostTag postTag)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    INSERT INTO PostTag (TagId, PostId)
                    OUTPUT INSERTED.ID
                    VALUES (@tagId, @postId);
                ";

                    cmd.Parameters.AddWithValue("@tagId", postTag.TagId);
                    cmd.Parameters.AddWithValue("@postId", postTag.PostId);


                    int id = (int)cmd.ExecuteScalar();

                    postTag.Id = id;
                }
            }
        }
        
        //delete a postTag
        public void DeletePostTag(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            DELETE FROM PostTag
                            WHERE Id = @id
                        ";

                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }


    }
}
