using System.ComponentModel.DataAnnotations.Schema;

namespace Wordle.Api.Data;

public class AttributeDto
{
    public required string AttributeType { get; set; }
    public required string AbilityName { get; set; }
    public required string Description { get; set; }
}