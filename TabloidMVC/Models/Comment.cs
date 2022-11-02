using System;

namespace TabloidMVC.Models
{
    public class Comment
    {
        // select Id, PostId, UserProfileId, Subject, Content, CreateDateTime from comment

            public int Id { get; set; }
            public int PostId { get; set; }
            public int UserProfileId { get; set; }
            public string Subject { get; set; }
            public string Content { get; set; }
            public DateTime CreateDateTime { get; set; }
    }
}
