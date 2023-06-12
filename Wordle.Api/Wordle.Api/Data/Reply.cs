using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Wordle.Api.Data
{
    public class Reply
    {
        public Guid ReplyId { get; set; }
        public Guid OriginalPost { get; set; }
        public Guid ReplyTo { get; set; } = Guid.Empty;
        public Guid UserId { get; set; }
        public required string Content { get; set; }

    }
}
