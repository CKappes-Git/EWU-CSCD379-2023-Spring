using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wordle.Api.Data;
using Wordle.Api.Dtos;
using Wordle.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Wordle.Api.Identity;

namespace Wordle.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CivilizationController : ControllerBase
    {
        private readonly CivService _civService;
        public UserManager<AppUser> _userManager;
        public JwtConfiguration _jwtConfiguration;

        public CivilizationController(CivService civService, UserManager<AppUser> userManager, JwtConfiguration jwtConfiguration)
        {
            _civService = civService;
            _userManager = userManager;
            _jwtConfiguration = jwtConfiguration;
        }

        [HttpGet("GetCivs")]
        public async Task<IEnumerable<Civ>> GetCivs(string game, int? count, string start = "")
        {
            return await _civService.GetCivsAsync(game, count, start);
        }

        [HttpGet("GetLeaders")]
        public async Task<IEnumerable<Leader>> GetLeaders(string game, int? count, string? civName, string start = "")
        {
            return await _civService.GetLeadersAsync(game, count, civName, start);
        }

        [HttpPost("AddCiv")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<Civ> AddCiv(string game, string civName)
        {
            return await _civService.AddCivAsync(game, civName);
        }

        
        [HttpPost("AddLeader")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<Leader> AddLeader(string game, string civName, string leaderName)
        {
            return await _civService.AddLeaderAsync(game, civName, leaderName);
        }

        [HttpPost("AddAttributes")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<LeaderInfoDto> AddAttributes(string game, LeaderInfoDto leaderInfo)
        {
            return await _civService.AddAttributesAsync(game, leaderInfo);
        }

        [HttpGet("AllLeaderData")]
        public async Task<LeaderInfoDto> GetAllLeaderData(string game, string leaderName)
        {
            return await _civService.GetLeaderInfoAsync(game, leaderName);
        }

        [HttpPost("DeleteCivAttribute")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<CivAttribute> DeleteCivAttribute(int civAttributeID)
        {
            return await _civService.DeleteCivAttributeAsync(civAttributeID);
        }

        [HttpPost("DeleteLeaderAttribute")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<LeaderAttribute> DeleteLeaderAttribute(int leaderAttributeID)
        {
            return await _civService.DeleteLeaderAttributeAsync(leaderAttributeID);
        }

        [HttpPost("DeleteLeader")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<Leader> DeleteLeader(string game, string leaderName)
        {
            return await _civService.DeleteLeaderAsync(game, leaderName);
        }

        [HttpPost("DeleteCiv")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<Civ> DeleteCiv(string game, string civName)
        {
            return await _civService.DeleteCivAsync(game, civName);
        }

        [HttpGet("paginatedLeaders")]
        public async Task<IEnumerable<Leader>> GetPaginatedWords(string game, int page = 1, int count = 10, string start = "")
        {
            return (await _civService.GetPaginatedLeadersAsync(game, page, count, start));
        }
        [HttpPost("setBackgroundUrl")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<String> SetBackground(string game, string civName, string url)
        {
            return await _civService.SetBackgroundUrl(game, civName, url);
        }
        [HttpGet("GetBackgroundUrl")]
        public async Task<String> GetBackground(string game, string civName)
        {
            return await _civService.GetBackgroundUrl(game, civName);
        }
        [HttpGet("GetLeaderNotes")]
        [Authorize]
        public async Task<IEnumerable<LeaderNotes>> GetLeaderNotes(string game, string leaderName, string appUserId)
        {
            return await _civService.GetLeaderNotes(game, leaderName, appUserId);
        }
        [HttpPost("SetLeaderNote")]
        [Authorize]
        public async Task<int> SetLeaderNote(LeaderNoteDto leaderNote)
        {
            return await _civService.SetLeaderNote(leaderNote);
        }

        [HttpPost("DeleteLeaderNote")]
        [Authorize]
        //find way to use authorize to pass in user ID (could also be done in app, but I would prefer to do it with the header if possible)
        public async Task<Boolean> DeleteLeaderNote(int leaderNoteId)
        {
            return await _civService.DeleteLeaderNote(leaderNoteId);
        }
    }
}
