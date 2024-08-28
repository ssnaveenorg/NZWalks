using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository _WalkRepository;
        private readonly IMapper _Mapper;
        private readonly IRegionRepository _regionRepository;
        private readonly IWalkDifficultyRepository _walkDifficultyRepository;

        public WalksController(IWalkRepository walkRepository, IMapper mapper, IRegionRepository regionRepository, IWalkDifficultyRepository walkDifficultyRepository)
        {
            this._WalkRepository = walkRepository;
            this._Mapper = mapper;
            this._regionRepository = regionRepository;
            this._walkDifficultyRepository = walkDifficultyRepository;
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
            if (!(await ValidateAddWalkAsync(addWalksRequest)))
            {
                return BadRequest(ModelState);
            }

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
            if (!ValidateUpdateWalk(updateWalkRequest))
            {
                return BadRequest(ModelState);
            }
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

        #region Private Methods

        private async Task<bool> ValidateAddWalkAsync(AddWalksRequest addWalksRequest)
        {
            if (addWalksRequest == null)
            {
                ModelState.AddModelError(nameof(addWalksRequest), $"Add walk data is required");
                return false;
            }

            if (string.IsNullOrWhiteSpace(addWalksRequest.Name))
            {
                ModelState.AddModelError(nameof(addWalksRequest.Name), $"{nameof(addWalksRequest.Name)} cannot be null or empty");
            }
            if (addWalksRequest.Length <= 0)
            {
                ModelState.AddModelError(nameof(addWalksRequest.Length), $"{nameof(addWalksRequest.Length)} cannot be less than or equal to 0");
            }

            var region = await _regionRepository.GetAsync(addWalksRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(addWalksRequest.RegionId), $"{nameof(addWalksRequest.RegionId)} is invalid");
            }

            var walkDifficulty = await _walkDifficultyRepository.GetAsync(addWalksRequest.WalkDifficultyId);
            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(addWalksRequest.WalkDifficultyId), $"{nameof(addWalksRequest.WalkDifficultyId)} is invalid");
            }


            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }

        private bool ValidateUpdateWalk(UpdateWalkRequest updateWalkRequest)
        {
            if (updateWalkRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest), $"Add walk data is required");
                return false;
            }

            if (string.IsNullOrWhiteSpace(updateWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Name), $"{nameof(updateWalkRequest.Name)} cannot be null or empty");
            }
            if (string.IsNullOrWhiteSpace(updateWalkRequest.Code))
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Code), $"{nameof(updateWalkRequest.Code)} cannot be null or empty");
            }
            if (updateWalkRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Area), $"{nameof(updateWalkRequest.Area)} cannot be less than or equal to 0");
            }
            //if (updateWalkRequest.Lat <= 0)
            //{
            //    ModelState.AddModelError(nameof(updateWalkRequest.Lat), $"{nameof(updateWalkRequest.Lat)} cannot be less than or equal to 0");
            //}
            //if (updateWalkRequest.Long <= 0)
            //{
            //    ModelState.AddModelError(nameof(updateWalkRequest.Long), $"{nameof(updateWalkRequest.Long)} cannot be less than or equal to 0");
            //}
            if (updateWalkRequest.Population <= 0)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Population), $"{nameof(updateWalkRequest.Population)} cannot be less than or equal to 0");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;

            #endregion

        }
    }
}
