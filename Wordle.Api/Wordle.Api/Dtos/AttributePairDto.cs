using System.ComponentModel.DataAnnotations.Schema;

namespace Wordle.Api.Data;

public class AttributePairDto
{
    public List<AttributeDto>? CivAttributes { get; set; }
    public List<AttributeDto>? LeaderAttributes { get; set; }
}