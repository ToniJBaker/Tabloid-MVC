﻿using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IPostRepository
    {
        void Add(Post post);
        List<Post> GetAllPublishedPosts();
        List<Post> GetAllUserPosts(int id);

        Post GetPublishedPostById(int id);
        Post GetUserPostById(int id, int userProfileId);
        void DeletePost(int id);
        void EditPost(Post post);
    }
}