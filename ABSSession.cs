using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Chrona.Engine.Core;

public interface ISession
{
    IEnumerable<IEntity> Entities { get; }
    IEnumerable<IEvent> OnNextTurn();
    void OnMessage(IMessage message);

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
    IEntity From { get; }
    IEntity To { get; }
    IEventDef Def { get; }
}

public interface IEventDef
{
    public ICondition Condition { get; }
    public ITargetFinder TargetFinder { get; }

    public IOption Option { get; }
}

public interface IOption
{
    public IEnumerable<IMessageBind> MessageBinds { get; }
}

public class Option : IOption
{
    public IEnumerable<IMessageBind> MessageBinds { get; set; }
}

public interface IMessageBind
{
    public Type MessageType { get; }

    public DataVisitor TargetVisitor { get; }
    public DataVisitor ValueVisitor { get; }
}

public class MessageBind : IMessageBind
{
    public Type MessageType { get; set; }

    public DataVisitor TargetVisitor { get; set; }
    public DataVisitor ValueVisitor { get; set; }
}

public interface DataVisitor
{
    object Get(IEvent @event);
}

public interface ITargetFinder
{
    IEnumerable<ICondtionFactor> ConditionFactors { get; }

    IEventTarget Targets { get; }
    IEntity Find(IEntity entity, ABSSession session);
}

public interface IEventTarget
{
    IEnumerable<IEntity> Get(IEntity entity, ISession session);
}

public interface ICondtionFactor
{
    public ICondition Condition { get; }

    public double Factor { get; }
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

    private Random random = new Random();
    public Dictionary<Type, Action<IMessage>> dictMessageProcess = new Dictionary<Type, Action<IMessage>>();

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
                if (eventDef.TargetFinder != null)
                {
                    foreach (var item in eventDef.TargetFinder.Targets.Get(entity, this))
                    {
                        var factorSum = eventDef.TargetFinder.ConditionFactors.Where(x => x.Condition.IsSatisfied(item, this))
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
                if (target != Player)
                {
                    @event.AIDo();
                    continue;
                }

                yield return @event;
            }
        }
    }

    public void OnMessage(IMessage message)
    {
        dictMessageProcess[message.GetType()].Invoke(message);
    }
}

public class Event : IEvent
{
    public static Action<IMessage> ProcessMessage;

    public IEntity From { get; set; }

    public IEntity To { get; set; }

    public IEventDef Def { get; set; }

    public Event(IEntity from, IEntity to, IEventDef def)
    {
        this.From = from;
        this.To = to;
        this.Def = def;
    }


    public void DoOption()
    {
        var messageBinds = Def.Option.MessageBinds;
        foreach (var bind in messageBinds)
        {
            var message = Activator.CreateInstance(bind.MessageType) as IMessage;
            message.Target = bind.TargetVisitor.Get(this);
            message.Value = bind.ValueVisitor.Get(this);
            ProcessMessage(message);
        }
    }

    internal void AIDo()
    {
        DoOption();
    }
}

public interface IMessage
{
    public object Target { get; set; }
    public object Value { get; set; }
}

public class TrueCondtion : ICondition
{
    public bool IsSatisfied(IEntity entity, ISession session)
    {
        return true;
    }
}