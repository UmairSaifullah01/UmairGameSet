using System;
using System.Collections;
using System.Collections.Generic;

public class Enumerator<T> : IEnumerator<T>
{

	readonly T[] elements;
	int          position = -1;

	public Enumerator(T[] list)
	{
		elements = list;
	}

	public bool MoveNext()
	{
		position++;
		return (position < elements.Length);
	}

	public void Reset()
	{
		position = -1;
	}

	public T Current
	{
		get
		{
			try
			{
				return elements[position];
			}
			catch (IndexOutOfRangeException)
			{
				throw new InvalidOperationException();
			}
		}
	}

	object IEnumerator.Current => Current;

	void IDisposable.Dispose()
	{
	}

}