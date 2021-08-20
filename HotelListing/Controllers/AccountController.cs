using AutoMapper;
using HotelListing.Common;
using HotelListing.Core;
using HotelListing.Dtos;
using HotelListing.Services.Jwt;
using Microsoft.AspNetCore.Authorization;
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
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AccountController> _logger;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager, IJwtService jwtService,
                                 ILogger<AccountController> logger, IMapper mapper)
        {
            _userManager = userManager;
            _jwtService = jwtService;
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

                await _userManager.AddToRoleAsync(user, "Admin");

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occured in method: {nameof(Register)}");
                return StatusCode(HttpStatusCode.InternalServerError.ToInt(), "Something went wrong, please try again later!");
            }
        }

        [HttpPost]
        [Route("Token")]
        public async Task<IActionResult> Token(LoginUserDto userDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = await _userManager.FindByNameAsync(userDto.Email);
                if (user == null || !await _userManager.CheckPasswordAsync(user, userDto.Password))
                    return BadRequest("Username or Password is incorrect.");

                return Ok(new { Token = await _jwtService.Generate(user) });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occured in method: {nameof(Token)}");
                return StatusCode(HttpStatusCode.InternalServerError.ToInt(), "Something went wrong, please try again later!");
            }
        }
    }
}
