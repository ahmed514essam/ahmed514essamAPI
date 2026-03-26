using System.ComponentModel.DataAnnotations;

namespace ahmed514essamAPI.Models
{
    public class About
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string BirthDate { get; set; }
        public string Phone { get; set; }
        public string From { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }




        public string FacebookLink { get; set; }
        public string LinkedinLink { get; set; }
        public string GithubLink { get; set; }
        public string WhatsLink { get; set; }
        public string InstagramLink { get; set; }



        public string WhoAmI { get; set; }
        public string Education { get; set; }
        public int DeliveryOrder { get; set; }
        public ICollection<Images> Images { get; set; } = new List<Images>();
    }
}
