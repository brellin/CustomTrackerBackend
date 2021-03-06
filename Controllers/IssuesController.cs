using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CustomTrackerBackend.Models;
using CustomTrackerBackend.Models.Inputs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomTrackerBackend
{
    [Route("[controller]")]
    [ApiController]
    public class IssuesController : ControllerBase
    {
        private readonly UserContext _context;

        public IssuesController(UserContext context) { _context = context; }

        // GET: api/Issues
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetIssues()
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var userIssues = _context.Issues
                    .Include(i => i.User)
                    .Include(i => i.Group)
                    .AsEnumerable()
                    .Where(i => i.User.Id == userId);
                if (userIssues.Count() < 1) return NotFound(new { message = "Good request, but no issues found" });
                await Task.Delay(500);
                return Ok(new { issues = userIssues });
            }
            catch (Exception err) { return StatusCode(500, err); }
        }

        // GET: api/Issue/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetIssue(long id)
        {
            try
            {
                Issue issue = await _context.Issues
                    .Include(i => i.User)
                    .Include(i => i.Group)
                    .FirstAsync(i => i.Id == id);
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (issue == null) return NotFound("This issue does not exist");
                if (userId != issue.UserId) return Unauthorized(new { error = "You are not authorized to view this issue" });
                return Ok(issue);
            }
            catch (Exception err)
            {
                if (err.Source == "System.Linq") return NotFound($"Issue with id '{id}' does not exist");
                return StatusCode(500, err);
            }
        }

        // PUT: api/Issue/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIssue(long id, IssueInput input)
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                Issue issueMatch = await _context.Issues.FirstAsync(i => i.Id == id);
                if (issueMatch == null) return NotFound(new { error = "This issue does not exist" });
                if (issueMatch.UserId != userId) return Unauthorized(new { error = "You are not authorized to edit this issue" });
                foreach (var field in typeof(Issue).GetProperties())
                {
                    try
                    {
                        var fieldMatch = typeof(IssueInput).GetProperties().First(f => f.Name == field.Name).GetValue(input);
                        if (fieldMatch != null) field.SetValue(issueMatch, fieldMatch);
                    }
                    catch (Exception) { }
                }
                await _context.SaveChangesAsync();
                return Ok(issueMatch);
            }
            catch (Exception err)
            {
                if (err.Source == "System.Linq") return NotFound($"Issue with id '{id}' does not exist");
                return StatusCode(500, err);
            }
        }

        // POST: api/Issue
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Issue>>> PostIssue(IssueInput input)
        {
            try
            {
                string username = User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (input.Group == null) input.Group = username.ToString();
                bool groupExists = _context.Groups.Any(g => g.Name == input.Group);
                if (!groupExists)
                {
                    _context.Groups.Add(new Group() { Name = input.Group, OwnerId = userId });
                    _context.SaveChanges();
                }
                Group group = _context.Groups.First(g => g.Name == input.Group);
                Issue issue = new Issue()
                {
                    Name = input.Name,
                    IsComplete = input.IsComplete,
                    Detail = input.Detail,
                    GroupId = group.Id
                };
                issue.UserId = userId;
                _context.Issues.Add(issue);
                await _context.SaveChangesAsync();
                issue.User = await _context.Users.FirstAsync(u => u.Id == issue.UserId);
                return CreatedAtAction(nameof(PostIssue), new { id = issue.Id }, new { issue });
            }
            catch (Exception err) { return StatusCode(500, err); }
        }

        // DELETE: api/Issue/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIssue(long id)
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                Issue issue = await _context.Issues.FindAsync(id);
                if (issue == null) return NotFound(new { error = "This issue does not exist" });
                if (userId != issue.UserId) return Unauthorized(new { error = "You are not authorized to delete this issue" });
                _context.Issues.Remove(issue);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception err)
            {
                if (err.Source == "Microsoft.EntityFrameworkCore") return NotFound($"Issue with id '{id}' does not exist");
                return StatusCode(500, err);
            }
        }
    }
}
