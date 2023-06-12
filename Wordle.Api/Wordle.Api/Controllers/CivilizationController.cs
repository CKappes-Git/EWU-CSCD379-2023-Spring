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
        public async Task<IEnumerable<Leader>> GetLeaders(int? count, string start = "")
        {
            return await _civService.GetLeadersAsync(count, start);
        }

        [HttpPost("AddCiv")]
        public async Task<Civ> AddCiv(string civName)
        {
            return await _civService.AddCivAsync(civName);
        }

        
        [HttpPost("AddLeader")]
        //[Authorize(Policy = Policies.MasterOfTheUniverse)]
        public async Task<Leader> AddLeader(string civName, string leaderName)
        {
            return await _civService.AddLeaderAsync(civName, leaderName);
        }
        /*
        [HttpPost("EditAttribute")]
        [Authorize(Policy = Policies.MasterOfTheUniverse)]
        public async Task<Word> DeleteWord([FromBody] WordDto word)
        {
            return await _civService.DeleteWordAsync(word.Text);
        }
        */
        [HttpGet("paginatedLeaders")]
        public async Task<IEnumerable<Leader>> GetPaginatedWords(int page = 1, int count = 10, string start = "")
        {
            return (await _civService.GetPaginatedLeadersAsync(page, count, start));
        }
    }
}
