namespace ahmed514essamAPI.Dtos
{
    public class UpdateSkillDto
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public List<IFormFile>? NewImages { get; set; }
        public List<int>? DeleteImageIds { get; set; }
    }
}
