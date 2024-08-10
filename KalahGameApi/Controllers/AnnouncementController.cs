using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using KalahGameApi.Data;
using KalahGameApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalahGameApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementsController : ControllerBase
    {
        private readonly IMongoCollection<Announcement> _announcements;

        public AnnouncementsController(LeaderboardContext context)
        {
            _announcements = context.Announcements;
        }

        // GET: api/Announcements
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Announcement>>> GetAnnouncements()
        {
            var announcements = await _announcements.Find(a => true).ToListAsync();
            return announcements;
        }

        // POST: api/Announcements
        [HttpPost]
        public async Task<ActionResult<Announcement>> PostAnnouncement([FromBody] Announcement announcement)
        {
            // 获取当前最大ID
            var allAnnouncements = await _announcements.Find(a => true).ToListAsync();
            var maxId = allAnnouncements.DefaultIfEmpty().Max(a => int.TryParse(a?.Id, out int id) ? id : 0);
          

            // 生成顺序ID
            announcement.Id = (maxId + 1).ToString();

            // 设置当前日期
            announcement.Date = DateTime.UtcNow;

            // 插入公告
            await _announcements.InsertOneAsync(announcement);

            return CreatedAtAction(nameof(GetAnnouncements), new { id = announcement.Id }, announcement);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnnouncement(string id, [FromBody] Announcement announcement)
        {
            // Ensure that the Id in the announcement matches the id in the route
            if (announcement.Id != id)
            {
                return BadRequest("Announcement ID mismatch");
            }

            // Only allow updates to other fields, not the Id
            var updateResult = await _announcements.ReplaceOneAsync(a => a.Id == id, announcement);

            if (updateResult.MatchedCount == 0)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/Announcements/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnnouncement(string id)
        {
            var deleteResult = await _announcements.DeleteOneAsync(a => a.Id == id);

            if (deleteResult.DeletedCount == 0)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
