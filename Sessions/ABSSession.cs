﻿using Chrona.Engine.Core.Interfaces;
using System.Reflection;

namespace Chrona.Engine.Core.Sessions;


public abstract class Entity : IEntity
{

}


public abstract class ABSSession : ISession
{
    public abstract IEntity Player { get; set; }
    public abstract IEnumerable<IEntity> Entities { get; }

    public IModder Modder { get; set; }

    public Dictionary<Type, MethodInfo> dictMessageProcess = new Dictionary<Type, MethodInfo>();

    public ABSSession()
    {
        var methods = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(x => x.GetCustomAttribute<MessageProcessAttribute>() != null);

        foreach (var method in methods)
        {
            var parameters = method.GetParameters();
            if (parameters.Length != 1)
            {
                throw new Exception();
            }

            if (!parameters[0].ParameterType.IsAssignableTo(typeof(IMessage)))
            {
                throw new Exception();
            }

            dictMessageProcess.Add(parameters[0].ParameterType, method);
        }
    }

    public void OnNextTurn()
    {

    }

    public void OnMessage(IMessage message)
    {
        dictMessageProcess[message.GetType()].Invoke(this, new object[] { message });
    }
}

public class MessageProcessAttribute : Attribute
{

}