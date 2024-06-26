using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Reactive.Bindings;

namespace AtsEx.Setup
{
    internal static class TargetPath
    {
        public static ReactivePropertySlim<InstallationTarget> Bve6Path { get; } = new ReactivePropertySlim<InstallationTarget>(InstallationTarget.NotIdentified);
        public static ReactivePropertySlim<InstallationTarget> Bve5Path { get; } = new ReactivePropertySlim<InstallationTarget>(InstallationTarget.NotIdentified);
        public static ReactivePropertySlim<InstallationTarget> ScenarioDirectory { get; } = new ReactivePropertySlim<InstallationTarget>(InstallationTarget.NotIdentified);
        public static ReactivePropertySlim<bool> InstallSdk { get; } = new ReactivePropertySlim<bool>(false);

        public static ReactivePropertySlim<bool> CopyBve { get; } = new ReactivePropertySlim<bool>(false);
    }
}
