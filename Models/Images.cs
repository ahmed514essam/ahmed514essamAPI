using System.ComponentModel.DataAnnotations;

namespace ahmed514essamAPI.Models
{
    public class Images
    {
        [Key]
        public int Id { get; set; }

        public string Url { get; set; }
        public string PublicId { get; set; }

        public int? EntityId { get; set; }       
        public string? EntityType { get; set; }          

    }
}
