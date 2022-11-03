using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Models;
using System.Linq;

namespace TabloidMVC.Repositories
{
    public class TagRepository : BaseRepository, ITagRepository
    {
        public TagRepository(IConfiguration config) : base(config) { }
        public List<Tag> GetAllTags()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT t.Id, t.Name, p.Id AS 'post Id', p.Title, p.Content, pt.TagId, pt.PostId
                        FROM Tag t
                        LEFT JOIN PostTag pt
                        ON t.id = pt.TagId
                        LEFT JOIN Post p
                        ON pt.PostId = p.Id";
                       
                        
                     
                    var reader = cmd.ExecuteReader();

                    var tags = new List<Tag>();

                    while (reader.Read())
                    {
                        //tags.Add(new Tag()
                        Post post = null;
                        Tag tag = new Tag()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                        };
                        if (!reader.IsDBNull(reader.GetOrdinal("post Id")))
                        {
                            post = new Post()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("post Id")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Content = reader.GetString(reader.GetOrdinal("Content"))
                            };
                            tag.RelatedPost.Add(post);
                        }
                        if (!tags.Any(x => x.Id == tag.Id))
                        {
                            tags.Add(tag);
                        }
                        else
                        {
                            if (post != null)
                            {
                                tags.FirstOrDefault(x => x.Id == tag.Id).RelatedPost.Add(post);
                            }
                        }
                    }

                    reader.Close();

                    return tags;
                    }
            }
        }
        public Tag GetTagById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id,[Name]
                        FROM Tag
                        WHERE Id = @id
                        
                    ";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Tag tag = new Tag
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };

                        reader.Close();
                        return tag;
                    }
                    else
                    {
                        reader.Close();
                        return null;
                    }
                }
            }
        }
        public void AddTag(Tag tag)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    INSERT INTO Tag ([Name])
                    OUTPUT INSERTED.ID
                    VALUES (@name);
                ";

                    cmd.Parameters.AddWithValue("@name", tag.Name);

                    int id = (int)cmd.ExecuteScalar();

                    tag.Id = id;
                }
            }
        }
        public void DeleteTag(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            DELETE FROM Tag
                            WHERE Id = @id
                        ";

                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void UpdateTag(Tag tag)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE Tag
                            SET 
                                [Name] = @name
                             
                            WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@name", tag.Name);
                    cmd.Parameters.AddWithValue("@id", tag.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<PostTag> GetPostTagsByPostId(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT t.Id AS 'Tag id',t.Name, pt.id AS 'PostTag id', pt.PostId AS 'Post id', pt.TagId AS 'Tag id', p.id AS 'Single Post id'
                        FROM Tag t
                        LEFT JOIN PostTag pt ON t.id = pt.TagId
                        LEFT JOIN Post p ON pt.PostId = P.id
                        WHERE p.id = @id
                    ";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();
                    var postTagsByPostId = new List<PostTag>();

                    if (reader.Read())
                    {
                        PostTag postTag = new PostTag
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("PostTag id")),
                            PostId = reader.GetInt32(reader.GetOrdinal("Post id")),
                            TagId = reader.GetInt32(reader.GetOrdinal("Tag id")),
                            Post = new Post
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Single Post id"))
                            },
                            Tag = new Tag
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Tag id")),
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            }


                        };
                        postTagsByPostId.Add(postTag);

                        reader.Close();
                        return postTagsByPostId;
                    }
                    else
                    {
                        reader.Close();
                        return null;
                    }
                }
            }
        }

    }
}
