using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vanara.PInvoke;

namespace BveEx.Setup.Installing
{
    internal class Installer : IDisposable
    {
        private static readonly string CommonDocumentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
        private static readonly string DesktopDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

        private static readonly string Namespace = "BveEx.Setup.Packages";
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
            ShortcutFactory.CreateToDesktop($"BVE Trainsim {bveVersion} with BveEX.lnk", newBvePath);

            Task.Delay(DelayMilliseconds / 2).Wait();

            StateReporter.Report(new InstallationState(90, $"BVE Trainsim {bveVersion} のコピー処理を完了しています..."));

            return newBvePath;
        }

        public void Install()
        {
            {
                StateReporter.Report(new InstallationState(100, "BveEX Caller InputDevice を準備しています..."));

                Package callerPackage = Package.FromResource($"{Namespace}.{CallerInfo.FileName}");

                if (!TargetPath.Bve6Path.Value.HasInstalled)
                {
                    LocateCallerAndLink(TargetPath.Bve6Path.Value.Path, 6, 120);

                    TargetPath.Bve6Path.Value.MarkAsInstalled();
                }

                if (!TargetPath.Bve5Path.Value.HasInstalled)
                {
                    LocateCallerAndLink(TargetPath.Bve5Path.Value.Path, 5, 160);

                    string bve5Path = TargetPath.Bve5Path.Value.Path;
                    string bve5FileName = Path.GetFileName(bve5Path);
                    StateReporter.Report(new InstallationState(190, $"Bve Trainsim 5 の {bve5FileName}.config を編集しています..."));

                    Package configPackage = Package.FromResource($"{Namespace}.Bve5Config.xml");
                    configPackage.Locate($"{bve5Path}.config");

                    Task.Delay(DelayMilliseconds).Wait();
                    TargetPath.Bve5Path.Value.MarkAsInstalled();
                }

                void LocateCallerAndLink(string bvePath, int bveVersion, int progressValueOrigin)
                {
                    StateReporter.Report(new InstallationState(progressValueOrigin, $"BVE Trainsim {bveVersion} に BveEX Caller InputDevice を配置しています..."));

                    string bveDirectory = Path.GetDirectoryName(bvePath);
                    string inputDevicesDirectory = Path.Combine(bveDirectory, "Input Devices");
                    string inputDevicesBveExDirectory = Path.Combine(inputDevicesDirectory, "BveEx");

                    callerPackage.Locate(Path.Combine(inputDevicesDirectory, CallerInfo.FileName));

                    Task.Delay(DelayMilliseconds / 2).Wait();

                    if (Directory.Exists(inputDevicesBveExDirectory))
                    {
                        DirectoryInfo directoryInfo = new DirectoryInfo(inputDevicesBveExDirectory);
                        if (!directoryInfo.Attributes.HasFlag(FileAttributes.ReparsePoint))
                        {
                            StateReporter.Report(new InstallationState(progressValueOrigin + 10, $"BVE Trainsim {bveVersion} に配置されている既存の BveEX のバックアップを作成しています..."));

                            ZipFile.CreateFromDirectory(inputDevicesBveExDirectory, FileNamer.CreateFilePathInSequence(inputDevicesBveExDirectory + "_old.zip"));
                            Directory.Delete(inputDevicesBveExDirectory, true);
                        }
                    }

                    StateReporter.Report(new InstallationState(progressValueOrigin + 20, "BveEX Caller InputDevice に BveEX 本体の位置を指定しています..."));

                    using (StreamWriter sw = new StreamWriter(Path.Combine(inputDevicesDirectory, "BveEx.Caller.InputDevice.txt"), false))
                    {
                        sw.Write(ApplicationInfo.BveExDirectory);
                        sw.Close();
                    }

                    Task.Delay(DelayMilliseconds / 2).Wait();
                }
            }

            {
                StateReporter.Report(new InstallationState(200, "BveEX 本体パッケージを展開・配置しています..."));

                ArchivedPackage archive = ArchivedPackage.FromResource($"{Namespace}.BveEx.zip");
                archive.ExtractAndLocate(ApplicationInfo.BveExDirectory);

                Task.Delay(DelayMilliseconds).Wait();
            }

            if (TargetPath.InstallSdk.Value)
            {
                StateReporter.Report(new InstallationState(270, "BveEX SDK を展開・配置しています..."));

                string sdkDirectory = Path.Combine(DesktopDirectory, "BveEX SDK");
                string destPath = FileNamer.CreateDirectoryNameInSequence(sdkDirectory);

                ArchivedPackage archive = ArchivedPackage.FromResource($"{Namespace}.BveEx.Sdk.zip");
                archive.ExtractAndLocate(destPath);

                Task.Delay(DelayMilliseconds).Wait();
            }

            {
                StateReporter.Report(new InstallationState(300, $"ユーザーの一覧を取得しています..."));

                WinUserCollection users = WinUserCollection.Create();
                for (int i = 0; i < users.Count; i++)
                {
                    ReportState(0, "このユーザーで BVE Trainsim が使用されているか確認しています...");
                    Task.Delay(DelayMilliseconds / 4).Wait();

                    if (!TargetPath.Bve6Path.Value.HasInstalled)
                    {
                        ReportState(0.2, "BVE Trainsim 6 の設定ファイルを編集しています...");
                        EditPreferences("BveTs6.Preferences.xml");
                    }

                    if (!TargetPath.Bve5Path.Value.HasInstalled)
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

                        preferenceEditor.AddInputDevice("bveex.caller.inputdevice");
                        preferenceEditor.Save();

                        Task.Delay(DelayMilliseconds).Wait();
                    }
                }
            }

            if (!TargetPath.ScenarioDirectory.Value.HasInstalled)
            {
                string sampleDirectory = Path.Combine(TargetPath.ScenarioDirectory.Value.Path, "BveEx.Samples");
                if (Directory.Exists(sampleDirectory))
                {
                    StateReporter.Report(new InstallationState(400, "シナリオフォルダに配置されている既存の BveEX サンプルシナリオのバックアップを作成しています..."));

                    ZipFile.CreateFromDirectory(sampleDirectory, FileNamer.CreateFilePathInSequence(sampleDirectory + "_old.zip"));
                    Directory.Delete(sampleDirectory, true);
                }

                StateReporter.Report(new InstallationState(420, "BveEX サンプルシナリオを展開・配置しています..."));

                ArchivedPackage archive = ArchivedPackage.FromResource($"{Namespace}.Scenarios.zip");
                archive.ExtractAndLocate(TargetPath.ScenarioDirectory.Value.Path);

                Task.Delay(DelayMilliseconds).Wait();
                TargetPath.ScenarioDirectory.Value.MarkAsInstalled();
            }

            {
                StateReporter.Report(new InstallationState(470, "パッケージの配置先を記録しています..."));

                Data.InstallationSettings settings = new Data.InstallationSettings()
                {
                    Bve6Path = TargetPath.Bve6Path.Value.Path,
                    Bve5Path = TargetPath.Bve5Path.Value.Path,
                    ScenarioDirectory = TargetPath.ScenarioDirectory.Value.Path,
                };

                try
                {
                    settings.Save();
                }
                catch { }

                Task.Delay(DelayMilliseconds / 2).Wait();
            }

            StateReporter.Report(new InstallationState(500, "インストール処理を完了させています..."));
            Task.Delay(DelayMilliseconds * 4).Wait();
        }
    }
}
