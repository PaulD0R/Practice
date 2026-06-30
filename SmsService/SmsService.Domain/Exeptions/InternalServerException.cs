namespace SmsService.Domain.Exeptions;

public class InternalServerException(string message) : Exception(message);