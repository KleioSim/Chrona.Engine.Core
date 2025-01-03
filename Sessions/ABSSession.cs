﻿using Chrona.Engine.Core.Interfaces;
using System.Reflection;

namespace Chrona.Engine.Core.Sessions;



//public abstract class AbstractSession : ISession
//{
//    public abstract IEntity Player { get; set; }
//    public abstract IReadOnlyDictionary<string, IEntity> Entities { get; }

//    public IModder Modder { get; set; }

//    public Dictionary<Type, MethodInfo> dictMessageProcess = new Dictionary<Type, MethodInfo>();

//    public AbstractSession()
//    {
//        //IEntity.SendMessage = OnMessage;

//        var methods = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic)
//            .Where(x => x.GetCustomAttribute<MessageProcessAttribute>() != null);

//        foreach (var method in methods)
//        {
//            var parameters = method.GetParameters();
//            if (parameters.Length != 1)
//            {
//                throw new Exception();
//            }

//            if (!parameters[0].ParameterType.IsAssignableTo(typeof(IMessage)))
//            {
//                throw new Exception();
//            }

//            dictMessageProcess.Add(parameters[0].ParameterType, method);
//        }
//    }

//    //public void OnNextTurn()
//    //{
//    //    foreach (var entity in Entities)
//    //    {
//    //        entity.IsInteractionDateOut = false;
//    //    }
//    //}

//    [DataChange]
//    public void OnMessage(IMessage message)
//    {
//        dictMessageProcess[message.GetType()].Invoke(this, new object[] { message });
//    }
//}

//public class MessageProcessAttribute : Attribute
//{

//}