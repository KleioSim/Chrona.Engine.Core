namespace Chrona.Engine.Core.Interfaces;

//public interface IEventDef
//{
//    public ICondition Condition { get; }
//    public ITargetFinder TargetFinder { get; }

//    public IOption Option { get; }

//}


public interface IEventDef
{
    bool IsSatisfied(IEntity entity, ISession session);
    IEntity FindTarget(IEntity entity, ISession session);

    public IOption Option { get; }
}