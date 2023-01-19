using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformController : ControllerBase
    {
        private IMapper _mapper;
        private IPlatformRepository _repository;
        private readonly ICommandDataClient _dataCommandClient;

        public PlatformController(
            IMapper mapper,
            IPlatformRepository repository,
            ICommandDataClient dataCommandClient
            )
        {
            _mapper = mapper;
            _repository = repository;
            _dataCommandClient = dataCommandClient;
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
        public async Task<ActionResult<PlatformReadDto>> GetPlatFormById([FromBody] PlatformCreateDto platformCreateDto)
        {
            var platformModel = _mapper.Map<Platform>(platformCreateDto);
            _repository.CreatePlatform(platformModel);
            _repository.SaveChanges();

            var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);

            try
            {
                await _dataCommandClient.SendPlatformToCommand(platformReadDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send Message to synchronousely {ex}");

            }
            return CreatedAtRoute(nameof(GetPlatFormById), new { Id = platformReadDto.Id }, platformReadDto);
        }
    }
}
