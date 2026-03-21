namespace ahmed514essamAPI.Dtos
{
    public class CreateCertificateDto
    {
        public string Name { get; set; }
        public string CertificatesLink { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
