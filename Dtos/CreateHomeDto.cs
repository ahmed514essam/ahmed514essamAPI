namespace ahmed514essamAPI.Dtos
{
    public class CreateHomeDto
    {
        public string subTitle { get; set; }
        public string Summary { get; set; }
        public string FacebookLink { get; set; }
        public string LinkedinLink { get; set; }
        public string GithubLink { get; set; }
        public string WhatsLink { get; set; }
        public string InstagramLink { get; set; }
        public string DownloadResume { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
