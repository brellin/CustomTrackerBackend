using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CustomTrackerBackend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CustomTrackerBackend
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssuesController : ControllerBase
    {
        private readonly UserContext _context;
        private UserManager<User> _userManager;

        public IssuesController(UserContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Issues
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetIssues()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userIssues = _context.Issues
                .Include(i => i.User)
                .AsEnumerable()
                .Where(i => i.User.Id == userId);

            if (userIssues.Count() < 1) return NotFound(new { message = "Good request, but no issues found" });

            await Task.Delay(500);

            return Ok(new { issues = userIssues });
        }

        // GET: api/Issue/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Issue>> GetIssue(long id)
        {
            Issue issue = await _context.Issues.Include(i => i.User).FirstAsync(i => i.Id == id);

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (issue == null) return NotFound("This issue does not exist");

            if (userId != issue.UserId) return Unauthorized(new { error = "You are not authorized to view this issue" });

            return Ok(issue);
        }

        // PUT: api/Issue/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIssue(long id, Issue issue)
        {
            if (id != issue.Id) return BadRequest("Issue ID does not match route ID");

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Issue issueMatch = await _context.Issues.FindAsync(id);

            if (issueMatch == null) return NotFound(new { error = "This issue does not exist" });

            if (issueMatch.UserId != userId) return Unauthorized(new { error = "You are not authorized to edit this issue" });

            _context.Entry(issue).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Issue
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Issue>> PostIssue(Issue issue)
        {
            issue.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _context.Issues.Add(issue);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(PostIssue), new { id = issue.Id }, new { issue });
        }

        // DELETE: api/Issue/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Issue>> DeleteIssue(long id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Issue issue = await _context.Issues.FindAsync(id);

            if (issue == null) return NotFound(new { error = "This issue does not exist" });

            if (userId != issue.UserId) return Unauthorized(new { error = "You are not authorized to delete this issue" });

            _context.Issues.Remove(issue);
            await _context.SaveChangesAsync();

            return issue;
        }
    }
}
