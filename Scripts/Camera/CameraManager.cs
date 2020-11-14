using System.Collections.Generic;
using System.Linq;
using UMGS;
using UnityEngine;

public class CameraManager : SingletonInternal<CameraManager>
{

	[SerializeField] string                            entryCameraName;
	private          Dictionary<string, ICameraBinder> cameras = new Dictionary<string, ICameraBinder>();
	ICameraBinder                                      currentCamera;
	ICameraBinder                                      previousCamera;

	protected override void Awake()
	{
		base.Awake();
		var cams = GetComponentsInChildren<ICameraBinder>(true);
		foreach (ICameraBinder cam in cams)
		{
			cameras.Add(cam.Id, cam);
			cam.Init();
		}

		ActiveCamera(entryCameraName);
	}


	public static void DeactivateAll()
	{
		foreach (var instanceCamera in instance.cameras)
		{
			instanceCamera.Value?.Deactivate();
		}
	}
	

	public static void ActiveCamera(string cameraName)
	{
		ICameraBinder instanceCamera = instance.cameras[cameraName];
		if (instanceCamera == null) return;
		if (instance.currentCamera != null)
		{
			instance.previousCamera = instance.currentCamera;
			instance.previousCamera.Deactivate();
		}

		instance.currentCamera = instanceCamera;
		instanceCamera.Activate();
	}

	public static ICameraBinder GetActiveCamera()
	{
		return instance.currentCamera;
	}

	public static void ActivePreviousCamera()
	{
		if (instance.previousCamera != null)
		{
			var currentCamera = instance.currentCamera;
			currentCamera.Deactivate();
			instance.previousCamera.Activate();
			instance.currentCamera  = instance.previousCamera;
			instance.previousCamera = currentCamera;
		}
	}

	public static void DeactivateCamera(string cameraName, float delay = 0.0f)
	{
		ICameraBinder desireCamera = instance.cameras[cameraName];
		if (desireCamera == null) return;
		CoroutineHandler.AfterWait(desireCamera.Deactivate, delay);
	}

}