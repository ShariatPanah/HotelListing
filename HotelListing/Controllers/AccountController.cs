using AutoMapper;
using HotelListing.Common;
using HotelListing.Core;
using HotelListing.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        //private readonly SignInManager<AppUser> _singInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager, /*SignInManager<AppUser> singinManager,*/
                                 ILogger<AccountController> logger, IMapper mapper)
        {
            _userManager = userManager;
            //_singInManager = singinManager;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] AppUserDto userDto)
        {
            _logger.LogInformation($"Registeration attempt for {userDto.Email}");
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = _mapper.Map<AppUser>(userDto);
                user.UserName = userDto.Email;
                var result = await _userManager.CreateAsync(user, userDto.Password);

                if (!result.Succeeded)
                    return BadRequest();

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occured in method: {nameof(Register)}");
                return StatusCode(HttpStatusCode.InternalServerError.ToInt(), "Something went wrong, please try again later!");
            }
        }
    }
}
