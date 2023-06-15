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
        public async Task<IEnumerable<Civ>> GetCivs(int? count, string start = "")
        {
            return await _civService.GetCivsAsync(count, start);
        }

        [HttpGet("GetLeaders")]
        public async Task<IEnumerable<Leader>> GetLeaders(int? count, string? civName, string start = "")
        {
            return await _civService.GetLeadersAsync(count, civName, start);
        }

        [HttpPost("AddCiv")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<Civ> AddCiv(string civName)
        {
            return await _civService.AddCivAsync(civName);
        }

        
        [HttpPost("AddLeader")]
        [Authorize(Roles = Roles.Admin)]
        //[Authorize(Policy = Policies.MasterOfTheUniverse)]
        public async Task<Leader> AddLeader(string civName, string leaderName)
        {
            return await _civService.AddLeaderAsync(civName, leaderName);
        }

        [HttpPost("AddAttributes")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<LeaderInfoDto> AddAttributes(LeaderInfoDto leaderInfo)
        {
            return await _civService.AddAttributesAsync(leaderInfo);
        }

        [HttpGet("AllLeaderData")]
        public async Task<LeaderInfoDto> GetAllLeaderData(string leaderName)
        {
            return await _civService.GetLeaderInfoAsync(leaderName);
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
        public async Task<Leader> DeleteLeader(string leaderName)
        {
            return await _civService.DeleteLeaderAsync(leaderName);
        }

        [HttpPost("DeleteCiv")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<Civ> DeleteCiv(string civName)
        {
            return await _civService.DeleteCivAsync(civName);
        }

        [HttpGet("paginatedLeaders")]
        public async Task<IEnumerable<Leader>> GetPaginatedWords(int page = 1, int count = 10, string start = "")
        {
            return (await _civService.GetPaginatedLeadersAsync(page, count, start));
        }
        [HttpPost("setBackgroundUrl")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<String> SetBackground(string civName, string url)
        {
            return await _civService.SetBackgroundUrl(civName, url);
        }
        [HttpGet("GetBackgroundUrl")]
        public async Task<String> GetBackground(string civName)
        {
            return await _civService.GetBackgroundUrl(civName);
        }
        [HttpGet("GetLeaderNotes")]
        [Authorize]
        public async Task<IEnumerable<LeaderNotes>> GetLeaderNotes(string leaderName, string appUserId)
        {
            return await _civService.GetLeaderNotes(leaderName, appUserId);
        }
        [HttpPost("SetLeaderNote")]
        [Authorize]
        public async Task<int> SetLeaderNote(LeaderNoteDto leaderNote)
        {
            return await _civService.SetLeaderNote(leaderNote);
        }

        [HttpPost("DeleteLeaderNote")]
        [Authorize]
        public async Task<Boolean> DeleteLeaderNote(int leaderNoteId)
        {
            return await _civService.DeleteLeaderNote(leaderNoteId);
        }
    }
}
