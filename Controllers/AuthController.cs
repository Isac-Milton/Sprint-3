using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PedidosLanchonete.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO login)
        {
            try
            {
                if (
                    login.Email == "admin@admin.com" &&
                    login.Senha == "123456"
                )
                {
                    var key =
                        Encoding.ASCII.GetBytes(
                            "CHAVE_SUPER_SECRETA_PEDIDOS_LANCHONETE_2026_123456"
                        );

                    var tokenHandler =
                        new JwtSecurityTokenHandler();

                    var tokenDescriptor =
                        new SecurityTokenDescriptor
                        {
                            Subject =
                                new ClaimsIdentity(
                                    new Claim[]
                                    {
                                        new Claim(
                                            ClaimTypes.Email,
                                            login.Email
                                        ),

                                        new Claim(
                                            ClaimTypes.Role,
                                            "Admin"
                                        )
                                    }),

                            Expires =
                                DateTime.UtcNow.AddHours(2),

                            SigningCredentials =
                                new SigningCredentials(
                                    new SymmetricSecurityKey(key),
                                    SecurityAlgorithms.HmacSha256Signature
                                )
                        };

                    var token =
                        tokenHandler.CreateToken(
                            tokenDescriptor
                        );

                    return Ok(new
                    {
                        mensagem = "Login realizado com sucesso",

                        token =
                            tokenHandler.WriteToken(token)
                    });
                }

                return Unauthorized(new
                {
                    erro = "Email ou senha inválidos"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    erro = ex.Message
                });
            }
        }
    }

    public class LoginDTO
    {
        public string Email { get; set; } = string.Empty;

        public string Senha { get; set; } = string.Empty;
    }
}