using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("(Controller")]
    public class WalkDifficultyController : Controller
    {
        private readonly IWalkDifficultyRepository _walkDifficultyRepository;
        private readonly IMapper _mapper;

        public WalkDifficultyController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            this._walkDifficultyRepository = walkDifficultyRepository;
            this._mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficulty()
        {
            var walk = await _walkDifficultyRepository.GetAllAsync();
            var walkDiffDTO = _mapper.Map<List<Models.DTO.WalkDifficulty>>(walk);
            return Ok(walkDiffDTO);
        }

        [HttpGet]
        [Route("{Id:guid}")]
        [ActionName("GetWalkDifficultyById")]
        public async Task<IActionResult> GetWalkDifficultyById(Guid Id)
        {
            var walkDiff = await _walkDifficultyRepository.GetAsync(Id);
            if (walkDiff == null)
            {
                return NotFound();
            }
            var walkDiffDTO = _mapper.Map<Models.DTO.WalkDifficulty>(walkDiff);
            return Ok(walkDiffDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkDifficultyAsync(Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            if (!ValidateAddWalkDifficultyAsync(addWalkDifficultyRequest))
            {
                return BadRequest(ModelState);
            }
            var walkDiffDomain = _mapper.Map<Models.Domain.WalkDifficulty>(addWalkDifficultyRequest);
            walkDiffDomain = await _walkDifficultyRepository.AddAsync(walkDiffDomain);
            var walkDiffDTO = _mapper.Map<Models.DTO.WalkDifficulty>(walkDiffDomain);
            return CreatedAtAction(nameof(GetWalkDifficultyById), new { Id = walkDiffDTO.Id }, walkDiffDTO);

        }

        [HttpPut]
        [Route("{Id:guid}")]
        public async Task<IActionResult> UpdateWalkDifficultyAsync(Guid Id, Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            if (!ValidateUpdateWalkDifficultyAsync(updateWalkDifficultyRequest))
            {
                return BadRequest(ModelState);
            }
            var walkDiffDomain = _mapper.Map<Models.Domain.WalkDifficulty>(updateWalkDifficultyRequest);
            walkDiffDomain = await _walkDifficultyRepository.UpdateAsync(Id, walkDiffDomain);
            if (walkDiffDomain == null) { return NotFound(); }
            var walkDiffDTO = _mapper.Map<Models.Domain.WalkDifficulty>(walkDiffDomain);
            return Ok(walkDiffDTO);
        }

        [HttpDelete]
        [Route("{Id:guid}")]
        public async Task<IActionResult> DeleteWalkDifficulty([FromRoute] Guid Id)
        {
            var data = await _walkDifficultyRepository.DeleteAsync(Id);
            if (data == null) { return NotFound(); }
            var DTO = _mapper.Map<Models.DTO.WalkDifficulty>(data);
            return Ok(DTO);
        }

        #region Private Methods

        private bool ValidateAddWalkDifficultyAsync(AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            if (addWalkDifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkDifficultyRequest), "Add Walk shoud have value");
                return false;
            }
            if (string.IsNullOrWhiteSpace(addWalkDifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(addWalkDifficultyRequest.Code), $"{nameof(addWalkDifficultyRequest.Code)} cannot be null or empty");
            }
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }
        private bool ValidateUpdateWalkDifficultyAsync(UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            if (updateWalkDifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest), "Add Walk shoud have value");
                return false;
            }
            if (string.IsNullOrWhiteSpace(updateWalkDifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest.Code), $"{nameof(updateWalkDifficultyRequest.Code)} cannot be null or empty");
            }
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}

