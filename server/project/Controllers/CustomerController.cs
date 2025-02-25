using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using project.BLL;
using project.Models;
using project.Models.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace project.Controllers
{
    [Route("customer/api")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService customerService;
        private readonly IMapper mapper;
        private readonly IConfiguration config;
        public CustomerController(ICustomerService customerService, IMapper mapper, IConfiguration config)
        {
            this.customerService = customerService;
            this.mapper = mapper;
            this.config = config;
        }

        
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<Customer>> AddCustomer(CustomerDTO customerDto)
        {
            try
            {
                // בדיקה אם ה-DTO התקבל כראוי
                if (customerDto == null)
                {
                    return BadRequest("CustomerDTO is null");
                }

                // הדפסת תוכן ה-DTO ללוגים
                Console.WriteLine($"Received CustomerDTO: Name={customerDto.Name}, Email={customerDto.Email}, Phone={customerDto.Phone}, Password={customerDto.Password}");

                var c = mapper.Map<Customer>(customerDto);

                // בדיקת התוצאה לאחר המיפוי
                if (c == null)
                {
                    return BadRequest("Mapping failed");
                }

                Console.WriteLine($"Mapped Customer: Name={c.Name}, Email={c.Email}, Phone={c.Phone}, Password={c.Password}, Role={c.Role}");

                var result = await customerService.AddCustomer(c);

                // בדיקת התוצאה מהשירות
                if (result == null)
                {
                    return BadRequest("Customer service failed to add the customer");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                // הוספת הודעת השגיאה המלאה ללוגים
                Console.WriteLine($"Exception: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
        //private string GenerateToken(string Usernaeme)
        //{
        //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("Jwt:Key").Value));
        //    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        //    var claims = new[]
        //    {
        //        new Claim(ClaimTypes.NameIdentifier, Usernaeme)
        //    };

        //    var token = new JwtSecurityToken(
        //        config.GetSection("Jwt:Issuer").Value,
        //        config.GetSection("Jwt:Audience").Value,
        //        claims,
        //        expires: DateTime.Now.AddMinutes(15),
        //        signingCredentials: credentials);
        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            try
            {
                return Ok(await customerService.GetCustomers());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(string name, string password)
        {
            try
            {
                var user = await customerService.Login(name, password);
                if (user != null)
                {
                    var token = Generate(user);
                    return Ok(token);
                }
                else
                {
                    return Unauthorized("Invalid username or password.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [AllowAnonymous]
        public async Task<ActionResult<Customer>> UpdateCustomer(CustomerDTO customerDto, int Id)
        {
            try
            {
                var c = mapper.Map<Customer>(customerDto);
                return Ok(await customerService.UpdateCustomer(c, Id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [AllowAnonymous]
        public async Task DeleteCustomer(int id)
        {
            await customerService.DeleteCustomer(id);
        }

        [AllowAnonymous]
        private string Generate(Customer user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
        new Claim("UserID", user.Id.ToString()),
        new Claim("role", user.Role),
        new Claim(ClaimTypes.NameIdentifier, user.Name),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.OtherPhone, user.Phone),
        new Claim(ClaimTypes.Role, user.Role)
    };
            var token = new JwtSecurityToken(config["Jwt:Issuer"],
                                             config["Jwt:Audience"],
                                             claims,
                                             expires: DateTime.Now.AddMinutes(15),
                                             signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}