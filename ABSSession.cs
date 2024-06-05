﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Chrona.Engine.Core;

public interface ISession
{
    IEnumerable<IEntity> Entities { get; }
    IEnumerable<IEvent> OnNextTurn();

    IModder Modder { get; set; }
}

public interface IEntity
{

}

public abstract class Entity : IEntity
{

}


public interface IEvent
{

}

public interface IEventDef
{
    public ICondition Condition { get; }
    public ITargetFinder TargetFinder { get; }
}

public interface ITargetFinder
{
    IEntity Find(IEntity entity, ABSSession session);
}

public interface ICondition
{
    bool IsSatisfied(IEntity entity, ISession session);
}

public interface IModder
{
    Dictionary<Type, IEnumerable<IEventDef>> EventDefs { get; }
}

public class Modder : IModder
{
    public Dictionary<Type, IEnumerable<IEventDef>> EventDefs { get; }

    public Modder(string modPath)
    {
        EventDefs = AppDomain.CurrentDomain.GetAssemblies().
            SelectMany(assm => assm.GetExportedTypes()
                                  .Where(x => x.IsAssignableTo(typeof(IEventDef)) && x.GetCustomAttribute<DefToAttribute>() != null))
            .GroupBy(x => x.GetCustomAttribute<DefToAttribute>().toType)
            .ToDictionary(k => k.Key, v => v.Select(type => Activator.CreateInstance(type) as IEventDef));
    }
}

public class DefToAttribute : Attribute
{
    public readonly Type toType;
    public DefToAttribute(Type type)
    {
        toType = type;
    }
}

public abstract class ABSSession : ISession
{
    public abstract IEntity Player { get; set; }
    public abstract IEnumerable<IEntity> Entities { get; }

    public IModder Modder { get; set; }

    public IEnumerable<IEvent> OnNextTurn()
    {
        foreach (var entity in Entities)
        {
            foreach (var eventDef in Modder.EventDefs[entity.GetType()])
            {
                if (!eventDef.Condition.IsSatisfied(entity, this))
                {
                    continue;
                }

                IEntity target = null;
                if (eventDef.TargetFinder != null
                    && (target = eventDef.TargetFinder.Find(entity, this)) == null)
                {
                    continue;
                }

                var @event = new Event(entity, target, eventDef);
                if (target != Player)
                {
                    @event.AIDo();
                    continue;
                }

                yield return @event;
            }
        }
    }
}

internal class Event : IEvent
{
    public Event(IEntity from, IEntity to, IEventDef def)
    {
    }

    internal void AIDo()
    {

    }
}

public class TrueCondtion : ICondition
{
    public bool IsSatisfied(IEntity entity, ISession session)
    {
        return true;
    }
}

public class TargetSelf : ITargetFinder
{
    public IEntity Find(IEntity entity, ABSSession session)
    {
        return entity;
    }
}