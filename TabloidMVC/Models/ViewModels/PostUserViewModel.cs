using System.Collections.Generic;

namespace TabloidMVC.Models.ViewModels
{
    public class PostUserViewModel
    {
        public List<Post> Posts { get; set; }
        public int currentUserId { get; set; }
    }
}
