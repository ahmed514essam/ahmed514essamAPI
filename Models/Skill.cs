using System.ComponentModel.DataAnnotations;

namespace ahmed514essamAPI.Models
{
    public class Skill
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int DeliveryOrder { get; set; }
        public ICollection<Images> Images { get; set; } = new List<Images>();

    }
}
