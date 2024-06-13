namespace Chrona.Engine.Core.Interfaces;

public interface IInteraction
{
    string Desc { get; }

    bool IsVaild(ISession session);

    void Invoke(ISession session);
}