namespace ahmed514essamAPI.Dtos
{
    public class UpdateProjectDto
    {
        public string Name { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public string DemoLink { get; set; }
        public string RepoLink { get; set; }
        public List<IFormFile>? NewImages { get; set; }
        public List<int>? DeleteImageIds { get; set; }
    }
}
