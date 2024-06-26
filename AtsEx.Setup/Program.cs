using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.Setup
{
    internal static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            bool isInteractive = true;

            {
                RootCommand rootCommand = new RootCommand("AtsEX をインストールします。");

                Command nonInteractiveCommand = new Command("non-interactive", "非対話モードで実行します。");
                nonInteractiveCommand.AddAlias("n");

                {
                    Option<string> bve6PathOption = new Option<string>("--bve6", "AtsEX を適用する BVE Trainsim 6 のパスを指定します。");
                    bve6PathOption.AddAlias("-6");

                    Option<string> bve5PathOption = new Option<string>("--bve5", "AtsEX を適用する BVE Trainsim 5 のパスを指定します。");
                    bve5PathOption.AddAlias("-5");

                    Option<string> scenarioDirectoryOption = new Option<string>("--scenario", "AtsEX サンプルシナリオを配置するシナリオフォルダを指定します。");
                    scenarioDirectoryOption.AddAlias("-s");

                    nonInteractiveCommand.AddOption(bve6PathOption);
                    nonInteractiveCommand.AddOption(bve5PathOption);
                    nonInteractiveCommand.AddOption(scenarioDirectoryOption);

                    nonInteractiveCommand.SetHandler((bve6Path, bve5Path, scenarioDirectory) =>
                    {
                        isInteractive = false;

                        TargetPath.Bve6Path.Value = new InstallationTarget(bve6Path);
                        TargetPath.Bve5Path.Value = new InstallationTarget(bve5Path);
                        TargetPath.ScenarioDirectory.Value = new InstallationTarget(scenarioDirectory);
                    }, bve6PathOption, bve5PathOption, scenarioDirectoryOption);
                }

                rootCommand.AddCommand(nonInteractiveCommand);
                rootCommand.Invoke(args);
            }

            Models.Navigator.Initialize(isInteractive);

            App app = new App();
            app.InitializeComponent();
            app.Run();
        }
    }
}
