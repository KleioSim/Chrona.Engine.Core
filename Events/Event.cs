using Chrona.Engine.Core.Interfaces;

namespace Chrona.Engine.Core.Events;

public class Event : IEvent
{
    public string Title => throw new NotImplementedException();

    public string Desc => throw new NotImplementedException();

    public IOption Option { get; }

    private IEventDef Def { get; }

    private EventContext context { get; }

    public Event(EventContext context, IEventDef def)
    {
        this.context = context;
        Def = def;

        Option = new Option(def.OptionDef, context);
    }

    internal void AIDo()
    {
        Option.Do();
    }
}

public struct EventContext : IEventContext
{
    public IEntity From { get; }
    public IEntity To { get; }
    public ISession Session { get; }

    public EventContext(IEntity from, IEntity target, ISession session)
    {
        this.From = from;
        this.To = target;
        this.Session = session;
    }
}