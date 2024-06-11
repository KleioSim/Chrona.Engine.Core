
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
                if (!eventDef.IsSatisfied(entity, session))
                {
                    continue;
                }

                var target = eventDef.FindTarget(entity, session);
                if (target == null)
                {
                    continue;
                }

                var @event = new Event(entity, target, session, eventDef);
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

