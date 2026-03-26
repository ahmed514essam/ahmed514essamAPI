using System.ComponentModel.DataAnnotations;

namespace ahmed514essamAPI.Dtos
{
    public class CreateSkillDto
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
