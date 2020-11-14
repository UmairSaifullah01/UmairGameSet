using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


namespace UAnimationSystem
{


	public class OnLocalPathAnimation : UAnimation
	{

		[Header("Path Options")] [SerializeField] bool          closePath = false;
		[SerializeField]                  bool          lookPath  = false;
		[SerializeField]                  PathMode      pathMode  = PathMode.Full3D;
		[SerializeField]                  PathType      pathType  = PathType.Linear;
		[SerializeField]                  List<Vector3> path      = new List<Vector3>();

		protected override Tweener CreateAnimation()
		{
			// for make first position equal to fist node
			transform.localPosition = path[0];
			path.RemoveAt(0);
			var t = transform.DOLocalPath(path.ToArray(), duration, pathType, pathMode).SetOptions(closePath);
			if (lookPath)
				t.SetLookAt(0f);
			return t;
		}

		[ContextMenu("SavePoint #p")]
		public void AddPoint()
		{
			path.Add(transform.position);
			if (path.Count > 0)
			{
				transform.position = path[0];
			}
		}

	}


}