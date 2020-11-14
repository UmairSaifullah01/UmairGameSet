using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


namespace UAnimationSystem
{


	public class OnPathMoveAnimation : UAnimation
	{

		[SerializeField] PathType      pathType = PathType.Linear;
		[SerializeField] List<Vector3> path     = new List<Vector3>();

		protected override Tweener CreateAnimation()
		{
			return transform.DOPath(path.ToArray(), duration, pathType, PathMode.Sidescroller2D);
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