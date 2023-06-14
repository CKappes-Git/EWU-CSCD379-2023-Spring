using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wordle.Api.Data;

public class LeaderNotes
{
    [Key]
    public int LeaderNoteID { get; set; }

    [ForeignKey("Leader")]
    public int LeaderID { get; set; }

    [ForeignKey("AppUser")]
    public required string AppUserID { get; set; }
    public required string NoteName { get; set; }

    public string ScienceTree { get; set; } = "";
    public string CultureTree { get; set; } = "";
    public string Production { get; set; } = "";
    public string Notes { get; set; } = "";

}