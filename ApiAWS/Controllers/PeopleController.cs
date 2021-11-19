using Microsoft.Extensions.Logging;
using ApiLibrary.Models;
using ApiLibrary.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ApiAWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PeopleController> _logger;

        public PeopleController(IUnitOfWork unitOfWork, ILogger<PeopleController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // GET: api/people
        [HttpGet]
        public People GetPeople()
        {
            return _unitOfWork.ApiRepository.GetPeople();
        }

        // GET: api/people/search?name=
        [HttpGet("Search")]
        public People GetSearchPeople(string name = "")
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            return _unitOfWork.ApiRepository.GetSearchPeople(name);
        }

        // GET: api/people/5
        [HttpGet("{id}")]
        public Person GetPerson(int id)
        {
            return _unitOfWork.ApiRepository.GetPerson(id);
        }

        // POST: api/people
        [HttpPost]
        public async Task<IActionResult> CreatePerson(Person person)
        {
            if (person != null)
            {
                await _unitOfWork.ApiRepository.CreatePerson(person);
                return Ok(person);
            }

            _logger.LogWarning(string.Format(
                "API Controller : Invalid data provided : {0}", person));
            return BadRequest();
        }

        // DELETE: api/people/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {

            if (id >= 0)
            {
                await _unitOfWork.ApiRepository.DeletePerson(id);
                return Ok();
            }

            _logger.LogInformation(string.Format(
                "API Controller : JSON object not provided."));
            return BadRequest();
        }

        // PUT: api/people/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePerson(Person person, int id)
        {
            if (person == null)
            {
                _logger.LogWarning(string.Format(
                    "API Controller : JSON object not provided."));
                return BadRequest();
            }

            await _unitOfWork.ApiRepository.UpdatePerson(person, id);
            return Ok(person);
        }
    }
}
