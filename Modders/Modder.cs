using System.Reflection;
using Chrona.Engine.Core.Events;
using Chrona.Engine.Core.Interfaces;
using Chrona.Engine.Core.Sessions;

namespace Chrona.Engine.Core.Modders;

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
