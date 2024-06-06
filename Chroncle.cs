﻿using Chrona.Engine.Core.Events;
using Chrona.Engine.Core.Interfaces;

namespace Chrona.Engine.Core;

public class Chroncle
{
    public ISession? Session
    {
        get => session;
        set
        {
            session = value;

            Event.ProcessMessage = null;
            if (session != null)
            {
                Event.ProcessMessage = session.OnMessage;
            }
        }
    }

    public IModder Modder { get; }

    private IEventSystem eventSystem = new EventSystem();

    private ISession? session;

    public Chroncle(IModder modder)
    {
        this.Modder = modder;
    }

    public IEnumerable<IEvent> OnNextTurn()
    {
        if (Session == null)
        {
            throw new InvalidOperationException();
        }

        foreach (var @event in eventSystem.OnNexturn(Session, Modder.EventDefs))
        {
            yield return @event;
        }

        Session.OnNextTurn();
    }
}