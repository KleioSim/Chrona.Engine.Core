using Chrona.Engine.Core.Interfaces;

namespace Chrona.Engine.Core.Events;

public class Option : IOption
{
    public string Desc { get; init; }

    public Func<IEntity, IEntity, ISession, IEnumerable<IMessage>> ProductMessage;

    public IEnumerable<IMessage> Do(IEntity entity, IEntity to, ISession session)
    {
        return ProductMessage(entity, to, session);
    }
}