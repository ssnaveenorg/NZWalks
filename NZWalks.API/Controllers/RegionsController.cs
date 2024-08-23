using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRegionsAsync()
        {
            var regions = await regionRepository.GetAllAsync();

            //return DTO regions
            //var regionsDTO = new List<Models.DTO.Region>();
            //regions.ToList().ForEach(region =>
            //{
            //    var regionDTO = new Models.DTO.Region()
            //    {
            //        Id = region.Id,
            //        Code = region.Code,
            //        Name = region.Name,
            //        Area = region.Area,
            //        Lat = region.Lat,
            //        Long = region.Long,
            //        Population = region.Population,
            //    };
            //    regionsDTO.Add(regionDTO);
            //});

            var regionsDTO = mapper.Map<List<Models.DTO.Region>>(regions);

            return Ok(regionsDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var region = await regionRepository.GetAsync(id);

            if (region == null)
            {
                return NotFound();
            }

            var data = mapper.Map<Models.DTO.Region>(region);

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(AddRegionRequest addRegionRequest)
        {
            //Request to Domain
            var region = new Models.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Name = addRegionRequest.Name,
                Population = addRegionRequest.Population,
            };

            //Pass details to Repository
            region = await regionRepository.AddAsync(region);


            //Convert back to DTO
            var regionDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population,
            };

            return CreatedAtAction(nameof(GetRegionAsync), new { id = regionDTO.Id }, regionDTO);
        }

        [HttpDelete]
        [Route("{Id:guid}")]
        public async Task<IActionResult> DeleteRegionByIdAsync(Guid Id)
        {
            //Get region for DB
            var region = await regionRepository.DeleteAsync(Id);

            //If not found sending null
            if (region == null)
            {
                return NotFound();
            }

            //Convert response to DTO
            var data = mapper.Map<Models.DTO.Region>(region);

            //return OK response
            return Ok(data);
        }

        [HttpPut]
        [Route("{Id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid Id, [FromBody] UpdateRegionRequest updateRegionRequest)
        {
            //Convert DTO to Domain model
            var region = new Models.Domain.Region()
            {
                Code = updateRegionRequest.Code,
                Area = updateRegionRequest.Area,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Name = updateRegionRequest.Name,
                Population = updateRegionRequest.Population,
            };

            //Update Region using repository
            region = await regionRepository.UpdateAsync(Id, region);

            //If Null then NotFound
            if (region == null)
            {
                return NotFound();
            }
            //Convert Domain back to DTO
            var regionDTO = mapper.Map<Models.DTO.Region>(region);

            //Return OK Response
            return Ok(regionDTO);
        }

    }
}
