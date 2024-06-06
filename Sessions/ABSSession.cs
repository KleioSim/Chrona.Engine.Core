using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Chrona.Engine.Core.Events;
using Chrona.Engine.Core.Interfaces;

namespace Chrona.Engine.Core.Sessions;


public abstract class Entity : IEntity
{

}


public abstract class ABSSession : ISession
{
    public abstract IEntity Player { get; set; }
    public abstract IEnumerable<IEntity> Entities { get; }

    public IModder Modder { get; set; }

    public Dictionary<Type, Action<IMessage>> dictMessageProcess = new Dictionary<Type, Action<IMessage>>();

    public void OnNextTurn()
    {

    }

    public void OnMessage(IMessage message)
    {
        dictMessageProcess[message.GetType()].Invoke(message);
    }
}
