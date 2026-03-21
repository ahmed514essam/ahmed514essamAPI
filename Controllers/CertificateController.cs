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
    public class CertificateController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly AppDbContext context;
        private readonly CloudinaryService cloudinary;

        public CertificateController(AppDbContext context, IConfiguration config, CloudinaryService cloudinary)
        {
            this.context = context;
            this.config = config;
            this.cloudinary = cloudinary;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddCertificate([FromForm] CreateCertificateDto dto)
        {
            var Certi = new Certificate
            {
                Name = dto.Name,
                CertificatesLink = dto.CertificatesLink,

                Images = new List<Images>()
            };
            if (dto.Images != null && dto.Images.Count > 0)
            {
                foreach (var file in dto.Images)
                {
                    var (Url, PublicId) = await cloudinary.UploadImageAsync(file);
                    Certi.Images.Add(new Images
                    {
                        Url = Url,
                        PublicId = PublicId
                    });

                }
            }
            context.Certificates.Add(Certi);
            await context.SaveChangesAsync();
            return Ok(Certi);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCertificate(int id, [FromForm] UpdateCertificateDto dto)
        {
            var ceri = await context.Certificates.Include(a => a.Images).FirstOrDefaultAsync(a => a.Id == id);
            if (ceri == null) return NotFound();

            ceri.Name = dto.Name;
            ceri.CertificatesLink = dto.CertificatesLink;


            if (dto.DeleteImageIds != null)
            {
                var imagesDeleting = ceri.Images.Where(a => dto.DeleteImageIds.Contains(a.Id)).ToList();
                foreach (var img in imagesDeleting)
                {
                    await cloudinary.DeleteImageAsync(img.PublicId);
                    ceri.Images.Remove(img);
                }
            }
            if (dto.NewImages != null)
            {
                foreach (var file in dto.NewImages)
                {
                    var (Url, PublicId) = await cloudinary.UploadImageAsync(file);

                    ceri.Images.Add(new Images
                    {
                        Url = Url,
                        PublicId = PublicId
                    });
                }
            }
            await context.SaveChangesAsync();
            return Ok(ceri);
        }

        [HttpGet]
        public async Task<IActionResult> ShowCertificate()
        {
            var ceri = await context.Certificates.Include(a => a.Images).ToListAsync();
            return Ok(ceri);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ShowCertificateById(int id)
        {
            var ceri = await context.Certificates.Include(a => a.Images).FirstOrDefaultAsync(a => a.Id == id);
            if (ceri == null) return NotFound();
            return Ok(ceri);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCertificate(int id)
        {
            var ceri = await context.Certificates.Include(a => a.Images).FirstOrDefaultAsync(a => a.Id == id);
            if (ceri == null) return NotFound();


            foreach (var img in ceri.Images)
            {
                await cloudinary.DeleteImageAsync(img.PublicId);
            }

            context.Certificates.Remove(ceri);
            await context.SaveChangesAsync();

            return Ok("Deleted");
        }
    }
}
