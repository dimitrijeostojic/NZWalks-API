using System.ComponentModel.DataAnnotations.Schema;

namespace NZWalks.Models.Domain
{
    public class Image
    {
        public Guid Id { get; set; }
        [NotMapped]
        public IFormFile File { get; set; } //Fajl koji se uploaduje
        public string FileName { get; set; } //Naziv fajla
        public string? FileDescription { get; set; } //Opis fajla
        public string FileExstension { get; set; } //Ekstenzija fajla
        public long FileSizeInBytes { get; set; } //Velicina fajla u bajtovima
        public string FilePath { get; set; } //Putanja slike (URL)

    }
}
