namespace Chrona.Engine.Core.Interfaces;

public interface ISession
{
    //IEntity Player { get; }
    //IReadOnlyDictionary<string, IEntity> Entities { get; }
    //void OnNextTurn();
    void OnMessage(IMessage message);

    //IModder Modder { get; set; }
}
