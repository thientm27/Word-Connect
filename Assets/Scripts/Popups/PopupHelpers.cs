
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Utilities
{
	public static class PopupHelpers
	{

		public static void Show(string name)
		{
			int index = SceneManager.sceneCount;
			var scene = SceneManager.GetActiveScene();
			SetEventSystem(scene, false);
			SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive).completed += delegate(AsyncOperation op)
			{
				SetSceneActive(SceneManager.GetSceneAt(index));
			};
		}

		/// <summary>
		/// New close with special sence
		/// </summary>
		/// <param name="scene"></param>
		public static void Close(Scene scene)
		{
			SetEventSystem(scene, false);
			SceneManager.UnloadSceneAsync(scene).completed += delegate(AsyncOperation operation) { SetSceneActive(); };
		}

		private static void SetSceneActive(Scene scene)
		{
			foreach (var raycaster in Object.FindObjectsOfType<BaseRaycaster>())
			{
				raycaster.enabled = raycaster.gameObject.scene == scene;
			}

			SceneManager.SetActiveScene(scene);
			SetEventSystem(scene, true);
		}

		/// <summary>
		/// auto find top scene to active
		/// </summary>
		private static void SetSceneActive()
		{
			int index = SceneManager.sceneCount;
			var scene = SceneManager.GetSceneAt(index - 1);

			foreach (var raycaster in Object.FindObjectsOfType<BaseRaycaster>())
			{
				raycaster.enabled = raycaster.gameObject.scene == scene;
			}

			if (scene.isLoaded)
			{
				SceneManager.SetActiveScene(scene);
			}

			SetEventSystem(scene, true);
		}

		private static void SetEventSystem(Scene scene, bool isActive)
		{
			var gameObjects = scene.GetRootGameObjects();
			for (int i = 0; i < gameObjects.Length; i++)
			{
				var eventSystem = gameObjects[i].GetComponent<EventSystem>();
				if (eventSystem == null) continue;

				eventSystem.gameObject.SetActive(isActive);
			}
		}
	}
}
