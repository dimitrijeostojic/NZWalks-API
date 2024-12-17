using NZWalks.Data;
using NZWalks.Models.Domain;

namespace NZWalks.Repositories
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment; // Omogućava pristup root putanji aplikacije
        private readonly IHttpContextAccessor httpContextAccessor; // Omogućava pristup trenutnom HTTP kontekstu
        private readonly NZWalksDbContext dbContext; // Pristup bazi podataka

        public LocalImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, NZWalksDbContext dbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }
        public async Task<Image> Upload(Image image)
        {
            // Generisanje putanje na kojoj će slika biti sačuvana lokalno
            var localFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", $"{image.FileName}{image.FileExstension}");

            // Kreira file stream i kopira sadržaj slike u lokalnu putanju
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            // Generiše URL putanju slike (npr. https://localhost:1234/Images/image.jpg)
            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}:{httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExstension}";

            // Postavlja URL putanju u objektu `Image`
            image.FilePath = urlFilePath;

             //Dodaje sliku u bazu podataka
             await dbContext.Images.AddAsync(image);
             await dbContext.SaveChangesAsync();

            return image;
        }
    }
}
