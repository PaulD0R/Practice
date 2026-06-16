using System.ComponentModel.DataAnnotations;

namespace NotificationService.Application.DTOs;

public record NotificationRequest(
    [EmailAddress] string? Email,
    [Phone] string? Phone, 
    string? Subject,
    [Required] string Text);