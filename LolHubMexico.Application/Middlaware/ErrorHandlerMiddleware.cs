using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using LolHubMexico.Application.Exceptions;

namespace LolHubMexico.Application.Middlaware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context); 
            }
            catch (Exception error)
            {
                context.Response.ContentType = "application/json";

                // Por defecto será error interno del servidor
                var response = context.Response;
                response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var result = JsonSerializer.Serialize(new { error = "Ocurrió un error inesperado." });

                // Si es una excepción controlada (como AppException), la manejamos diferente
                if (error is AppException)
                {
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(new { error = error.Message });
                }

                await context.Response.WriteAsync(result);
            }
        }
    }
}
