using System.ComponentModel.DataAnnotations;

namespace ahmed514essamAPI.Models
{
    public class Home
    {
        [Key]
        public int Id { get; set; }
        public string subTitle { get; set; }
        public string Summary { get; set; }
        public string FacebookLink { get; set; }
        public string LinkedinLink { get; set; }
        public string GithubLink { get; set; }
        public string WhatsLink { get; set; }
        public string InstagramLink { get; set; }
        public string DownloadResume { get; set; }
        public int DeliveryOrder { get; set; }
        public ICollection<Images> Images { get; set; } = new List<Images>();

    }
}
