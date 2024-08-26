using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository _WalkRepository;
        private readonly IMapper _Mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this._WalkRepository = walkRepository;
            this._Mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            var walks = await _WalkRepository.GetAllAsync();
            var walksDTO = _Mapper.Map<List<Models.DTO.Walks>>(walks);
            return Ok(walksDTO);
        }

        [HttpGet]
        [Route("{Id:Guid}")]
        [ActionName("GetWalksAsync")]
        public async Task<IActionResult> GetWalksAsync(Guid Id)
        {
            var walk = await _WalkRepository.GetAsync(Id);
            if (walk == null)
            {
                return NotFound();
            }
            var walkDTO = _Mapper.Map<Models.DTO.Walks>(walk);
            return Ok(walkDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalksAsync([FromBody] Models.DTO.AddWalksRequest addWalksRequest)
        {
            //DTO to Domain
            var walk = new Models.Domain.Walk
            {
                Length = addWalksRequest.Length,
                Name = addWalksRequest.Name,
                RegionId = addWalksRequest.RegionId,
                WalkDifficultyId = addWalksRequest.WalkDifficultyId,
            };

            //Add to DB
            walk = await _WalkRepository.AddAsync(walk);

            //Conver and return Domain to DTO
            var walkDTO = _Mapper.Map<Models.DTO.Walks>(walk);

            return CreatedAtAction(nameof(GetWalksAsync), new { Id = walkDTO.Id }, walkDTO);
        }

        [HttpPut]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid Id, [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            //Convert DTO to Domain
            var walkDomain = _Mapper.Map<Models.Domain.Walk>(updateWalkRequest);

            //Pass Details to Repository
            walkDomain = await _WalkRepository.UpdateAsync(Id, walkDomain);

            if (walkDomain == null)
            {
                return NotFound();
            }

            //Convert and return Domain to DTO
            var walkDTO = _Mapper.Map<Models.DTO.Walks>(walkDomain);
            return Ok(walkDTO);
        }

        [HttpDelete]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> DeleteWalkById([FromRoute] Guid Id)
        {
            var walk = await _WalkRepository.DeleteAsync(Id);
            if (walk == null)
            {
                return NotFound();
            }
            var walkDTO = _Mapper.Map<Models.DTO.Walks>(walk);
            return Ok(walkDTO);
        }
    }
}
