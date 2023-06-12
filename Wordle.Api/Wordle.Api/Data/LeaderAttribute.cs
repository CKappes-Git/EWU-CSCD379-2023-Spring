namespace Wordle.Api.Data;

public class LeaderAttribute
{
    public int LeaderID { get; set; }
    public required string AttributeType { get; set; }
    public required string AbilityName { get; set; }
    public required string Description { get; set; }

}