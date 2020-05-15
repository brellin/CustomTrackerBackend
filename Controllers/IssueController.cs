using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CustomTrackerBackend.Models;

namespace CustomTrackerBackend
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssueController : ControllerBase
    {
        private readonly UserContext _context;

        public IssueController(UserContext context)
        {
            _context = context;
        }

        // GET: api/Issue
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Issue>>> GetIssues()
        {
            return await _context.Issues.ToListAsync();
        }

        // GET: api/Issue/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Issue>> GetIssue(long id)
        {
            var issue = await _context.Issues.FindAsync(id);

            if (issue == null)
            {
                return NotFound();
            }

            return issue;
        }

        // PUT: api/Issue/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIssue(long id, Issue issue)
        {
            if (id != issue.Id)
            {
                return BadRequest();
            }

            _context.Entry(issue).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IssueExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Issue
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Issue>> PostIssue(Issue issue)
        {
            _context.Issues.Add(issue);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIssue", new { id = issue.Id }, issue);
        }

        // DELETE: api/Issue/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Issue>> DeleteIssue(long id)
        {
            var issue = await _context.Issues.FindAsync(id);
            if (issue == null)
            {
                return NotFound();
            }

            _context.Issues.Remove(issue);
            await _context.SaveChangesAsync();

            return issue;
        }

        private bool IssueExists(long id)
        {
            return _context.Issues.Any(e => e.Id == id);
        }
    }
}
