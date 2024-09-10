namespace Chrona.Engine.Core.Interfaces;

public interface IEntity
{
    static Action<IMessage> SendMessage { get; set; }
    string Id { get; }
    //bool IsInteractionDateOut { get; set; }
    //IEnumerable<IInteraction> Interactions { get; }
}