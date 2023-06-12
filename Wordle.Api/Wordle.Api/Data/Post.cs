using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Wordle.Api.Data
{
    public class Post
    {
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }

    }
}
