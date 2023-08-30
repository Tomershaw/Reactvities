using System.Security.Claims;
using API.DTOs;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        public readonly TokenService _tokenService;
        public IConfiguration _config ;
        private readonly HttpClient _httpClient;
        public AccountController(UserManager<AppUser> userManager, TokenService tokenService, IConfiguration config)
        {
            _config = config;
            _tokenService = tokenService;
            _userManager = userManager;
            _httpClient =  new HttpClient
            {
                BaseAddress = new System.Uri("https://graph.facebook.com")
            };

        }

        [AllowAnonymous]
        [HttpPost("fbLogin")]   
        public async Task<ActionResult<UserDto>> FacebookLogin(string accessToken){
            var fbverifyKeys =_config["Facebook:AppId"] + "|" + _config["Facebook:ApiSecret"];
            var verifyTokenResponse = await _httpClient.GetAsync($"debug_token?input_token={accessToken}&access_token={fbverifyKeys}");

            if(!verifyTokenResponse.IsSuccessStatusCode) return Unauthorized();

            var fbUrl =$"me?access_token={accessToken}&fields=name,email,picture.width(100).height(100)";

            //var fbInfo =await _httpClient.GetFromJsonAsync<FacebookDto>(fbUrl);
            var response = await _httpClient.GetAsync(fbUrl);
            var json = await response.Content.ReadAsStringAsync();

            FacebookDto fbInfo = JsonConvert.DeserializeObject<FacebookDto>(json);


            var user = await _userManager.Users.Include(p => p.Photos)
            .FirstOrDefaultAsync(x => x.Email ==  fbInfo.Email);

            if (user != null ) return CreateUserObject(user);

            user = new AppUser
            {
                DisplayName =fbInfo.Name,
                Email = fbInfo.Email,
                UserName= fbInfo.Email, 
                Photos = new List<Photo>
                {
                    new Photo
                    {
                        Id ="fb_" + fbInfo.Id,
                        Url = fbInfo.Picture.Data.Url,
                        IsMain = true
                    }
                }
            };
            var result = await _userManager.CreateAsync(user);
            
            if(!result.Succeeded) return BadRequest("problem createing user account");

            return CreateUserObject(user);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.Users.Include(p => p.Photos)
            .FirstOrDefaultAsync(x => x.Email == loginDto.Email);

            if (user == null) return Unauthorized();

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (result)
            {
                return CreateUserObject(user);
            }

            return Unauthorized();

        }

        [AllowAnonymous]
        [HttpPost("register")]

        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {


            if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
            {
                ModelState.AddModelError("email", "Email taken");
                return ValidationProblem();

            }

            if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.Username))
            {
                ModelState.AddModelError("username", "Username taken");
                return ValidationProblem();

            }


            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Username
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                return CreateUserObject(user);
            }
            return BadRequest(result.Errors);

        }


        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCuttentUser()
        {
            var user = await _userManager.Users.Include(p => p.Photos)
           .FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Email));
            return CreateUserObject(user);

        }

        private UserDto CreateUserObject(AppUser user)
        {
            return new UserDto
            {
                DisplayName = user.DisplayName,
                Image = user?.Photos?.FirstOrDefault(x => x.IsMain)?.Url,
                Token = _tokenService.CreateToken(user),
                Username = user.UserName,
                CanCreateActivity = user.CanCreateActivity
            };
        }


    }
}