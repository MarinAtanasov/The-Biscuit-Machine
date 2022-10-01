using AppBrix.Modules;
using System;
using System.Collections.Generic;

namespace BiscuitMachine.Testing;

public sealed class TestMainModule<T> : MainModuleBase where T : class, IModule
{
    public override IEnumerable<Type> Dependencies => new[] { typeof(T) };
}
