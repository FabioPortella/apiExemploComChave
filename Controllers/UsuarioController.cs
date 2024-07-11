using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;


[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly DataContext context;

    public UsuarioController(DataContext _context)
    {
        context = _context;
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] Usuario item)
    {
        try
        {
            item.Senha = ObterSenha(item);

            await context.Usuarios.AddAsync(item);
            await context.SaveChangesAsync();
            return Ok("Usuário salvo com sucesso");
        }
        catch
        {
            return BadRequest("Erro ao salvar o usuário informado");
        }
    }

    [NonAction]
    private static string Hash(string password)
    {
        HashAlgorithm hasher = HashAlgorithm.Create(HashAlgorithmName.SHA512.Name);
        byte[] stringBytes = Encoding.ASCII.GetBytes(password);
        byte[] byteArray = hasher.ComputeHash(stringBytes);

        StringBuilder stringBuilder = new StringBuilder();
        foreach (byte b in byteArray)
        {
            stringBuilder.AppendFormat("{0:x2}", b);
        }

        return stringBuilder.ToString();
    }

    [NonAction]
    private static string ObterSenha(Usuario usuario)
    {
        if (usuario == null || usuario.Senha == null || usuario.Senha.Trim() == "")
            throw new Exception();

        string retorno = usuario.Senha;

        retorno = "sdfgg5g5" + retorno;
        retorno = Hash(retorno);
        retorno += "w54gw4545445";
        retorno = Hash(retorno);

        return retorno;
    }

    [HttpPost("autenticar")]
    public async Task<ActionResult> Autenticar([FromBody] Usuario item)
    {
        try
        {
            Usuario? existe = await context.Usuarios.FirstOrDefaultAsync(x => x.Email == item.Email);
            if (existe == null)
                return BadRequest("E-mail e/ou senha inválido(s)");

            item.Senha = ObterSenha(item);

            if (item.Senha != existe.Senha)
                return BadRequest("E-mail e/ou senha inválido(s)");

            existe.Senha = "";
            return Ok(existe);
        }
        catch
        {
            return BadRequest("Erro geral");
        }
    }
}