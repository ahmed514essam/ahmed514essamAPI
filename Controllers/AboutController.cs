using Microsoft.EntityFrameworkCore;
using ahmed514essamAPI.Data;
using ahmed514essamAPI.Dtos;
using ahmed514essamAPI.Models;
using essamClothes.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ahmed514essamAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AboutController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly AppDbContext context;
        private readonly CloudinaryService cloudinary;

        public AboutController(AppDbContext context , IConfiguration config , CloudinaryService cloudinary)
        {
            this.context = context;
            this.config = config;
            this.cloudinary = cloudinary;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddAbout([FromForm] CreateAboutDto dto)
        {
            var About = new About
            { 
                Name = dto.Name,
                BirthDate = dto.BirthDate,
                Phone = dto.Phone,
                From = dto.From,
                Address = dto.Address,
                Email = dto.Email,
                FacebookLink = dto.FacebookLink,
                LinkedinLink = dto.LinkedinLink,
                GithubLink = dto.GithubLink,
                WhatsLink = dto.WhatsLink,
                InstagramLink = dto.InstagramLink,
                WhoAmI = dto.WhoAmI,
                Images = new List<Images>()
            };
            if (dto.Images != null && dto.Images.Count > 0)
            {
                foreach (var file in dto.Images)
                {
                    var (Url, PublicId) = await cloudinary.UploadImageAsync(file);
                    About.Images.Add(new Images
                    {
                        Url = Url,
                        PublicId = PublicId
                    });

                }
            }
            context.Abouts.Add(About);
            await context.SaveChangesAsync();

            return Ok(About);

        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAbout(int id , [FromForm] UpdateAboutDto dto)
        {
            var about = await context.Abouts.Include(a => a.Images).FirstOrDefaultAsync(a => a.Id == id);
            if (about == null) return NotFound();

            about.Name = dto.Name;
            about.BirthDate = dto.BirthDate;
            about.Phone = dto.Phone;
            about.From = dto.From;
            about.Address = dto.Address;
            about.Email = dto.Email;
            about.FacebookLink = dto.FacebookLink;
            about.LinkedinLink = dto.LinkedinLink;
            about.GithubLink = dto.GithubLink;
            about.WhatsLink = dto.WhatsLink;
            about.InstagramLink = dto.InstagramLink;
            about.WhoAmI = dto.WhoAmI;

            if (dto.DeleteImageIds != null)
            {
                var imagesDeleting = about.Images.Where(a => dto.DeleteImageIds.Contains(a.Id)).ToList();
                foreach (var img in imagesDeleting)
                {
                    await cloudinary.DeleteImageAsync(img.PublicId);
                    about.Images.Remove(img);
                }
            }
            if (dto.NewImages != null)
            {
                foreach (var file in dto.NewImages)
                {
                    var (Url, PublicId) = await cloudinary.UploadImageAsync(file);

                    about.Images.Add(new Images
                    {
                        Url = Url,
                        PublicId = PublicId
                    });
                }
            }
            await context.SaveChangesAsync();
            return Ok(about);
        }



        [HttpGet]
        public async Task<IActionResult> ShowAbout()
        {
            var about = await context.Abouts.Include(a => a.Images).ToListAsync();
            return Ok(about);
        }

    }
}
