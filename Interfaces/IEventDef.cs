namespace Chrona.Engine.Core.Interfaces;

//public interface IEventDef
//{
//    public ICondition Condition { get; }
//    public ITargetFinder TargetFinder { get; }

//    public IOption Option { get; }

//}


public interface IEventDef
{

    public Func<IEntity, ISession, bool> IsSatisfied { get; }
    public Func<IEntity, ISession, IEntity> FindTarget { get; }

    public IOptionDef OptionDef { get; }
}