using AutoMapper;
using CarRental.Api.Models.Rental;
using CarRental.Services.Rentals;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRental.Api.Controllers
{
    /// <summary>
    /// Endpoints for Rental resource
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalService _service;
        private readonly IMapper _mapper;

        public RentalsController(IRentalService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all the rentals
        /// </summary>
        /// <returns>List of rentals</returns>
        /// <response code="200">Returns the list of rentals</response>    
        /// <response code="500">If an error occurs</response>    
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetRentals()
        {
            var rentals = await _service.GetAll();
            var responseModel = _mapper.Map<List<RentalResponseModel>>(rentals);
            return Ok(responseModel);
        }


        /// <summary>
        /// Get a rental by id
        /// </summary>
        /// <param name="id">rental id</param>
        /// <returns>Returns the rental</returns>
        /// <response code="200">Returns the rental</response>    
        /// <response code="404">If the rental doesn't exists</response>    
        /// <response code="500">If an error ocurres</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var rentalDto = await _service.GetById(id);
            if (rentalDto == null)
            {
                return NotFound();
            }
            var ResponseModel = _mapper.Map<RentalResponseModel>(rentalDto);

            return Ok(ResponseModel);
        }

        /// <summary>
        /// Add a new Rental
        /// </summary>
        /// <remarks>
        /// Rentals can be created only with available vehicles for the selected date range.
        /// </remarks>
        /// <param name="rentalRequest">Request for add rental</param>
        /// <returns>Returns the created rental</returns>
        /// <response code="201">Rental created</response>
        /// <response code="400">Bad Request</response>
        /// <response code="500">If an error occurs</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Post([FromBody] RentalRequestModel rentalRequest)
        {
            var rentalDto = _mapper.Map<RentalDto>(rentalRequest);
            var addedRentalDto = await _service.AddNewRental(rentalDto);
            var responseModel = _mapper.Map<RentalResponseModel>(addedRentalDto);
            return CreatedAtAction(nameof(GetById), new { id = responseModel.Id }, responseModel);
        }

        /// <summary>
        /// Delete Rental
        /// </summary>
        /// <param name="id">Rental id</param>
        /// <returns>No content</returns>
        /// <response code="204">Rental Deleted</response>    
        /// <response code="500">If an error occurs</response>    
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.Delete(id);
            return NoContent();
        }

        /// <summary>
        /// Cancel an existing Rental
        /// </summary>
        /// <remarks>
        /// When Cancel a Rental the vehicle asociated will be available for new rentals.
        /// </remarks>
        /// <param name="id">Rental id to be Canceled</param>
        /// <returns>Returns the canceled rental</returns>
        /// <response code="200">Rental canceled</response>
        /// <response code="500">If an error occurs</response>
        [HttpPatch("{id}/Cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            var canceledRental = await _service.CancelRental(id);
            var rentalResposeModel = _mapper.Map<RentalResponseModel>(canceledRental);
            return Ok(rentalResposeModel);
        }
    }
}
