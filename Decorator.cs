using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Chrona.Engine.Core;


public class Decorator : DispatchProxy
{
    public static uint Label { get; set; }
    private object _decorated;

    private Dictionary<MethodInfo, bool> method2DataChangeFlag = new Dictionary<MethodInfo, bool>();

    protected override object Invoke(MethodInfo targetMethod, object[] args)
    {
        if (!method2DataChangeFlag.TryGetValue(targetMethod, out var flag))
        {
            var methodParameterTypes = targetMethod.GetParameters().Select(p => p.ParameterType).ToArray();
            var realMethodInfo = _decorated.GetType().GetMethod(targetMethod.Name, methodParameterTypes);

            var attribute = realMethodInfo.GetCustomAttribute<DataChangeAttribute>();
            flag = attribute != null;

            method2DataChangeFlag.Add(targetMethod, flag);
        }

        if (flag)
        {
            Label++;
        }

        var result = targetMethod.Invoke(_decorated, args);

        return result;
    }

    public static T Create<T>(T decorated)
    {
        Label++;

        object proxy = Create<T, Decorator>();
        ((Decorator)proxy).SetParameters(decorated);

        return (T)proxy;
    }

    private void SetParameters<T>(T decorated)
    {
        if (decorated == null)
        {
            throw new ArgumentNullException(nameof(decorated));
        }
        _decorated = decorated;
    }
}
