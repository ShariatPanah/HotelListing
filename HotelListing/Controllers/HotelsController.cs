using AutoMapper;
using HotelListing.Data.UnitOfWork;
using HotelListing.Dtos;
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
    public class HotelsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HotelsController> _logger;
        private readonly IMapper _mapper;

        public HotelsController(IUnitOfWork unitOfWork, ILogger<HotelsController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetHotels()
        {
            try
            {
                var hotels = await _unitOfWork.Hotels.GetAllAsync();
                var dtos = _mapper.Map<IList<HotelDto>>(hotels);
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error Occured in method: {nameof(GetHotels)}");
                return StatusCode(500, "Something went wrong, Please try again later!");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetHotel(int id)
        {
            try
            {
                var hotel = await _unitOfWork.Hotels.Include(h => h.Country).FirstOrDefaultAsync(h => h.Id == id);
                if (hotel == null)
                    return NotFound();

                var dto = _mapper.Map<HotelDto>(hotel);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error Occured in method: {nameof(GetHotel)}");
                return StatusCode(500, "Something went wrong, Please try again later!");
            }
        }
    }
}
