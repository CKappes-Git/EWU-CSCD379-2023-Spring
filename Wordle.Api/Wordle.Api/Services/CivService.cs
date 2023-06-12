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

    public async Task<Word> AddWordAsync(string? newWord, bool isCommon)
    {
        if (newWord is null || newWord.Length != 5)
        {
            throw new ArgumentException("Word must be 5 characters long");
        }
        var word = await _db.Words.FirstOrDefaultAsync(w => w.Text == newWord);
        if (word != null)
        {
            word.IsCommon = isCommon;
        }
        else
        {
            word = new()
            {
                Text = newWord,
                IsCommon = isCommon
            };
            _db.Words.Add(word);
        }
        await _db.SaveChangesAsync();
        return word;
    }

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