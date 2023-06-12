namespace Wordle.Api.Data;

public class Leader
{
    public int LeaderID { get; set; }
    public int CivID { get; set; }
    public required string Name { get; set; }

}