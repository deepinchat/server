using Deepin.Infrastructure.EventBus;

namespace Deepin.Application.IntegrationEvents;
public record SendEmailIntegrationEvent(string[] To, string Subject, string Body, bool IsBodyHtml = false, string[]? CC = null) : IntegrationEvent;