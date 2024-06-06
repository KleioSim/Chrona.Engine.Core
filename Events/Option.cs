using Chrona.Engine.Core.Interfaces;

namespace Chrona.Engine.Core.Events;

public class Option : IOption
{
    public IEnumerable<IMessageBind> MessageBinds { get; set; }
}
