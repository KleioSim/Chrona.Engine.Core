using Chrona.Engine.Core.Interfaces;

namespace Chrona.Engine.Core;

public class Interaction : IInteraction
{
    public static Action<IMessage>? ProcessMessage;

    public string Desc => def.GetDesc(owner);


    public void Invoke(ISession session)
    {
        foreach (var message in def.Invoke(owner, session))
        {
            ProcessMessage(message);
        }
    }

    private readonly IInteractionDef def;
    private readonly IEntity owner;

    public bool IsVaild(ISession session)
    {
        return def.IsVaild(owner, session);
    }

    public Interaction(IInteractionDef def, IEntity owner)
    {
        this.def = def;
        this.owner = owner;
    }
}