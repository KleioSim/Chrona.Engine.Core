using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using Chrona.Engine.Core.Events;
using Chrona.Engine.Core.Interfaces;
using Chrona.Engine.Core.Sessions;

namespace Chrona.Engine.Core.Modders;

public class Modder : IModder
{
    public Dictionary<Type, IEnumerable<IEventDef>> EventDefs { get; } = new Dictionary<Type, IEnumerable<IEventDef>>();

    public Modder(string modRootPath)
    {

        foreach (var modPath in Directory.EnumerateDirectories(modRootPath))
        {
            var dllPath = Path.Combine(modPath, "dll", Path.GetFileName(modPath) + ".dll");
            if (!File.Exists(dllPath))
            {
                continue;
            }

            var alc = AssemblyLoadContext.GetLoadContext(Assembly.GetExecutingAssembly());
            var assembly = alc.LoadFromAssemblyPath(dllPath);

            var types = assembly.ExportedTypes;

            foreach (var eventType in types.Where(x => x.IsAssignableTo(typeof(IEventDef)) && x.GetCustomAttribute<DefToAttribute>() != null))
            {
                var toType = eventType.GetCustomAttribute<DefToAttribute>().toType;
                if (!EventDefs.ContainsKey(toType))
                {
                    EventDefs.Add(toType, new List<IEventDef>());
                }

                var list = EventDefs[toType] as List<IEventDef>;
                list.Add(Activator.CreateInstance(eventType) as IEventDef);
            }
        }
    }
}
