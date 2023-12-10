using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace PatataStudio
{
	public static class ToolsMenu
	{
		[MenuItem("Tools/Setup/Create Default Folders")]
		public static void CreateDeafaultFolders() 
		{
			CreateDirectiories("Project", "Scripts", "Materials", "Scenes", "Textures", "Settings", "UI", "Prefabs");
			AssetDatabase.Refresh();
		}

		public static void CreateDirectiories(string root, params string[] folders)
		{
			var _fullPath = Path.Combine(Application.dataPath, root);

			foreach (var newFolder in folders)
			{
				Directory.CreateDirectory(Path.Combine(_fullPath, newFolder));
			}
		}
	}
}