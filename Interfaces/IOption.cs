namespace Chrona.Engine.Core.Interfaces;

//public interface IOption
//{
//    public IEnumerable<IMessageBind> MessageBinds { get; }
//}
public interface IOption
{
    string Desc { get; }
    IEnumerable<IMessage> Do(IEntity from, IEntity to, ISession session);
}