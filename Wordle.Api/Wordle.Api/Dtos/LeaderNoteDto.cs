using System.ComponentModel.DataAnnotations.Schema;

namespace Wordle.Api.Dtos;

public class LeaderNoteDto
{
    public int LeaderNoteID { get; set; }

    
    public int LeaderID { get; set; }
    public required string AppUserID { get; set; }
    public required string NoteName { get; set; }
    public string ScienceTree { get; set; } = "";
    public string CultureTree { get; set; } = "";
    public string Production { get; set; } = "";
    public string Notes { get; set; } = "";

}