using DevInSales.Context;
using DevInSales.DTOs;
using DevInSales.Services;
using Microsoft.AspNetCore.Mvc;

namespace DevInSales.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SqlContext _context;

        public AuthController(SqlContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Faz login com email e senha.
        /// </summary>
        /// <returns>token de autenticação</returns>
        /// <response code="200">Registro encontrado.</response>
        /// <response code="404">Registro não encontrado.</response>
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Login([FromBody] LoginDTO login)
        {
            try
            {
                var user = _context.User.First(x => x.Email.ToLower() == login.Email.ToLower() && x.Password.ToLower() == login.Password.ToLower());

                if (user == null)
                {
                    return NotFound("User or passaword not found");
                }

                var profile = _context.Profile.First(x => x.Id == user.ProfileId);

                var token = TokenService.GenerateToken(user.Name, profile.Role);

                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }

        }
    }
}