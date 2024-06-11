﻿using Chrona.Engine.Core.Events;

namespace Chrona.Engine.Core.Interfaces;

public interface IOption
{
    string Desc { get; }
    string Tip { get; }

    void Do();
}

public interface IOptionDef
{
    Func<IEventContext, string> GetDesc { get; }
    Func<IEventContext, IEnumerable<IMessage>> ProductMessage { get; }
}

public interface IEventContext
{
    IEntity From { get; }
    IEntity To { get; }
    ISession Session { get; }
}