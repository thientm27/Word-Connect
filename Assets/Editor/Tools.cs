using System.Collections.Generic;

using UnityEditor;
using UnityEditor.Animations;

using UnityEngine;

namespace Editor
{
	public static class Tools
	{
		[MenuItem("Tools/OneChain/Clear Data", false, 1)]
		private static void ClearData()
		{
			PlayerPrefs.DeleteAll();

			/*
			var thumbnailsPath = Path.Combine(Application.persistentDataPath, "thumbnails");
			if (Directory.Exists(thumbnailsPath)) Directory.Delete(thumbnailsPath, true);
			*/

			Debug.Log("Clear data Done!");
		}

		[MenuItem("Tools/OneChain/Reveal in Finder", false, 2)]
		private static void RevealInFinder() => EditorUtility.RevealInFinder(Application.persistentDataPath);

		[MenuItem("Tools/OneChain/Check Events", false, 1)]
		private static void Check() => Checker.Check();

		[MenuItem("Tools/OneChain/Nest Animation in Controller", true)]
		public static bool NestAnimationValidate()
		{
			var animationClipCount = 0;
			var animationControllerCount = 0;
			foreach (var selectionObject in Selection.objects)
			{
				if (selectionObject.GetType() == typeof(AnimationClip))
				{
					++animationClipCount;
				}
				if (selectionObject.GetType() == typeof(AnimatorController))
				{
					++animationControllerCount;
				}
				if (animationControllerCount > 1)
				{
					return false;
				}
			}

			return animationControllerCount == 1 && animationClipCount > 0;
		}

		[MenuItem("Tools/OneChain/Nest Animation in Controller")]
		public static void NestAnimation()
		{
			AnimatorController animatorController = null;
			foreach (var selectionObject in Selection.objects)
			{
				if (selectionObject.GetType() == typeof(AnimatorController))
				{
					animatorController = (AnimatorController)selectionObject;
					break;
				}
			}

			var animationClips = new List<AnimationClip>();
			foreach (var selectionObject in Selection.objects)
			{
				if (selectionObject.GetType() == typeof(AnimationClip))
				{
					animationClips.Add((AnimationClip)selectionObject);
				}
			}

			foreach (var animationClip in animationClips)
			{
				var animationClipClone = Object.Instantiate(animationClip);
				animationClipClone.name = animationClip.name;

				AssetDatabase.AddObjectToAsset(animationClipClone, animatorController);
				AssetDatabase.SaveAssets();
				AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(animatorController));
				AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(animationClip));
			}

			AssetDatabase.SaveAssets();
		}
	}
}
