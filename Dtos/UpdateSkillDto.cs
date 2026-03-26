using System.ComponentModel.DataAnnotations;

namespace ahmed514essamAPI.Dtos
{
    public class UpdateSkillDto
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public List<IFormFile>? NewImages { get; set; }
        public List<int>? DeleteImageIds { get; set; }
    }
}
