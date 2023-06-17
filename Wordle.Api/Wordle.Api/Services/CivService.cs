using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
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

    public async Task<IEnumerable<Civ>> GetCivsAsync(string game, int? count, string start = "")
    {
        count ??= 10;
        var civs = await _db.Civs
          .Where(c => c.CivName.StartsWith(start) && c.Game == game)
          .Take(count.Value)
          .OrderBy(c => c.CivName)
          .ToListAsync();
        return civs;
    }
    public async Task<IEnumerable<Leader>> GetLeadersAsync(string game, int? count, string? civName, string start = "")
    {
        count ??= 10;
        if(civName != null)
        {
            var civ = await _db.Civs.FirstOrDefaultAsync(c => c.CivName.StartsWith(civName) && c.Game == game);
            if(civ != null)
            {
                var leader = await _db.Leaders
                  .Where(l => l.Name.StartsWith(start) && l.CivID == civ.CivID)
                  .Take(count.Value)
                  .OrderBy(l => l.Name)
                  .ToListAsync();
                return leader;
            }
        }
        
        var leaders = await _db.Leaders
            .Where(l => l.Name.StartsWith(start) && l.Game == game)
            .Take(count.Value)
            .OrderBy(l => l.Name)
            .ToListAsync();
        return leaders;
        
    }

    public async Task<Civ> AddCivAsync(string game, string newCivName)
    {
        if (newCivName is null || game is null)
        {
            throw new ArgumentException("Required Parameter was null");
        }

        var civ = await _db.Civs.FirstOrDefaultAsync(c => c.CivName == newCivName && c.Game == game);
        if (civ == null)
        {
            civ = new() { CivName = newCivName, Game = game };
            _db.Civs.Add(civ);
        }
        
        await _db.SaveChangesAsync();
        return civ;
    }

    public async Task<Leader> AddLeaderAsync(string game, string civName, string leaderName)
    {
        if (leaderName is null || civName is null || game is null)
        {
            throw new ArgumentException("The leader is missing something");
        }

        var civ = await _db.Civs.FirstOrDefaultAsync(c => c.CivName == civName && c.Game == game);
        if (civ == null) {
            civ = await AddCivAsync(game, civName);
        }
        var leader = await _db.Leaders.FirstOrDefaultAsync(l => l.Name == leaderName && l.Game == game);
        if (leader == null)
        {
             leader = new()
            {
                Name = leaderName,
                CivID = civ.CivID,
                Game = game
            };
            _db.Leaders.Add(leader);
        }
        await _db.SaveChangesAsync();
        return leader;
    }

    public async Task<LeaderInfoDto> AddAttributesAsync(string game, LeaderInfoDto leaderInfo)
    {
        if (leaderInfo is null)
        {
            throw new ArgumentException("The leader is missing");
        }
        
        var civ = await _db.Civs.FirstOrDefaultAsync(c => c.CivName == leaderInfo.CivName && c.Game == game);
        if (civ == null)
        {
            civ = await AddCivAsync(game, leaderInfo.CivName);
        }
        var leader = await _db.Leaders.FirstOrDefaultAsync(l => l.Name == leaderInfo.LeaderName && l.Game == game);
        if (leader == null)
        {
            leader = await AddLeaderAsync(game, leaderInfo.CivName, leaderInfo.LeaderName);
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
        return await GetLeaderInfoAsync(game, leader.Name);

    }

    public async Task<LeaderInfoDto> GetLeaderInfoAsync(string game, string leaderName)
    {
        var leader = await _db.Leaders.FirstOrDefaultAsync(l => l.Name == leaderName && l.Game == game);
        if (leader == null)
        {
            throw new ArgumentException("There is no leader by that name");
        }
        var civ = await _db.Civs.FirstOrDefaultAsync(c => c.CivID == leader.CivID && c.Game == game);
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

    public async Task<String> GetBackgroundUrl(string game, string civName)
    {
        if(civName is null) { throw new ArgumentNullException("CivName is null"); }
        var civ = await _db.Civs.FirstOrDefaultAsync(c => c.CivName == civName && c.Game == game);
        if (civ == null) { throw new ArgumentNullException("Civ does not exist"); }
        var background = await _db.CivBackgrounds.FirstOrDefaultAsync(b => b.CivID == civ.CivID);
        //returns an error link
        if (background == null) { return "https://cdn.pixabay.com/photo/2017/02/12/21/29/false-2061131_1280.png"; }
        else
        {
            return background.Url;
        }
    }

    public async Task<String> SetBackgroundUrl(string game, string civName, string backgroundUrl)
    {
        if(civName is null || backgroundUrl is null)
        {
            throw new ArgumentNullException("Param is empty");
        }
        var civ = _db.Civs.FirstOrDefault(c => c.CivName == civName && c.Game == game); 
        if (civ == null) {
            throw new ArgumentNullException("Civ does not exist, cannot give it a background");
        }
        var background = _db.CivBackgrounds.FirstOrDefault(b => b.CivID == civ.CivID);
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

    
    public async Task<CivAttribute> DeleteCivAttributeAsync(int civAttributeID)
    {
        if(civAttributeID <= 0)
        {
            throw new ArgumentException("invalid id for deletion");
        }
        var targetAttribute = await _db.CivAttributes.FirstOrDefaultAsync(a => a.CivAttributeID == civAttributeID);
        if (targetAttribute != null)
        {
            _db.CivAttributes.Remove(targetAttribute);
        }
        await _db.SaveChangesAsync();
        return targetAttribute;

    }

    public async Task<LeaderAttribute> DeleteLeaderAttributeAsync(int leaderAttributeID)
    {
        if (leaderAttributeID <= 0)
        {
            throw new ArgumentException("invalid id for deletion");
        }
        var targetAttribute = await _db.LeaderAttributes.FirstOrDefaultAsync(a => a.LeaderAttributeID == leaderAttributeID);
        if (targetAttribute != null)
        {
            _db.LeaderAttributes.Remove(targetAttribute);
        }
        await _db.SaveChangesAsync();
        return targetAttribute;

    }

    public async Task<IEnumerable<Leader>> GetPaginatedLeadersAsync(string game, int page = 1, int count = 10, string start = "")
    {
        if (page < 1) page = 1;
        if (count < 1 || count > 100) count = 10;
        page--;
        var index = page * count;

        var totalCount = await _db.Leaders.CountAsync();
        totalCount -= count;
        var leaders = await _db.Leaders
          .Where(l => l.Name.StartsWith(start) && l.Game == game)
          .OrderBy(l => l.Name)
          .Skip(index)
          .Take(count)
          .ToListAsync();
        return leaders;
    }

    public async Task<Leader> DeleteLeaderAsync(string game, string leaderName)
    {
        if(leaderName is null)
        {
            throw new ArgumentNullException(nameof(leaderName));
        }
        var leader = _db.Leaders.Where(l => l.Name == leaderName && l.Game == game).FirstOrDefault();
        if (leader != null)
        {
            //delete all leader notes
            var leaderNotes = await _db.LeaderNotes
                .Where(n => n.LeaderID == leader.LeaderID)
                .ToListAsync();

            if (leaderNotes != null)
            {
                foreach (var n in leaderNotes)
                {
                    _db.LeaderNotes.Remove(n);
                }
            }


            //delete all leader attributes
            var leaderAttributes = await _db.LeaderAttributes
                .Where(a => a.LeaderID == leader.LeaderID)
                .ToListAsync();
            if (leaderAttributes != null)
            {
                foreach (var a in leaderAttributes)
                {
                    _db.LeaderAttributes.Remove(a);
                }
            }

            _db.Leaders.Remove(leader);

            await _db.SaveChangesAsync();
        }

        return leader;
        
    }

    public async Task<Civ> DeleteCivAsync(string game, string civName)
    {
        if(civName is null)
        {
            throw new ArgumentNullException("Civ name was null");
        }

        var civ = await _db.Civs.Where(c => c.CivName == civName && c.Game == game).FirstOrDefaultAsync();
        if(civ != null)
        {
            var leaders = await _db.Leaders.Where(l => l.CivID == civ.CivID && l.Game == game).ToListAsync();
            if(leaders != null)
            {
                foreach(var l in leaders)
                {
                    await DeleteLeaderAsync(game, l.Name);
                }
            }

            var civAttributes = await _db.CivAttributes
                .Where(a => a.CivID == civ.CivID) 
                .ToListAsync();
            if(civAttributes != null)
            {
                foreach( var a in civAttributes)
                {
                    _db.CivAttributes.Remove(a);
                }
            }
            _db.Civs.Remove(civ);

            await _db.SaveChangesAsync();
        }

        return civ;
    }

    public async Task<IEnumerable<LeaderNotes>> GetLeaderNotes(string game, string leaderName, string appUserId)
    {
        if (leaderName == null) { throw new ArgumentNullException("Leader Id cannot be null"); }
        if (appUserId == null) { throw new ArgumentNullException("AppUser Id cannot be null"); }

        var leader = await _db.Leaders.Where(l => l.Name == leaderName && l.Game == game).FirstOrDefaultAsync();
        if(leader != null)
        {
            var notes = await _db.LeaderNotes.Where(n => n.LeaderID == leader.LeaderID && n.AppUserID == appUserId).ToListAsync();
            return notes;
        }
        return null;
    }

    public async Task<int> SetLeaderNote(LeaderNoteDto newNote)
    {
        //notes target the leader directly and don't need a game reference
        if (newNote == null) { throw new ArgumentNullException(); }
        if(newNote.LeaderNoteID == 0)
        {//adding a new note
            LeaderNotes note = new() { 
                LeaderID = newNote.LeaderID,
                AppUserID = newNote.AppUserID,
                NoteName = newNote.NoteName,
                ScienceTree = newNote.ScienceTree,
                CultureTree = newNote.CultureTree,
                Production = newNote.Production,
                Notes = newNote.Notes,
            };
            _db.Add(note);
            await _db.SaveChangesAsync();
            return note.LeaderNoteID;
        }
        else
        {//updating previous note
            var oldNote = await _db.LeaderNotes.Where(n => n.LeaderNoteID == newNote.LeaderNoteID).FirstOrDefaultAsync();
            if (oldNote != null)
            {
                oldNote.NoteName = newNote.NoteName;
                oldNote.ScienceTree = newNote.ScienceTree;
                oldNote.CultureTree = newNote.CultureTree;
                oldNote.Production = newNote.Production;
                oldNote.Notes = newNote.Notes;
            }
            else {
                LeaderNotes note = new()
                {
                    LeaderID = newNote.LeaderID,
                    AppUserID = newNote.AppUserID,
                    NoteName = newNote.NoteName,
                    ScienceTree = newNote.ScienceTree,
                    CultureTree = newNote.CultureTree,
                    Production = newNote.Production,
                    Notes = newNote.Notes,
                };
                _db.Add(note);
                await _db.SaveChangesAsync();
                return note.LeaderNoteID;
            }
            
        }

        await _db.SaveChangesAsync();
        return newNote.LeaderNoteID;
    }

    public async Task<Boolean> DeleteLeaderNote(int targetNoteId)
    {

        var target = await _db.LeaderNotes.Where(n => n.LeaderNoteID == targetNoteId).FirstOrDefaultAsync();
        if (target != null)
        {
            _db.LeaderNotes.Remove(target);
        }
        await _db.SaveChangesAsync();
        return true;
    }
}