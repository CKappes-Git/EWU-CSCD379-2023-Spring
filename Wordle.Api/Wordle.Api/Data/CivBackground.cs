using System.ComponentModel.DataAnnotations.Schema;

namespace Wordle.Api.Data;

public class CivBackground
{
    public int CivBackgroundId { get; set; }

    [ForeignKey("Civ")]
    public int CivID { get; set; }
    public required string Url { get; set; }

}