﻿using System.Collections.Generic;

namespace TabloidMVC.Models.ViewModels
{
    public class PostEditViewModel
    {
        public Post Post { get; set; }
        public List<Category> CategoryOptions { get; set; }
        public List<PostTag> PostTags { get; set; }

        public int currentUserId { get; set; }
    }
}
