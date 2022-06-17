using DevInSales.Context;
using DevInSales.DTOs;
using DevInSales.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevInSales.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SqlContext _context;

        public AuthController(SqlContext context)
        {
            _context = context;
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult> AuthenticateAsync([FromBody] UserLoginDto dto)

        // TODO: validar se o dto está preenchido.
        {
            var user = await _context.User.Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.Email == dto.Email && u.Password == dto.Password);

            if (user == null) return BadRequest(new { Message = "Usuário ou senha inválidos." });

            var token = TokenService.GenerateToken(user);


            // TODO: usar o DTO >> UserResponseDTO
            var result = new
            {
                token,
                User = new
                {
                    user.Id,
                    user.Name,
                    user.Email
                }
            };

            return Ok(new { result });
        }

        [HttpGet("endpoint-aberto")]
        public ActionResult EndpontAberto()
        {
            return Ok(new { Message = "Bem vindo ao endpoint aberto! =)" });
        }

        [HttpGet("endpoint-usuario")]
        [Authorize(Roles = "Usuário, Gerente, Administrador")]
        public ActionResult EndpontUsuario()
        {
            return Ok(new { Message = "Bem vindo ao endpoint do usuário! =)" });
        }

        [HttpGet("endpoint-gerente")]
        [Authorize(Roles = "Gerente, Administrador")]
        public ActionResult EndpontGerente()
        {
            return Ok(new { Message = "Bem vindo ao endpoint do gerente! =)" });
        }

        [HttpGet("endpoint-adm")]
        [Authorize(Roles = "Administrador")]
        public ActionResult EndpontAdm()
        {
            return Ok(new { Message = "Bem vindo ao endpoint do adm! =)" });
        }
    }
}