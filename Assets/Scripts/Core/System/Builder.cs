using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

#if UNITY_EDITOR
using UnityEditor;

namespace Core
{
    [InitializeOnLoad]
    public static class Builder
    {
        static string unityBuildType = "release"; // "debug" for a Unity Development build (profiler, script debugging)

        public class BuilderParams
        {
            public BuildTargetGroup targetGroup;
            public BuildTarget target;
            public string completionMessage;
            public string suffix;
            public string platformText;
            public bool debugSigning;
            public bool bundle;
            public bool automatic;

            public Action postBuild;

            public BuilderParams()
            {
                postBuild = null;
                suffix = "-Default";
                targetGroup = BuildTargetGroup.Standalone;
                target = BuildTarget.StandaloneWindows64;
                debugSigning = false;
            }
        }


        [MenuItem("Ambition/Build/Build Windows", false, 800)]
        public static void BuildWindows() 
        { 
            var p = new BuilderParams();
            p.targetGroup = BuildTargetGroup.Standalone;
            p.target = BuildTarget.StandaloneWindows64;
            p.completionMessage = "windows build complete";
            p.suffix = "-Win64";
            p.platformText = "Windows";
            Build(p); 
        }

        [MenuItem("Ambition/Build/Build Mac", false, 801)]
        public static void BuildMacOs()
        { 
            var p = new BuilderParams();
            p.targetGroup = BuildTargetGroup.Standalone;
            p.target = BuildTarget.StandaloneOSX;
            p.completionMessage = "mac build complete";
            p.suffix = "-OSX";
            p.platformText = "Mac";
            Build(p); 
        }
    
        private static string GetSceneMatching( string sceneName )
        {
            var scenes = EditorBuildSettings.scenes.Where( s => s.path.Contains(sceneName + ".unity") ).ToArray();
            if (scenes.Count() > 0)
            {
                return scenes[0].path;
            }

            Debug.LogWarningFormat("No match for scene '{0}'", sceneName);
            return string.Empty; 
        }

        private static void EnableScenes( string[] paths )
        {
            var scenes = EditorBuildSettings.scenes;
            foreach (var s in scenes)
            {
                s.enabled = paths.Contains(s.path);
                if (s.enabled)
                {
                    //Debug.Log("Enabling scene: "+s.path);
                }
            }
        
            EditorBuildSettings.scenes = scenes;
        }

        private static string _getScene( string sceneName )
        {
            var scenes = EditorBuildSettings.scenes.Where( s => s.path.Contains(sceneName + ".unity") ).ToArray();
            if (scenes.Count() > 0)
            {
                return scenes[0].path;
            }

            Debug.LogWarningFormat("No match for scene '{0}'", sceneName);
            return string.Empty; 
        }

        // Gather the levels marked active in project settings.
        public static string[] GetScenes()
        {
            // ***IMPORTANT*** If there's a splash scene, list it first
            var scenesToBuild = new List<string>();
            scenesToBuild.Add(_getScene("Game_Ambition"));
            return scenesToBuild.Distinct().Where( s => s != string.Empty ).ToArray();
        }

        private static void UpdateAppVersion()
        {
            var cfg = ConfigurationModel.Config;

            PlayerSettings.bundleVersion = string.Format("{0}.{1:00}", cfg.Version.Major, cfg.Version.Minor );
            PlayerSettings.iOS.buildNumber = cfg.Version.Build.ToString();
            PlayerSettings.Android.bundleVersionCode = cfg.Version.Build;
        }

        public static void ProcessChanges()
        {
            UpdateAppVersion();
        }

