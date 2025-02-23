﻿using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ITagRepository
    {
        List<Tag> GetAllTags();
        void AddTag(Tag tag);
        void DeleteTag(int id);
        void UpdateTag(Tag tag);    
        Tag GetTagById(int id);
        List<PostTag> GetPostTagsByPostId(int id);
    }
}
