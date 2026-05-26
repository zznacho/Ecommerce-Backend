using System;

namespace MiApp.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
}