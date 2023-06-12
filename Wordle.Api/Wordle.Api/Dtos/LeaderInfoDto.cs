using Wordle.Api.Data;

namespace Wordle.Api.Dtos
{
    public class LeaderInfoDto
    {
        public required string LeaderName { get; set; }
        public required string CivName { get; set; }
        public List<CivAttribute>? CivAttributes { get; set; }
        public List<LeaderAttribute>? LeaderAttributes { get; set; }
    }
}
