using Chrona.Engine.Core.Interfaces;

namespace Chrona.Engine.Core.Events;

public class Event : IEvent
{
    public static Action<IMessage>? ProcessMessage;

    public IEntity From { get; set; }

    public IEntity To { get; set; }

    public IEventDef Def { get; set; }

    public Event(IEntity from, IEntity to, IEventDef def)
    {
        From = from;
        To = to;
        Def = def;
    }


    public void DoOption()
    {
        var messageBinds = Def.Option.MessageBinds;
        foreach (var bind in messageBinds)
        {
            var message = Activator.CreateInstance(bind.MessageType) as IMessage;
            message.Target = bind.TargetVisitor.Get(this);
            message.Value = bind.ValueVisitor.Get(this);
            ProcessMessage(message);
        }
    }

    internal void AIDo()
    {
        DoOption();
    }
}
