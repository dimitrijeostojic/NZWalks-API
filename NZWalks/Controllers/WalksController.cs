using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.CustomActionFilter;
using NZWalks.Models.Domain;
using NZWalks.Models.DTO;
using NZWalks.Repositories;
using System.Net;

namespace NZWalks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }

        //https://localhost:portnumber/api/walks?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? sortBy, [FromQuery] bool? isAscending, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var walksDomain = await walkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);
            //Create a new exception
            throw new Exception("This is a new exception");

            //map to dto
            var walksDto = mapper.Map<List<WalkDto>>(walksDomain);
            return Ok(walksDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomain = await walkRepository.GetByIdAsync(id);
            if (walkDomain == null)
            {
                return NotFound();
            }

            //map to dto
            var walkDto = mapper.Map<WalkDto>(walkDomain);
            return Ok(walkDto);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            var walkDomain = mapper.Map<Walk>(addWalkRequestDto);
            walkDomain = await walkRepository.CreateAsync(walkDomain);
            //map to dto
            var walkDto = mapper.Map<WalkDto>(walkDomain);

            return CreatedAtAction(nameof(GetById), new { id = walkDto.Id }, walkDto);

        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRequestDto updateWalkRequestDto)
        {
            //map to domain
            var walkDomain = mapper.Map<Walk>(updateWalkRequestDto);
            walkDomain = await walkRepository.UpdateAsync(id, walkDomain);
            if (walkDomain == null)
            {
                return NotFound();
            }
            //map to dto
            var walkDto = mapper.Map<WalkDto>(walkDomain);
            return Ok(walkDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var walkDomain = await walkRepository.DeleteAsync(id);
            if (walkDomain == null)
            {
                return NotFound();
            }
            //map to dto
            var walkDto = mapper.Map<WalkDto>(walkDomain);
            return Ok(walkDto);
        }
    }
}
