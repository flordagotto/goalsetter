using AutoMapper;
using CarRental.Api.Models.Client;
using CarRental.Services.Clients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRental.Api.Controllers
{
    /// <summary>
    /// Endpoints for client resource
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _service;
        private readonly IMapper _mapper;

        public ClientsController(IClientService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all the clients
        /// </summary>
        /// <returns>List of clients</returns>
        /// <response code="200">Returns the list of clients</response>    
        /// <response code="500">If a server realted error occurs</response>  
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetClients()
        {
            var clients = await _service.GetAll();
            var responseModel = _mapper.Map<List<ClientResponseModel>>(clients);
            return Ok(responseModel);
        }

        /// <summary>
        /// Get a client by id
        /// </summary>
        /// <param name="id">Client id</param>
        /// <returns>Returns the client</returns>
        /// <response code="200">Returns the client</response>    
        /// <response code="404">If the client doesn't exists</response>    
        /// <response code="500">If a server realted error occurs</response>  
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var clientDto = await _service.GetById(id);
            if (clientDto == null)
            {
                return NotFound();
            }
            var clientResponseModel = _mapper.Map<ClientResponseModel>(clientDto);

            return Ok(clientResponseModel);
        }

        /// <summary>
        /// Add a new client
        /// </summary>
        /// <param name="clientRequest">Request for add client</param>
        /// <returns>Returns the created client</returns>
        /// <response code="201">Client created</response>
        /// <response code="400">Bad Request</response>
        /// <response code="500">If a server realted error occurs</response>  
        // POST: api/People
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] ClientRequestModel clientRequest)
        {
            var clientDto = _mapper.Map<ClientDto>(clientRequest);
            var addedClientDto = await _service.AddNewClient(clientDto);
            var responseModel = _mapper.Map<ClientResponseModel>(addedClientDto);
            return CreatedAtAction(nameof(GetById), new { id = responseModel.Id }, responseModel);
        }

        /// <summary>
        /// Delete client
        /// </summary>
        /// <param name="id">client id</param>
        /// <returns>No content</returns>
        /// <response code="204">Client Deleted</response>    
        /// <response code="500">If a server realted error occurs</response>  
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.Delete(id);
            return NoContent();
        }
    }
}
