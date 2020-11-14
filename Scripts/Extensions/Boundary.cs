using UnityEngine;


namespace UMGS
{


	[System.Serializable]
	public class Boundary
	{

	}


	public class Range
	{

		public float min;
		public float max;

		public Range(float min, float max)
		{
			this.min = min;
			this.max = max;
		}

		public virtual float Clamp(float value)
		{
			return Mathf.Clamp(value, min, max);
		}

	}

	public class RangeBy : Range
	{

		public float from;

		public RangeBy(float from, float min, float max) : base(min, max)
		{
			this.@from = from;
		}

		public override float Clamp(float value)
		{
			return Mathf.Clamp(value, @from - min, @from + max);
		}

	}

	public class RectangleBoundary : Boundary
	{

		float width;
		float height;
		float centerX, centerY;

		public RectangleBoundary(float centerX, float centerY, float width, float height)
		{
			this.centerX = centerX;
			this.centerY = centerY;
			this.width   = width;
			this.height  = height;
		}

		public void Clamp(ref float x, ref float y)
		{
			x = Mathf.Clamp(x, centerX - width  / 2, centerX + width  / 2);
			y = Mathf.Clamp(x, centerY - height / 2, centerY + height / 2);
		}

	}


	public class CubicBoundary : Boundary
	{

		float width;
		float height;
		float length;
		float centerX, centerY, centerZ;

		public CubicBoundary(float centerX, float centerY, float centerZ, float width, float height, float length)
		{
			this.centerX = centerX;
			this.centerY = centerY;
			this.centerZ = centerZ;
			this.width   = width;
			this.height  = height;
			this.length  = length;
		}

		public void Clamp(ref float x, ref float y, ref float z)
		{
			x = Mathf.Clamp(x, centerX - width  / 2, centerX + width  / 2);
			y = Mathf.Clamp(x, centerY - height / 2, centerY + height / 2);
			z = Mathf.Clamp(z, centerZ - length / 2, centerZ + length / 2);
		}

	}

	public class SphereBoundary : Boundary
	{

		float radius;
		float centerX, centerY, centerZ;

		public SphereBoundary(float centerX, float centerY, float centerZ, float radius)
		{
			this.centerX = centerX;
			this.centerY = centerY;
			this.centerZ = centerZ;
			this.radius  = radius;
		}

		public void Clamp(ref float x, ref float y, ref float z)
		{
			x = Mathf.Clamp(x, centerX - radius, centerX + radius);
			y = Mathf.Clamp(x, centerY - radius, centerY + radius);
			z = Mathf.Clamp(z, centerZ - radius, centerZ + radius);
		}

	}

	public class CircleBoundary : Boundary
	{

		float radius;
		float centerX, centerY;

		public CircleBoundary(float centerX, float centerY, float radius)
		{
			this.centerX = centerX;
			this.centerY = centerY;
			this.radius  = radius;
		}

		public void Clamp(ref float x, ref float y)
		{
			x = Mathf.Clamp(x, centerX - radius, centerX + radius);
			y = Mathf.Clamp(x, centerY - radius, centerY + radius);
		}

	}


}