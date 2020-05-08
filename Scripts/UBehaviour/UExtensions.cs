using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UMGS
{


	public static class UExtensions
	{

		public static bool ContainsLayer(this LayerMask layermask, int layer)
		{
			return layermask == (layermask | (1 << layer));
		}

		/// <summary>
		/// Check if Transfom is children
		/// </summary>
		/// <param name="me"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		public static bool isChild(this Transform me, Transform target)
		{
			if (!target) return false;
			var objName = target.gameObject.name;
			var obj     = me.FindChildByNameRecursive(objName);
			if (obj == null) return false;
			else return obj.Equals(target);
		}

		public static Transform FindChildByNameRecursive(this Transform me, string name)
		{
			if (me.name == name)
				return me;
			for (int i = 0; i < me.childCount; i++)
			{
				var child = me.GetChild(i);
				var found = child.FindChildByNameRecursive(name);
				if (found != null)
					return found;
			}

			return null;
		}

		public static T[] Append<T>(this T[] arrayInitial, T[] arrayToAppend)
		{
			if (arrayToAppend == null)
			{
				throw new ArgumentNullException("The appended object cannot be null");
			}

			if ((arrayInitial is string) || (arrayToAppend is string))
			{
				throw new ArgumentException("The argument must be an enumerable");
			}

			T[] ret = new T[arrayInitial.Length + arrayToAppend.Length];
			arrayInitial.CopyTo(ret, 0);
			arrayToAppend.CopyTo(ret, arrayInitial.Length);
			return ret;
		}

		public static Vector3 Difference(this Vector3 vector, Vector3 otherVector)
		{
			return otherVector - vector;
		}

		public static void SetActiveChildren(this GameObject gameObjet, bool value)
		{
			foreach (Transform child in gameObjet.transform)
				child.gameObject.SetActive(value);
		}

		public static void SetLayerRecursively(this GameObject obj, int layer)
		{
			obj.layer = layer;
			foreach (Transform child in obj.transform)
			{
				child.gameObject.SetLayerRecursively(layer);
			}
		}

		public static float ClampAngle(float angle, float min, float max)
		{
			do
			{
				if (angle < -360)
					angle += 360;
				if (angle > 360)
					angle -= 360;
			} while (angle < -360 || angle > 360);

			return Mathf.Clamp(angle, min, max);
		}


		public static List<T> DevCopy<T>(this List<T> list)
		{
			List<T> _list = new List<T>();
			if (list == null || list.Count == 0) return list;
			for (int i = 0; i < list.Count; i++)
			{
				_list.Add(list[i]);
			}

			return _list;
		}

		public static List<T> DevToList<T>(this T[] array)
		{
			List<T> list = new List<T>();
			if (array == null || array.Length == 0) return list;
			for (int i = 0; i < array.Length; i++)
			{
				list.Add(array[i]);
			}

			return list;
		}

		public static T[] DevToArray<T>(this List<T> list)
		{
			T[] array = new T[list.Count];
			if (list == null || list.Count == 0) return array;
			for (int i = 0; i < list.Count; i++)
			{
				array[i] = list[i];
			}

			return array;
		}

		public static void ResetTransformation(this Transform trans)
		{
			trans.position   = Vector3.zero;
			trans.rotation   = Quaternion.identity;
			trans.localScale = new Vector3(1, 1, 1);
		}

		public static void ResetLocalTransformation(this Transform trans)
		{
			trans.localPosition = Vector3.zero;
			trans.localRotation = Quaternion.identity;
			trans.localScale    = new Vector3(1, 1, 1);
		}

		public static void ResetTransformPosition(this Transform trans)
		{
			trans.position = Vector3.zero;
		}

		public static void ResetTransformScale(this Transform trans)
		{
			trans.localScale = new Vector3(1, 1, 1);
		}

		public static void ResetLocalScaleAndPosition(this Transform trans)
		{
			trans.localPosition = Vector3.zero;
			trans.localScale    = new Vector3(1, 1, 1);
		}

		public static void DestroyAllChilds(this Transform transformObject)
		{
			foreach (Transform child in transformObject)
			{
				GameObject.Destroy(child.gameObject);
			}
		}

		public static bool NotNull<T>(this T trans)
		{
			if (trans != null)
				return true;
			return false;
		}

		public static Vector3 BoxSize(this BoxCollider boxCollider)
		{
			var length = boxCollider.transform.lossyScale.x * boxCollider.size.x;
			var width  = boxCollider.transform.lossyScale.z * boxCollider.size.z;
			var height = boxCollider.transform.lossyScale.y * boxCollider.size.y;
			return new Vector3(length, height, width);
		}

		public static bool Contains(this Enum keys, Enum flag)
		{
			if (keys.GetType() != flag.GetType())
				throw new ArgumentException("Type Mismatch");
			return (Convert.ToUInt64(keys) & Convert.ToUInt64(flag)) != 0;
		}

	}

	public struct BoxPoint
	{

		public Vector3 top;
		public Vector3 center;
		public Vector3 bottom;

	}

	[Flags]
	public enum HitBarPoints
	{

		None   = 0,
		Top    = 1,
		Center = 2,
		Bottom = 4

	}


}