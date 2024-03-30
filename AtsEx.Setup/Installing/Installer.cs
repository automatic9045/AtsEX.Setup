using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vanara.PInvoke;

namespace AtsEx.Setup.Installing
{
    internal class Installer : IDisposable
    {
        private static readonly string CommonDocumentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
        private static readonly string Namespace = "AtsEx.Setup.Packages";
        private static readonly int DelayMilliseconds = 500;

        private readonly IProgress<InstallationState> StateReporter;
        private readonly ShortcutFactory ShortcutFactory;

        public Installer(IProgress<InstallationState> stateReporter)
        {
            StateReporter = stateReporter;
            ShortcutFactory = new ShortcutFactory();
        }

        public void Dispose()
        {
            ShortcutFactory.Dispose();
        }

        public string CopyBve(string bvePath, int bveVersion)
        {
            StateReporter.Report(new InstallationState(20, $"BVE Trainsim {bveVersion} をコピーしています..."));

            string destDirectory = Path.Combine(CommonDocumentsDirectory, $"mackoy\\BveTs{bveVersion}");
            DirectoryExtensions.CopyDirectory(Path.GetDirectoryName(bvePath), destDirectory, true);

            Task.Delay(DelayMilliseconds / 2).Wait();

            StateReporter.Report(new InstallationState(70, $"コピーした BVE Trainsim {bveVersion} のショートカットを作成しています..."));

            string newBvePath = Path.Combine(destDirectory, Path.GetFileName(bvePath));
            ShortcutFactory.CreateToDesktop($"BVE Trainsim {bveVersion} with AtsEX.lnk", newBvePath);

            Task.Delay(DelayMilliseconds / 2).Wait();

            StateReporter.Report(new InstallationState(90, $"BVE Trainsim {bveVersion} のコピー処理を完了しています..."));

            return newBvePath;
        }

