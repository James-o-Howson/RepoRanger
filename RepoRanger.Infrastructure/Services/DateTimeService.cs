using RepoRanger.Application.Common.Interfaces;

namespace RepoRanger.Infrastructure.Services;

public sealed class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}