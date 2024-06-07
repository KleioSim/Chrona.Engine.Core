namespace Chrona.Engine.Core.Interfaces;

public interface IEventDef
{
    public ICondition Condition { get; }
    public ITargetFinder TargetFinder { get; }

    public IOption Option { get; }

}


public interface IEventDef2
{
    bool IsSatisfied(IEntity entity, ISession session);
    IEntity FindTarget(IEntity entity, ISession session);

    public IOption2 Option { get; }
}

public interface IOption2
{
    string Desc { get; }
    IEnumerable<IMessage> Do(IEntity entity, ISession session);
}