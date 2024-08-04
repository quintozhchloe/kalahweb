using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using KalahGameApi.Data;
using KalahGameApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalahGameApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaderboardController : ControllerBase
    {
        private readonly LeaderboardContext _context;

        public LeaderboardController(LeaderboardContext context)
        {
            _context = context;
        }

        // GET: api/Leaderboard
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeaderboardEntry>>> GetLeaderboardEntries()
        {
            var entries = await _context.LeaderboardEntries.Find(entry => true).ToListAsync();
            return entries.OrderByDescending(e => e.Score).ToList();
        }

        // GET: api/Leaderboard/playerName
        [HttpGet("{playerName}")]
        public async Task<ActionResult<LeaderboardEntry>> GetLeaderboardEntry(string playerName)
        {
            var entry = await _context.LeaderboardEntries.Find<LeaderboardEntry>(entry => entry.PlayerName == playerName).FirstOrDefaultAsync();

            if (entry == null)
            {
                return NotFound();
            }

            return entry;
        }

        // POST: api/Leaderboard
        [HttpPost]
        public async Task<ActionResult<LeaderboardEntry>> PostLeaderboardEntry([FromBody] LeaderboardEntry leaderboardEntry)
        {
            if (leaderboardEntry == null)
            {
                return BadRequest("LeaderboardEntry is required.");
            }

            await _context.LeaderboardEntries.InsertOneAsync(leaderboardEntry);
            return CreatedAtAction(nameof(GetLeaderboardEntry), new { playerName = leaderboardEntry.PlayerName }, leaderboardEntry);
        }

        // PUT: api/Leaderboard/playerName
        [HttpPut("{playerName}")]
        public async Task<IActionResult> PutLeaderboardEntry(string playerName, LeaderboardEntry leaderboardEntry)
        {
            var entry = await _context.LeaderboardEntries.Find<LeaderboardEntry>(entry => entry.PlayerName == playerName).FirstOrDefaultAsync();

            if (entry == null)
            {
                return NotFound();
            }

            await _context.LeaderboardEntries.ReplaceOneAsync(e => e.PlayerName == playerName, leaderboardEntry);

            return NoContent();
        }

        // DELETE: api/Leaderboard/playerName
        [HttpDelete("{playerName}")]
        public async Task<IActionResult> DeleteLeaderboardEntry(string playerName)
        {
            var entry = await _context.LeaderboardEntries.Find<LeaderboardEntry>(entry => entry.PlayerName == playerName).FirstOrDefaultAsync();

            if (entry == null)
            {
                return NotFound();
            }

            await _context.LeaderboardEntries.DeleteOneAsync(e => e.PlayerName == playerName);

            return NoContent();
        }
    }
}
