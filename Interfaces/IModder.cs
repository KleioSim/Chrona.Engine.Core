namespace Chrona.Engine.Core.Interfaces;

public interface IModder
{
    Dictionary<Type, IEnumerable<IEventDef>> EventDefs { get; }
}