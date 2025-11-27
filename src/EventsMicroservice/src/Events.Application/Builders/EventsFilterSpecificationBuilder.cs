using Events.Domain.Entities;
using System.Linq.Expressions;

namespace Events.Application.Builders;

public class EventsFilterSpecificationBuilder
{
    private readonly List<Expression<Func<Event, bool>>> Filters = new();

    public EventsFilterSpecificationBuilder FilterByUserId(Guid userId)
    {
        Filters.Add(x => x.UserId == userId);
        return this;
    }

    public EventsFilterSpecificationBuilder FilterByPublic(bool? isPublic)
    {
        if (isPublic.HasValue)
        {
            Filters.Add(x => x.IsPublic == isPublic);
        }
        return this;
    }

    public EventsFilterSpecificationBuilder FilterByName(string? name)
    {
        if (!string.IsNullOrWhiteSpace(name))
        {
            Filters.Add(x => x.Name.ToLower().Contains(name.ToLower()));
        }
        return this;
    }

    public EventsFilterSpecificationBuilder FilterByMinAndMaxAge(int? minAge, int? maxAge)
    {
        Filters.Add(x =>
        (minAge == null || x.MinAllowedAge >= minAge)
        && (maxAge == null || x.MaxAllowedAge <= maxAge));
        return this;
    }

    public EventsFilterSpecificationBuilder FilterByActive(bool? isActive)
    {
        if (isActive.HasValue)
        {
            Filters.Add(x => x.IsActive == isActive);
        }
        return this;
    }

    public List<Expression<Func<Event, bool>>> Build()
    {
        return Filters;
    }
}
