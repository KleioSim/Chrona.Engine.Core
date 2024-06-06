namespace Chrona.Engine.Core.Interfaces;

public interface IOption
{
    public IEnumerable<IMessageBind> MessageBinds { get; }
}
