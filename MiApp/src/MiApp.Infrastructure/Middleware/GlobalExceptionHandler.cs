using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiApp.Domain.Exceptions;
using FluentValidation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MiApp.Infrastructure.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // 1. Loguear siempre el error con su StackTrace completo para debugging
        _logger.LogError(exception, "Excepción no manejada: {Message}", exception.Message);

        // 2. Mapear el tipo de excepción al Status Code HTTP correspondiente (según la guía)
        var (statusCode, title) = exception switch
        {
            NotFoundException ex => (StatusCodes.Status404NotFound, ex.Message),
            ValidationException => (StatusCodes.Status400BadRequest, "Validación fallida"),
            DomainException ex => (StatusCodes.Status422UnprocessableEntity, ex.Message),
            _ => (StatusCodes.Status500InternalServerError, "Ocurrió un error inesperado en el servidor.")
        };

        // 3. Configurar la respuesta HTTP
        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/problem+json";

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = exception.Message,
            Instance = httpContext.Request.Path
        };

        // 4. Si es un error de FluentValidation, le anexamos la lista de campos inválidos
        if (exception is ValidationException validationException)
        {
            problemDetails.Extensions.Add("errors", validationException.Errors);
        }

        // 5. Escribir el JSON resultante en el cuerpo de la respuesta
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true; // Indica a .NET que ya manejamos la excepción con éxito
    }
}