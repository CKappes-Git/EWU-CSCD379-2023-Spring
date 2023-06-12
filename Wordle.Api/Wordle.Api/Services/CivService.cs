using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Wordle.Api.Data;
using Wordle.Api.Dtos;

namespace Wordle.Api.Services;

public class CivService
{
    private readonly AppDbContext _db;
    
    public CivService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Civ>> GetCivsAsync(int? count, string start = "")
    {
        count ??= 10;
        var civs = await _db.Civs
          .Where(c => c.CivName.StartsWith(start))
          .Take(count.Value)
          .OrderByDescending(c => c.CivName)
          .ToListAsync();
        return civs;
    }
    public async Task<IEnumerable<Leader>> GetLeadersAsync(int? count, string start = "")
    {
        count ??= 10;
        var leaders = await _db.Leaders
          .Where(l => l.Name.StartsWith(start))
          .Take(count.Value)
          .OrderByDescending(l => l.Name)
          .ToListAsync();
        return leaders;
    }

    public async Task<Civ> AddCivAsync(string newCivName)
    {
        if (newCivName is null)
        {
            throw new ArgumentException("The civilization name is null");
        }

        var civ = await _db.Civs.FirstOrDefaultAsync(c => c.CivName == newCivName);
        if (civ == null)
        {
            civ = new() { CivName = newCivName };
            _db.Civs.Add(civ);
        }
        
        await _db.SaveChangesAsync();
        return civ;
    }

    public async Task<Leader> AddLeaderAsync(string civName, string leaderName)
    {
        if (leaderName is null || civName is null)
        {
            throw new ArgumentException("The leader is missing something");
        }

        var civ = await _db.Civs.FirstOrDefaultAsync(c => c.CivName == civName);
        if (civ == null) {
            civ = await AddCivAsync(civName);
        }
        var leader = await _db.Leaders.FirstOrDefaultAsync(l => l.Name == leaderName);
        if (leader == null)
        {
             leader = new()
            {
                Name = leaderName,
                CivID = civ.CivID
            };
            _db.Leaders.Add(leader);
        }
        await _db.SaveChangesAsync();
        return leader;
    }

    public async Task<LeaderInfoDto> AddAttributesAsync(string civName, string leaderName, List<AttributeDto>? leaderAttributes = null, List<AttributeDto>? civAttributes = null)
    {
        if (leaderName is null || civName is null || (leaderAttributes is null && civAttributes is null))
        {
            throw new ArgumentException("The leader is missing something");
        }
        var civ = await _db.Civs.FirstOrDefaultAsync(c => c.CivName == civName);
        if (civ == null)
        {
            civ = await AddCivAsync(civName);
        }
        var leader = await _db.Leaders.FirstOrDefaultAsync(l => l.Name == leaderName);
        if (leader == null)
        {
            LeaderInfoDto temp = new() { CivName = civName, LeaderName = leaderName };
            leader = await AddLeaderAsync(civName, leaderName);
        }
        if(leaderAttributes is not null) {
            for (var i = 0; i < leaderAttributes.Count; i++)
            {
                LeaderAttribute lAttribute = new()
                {
                    LeaderID = leader.LeaderID,
                    AttributeType = leaderAttributes[i].AttributeType,
                    AbilityName = leaderAttributes[i].AbilityName,
                    Description = leaderAttributes[i].Description

                };
                _db.LeaderAttributes.Add(lAttribute);
            }
        }
        if (civAttributes is not null) {
            for (var i = 0; i < civAttributes.Count; i++)
            {
                CivAttribute cAttribute = new()
                {
                    CivID = civ.CivID,
                    AttributeType = civAttributes[i].AttributeType,
                    AbilityName = civAttributes[i].AbilityName,
                    Description = civAttributes[i].Description

                };
                _db.CivAttributes.Add(cAttribute);
            }
        }

        await _db.SaveChangesAsync();
        return await GetLeaderInfoAsync(leader.Name);

    }

    public async Task<LeaderInfoDto> GetLeaderInfoAsync(string leaderName)
    {
        var leader = await _db.Leaders.FirstOrDefaultAsync(l => l.Name == leaderName);
        if (leader == null)
        {
            throw new ArgumentException("There is no leader by that name");
        }
        var civ = await _db.Civs.FirstOrDefaultAsync(c => c.CivID == leader.CivID);
        if (civ == null)
        {
            throw new ArgumentException("That leader somehow has no civ, something has gone horribly wrong");
        }
        var leaderAttributes = await _db.LeaderAttributes
            .Where(l => l.LeaderID == leader.LeaderID)
            .ToListAsync();
        var civAttributes = await _db.CivAttributes
            .Where(c => c.CivID ==  civ.CivID)
            .ToListAsync();
        LeaderInfoDto leaderInfo = new()
        {
            LeaderName = leader.Name,
            CivName = civ.CivName,
            LeaderAttributes = leaderAttributes,
            CivAttributes = civAttributes
        };

        return leaderInfo;
    }

    /*
    public async Task<Word> DeleteWordAsync(string? targetWord)
    {
        if (string.IsNullOrEmpty(targetWord) || targetWord.Length != 5)
        {
            throw new ArgumentException("Word must be 5 characters long");
        }

        var word = await _db.Words.FirstOrDefaultAsync(w => w.Text == targetWord);
        if (word != null)
        {
            _db.Words.Remove(word);
        }
        await _db.SaveChangesAsync();
        return word;
    }
    */

    public async Task<IEnumerable<Leader>> GetPaginatedLeadersAsync(int page = 1, int count = 10, string start = "")
    {
        if (page < 1) page = 1;
        if (count < 1 || count > 100) count = 10;
        page--;
        var index = page * count;

        var totalCount = await _db.Words.CountAsync(word => word.IsCommon);
        totalCount -= count;
        var leaders = await _db.Leaders
          .Where(l => l.Name.StartsWith(start))
          .OrderBy(l => l.Name)
          .Skip(index)
          .Take(count)
          .ToListAsync();
        return leaders;
    }
}