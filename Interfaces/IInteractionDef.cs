namespace Chrona.Engine.Core.Interfaces;

public interface IInteractionDef
{
    string GetDesc(IEntity owner);

    Func<IEntity, ISession, bool> IsVaild { get; }

    Func<IEntity, ISession, IEnumerable<IMessage>> Invoke { get; }
}
