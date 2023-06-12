namespace Wordle.Api.Data;

public class CivAttribute
{
    public int CivID { get; set; }
    public required string AttributeType { get; set; }
    public required string AbilityName { get; set; }
    public required string Description { get; set; }

}