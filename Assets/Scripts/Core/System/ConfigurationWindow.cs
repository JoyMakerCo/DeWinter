using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Core
{
	public class ConfigurationWindow : EditorWindow {
		static string currentBranch;

		[MenuItem("Ambition/Build/Configuration", false, 600)]
		public static void ShowWindow() {
			currentBranch = getCurrentBranch();
			EditorWindow.GetWindow<ConfigurationWindow>();
		}

		[UnityEditor.Callbacks.DidReloadScripts]
		private static void OnScriptsReloaded() {
			currentBranch = getCurrentBranch();
		}

		public static string getCurrentBranch()
		{
			string branch = "unknown";
			string displayBranch = "n/a";
			try
			{
				//Create process
				System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
				pProcess.StartInfo.FileName = "git";
				pProcess.StartInfo.Arguments = "rev-parse --symbolic-full-name --abbrev-ref HEAD";
				pProcess.StartInfo.UseShellExecute = false;
				pProcess.StartInfo.RedirectStandardOutput = true;
				pProcess.StartInfo.UseShellExecute = false;
				pProcess.StartInfo.CreateNoWindow = true;   

				//Start the process
				pProcess.Start();

				//Get program output
				branch = pProcess.StandardOutput.ReadToEnd().Trim();
				displayBranch = string.IsNullOrEmpty(branch) ? "n/a" : branch;

				//Wait for process to finish
				pProcess.WaitForExit();
			}
			catch (Exception e)
			{
				branch = "unknown: " + e.ToString();
				displayBranch = "n/a";
			}

			ConfigurationModel.Config.Branch = displayBranch;
			currentBranch = displayBranch;

			return branch;
		}

		void OnGUI() {
			//	get the current build config
			var buildConfig = ConfigurationModel.Config;
			EditorGUILayout.ObjectField("Build Configuration", buildConfig, typeof(ConfigurationData), false);	
			drawActiveVersion();
		}

		void drawActiveVersion() {
			var buildConfig = ConfigurationModel.Config;

			EditorGUILayout.Separator();
			var labelStyle = new GUIStyle(EditorStyles.label) {
				fontSize = 14,
				fontStyle = FontStyle.Bold
			};
			GUILayout.Label( "Branch: " + currentBranch );
			var currentVersion = string.Format( "Version {0} {1}", buildConfig.Version, buildConfig.Mode );
			GUILayout.Label( currentVersion, labelStyle );

			var width2 = (position.width - 10f) * 0.5f;
			var width3 = (position.width - 12f) * 0.333f;

			GUILayoutOption[] buttonOpts2 = { GUILayout.Width (width2), GUILayout.Height(22f) };
			GUILayoutOption[] buttonOpts3 = { GUILayout.Width (width3), GUILayout.Height(22f) };

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Release Mode", buttonOpts2 ))
			{
				buildConfig.Mode = ConfigurationMode.Release;
				EditorUtility.SetDirty(buildConfig);
				AssetDatabase.SaveAssets();
			} 

			if (GUILayout.Button ("Debug Mode", buttonOpts2 ))
			{
				buildConfig.Mode = ConfigurationMode.Debug;
				EditorUtility.SetDirty(buildConfig);
				AssetDatabase.SaveAssets();
			} 
			GUILayout.EndHorizontal ();
					
			GUILayout.Label( "Version" );

			var newMaj = EditorGUILayout.IntField("Major", buildConfig.Version.Major);
			var newMin = EditorGUILayout.IntField("Minor", buildConfig.Version.Minor);
			var newBuild = EditorGUILayout.IntField("Build", buildConfig.Version.Build);
			if (( newMaj != buildConfig.Version.Major ) || 
				( newMin != buildConfig.Version.Minor ) || 
				( newBuild != buildConfig.Version.Build) )
				{
					buildConfig.Version = new BuildVersionVO( newMaj, newMin, newBuild );
					EditorUtility.SetDirty(buildConfig);
					AssetDatabase.SaveAssets();
				}
			
			GUILayout.BeginHorizontal ();

			var incMinor = string.Format( "Increment minor version to {0}.{1}", buildConfig.Version.Major, buildConfig.Version.Minor+1 );
			if (GUILayout.Button (incMinor, buttonOpts2 )) 
			{
				buildConfig.Version = new BuildVersionVO( buildConfig.Version.Major, buildConfig.Version.Minor+1, buildConfig.Version.Build );
				EditorUtility.SetDirty(buildConfig);
				AssetDatabase.SaveAssets();
			}

			var incBuild = string.Format( "Increment build number to {0}", buildConfig.Version.Build+1 );
			if (GUILayout.Button (incBuild, buttonOpts2 )) 
			{
				buildConfig.Version = new BuildVersionVO( buildConfig.Version.Major, buildConfig.Version.Minor, buildConfig.Version.Build+1 );
				EditorUtility.SetDirty(buildConfig);
				AssetDatabase.SaveAssets();
			}

			GUILayout.EndHorizontal ();
			EditorGUILayout.Separator();

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Build Windows", buttonOpts2 )) 
			{
				Builder.BuildWindows();
			}

			if (GUILayout.Button ("Build MacOS", buttonOpts2 )) 
			{
				Builder.BuildMacOs();
			}

			GUILayout.EndHorizontal ();

		}
	}
}
#endif