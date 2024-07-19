using RepoRanger.Application.Abstractions.Interfaces;

namespace RepoRanger.Infrastructure.Services;

public sealed class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}