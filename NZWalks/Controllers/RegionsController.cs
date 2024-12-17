using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.CustomActionFilter;
using NZWalks.Data;
using NZWalks.Models.Domain;
using NZWalks.Models.DTO;
using NZWalks.Repositories;
using System.Text.Json;

namespace NZWalks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper, ILogger<RegionsController> logger)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        //https://localhost:portnumber/api/regions
        [HttpGet]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll()
        {

            #region Logging in console (exception, information, warning, error)
            //try
            //{
            //    throw new Exception("This is a custom exception");
            //    logger.LogInformation("GetAllRegion Action Method was invoked");
            //    logger.LogWarning("This is a worning log");
            //    logger.LogError("This is a error log");

            //    var regionsDomain = await regionRepository.GetAllAsync();

            //    //Map to Dto
            //    var regionsDto = mapper.Map<List<RegionDto>>(regionsDomain);
            //    logger.LogInformation($"Finished GetAllRegion request with data {JsonSerializer.Serialize(regionsDto)}");
            //    return Ok(regionsDto);
            //}
            //catch (Exception ex)
            //{
            //    logger.LogError(ex, ex.Message);
            //    throw;
            //} 
            #endregion

            //logger.LogInformation("GetAllRegion Action Method was invoked");
            logger.LogWarning("This is a worning log");
            logger.LogError("This is a error log");

            var regionsDomain = await regionRepository.GetAllAsync();

            //Map to Dto
            var regionsDto = mapper.Map<List<RegionDto>>(regionsDomain);
            //logger.LogInformation($"Finished GetAllRegion request with data {JsonSerializer.Serialize(regionsDto)}");
            return Ok(regionsDto);
        }

        //https://localhost:portnumber/api/regions
        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var regionDomain = await regionRepository.GetByIdAsync(id);
            if (regionDomain == null)
            {
                return NotFound();
            }

            //Map to dto
            var regionDto = mapper.Map<RegionDto>(regionDomain);
            return Ok(regionDto);
        }

        //https://localhost:portnumber/api/regions
        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
                //map to domain
                var regionDomain = mapper.Map<Region>(addRegionRequestDto);
                regionDomain = await regionRepository.CreateAsync(regionDomain);

                //map to dto
                var regionDto = mapper.Map<RegionDto>(regionDomain);

                return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto); 

        }

        //https://localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
                //map dto to domain
                var regionDomain = mapper.Map<Region>(updateRegionRequestDto);

                regionDomain = await regionRepository.UpdateAsync(id, regionDomain);
                if (regionDomain == null)
                {
                    return NotFound();
                }

                //map to dto
                var regionDto = mapper.Map<RegionDto>(regionDomain);

                return Ok(regionDto); 
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer,Reader")]
        public async Task<IActionResult> Delete([FromRoute]Guid id)
        {

            var regionDomain = await regionRepository.DeleteAsync(id);
            if (regionDomain == null)
            {
                return NotFound();
            }

            //map to dto
            var regionDto = mapper.Map<RegionDto>(regionDomain);
            
            return Ok(regionDto);
        }
    }
}