        public void Install()
        {
            string atsExDirectory = Path.Combine(CommonDocumentsDirectory, "AtsEx");

            {
                StateReporter.Report(new InstallationState(100, "AtsEX Caller InputDevice を準備しています..."));

                Package callerPackage = Package.FromResource($"{Namespace}.{CallerInfo.FileName}");

                if (!(TargetPath.Bve6Path.Value is null))
                {
                    LocateCallerAndLink(TargetPath.Bve6Path.Value, 6, 120);

                    TargetPath.Bve6Path.Value = null;
                }

                if (!(TargetPath.Bve5Path.Value is null))
                {
                    LocateCallerAndLink(TargetPath.Bve5Path.Value, 5, 160);

                    string bve5FileName = Path.GetFileName(TargetPath.Bve5Path.Value);
                    StateReporter.Report(new InstallationState(190, $"Bve Trainsim 5 の {bve5FileName}.config を編集しています..."));

                    Package configPackage = Package.FromResource($"{Namespace}.Bve5Config.xml");
                    configPackage.Locate($"{TargetPath.Bve5Path.Value}.config");

                    Task.Delay(DelayMilliseconds).Wait();

                    TargetPath.Bve5Path.Value = null;
                }

                void LocateCallerAndLink(string bvePath, int bveVersion, int progressValueOrigin)
                {
                    StateReporter.Report(new InstallationState(progressValueOrigin, $"BVE Trainsim {bveVersion} に AtsEX Caller InputDevice を配置しています..."));

                    string bveDirectory = Path.GetDirectoryName(bvePath);
                    string inputDevicesDirectory = Path.Combine(bveDirectory, "Input Devices");
                    string inputDevicesAtsExDirectory = Path.Combine(inputDevicesDirectory, "AtsEx");

                    callerPackage.Locate(Path.Combine(inputDevicesDirectory, CallerInfo.FileName));

                    Task.Delay(DelayMilliseconds / 2).Wait();

                    if (Directory.Exists(inputDevicesAtsExDirectory))
                    {
                        DirectoryInfo directoryInfo = new DirectoryInfo(inputDevicesAtsExDirectory);
                        if (!directoryInfo.Attributes.HasFlag(FileAttributes.ReparsePoint))
                        {
                            StateReporter.Report(new InstallationState(progressValueOrigin + 10, $"BVE Trainsim {bveVersion} に配置されている既存の AtsEX のバックアップを作成しています..."));

                            ZipFile.CreateFromDirectory(inputDevicesAtsExDirectory, FileNamer.CreateFilePathInSequence(inputDevicesAtsExDirectory + "_old.zip"));
                            Directory.Delete(inputDevicesAtsExDirectory, true);
                        }
                    }

                    StateReporter.Report(new InstallationState(progressValueOrigin + 20, $"BVE Trainsim {bveVersion} に AtsEX 本体へのシンボリックリンクを作成しています..."));

                    Kernel32.CreateSymbolicLink(Path.Combine(inputDevicesDirectory, "AtsEx"), atsExDirectory, Kernel32.SymbolicLinkType.SYMBOLIC_LINK_FLAG_DIRECTORY);

                    Task.Delay(DelayMilliseconds / 2).Wait();
                }
            }

            {
                StateReporter.Report(new InstallationState(200, "AtsEX 本体パッケージを展開・配置しています..."));

                ArchivedPackage archive = ArchivedPackage.FromResource($"{Namespace}.AtsEx.zip");
                archive.ExtractAndLocate(atsExDirectory);

                Task.Delay(DelayMilliseconds).Wait();
            }

            {
                StateReporter.Report(new InstallationState(300, $"ユーザーの一覧を取得しています..."));

                WinUserCollection users = WinUserCollection.Create();
                for (int i = 0; i < users.Count; i++)
                {
                    ReportState(0, "このユーザーで BVE Trainsim が使用されているか確認しています...");
                    Task.Delay(DelayMilliseconds / 4).Wait();

                    if (!(TargetPath.Bve6Path.Value is null))
                    {
                        ReportState(0.2, "BVE Trainsim 6 の設定ファイルを編集しています...");
                        EditPreferences("BveTs6.Preferences.xml");
                    }

                    if (!(TargetPath.Bve5Path.Value is null))
                    {
                        ReportState(0.6, "BVE Trainsim 5 の設定ファイルを編集しています...");
                        EditPreferences("Preferences.xml");
                    }

                    void ReportState(double progressRate, string detail)
                    {
                        string userDescription = $"ユーザー '{users[i].Name}' ({i + 1} / {users.Count}): ";
                        int progressValue = 310 + (int)(90 * (i + progressRate) / users.Count);
                        StateReporter.Report(new InstallationState(progressValue, userDescription + detail));
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

            if (!(TargetPath.ScenarioDirectory.Value is null))
            {
                string sampleDirectory = Path.Combine(TargetPath.ScenarioDirectory.Value, "AtsEx.Samples");
                if (Directory.Exists(sampleDirectory))
                {
                    StateReporter.Report(new InstallationState(400, "シナリオフォルダに配置されている既存の AtsEX サンプルシナリオのバックアップを作成しています..."));

                    ZipFile.CreateFromDirectory(sampleDirectory, FileNamer.CreateFilePathInSequence(sampleDirectory + "_old.zip"));
                    Directory.Delete(sampleDirectory, true);
                }

                StateReporter.Report(new InstallationState(420, "AtsEX サンプルシナリオを展開・配置しています..."));

                ArchivedPackage archive = ArchivedPackage.FromResource($"{Namespace}.Scenarios.zip");
                archive.ExtractAndLocate(TargetPath.ScenarioDirectory.Value);

                Task.Delay(DelayMilliseconds).Wait();
            }

            StateReporter.Report(new InstallationState(500, "インストール処理を完了させています..."));
            Task.Delay(DelayMilliseconds * 4).Wait();
        }
    }
}
