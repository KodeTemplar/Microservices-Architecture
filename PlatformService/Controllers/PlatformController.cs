using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformController : ControllerBase
    {
        private IMapper _mapper;
        private IPlatformRepository _repository;

        public PlatformController(IMapper mapper, IPlatformRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatForms()
        {
            Console.WriteLine("---> Getting platforms...");

            var platforms = _repository.GetAllPlatforms();

            var platformDto = _mapper.Map<IEnumerable<PlatformReadDto>>(platforms);

            return Ok(platformDto);
        }

        [HttpGet("{Id}", Name = "GetPlatFormById")]
        public ActionResult<PlatformReadDto> GetPlatFormById(int Id)
        {
            Console.WriteLine("---> Getting platforms...");

            var platform = _repository.GetPlatformById(Id);
            if (platform != null)
            {
                var platformDto = _mapper.Map<PlatformReadDto>(platform);

                return Ok(platformDto);
            }
            return NotFound();
        }

        [HttpPost]
        public ActionResult<PlatformReadDto> GetPlatFormById([FromBody] PlatformCreateDto platformCreateDto)
        {
            var platformModel = _mapper.Map<Platform>(platformCreateDto);
            _repository.CreatePlatform(platformModel);
            _repository.SaveChanges();

            var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);
            return CreatedAtRoute(nameof(GetPlatFormById), new { Id = platformReadDto.Id }, platformReadDto);
        }
    }
}
