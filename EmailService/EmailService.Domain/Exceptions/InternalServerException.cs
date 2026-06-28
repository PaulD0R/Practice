namespace EmailService.Domain.Exceptions;

public class InternalServerException(string message) : Exception(message);
