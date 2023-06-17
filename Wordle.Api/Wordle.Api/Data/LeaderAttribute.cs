﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Wordle.Api.Data;

public class LeaderAttribute
{
    public int LeaderAttributeID { get; set; }

    [ForeignKey("Leader")]
    public int LeaderID { get; set; }
    public required string AttributeType { get; set; }
    public required string AbilityName { get; set; }
    public required string Description { get; set; }

}