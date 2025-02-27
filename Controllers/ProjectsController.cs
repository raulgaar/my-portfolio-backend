using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using my_portfolio_backend.Models;

namespace my_portfolio_backend.Controllers
{
    [Route("api/projects")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Get: api/Projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects([FromQuery] string lang = "en")
        {
            var projects = await _context.Projects.ToListAsync();

            var localizedProjects = projects.Select(p => new
            {
                Id = p.Id,
                Title = GetLocalizedValue(p.Title,lang),
                Description = GetLocalizedValue(p.Description,lang),
                Url = p.Url
            });

            return Ok(localizedProjects);
        }

        //Get: api/Projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if(project == null)
            {
                return NotFound();
            }

            return project;
        }

        //Post: api/Projects
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Project>> PostProject([FromBody] ProjectDto projectDto)
        {
            var project = new Project
            {
                Title = JsonSerializer.Serialize(projectDto.Title),
                Description = JsonSerializer.Serialize(projectDto.Description),
                Url = projectDto.Url
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProjects", new { id = project.Id }, project);
        }

        //Put: api/Projects/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(int id, Project project)
        {
            if (id != project.Id)
            {
                return BadRequest();
            }

            _context.Entry(project).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
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
        
        //Delete: api/Projects/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
           var project = await _context.Projects.FindAsync(id);
           if (project == null)
           {
            return NotFound();
           }

           _context.Projects.Remove(project);
           await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }

        [HttpGet("test-error")]
        public IActionResult TestError()
        {
            throw new Exception("Simulated Error");
        }

        private string GetLocalizedValue(string json, string lang)
        {
            if (string.IsNullOrWhiteSpace(json)) return string.Empty;

            try {
                var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                return dict.ContainsKey(lang) ? dict[lang] : dict.GetValueOrDefault("en", string.Empty);
            }
            catch
            {
                return string.Empty;
            }
        }
        public class ProjectDto
        {
            public Dictionary<string, string> Title { get; set; } = new();
            public Dictionary<string, string> Description { get; set; } = new();
            public string Url { get; set; } = string.Empty;
        }
    }
}