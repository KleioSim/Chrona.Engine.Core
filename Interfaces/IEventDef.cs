namespace Chrona.Engine.Core.Interfaces;

public interface IEventDef
{
    public ICondition Condition { get; }
    public ITargetFinder TargetFinder { get; }

    public IOption Option { get; }
}
