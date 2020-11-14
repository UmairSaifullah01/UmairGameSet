using UMGS;
using UnityEngine;
using UnityEngine.UI;


namespace UMUINew
{


	[RequireComponent(typeof(Image))]
	public class FillBarBinder : ViewBase
	{

		Image graphic;

		public override void Init(IViewModel viewModel)
		{
			base.Init(viewModel);
			graphic               =  GetComponent<Image>();
			graphic               =  GetComponent<Image>();
			viewModel.floatBinder += BarUpdate;
		}

		void BarUpdate(string id, float value)
		{
			if (Id == id)
				graphic.fillAmount = value;
		}

	}


}