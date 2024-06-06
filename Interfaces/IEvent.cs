namespace Chrona.Engine.Core.Interfaces;

public interface IEvent
{
    IEntity From { get; }
    IEntity To { get; }
    IEventDef Def { get; }
}