using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Models.Domain;
using NZWalks.Models.DTO;
using NZWalks.Repositories;

namespace NZWalks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        //POST: /api/Images/Upload
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto imageUploadRequestDto)
        {
            //Poziva metod za validaciju fajla
            ValidateFileUpload(imageUploadRequestDto);

            //Proverava da li je model validan (ako validacija nije dodala greške)
            if (ModelState.IsValid)
            {
                //Konvertuje DTO objekat u domen model
                var imageDomain = new Image
                {
                    File = imageUploadRequestDto.File, //Uploadovani fajl
                    FileDescription = imageUploadRequestDto.FileDescription, //Opis fajla
                    FileExstension = Path.GetExtension(imageUploadRequestDto.File.FileName), //Ekstenzija fajla (.jpg, .png, ...)
                    FileName = imageUploadRequestDto.FileName, //Naziv fajla
                    FileSizeInBytes = imageUploadRequestDto.File.Length //Veličina fajla u bajtovima
                };

                //Poziva repozitorijum da sačuva sliku
                imageDomain = await imageRepository.Upload(imageDomain);

                //Vraća HTTP 200 OK sa podacima o slici
                return Ok(imageDomain);
            }

            //Ako validacija nije prošla, vraća BadRequest sa greškama
            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(ImageUploadRequestDto imageUploadRequestDto)
        {
            //Dozvoljene ekstenzije fajlova
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            //Proverava da li ekstenzija fajla nije među dozvoljenima
            if (!allowedExtensions.Contains(Path.GetExtension(imageUploadRequestDto.File.FileName)))
            {
                ModelState.AddModelError("file", "Unsupported file extension");
            }
            //Proverava da li je fajl veći od 10 MB
            if (imageUploadRequestDto.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size more than 10MB, please upload a smaller size file");
            }
        }
    }
}
