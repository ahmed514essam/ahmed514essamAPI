using System.ComponentModel.DataAnnotations;

namespace ahmed514essamAPI.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public string DemoLink { get; set; }
        public string RepoLink { get; set; }
        public int DeliveryOrder { get; set; }
        public ICollection<Images> Images { get; set; } = new List<Images>();

    }
}
