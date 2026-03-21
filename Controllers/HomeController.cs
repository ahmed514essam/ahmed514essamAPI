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
    public class HomeController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly AppDbContext context;
        private readonly CloudinaryService cloudinary;

        public HomeController(AppDbContext context, IConfiguration config, CloudinaryService cloudinary)
        {
            this.context = context;
            this.config = config;
            this.cloudinary = cloudinary;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddHome([FromForm] CreateHomeDto dto)
        {
            var home = new Home
            {
                subTitle = dto.subTitle,
                Summary = dto.Summary,
                FacebookLink = dto.FacebookLink,
                LinkedinLink = dto.LinkedinLink,
                GithubLink = dto.GithubLink,
                WhatsLink = dto.WhatsLink,
                InstagramLink = dto.InstagramLink,
                DownloadResume = dto.DownloadResume,
                Images = new List<Images>()
            };
            if (dto.Images != null && dto.Images.Count > 0)
            {
                foreach (var file in dto.Images)
                {
                    var (Url, PublicId) = await cloudinary.UploadImageAsync(file);
                    home.Images.Add(new Images
                    {
                        Url = Url,
                        PublicId = PublicId
                    });

                }
            }
            context.Homes.Add(home);
            await context.SaveChangesAsync();

            return Ok(home);

        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHome(int id, [FromForm] UpdateHomeDto dto)
        {
            var home = await context.Homes.Include(a => a.Images).FirstOrDefaultAsync(a => a.Id == id);
            if (home == null) return NotFound();

            home.subTitle = dto.subTitle;
            home.Summary = dto.Summary;
            home.FacebookLink = dto.FacebookLink;
            home.LinkedinLink = dto.LinkedinLink;
            home.GithubLink = dto.GithubLink;
            home.WhatsLink = dto.WhatsLink;
            home.InstagramLink = dto.InstagramLink;
            home.DownloadResume = dto.DownloadResume;

            if (dto.DeleteImageIds != null)
            {
                var imagesDeleting = home.Images.Where(a => dto.DeleteImageIds.Contains(a.Id)).ToList();
                foreach (var img in imagesDeleting)
                {
                    await cloudinary.DeleteImageAsync(img.PublicId);
                    home.Images.Remove(img);
                }
            }
            if (dto.NewImages != null)
            {
                foreach (var file in dto.NewImages)
                {
                    var (Url, PublicId) = await cloudinary.UploadImageAsync(file);

                    home.Images.Add(new Images
                    {
                        Url = Url,
                        PublicId = PublicId
                    });
                }
            }
            await context.SaveChangesAsync();
            return Ok(home);
        }




        [HttpGet]
        public async Task<IActionResult> ShowHome()
        {
            var home = await context.Homes.Include(a => a.Images).ToListAsync();
            return Ok(home);
        }
    }
}
