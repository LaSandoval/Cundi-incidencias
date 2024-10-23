using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.RegularExpressions;

public class ValidarCorreoAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ActionArguments.ContainsKey("correo"))
        {
            var correo = context.ActionArguments["correo"] as string;
            if (!EsCorreoValido(correo))
            {
                context.Result = new BadRequestObjectResult(new { mensaje = "Correo inválido" });
                return;
            }
        }

        base.OnActionExecuting(context);  
    }

    private bool EsCorreoValido(string correo)
    {
        if (string.IsNullOrEmpty(correo))
            return false;

        string patronCorreo = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(correo, patronCorreo);
    }
}
