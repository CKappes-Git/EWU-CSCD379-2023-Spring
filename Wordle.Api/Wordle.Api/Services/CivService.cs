using Microsoft.AspNetCore.Mvc;
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
    public async Task<IEnumerable<Leader>> GetLeadersAsync(int? count, string? civName, string start = "")
    {
        count ??= 10;
        if(civName != null)
        {
            var civ = await _db.Civs.FirstOrDefaultAsync(c => c.CivName.StartsWith(civName));
            if(civ != null)
            {
                var leader = await _db.Leaders
                  .Where(l => l.Name.StartsWith(start) && l.CivID == civ.CivID)
                  .Take(count.Value)
                  .OrderByDescending(l => l.Name)
                  .ToListAsync();
                return leader;
            }
        }
        
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

    public async Task<LeaderInfoDto> AddAttributesAsync(LeaderInfoDto leaderInfo)
    {
        if (leaderInfo is null)
        {
            throw new ArgumentException("The leader is missing");
        }
        
        var civ = await _db.Civs.FirstOrDefaultAsync(c => c.CivName == leaderInfo.CivName);
        if (civ == null)
        {
            civ = await AddCivAsync(leaderInfo.CivName);
        }
        var leader = await _db.Leaders.FirstOrDefaultAsync(l => l.Name == leaderInfo.LeaderName);
        if (leader == null)
        {
            leader = await AddLeaderAsync(leaderInfo.CivName, leaderInfo.LeaderName);
        }

        if(leaderInfo.LeaderAttributes is not null) {
            var leaderAttributes = leaderInfo.LeaderAttributes;
            var leaderList = await _db.LeaderAttributes.Where(a => a.LeaderID == leader.LeaderID).ToListAsync();
            for (var i = 0; i < leaderAttributes.Count; i++)
            {
                if (leaderAttributes[i].LeaderAttributeID == 0) {
                    LeaderAttribute lAttribute = new()
                    {
                        LeaderID = leader.LeaderID,
                        AttributeType = leaderAttributes[i].AttributeType,
                        AbilityName = leaderAttributes[i].AbilityName,
                        Description = leaderAttributes[i].Description

                    };
                    _db.LeaderAttributes.Add(lAttribute);
                }
                else
                {
                    var temp = leaderList.Where(a => a.LeaderAttributeID == leaderAttributes[i].LeaderAttributeID).FirstOrDefault();
                    if (temp == null)
                    {//if there is not already an ability by that name for that leader
                        LeaderAttribute lAttribute = new()
                        {
                            LeaderID = leader.LeaderID,
                            AttributeType = leaderAttributes[i].AttributeType,
                            AbilityName = leaderAttributes[i].AbilityName,
                            Description = leaderAttributes[i].Description

                        };
                        _db.LeaderAttributes.Add(lAttribute);
                    }
                    else
                    {
                        temp.AttributeType = leaderAttributes[i].AttributeType;
                        temp.AbilityName = leaderAttributes[i].AbilityName;
                        temp.Description = leaderAttributes[i].Description;
                    }
                }
                
            }
        }
        if (leaderInfo.CivAttributes is not null) {
            var civAttributes = leaderInfo.CivAttributes;
            var civList = await _db.CivAttributes.Where(a => a.CivID == civ.CivID).ToListAsync();
            for (var i = 0; i < civAttributes.Count; i++)
            {
                if (civAttributes[i].CivAttributeID == 0)
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
                else
                {
                    var temp = civList.Where(a => a.CivAttributeID == civAttributes[i].CivAttributeID).FirstOrDefault();
                    if (temp == null)
                    {//if there is not already an ability by that name for that leader
                        CivAttribute cAttribute = new()
                        {
                            CivID = civ.CivID,
                            AttributeType = civAttributes[i].AttributeType,
                            AbilityName = civAttributes[i].AbilityName,
                            Description = civAttributes[i].Description

                        };
                        _db.CivAttributes.Add(cAttribute);
                    }
                    else
                    {
                        temp.AttributeType = civAttributes[i].AttributeType;
                        temp.AbilityName = civAttributes[i].AbilityName;
                        temp.Description = civAttributes[i].Description;
                    }
                }
                
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

    public async Task<String> GetBackgroundUrl(string civName)
    {
        if(civName is null) { throw new ArgumentNullException("CivName is null"); }
        var civ = await _db.Civs.FirstOrDefaultAsync(c => c.CivName == civName);
        if (civ == null) { throw new ArgumentNullException("Civ does not exist"); }
        var background = await _db.CivBackgrounds.FirstOrDefaultAsync(b => b.CivID == civ.CivID);
        if (background == null) { return "https://cdn.pixabay.com/photo/2017/02/12/21/29/false-2061131_1280.png"; }
        else
        {
            return background.Url;
        }
    }

    public async Task<String> SetBackgroundUrl(string civName, string backgroundUrl)
    {
        if(civName is null || backgroundUrl is null)
        {
            throw new ArgumentNullException("Param is empty");
        }
        var civ = _db.Civs.FirstOrDefault(c => c.CivName == civName); 
        if (civ == null) {
            throw new ArgumentNullException("Civ does not exist, cannot give it a background");
        }
        var background = _db.CivBackgrounds.FirstOrDefault(b => b.CivID==civ.CivID);
        if (background == null)
        {
            background = new()
            {
                CivID = civ.CivID,
                Url = backgroundUrl
            };
            _db.CivBackgrounds.Add(background);
        }
        else
        {
            background.Url = backgroundUrl;
        }
        await _db.SaveChangesAsync();
        return background.Url;
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