namespace ahmed514essamAPI.Dtos
{
    public class CreateSkillDto
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
