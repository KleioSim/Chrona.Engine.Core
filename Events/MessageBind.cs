using Chrona.Engine.Core.Interfaces;

namespace Chrona.Engine.Core.Events;

public class MessageBind : IMessageBind
{
    public Type MessageType { get; set; }

    public DataVisitor TargetVisitor { get; set; }
    public DataVisitor ValueVisitor { get; set; }
}
