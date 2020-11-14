using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repository : MonoBehaviour, IRepository, ICollection<IElement>
{

	Dictionary<string, IElement>       m_Elements   = new Dictionary<string, IElement>();
	Dictionary<string, List<IElement>> m_Categories = new Dictionary<string, List<IElement>>();
	public int                         Count      => m_Elements.Count;
	public bool                        IsReadOnly => false;
	public IElement this[string key] => m_Elements[key];
	Enumerator<IElement> enumerator;

	void Awake()
	{
		Init();
	}

	public void Init()
	{
		var entities = GetComponentsInChildren<IElement>(true);
		foreach (var entity in entities)
		{
			m_Elements.Add(entity.Id, entity);
			if (!m_Categories.ContainsKey(entity.category))
			{
				m_Categories.Add(entity.category, new List<IElement>());
			}

			m_Categories[entity.category].Add(entity);
		}

		enumerator = new Enumerator<IElement>(entities);
	}

	public IElement[] GetCategoryElements(string category)
	{
		return m_Categories[category].ToArray();
	}

	public void ForEachCategory(string category, Action<IElement> task)
	{
		foreach (IElement element in m_Categories[category])
		{
			task.Invoke(element);
		}
	}


	public IEnumerator<IElement> GetEnumerator()
	{
		return enumerator;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public void Add(IElement item)
	{
		m_Elements.Add(item.Id, item);
		if (!m_Categories.ContainsKey(item.category))
		{
			m_Categories.Add(item.category, new List<IElement>());
		}

		m_Categories[item.category].Add(item);
	}

	public void Clear()
	{
		m_Elements.Clear();
	}

	public bool Contains(IElement item)
	{
		return m_Elements.ContainsKey(item.Id);
	}

	public void CopyTo(IElement[] array, int arrayLength)
	{
		array = new IElement[arrayLength];
		int counter = 0;
		foreach (var element in m_Elements)
		{
			array[counter] = element.Value;
			counter++;
			if (counter >= arrayLength)
				return;
		}
	}

	public bool Remove(IElement item)
	{
		return m_Elements.Remove(item.Id) && m_Categories[item.Id].Remove(item);
		//throw new System.NotImplementedException();
	}

}