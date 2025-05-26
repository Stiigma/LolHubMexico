using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

public class FirebaseAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var authorizationHeader = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();

        if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var token = authorizationHeader.Substring("Bearer ".Length);

        try
        {
            FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);

            // Puedes guardar el UID en el contexto para usarlo en el controlador si lo deseas:
            context.HttpContext.Items["FirebaseUid"] = decodedToken.Uid;
        }
        catch (Exception)
        {
            context.Result = new UnauthorizedResult();
        }
    }
}
