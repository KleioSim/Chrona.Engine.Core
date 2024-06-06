namespace Chrona.Engine.Core.Interfaces;

public interface ISession
{
    IEntity Player { get; }
    IEnumerable<IEntity> Entities { get; }
    void OnNextTurn();
    void OnMessage(IMessage message);

    IModder Modder { get; set; }
}
