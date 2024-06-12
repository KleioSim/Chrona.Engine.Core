using Chrona.Engine.Core.Interfaces;
using System;

namespace Chrona.Engine.Core.Modders;

public class OptionDef : IOptionDef
{
    public Func<IEventContext, IEnumerable<IMessage>> ProductMessage { get; init; }

    public Func<IEventContext, string> GetDesc { get; init; }
}

public abstract class EventDef : IEventDef
{
    public abstract IOptionDef OptionDef { get; }

    public abstract Func<IEntity, ISession, bool> IsSatisfied { get; }

    public abstract Func<IEntity, ISession, IEntity> FindTarget { get; }

    public abstract PlayerFlag playerFlag { get; }
}