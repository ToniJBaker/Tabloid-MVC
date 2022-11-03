using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public class PostTagRepository: BaseRepository , IPostTagRepository
    {
        public PostTagRepository(IConfiguration config) : base(config) { }
       
        
        
    }
}
