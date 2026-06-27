using MediatR;

namespace NotificationService.Application.Commands.Archive;

public record ArchiveCommand(int DayCount) : IRequest;