using AutoMapper;
using CarRental.Api.Models.Vehicle;
using CarRental.Services.Vehicles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRental.Api.Controllers
{
    /// <summary>
    /// Endpoints for vehicle resource
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService _service;
        private readonly IMapper _mapper;

        public VehiclesController(IVehicleService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all the vehicles
        /// </summary>
        /// <returns>List of vehicles</returns>
        /// <response code="200">Returns the list of vehicles</response>    
        /// <response code="500">If an error occurs</response>    
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetVehicles()
        {
            var vehicles = await _service.GetAll();
            var reponseModel = _mapper.Map<List<VehicleResponseModel>>(vehicles);
            return Ok(reponseModel);
        }

        /// <summary>
        /// Get a vehicle by id
        /// </summary>
        /// <param name="id">vehicle id</param>
        /// <returns>Returns the vehicle</returns>
        /// <response code="200">Returns the vehicle</response>    
        /// <response code="404">If the vehicle doesn't exists</response>    
        /// <response code="500">If an error ocurres</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var vehicleDto = await _service.GetById(id);
            if (vehicleDto == null)
            {
                return NotFound();
            }

            var responseModel = _mapper.Map<VehicleResponseModel>(vehicleDto);
            return Ok(responseModel);
        }

        /// <summary>
        /// Add a new vehicle
        /// </summary>
        /// <param name="vehicleRequest">Request for add vehicle</param>
        /// <returns>Returns the created vehicle</returns>
        /// <response code="201">Vehicle created</response>
        /// <response code="400">Bad Request</response>
        /// <response code="500">If an error occurs</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] VehicleRequestModel vehicleRequest)
        {
            var vehicleDto = _mapper.Map<VehicleDto>(vehicleRequest);
            var addedVehicleDto = await _service.AddNewVehicle(vehicleDto);
            var responseModel = _mapper.Map<VehicleResponseModel>(addedVehicleDto);
            return CreatedAtAction(nameof(GetById), new { id = responseModel.Id }, responseModel);
        }

        /// <summary>
        /// Delete vehicle
        /// </summary>
        /// <param name="id">vehicle id</param>
        /// <returns>No content</returns>
        /// <response code="204">vehicle Deleted</response>    
        /// <response code="500">If an error occurs</response>    
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
