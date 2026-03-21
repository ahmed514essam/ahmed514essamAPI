namespace ahmed514essamAPI.Dtos
{
    public class UpdateCertificateDto
    {
        public string Name { get; set; }
        public string CertificatesLink { get; set; }
        public List<IFormFile>? NewImages { get; set; }
        public List<int>? DeleteImageIds { get; set; }
    }
}
