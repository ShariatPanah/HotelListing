using AutoMapper;
using HotelListing.Data.UnitOfWork;
using HotelListing.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CountriesController> _logger;
        private readonly IMapper _mapper;

        public CountriesController(IUnitOfWork unitOfWork, ILogger<CountriesController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCountries()
        {
            try
            {
                var countries = await _unitOfWork.Countries.GetAllAsync();
                var dtos = _mapper.Map<IList<CountryDto>>(countries);
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error Occured in method: {nameof(GetCountries)}");
                return StatusCode(500, "Something went wrong, Please try again later!");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCountry(int id)
        {
            try
            {
                var country = await _unitOfWork.Countries.Include(c => c.Hotels).FirstOrDefaultAsync(c => c.Id == id);
                if (country == null)
                    return NotFound();

                var dto = _mapper.Map<CountryDto>(country);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error Occured in method: {nameof(GetCountry)}");
                return StatusCode(500, "Something went wrong, Please try again later!");
            }
        }
    }
}
