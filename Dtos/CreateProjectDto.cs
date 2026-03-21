namespace ahmed514essamAPI.Dtos
{
    public class CreateProjectDto
    {
        public string Name { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public string DemoLink { get; set; }
        public string RepoLink { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
