
using Chrona.Engine.Core.Interfaces;

namespace Chrona.Engine.Core.Events;

internal class EventSystem : IEventSystem
{
    private Random random = new Random();

    public IEnumerable<IEvent> OnNexturn(ISession session, Dictionary<Type, IEnumerable<IEventDef>> eventDefs)
    {
        foreach (var entity in session.Entities)
        {
            foreach (var eventDef in eventDefs[entity.GetType()])
            {
                if (!eventDef.Condition.IsSatisfied(entity, session))
                {
                    continue;
                }

                IEntity target = null;
                if (eventDef.TargetFinder != null)
                {
                    foreach (var item in eventDef.TargetFinder.Targets.Get(entity, session))
                    {
                        var factorSum = eventDef.TargetFinder.ConditionFactors.Where(x => x.Condition.IsSatisfied(item, session))
                            .Sum(x => x.Factor);
                        if (factorSum < 0)
                        {
                            continue;
                        }
                        if (random.NextDouble() < factorSum)
                        {
                            target = item;
                            break;
                        }
                    }

                    if (target == null)
                    {
                        continue;
                    }

                }

                var @event = new Event(entity, target, eventDef);
                if (target != session.Player)
                {
                    @event.AIDo();
                    continue;
                }

                yield return @event;
            }
        }
    }
}
