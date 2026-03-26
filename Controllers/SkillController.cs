using ahmed514essamAPI.Data;
using ahmed514essamAPI.Dtos;
using ahmed514essamAPI.Models;
using essamClothes.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ahmed514essamAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly AppDbContext context;
        private readonly CloudinaryService cloudinary;

        public SkillController(AppDbContext context, IConfiguration config, CloudinaryService cloudinary)
        {
            this.context = context;
            this.config = config;
            this.cloudinary = cloudinary;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddSkill([FromForm] CreateSkillDto dto)
        {
            var skill = new Skill
            {
                    Id = dto.Id,
                Name = dto.Name,
                Type = dto.Type,

                Images = new List<Images>()
            };
            if (dto.Images != null && dto.Images.Count > 0)
            {
                foreach (var file in dto.Images)
                {
                    var (Url, PublicId) = await cloudinary.UploadImageAsync(file);
                    skill.Images.Add(new Images
                    {
                        Url = Url,
                        PublicId = PublicId
                    });

                }
            }
            context.Skills.Add(skill);
            await context.SaveChangesAsync();
            return Ok(skill);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSkill(int id, [FromForm] UpdateSkillDto dto)
        {
            var skill = await context.Skills.Include(a => a.Images).FirstOrDefaultAsync(a => a.Id == id);
            if (skill == null) return NotFound();

            skill.Id = dto.Id;
            skill.Name = dto.Name;
            skill.Type = dto.Type;
           

            if (dto.DeleteImageIds != null)
            {
                var imagesDeleting = skill.Images.Where(a => dto.DeleteImageIds.Contains(a.Id)).ToList();
                foreach (var img in imagesDeleting)
                {
                    await cloudinary.DeleteImageAsync(img.PublicId);
                    skill.Images.Remove(img);
                }
            }
            if (dto.NewImages != null)
            {
                foreach (var file in dto.NewImages)
                {
                    var (Url, PublicId) = await cloudinary.UploadImageAsync(file);

                    skill.Images.Add(new Images
                    {
                        Url = Url,
                        PublicId = PublicId
                    });
                }
            }
            await context.SaveChangesAsync();
            return Ok(skill);
        }

        [HttpGet]
        public async Task<IActionResult> ShowSkill()
        {
            var skill = await context.Skills.Include(a => a.Images).ToListAsync();
            return Ok(skill);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ShowSkillById(int id)
        {
            var skill = await context.Skills.Include(a => a.Images).FirstOrDefaultAsync(a => a.Id == id);
            if (skill == null) return NotFound();
            return Ok(skill);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSkill(int id)
        {
            var skill = await context.Skills.Include(a => a.Images).FirstOrDefaultAsync(a => a.Id == id);
            if (skill == null) return NotFound();


            foreach (var img in skill.Images)
            {
                await cloudinary.DeleteImageAsync(img.PublicId);
            }

            context.Skills.Remove(skill);
            await context.SaveChangesAsync();

            return Ok("Deleted");
        }
    }
}