        static void Build( BuilderParams parms ) 
        {
            var cfg = ConfigurationModel.Config;
            var startTime = DateTime.Now;
            var scenes = GetScenes();

            ProcessChanges();
            EnableScenes( scenes );

            var title = cfg.Title;
            var buildName = title + parms.suffix;
            var appName = title;

            // Ensure the build folder exists
            var projectFolder = Path.Combine( Path.Combine( UnityEngine.Application.dataPath, ".." ), ".." );
            var buildFolder = Path.Combine( projectFolder, buildName );
            var buildPath = Path.Combine( buildFolder, appName );

            Debug.Log("build folder is " + buildFolder);
            Debug.Log("build path is " + buildPath);

            var mode = cfg.Mode.ToString();

            switch (parms.target)
            {
                case BuildTarget.Android:
                    buildPath = string.Format( "{0}-{1}-{2}{3}.apk", buildPath, cfg.Version, mode, parms.suffix );
                    break;

                case BuildTarget.StandaloneWindows64:
                case BuildTarget.StandaloneWindows:
                    buildPath = buildPath + ".exe";
                    break;

                case BuildTarget.StandaloneOSX:
                    buildPath = buildPath + ".app";
                    break;

                default:
                    break;
            }


            var sceneText = "\n\nScenes:\n• " + string.Join("\n• ",scenes.Select( s => Path.GetFileNameWithoutExtension(s) ).ToArray());

            var additionalText = "";
            if (parms.target == BuildTarget.iOS)
            {
                additionalText = "\n\nClose project in XCode!";
            }

            
            // oh hey this is important info about Android debug signing keys
            // https://productforums.google.com/forum/#!msg/play/xm3h6Crq8lY/IOWg88XhAgAJ

            // see also
            // https://github.com/playgameservices/play-games-plugin-for-unity/issues/1871

            if (parms.target == BuildTarget.Android)
            {
                if (parms.debugSigning)
                {
                    additionalText = "\n\nDebug signing key will be used.";

                    PlayerSettings.Android.keystoreName = "Keychain/debug.keystore";
                    PlayerSettings.Android.keyaliasName = "androiddebugkey";
                    PlayerSettings.Android.keystorePass = "android";
                    PlayerSettings.Android.keyaliasPass = "android";
                    PlayerSettings.keystorePass = "android";
                    PlayerSettings.keyaliasPass = "android";
                }
                else
                {
                    additionalText = "\n\nRelease signing key will be used.";

                    PlayerSettings.Android.keystoreName = "Keychain/release.keystore";
                    PlayerSettings.Android.keyaliasName = "releasekey";
                    PlayerSettings.Android.keystorePass = "xxx";
                    PlayerSettings.Android.keyaliasPass = "xxx";
                    PlayerSettings.keystorePass = "xxx";
                    PlayerSettings.keyaliasPass = "xxx";
                }

                if (parms.bundle)
                {
                    additionalText += "\n\nAn .aab bundle will be built for Google Play distribution.";
                }
                else
                {
                    additionalText += "\n\nAn ARM7 APK will be built for sideloading or UDP distribution.";
                }
            }

            // In automatic mode, skip the confirmation dialog
            if (!parms.automatic)
            {
                if (cfg.mode == ConfigurationMode.Release)
                {
                    var confirm = EditorUtility.DisplayDialog( "Release Build", "Are you ready to make a release build?", "Yes", "No");

                    if (!confirm)
                    {
                        return;
                    }
                }
                var dlgTitle = string.Format( "Build {0} ({1})", title, cfg.Mode );
                var dlgBody = string.Format( "Build {0} {1} for {2} ({3})", title, cfg.Version, parms.platformText, cfg.Mode );
                var proceed = EditorUtility.DisplayDialog( dlgTitle, dlgBody + sceneText + additionalText, "Build", "Cancel");

                if (!proceed)
                {
                    return;
                }
            }

            // Only switch build targets once dev has confirmed the build!
            EditorUserBuildSettings.SwitchActiveBuildTarget( parms.targetGroup, parms.target );

            BuildOptions options;

            switch (unityBuildType.ToLower())
            {
                case "debug":
                    options = BuildOptions.Development | BuildOptions.AllowDebugging | BuildOptions.SymlinkLibraries;
                    break;
                default:
                    options = BuildOptions.SymlinkLibraries;
                    break;
            }

            if (parms.target == BuildTarget.Android)
            {
                // Gradle me
                EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle; 
                //options |= BuildOptions.AcceptExternalModificationsToPlayer;

                if (parms.bundle)
                {
                    // Build ARM64 + ARM32 platforms into an .AAB bundle; requires IL2CPP
                    PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
                    PlayerSettings.SetScriptingBackend( BuildTargetGroup.Android, ScriptingImplementation.IL2CPP );
                    EditorUserBuildSettings.buildAppBundle = true;
                }
                else
                {
                    // Build just an ARM7 APK using mono back end instead of IL2CPP;
                    // much faster for testing
                    PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
                    PlayerSettings.SetScriptingBackend( BuildTargetGroup.Android, ScriptingImplementation.Mono2x );
                    EditorUserBuildSettings.buildAppBundle = false;
                }

                PlayerSettings.Android.renderOutsideSafeArea = true;
            }

            System.IO.Directory.CreateDirectory(buildFolder);

            var report = BuildPipeline.BuildPlayer( scenes, Path.GetFullPath(buildPath), parms.target, options);

            var reportText = report.summary.result.ToString();
            if (!string.IsNullOrEmpty(reportText))
            {
                reportText = ": "+reportText + " ";
            }

            var elapsedTime = DateTime.Now - startTime;
            var minutes = elapsedTime.Minutes;
            var seconds = elapsedTime.Seconds;
            Debug.LogFormat("Build {0} - {1}: process complete at {2} {3}",
                                title,
                                parms.target.ToString(),
                                DateTime.Now.ToString(),
                                reportText );

            Debug.LogFormat("Build elapsed time {0}:{1:00}", minutes, seconds );


    #if UNITY_EDITOR_OSX
            Process say = new Process();
            say.StartInfo.FileName = "say";
            say.StartInfo.Arguments = parms.completionMessage;
            say.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            say.Start();
    #else
            EditorApplication.Beep();
    #endif

            // if there's a post-build action configured, 
            // like starting another build, do it.
            if (parms.postBuild != null)
            {
                parms.postBuild();
            }
        }
    }
}
#endif