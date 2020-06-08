using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CustomTrackerBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomTrackerBackend.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly UserContext context;
        public GroupsController(UserContext _context) { context = _context; }

        // GET groups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Group>>> GetGroups()
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                List<Group> ownedGroups = await context.Groups
                    .Include(g => g.Issues)
                    .Include(g => g.Owner)
                    .Where(g => g.OwnerId == userId)
                    .ToListAsync();
                if (ownedGroups.Count() == 0) return NotFound("No groups found");
                return Ok(ownedGroups);
            }
            catch (Exception err) { return StatusCode(500, err); }
        }

        // GET groups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Group>> GetGroupById(int id)
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                Group GroupMatch = context.Groups.Include(g => g.Issues).First(g => g.Id == id);
                await Task.Delay(500);
                if (GroupMatch == null) return NotFound();
                if (GroupMatch.OwnerId != userId) return Unauthorized();
                return Ok(GroupMatch);
            }
            catch (Exception err)
            {
                if (err.Source == "System.Linq") return NotFound($"Group with id of '{id}' does not exist");
                return StatusCode(500, err);
            }
        }

        // POST groups
        [HttpPost("")]
        public async Task<ActionResult<Group>> PostGroup(Group group)
        {
            if (!ModelState.IsValid) return UnprocessableEntity("Must include name");
            try
            {
                string userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                group.OwnerId = userId;
                await context.AddAsync(group);
                await context.SaveChangesAsync();
                return Ok(context.Groups.Include(g => g.Owner).Include(g => g.Issues).First(g => g.Name == group.Name));
            }
            catch (Exception err)
            {
                if (err.Source == "Microsoft.EntityFrameworkCore.Relational") return Conflict($"Group already exists with name '{group.Name}'");
                return StatusCode(500, err);
            }
        }

        // PUT groups/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Group>> PutGroup(int id, Group group)
        {
            if (!ModelState.IsValid) return UnprocessableEntity(ModelState);
            try
            {
                string userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                Group match = await context.Groups.FirstAsync(g => g.Id == id);
                if (userId != match.OwnerId) return Unauthorized();
                match.Name = group.Name;
                match.OwnerId = group.OwnerId;
                await context.SaveChangesAsync();
                return Ok(match);
            }
            catch (Exception err)
            {
                if (err.Source == "System.Linq") return NotFound($"Group with id of '{id}' does not exist");
                return StatusCode(500, err);
            }
        }

        // DELETE groups/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteGroupById(int id)
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                Group match = await context.Groups.FirstAsync(g => g.Id == id);
                if (userId != match.OwnerId) return Unauthorized();
                context.Groups.Remove(match);
                await context.SaveChangesAsync();
                return Accepted();
            }
            catch (Exception err)
            {
                if (err.Source == "System.Linq") return NotFound($"Group with id of '{id}' does not exist");
                return StatusCode(500, err);
            }
        }
    }
}
