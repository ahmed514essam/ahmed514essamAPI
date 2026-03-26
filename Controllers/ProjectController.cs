using ahmed514essamAPI.Data;
using ahmed514essamAPI.Dtos;
using ahmed514essamAPI.Models;
using essamClothes.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ahmed514essamAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly AppDbContext context;
        private readonly CloudinaryService cloudinary;

        public ProjectController(AppDbContext context, IConfiguration config, CloudinaryService cloudinary)
        {
            this.context = context;
            this.config = config;
            this.cloudinary = cloudinary;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddProject([FromForm] CreateProjectDto dto)
        {
            var project = new Project
            {
                Id = dto.Id,
                Name = dto.Name,
                SubTitle = dto.SubTitle,
                Description = dto.Description,
                DemoLink = dto.DemoLink,
                RepoLink = dto.RepoLink,
                Images = new List<Images>()
            };
            if (dto.Images != null && dto.Images.Count > 0)
            {
                foreach (var file in dto.Images)
                {
                    var (Url, PublicId) = await cloudinary.UploadImageAsync(file);
                    project.Images.Add(new Images
                    {
                        Url = Url,
                        PublicId = PublicId
                    });

                }
            }
            context.Projects.Add(project);
            await context.SaveChangesAsync();
            return Ok(project);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, [FromForm] UpdateProjectDto dto)
        {
            var project = await context.Projects.Include(a => a.Images).FirstOrDefaultAsync(a => a.Id == id);
            if (project == null) return NotFound();

            project.Id = dto.Id;
            project.Name = dto.Name;
            project.SubTitle = dto.SubTitle;
            project.Description = dto.Description;
            project.DemoLink = dto.DemoLink;
            project.RepoLink = dto.RepoLink;

            if (dto.DeleteImageIds != null)
            {
                var imagesDeleting = project.Images.Where(a => dto.DeleteImageIds.Contains(a.Id)).ToList();
                foreach (var img in imagesDeleting)
                {
                    await cloudinary.DeleteImageAsync(img.PublicId);
                    project.Images.Remove(img);
                }
            }
            if (dto.NewImages != null)
            {
                foreach (var file in dto.NewImages)
                {
                    var (Url, PublicId) = await cloudinary.UploadImageAsync(file);

                    project.Images.Add(new Images
                    {
                        Url = Url,
                        PublicId = PublicId
                    });
                }
            }
            await context.SaveChangesAsync();
            return Ok(project);
        }

        [HttpGet]
        public async Task<IActionResult> ShowProjects()
        {
            var project = await context.Projects.Include(a => a.Images).ToListAsync();
            return Ok(project);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ShowProjectById(int id)
        {
            var project = await context.Projects.Include(a => a.Images).FirstOrDefaultAsync(a => a.Id == id);
            if (project == null) return NotFound();
            return Ok(project);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await context.Projects.Include(a => a.Images).FirstOrDefaultAsync(a => a.Id == id);
            if (project == null) return NotFound();


            foreach (var img in project.Images)
            {
                await cloudinary.DeleteImageAsync(img.PublicId);
            }

            context.Projects.Remove(project);
            await context.SaveChangesAsync();

            return Ok("Deleted");
        }

    }
}
