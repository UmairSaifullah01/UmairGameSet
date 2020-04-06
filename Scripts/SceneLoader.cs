using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{

	public static event Action    OnSceneStartLoadEvent;
	public static event Action    OnSceneEndLoadEvent;
	private static AsyncOperation operation;

	public static AsyncOperation LoadSceneAsync(string name, bool isAdditive = false)
	{
		OnSceneStartLoadEvent?.Invoke();

		void SceneManagerOnsceneLoaded(Scene scene, LoadSceneMode mode)
		{
			OnSceneEndLoadEvent?.Invoke();
			SceneManager.sceneLoaded -= SceneManagerOnsceneLoaded;
		}

		SceneManager.sceneLoaded += SceneManagerOnsceneLoaded;
		operation                =  isAdditive ? SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive) : SceneManager.LoadSceneAsync(name, LoadSceneMode.Single);
		return operation;
	}

	public static AsyncOperation LoadnActiveAdditive(string name, bool isActive = false)
	{
		if (isActive)
		{
			void SetActiveScene()
			{
				SceneLoader.SetActiveScene(name);
				OnSceneEndLoadEvent -= SetActiveScene;
			}

			OnSceneEndLoadEvent += SetActiveScene;
		}

		return LoadSceneAsync(name, true);
	}

	public static void SetActiveScene(string name)
	{
		SceneManager.SetActiveScene(SceneManager.GetSceneByName(name));
	}

	public static void LoadSceneAsync(int activeIndex, params string[] sceneNames)
	{
		LoadMultiScenes(activeIndex, sceneNames);
	}
	public static void LoadSceneAsync(params string[] sceneNames)
	{
		LoadMultiScenes(0, sceneNames);
	}
	static void LoadMultiScenes(int activeIndex, string[] sceneNames)
	{
		OnSceneStartLoadEvent?.Invoke();
		int scenes = 0;
		SceneManager.LoadSceneAsync(sceneNames[scenes]);

		void OnSceneManagerOnsceneLoaded(Scene scene, LoadSceneMode mode)
		{
			scenes++;
			if (scenes >= sceneNames.Length)
			{
				if (activeIndex != 0)
					SetActiveScene(sceneNames[activeIndex]);
				OnSceneEndLoadEvent?.Invoke();
				SceneManager.sceneLoaded -= OnSceneManagerOnsceneLoaded;
				return;
			}

			SceneManager.LoadSceneAsync(sceneNames[scenes], LoadSceneMode.Additive);
		}

		SceneManager.sceneLoaded += OnSceneManagerOnsceneLoaded;
	}

	public static string GetActiveName()
	{
		return SceneManager.GetActiveScene().name;
	}

	public static void ReLoadScene()
	{
		LoadSceneAsync(GetActiveName());
	}

}