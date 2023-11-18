using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Reactive.Bindings;
using Reactive.Bindings.Disposables;
using Reactive.Bindings.Extensions;
using Vanara.PInvoke;

using AtsEx.Setup.Installing;

namespace AtsEx.Setup.Models
{
    internal class InstallingPageModel : INotifyPropertyChanged, IDisposable
    {
        private static readonly string Namespace = "AtsEx.Setup.Packages";
        private static readonly int DelayMilliseconds = 500;

        private readonly CompositeDisposable Disposables = new CompositeDisposable();

        public event PropertyChangedEventHandler PropertyChanged;

        public InstallingPageModel()
        {
        }

        public void Install(IProgress<State> stateReporter)
        {
            string atsExDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), "AtsEx");

            /*
            using (StartMenu startMenu = new StartMenu())
            {
                startMenu.CreateShortcut(@"AtsEX", atsExDirectory);
            }*/

            {
                stateReporter.Report(new State(100, "AtsEX 本体パッケージを展開・配置しています..."));

                ArchivedPackage archive = ArchivedPackage.FromResource($"{Namespace}.AtsEx.zip");
                archive.ExtractAndLocate(atsExDirectory);

                Task.Delay(DelayMilliseconds).Wait();
            }

            {
                stateReporter.Report(new State(200, "AtsEX Caller InputDevice を準備しています..."));

                string fileName = "AtsEx.Caller.InputDevice.dll";
                Package package = Package.FromResource($"{Namespace}.{fileName}");

                if (!(TargetPath.Bve6Path is null))
                {
                    LocateCallerAndLink(TargetPath.Bve6Path, 6, 220);
                }

                if (!(TargetPath.Bve5Path is null))
                {
                    LocateCallerAndLink(TargetPath.Bve5Path, 5, 260);
                }

                void LocateCallerAndLink(string bvePath, int bveVersion, int progressValueOrigin)
                {
                    stateReporter.Report(new State(progressValueOrigin, $"BVE Trainsim {bveVersion} に AtsEX Caller InputDevice を配置しています..."));

                    string bveDirectory = Path.GetDirectoryName(bvePath);
                    string inputDevicesDirectory = Path.Combine(bveDirectory, "Input Devices");
                    string inputDevicesAtsExDirectory = Path.Combine(inputDevicesDirectory, "AtsEx");

                    package.Locate(Path.Combine(inputDevicesDirectory, fileName));

                    Task.Delay(DelayMilliseconds / 2).Wait();

                    if (Directory.Exists(inputDevicesAtsExDirectory))
                    {
                        DirectoryInfo directoryInfo = new DirectoryInfo(inputDevicesAtsExDirectory);
                        if (!directoryInfo.Attributes.HasFlag(FileAttributes.ReparsePoint))
                        {
                            stateReporter.Report(new State(progressValueOrigin + 10, $"BVE Trainsim {bveVersion} に配置されている既存の AtsEX のバックアップを作成しています..."));

                            ZipFile.CreateFromDirectory(inputDevicesAtsExDirectory, FileNamer.CreateFilePathInSequence(inputDevicesAtsExDirectory + "_old.zip"));
                            Directory.Delete(inputDevicesAtsExDirectory, true);
                        }
                    }

                    stateReporter.Report(new State(progressValueOrigin + 20, $"BVE Trainsim {bveVersion} に AtsEX 本体へのシンボリックリンクを作成しています..."));

                    Kernel32.CreateSymbolicLink(Path.Combine(inputDevicesDirectory, "AtsEx"), atsExDirectory, Kernel32.SymbolicLinkType.SYMBOLIC_LINK_FLAG_DIRECTORY);

                    Task.Delay(DelayMilliseconds / 2).Wait();
                }
            }

            if (!(TargetPath.Bve5Path is null))
            {
                string bve5FileName = Path.GetFileName(TargetPath.Bve5Path);
                stateReporter.Report(new State(290, $"Bve Trainsim 5 の {bve5FileName}.config を編集しています..."));

                Package package = Package.FromResource($"{Namespace}.Bve5Config.xml");
                package.Locate($"{TargetPath.Bve5Path}.config");

                Task.Delay(DelayMilliseconds).Wait();
            }

            {
                stateReporter.Report(new State(300, $"ユーザーの一覧を取得しています..."));

                WinUserCollection users = WinUserCollection.Create();
                for (int i = 0; i < users.Count; i++)
                {
                    ReportState(0, "このユーザーで BVE Trainsim が使用されているか確認しています...");
                    Task.Delay(DelayMilliseconds / 4).Wait();

                    if (!(TargetPath.Bve6Path is null))
                    {
                        ReportState(0.2, "BVE Trainsim 6 の設定ファイルを編集しています...");
                        EditPreferences("BveTs6.Preferences.xml");
                    }

                    if (!(TargetPath.Bve5Path is null))
                    {
                        ReportState(0.6, "BVE Trainsim 5 の設定ファイルを編集しています...");
                        EditPreferences("Preferences.xml");
                    }

                    void ReportState(double progressRate, string detail)
                    {
                        string userDescription = $"ユーザー '{users[i].Name}' ({i + 1} / {users.Count}): ";
                        int progressValue = 310 + (int)(90 * (i + progressRate) / users.Count);
                        stateReporter.Report(new State(progressValue, userDescription + detail));
                    }

                    void EditPreferences(string xmlFileName)
                    {
                        PreferenceEditor preferenceEditor = PreferenceEditor.TryCreateFromUser(users[i], xmlFileName);
                        if (preferenceEditor is null) return;

                        preferenceEditor.AddInputDevice("atsex.caller.inputdevice");
                        preferenceEditor.Save();

                        Task.Delay(DelayMilliseconds).Wait();
                    }
                }
            }

            if (!(TargetPath.ScenarioDirectory is null))
            {
                stateReporter.Report(new State(400, "AtsEX サンプルシナリオを展開・配置しています..."));

                ArchivedPackage archive = ArchivedPackage.FromResource($"{Namespace}.Scenarios.zip");
                archive.ExtractAndLocate(TargetPath.ScenarioDirectory);

                Task.Delay(DelayMilliseconds).Wait();
            }

            stateReporter.Report(new State(500, "インストール処理を完了させています..."));
            Task.Delay(DelayMilliseconds * 4).Wait();
        }

        public void GoNext()
        {
            Navigator.Instance.Page.Value = Page.Completed;
        }

        public void Abort()
        {
            if (MessageBox.Show("本当にインストールを中止してもよろしいですか?", "AtsEX セットアップウィザード", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Navigator.Instance.Page.Value = Page.Welcome;
            }
        }

        public void Dispose()
        {
            Disposables.Dispose();
        }


        internal class State
        {
            public int Value { get; }
            public string Detail { get; }

            public State(int value, string detail)
            {
                Value = value;
                Detail = detail;
            }
        }
    }
}
