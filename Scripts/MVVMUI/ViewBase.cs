using System.Collections;
using System.Collections.Generic;
using UMGS;
using UnityEngine;


namespace UMUINew
{


	public class ViewBase : MonoBehaviour, IView
	{

		public string     Id        => gameObject.name;
		public IViewModel viewModel { get; set; }

		public virtual void Init(IViewModel viewModel)
		{
			this.viewModel = viewModel;
		}

		public virtual void Active(bool value) => gameObject.SetActive(value);

		public Transform GetTransform() => transform;

	}


}