using System;

namespace Streamliner.Core.Base;

public abstract class ServiceInfo
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    protected ServiceInfo(Guid id, string name)
    {
        Id = id;
        Name = name;
        Description = string.Empty;
    }

    protected ServiceInfo(Guid id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
}