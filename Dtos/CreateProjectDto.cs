using System.ComponentModel.DataAnnotations;

namespace ahmed514essamAPI.Dtos
{
    public class CreateProjectDto
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public string DemoLink { get; set; }
        public string RepoLink { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
