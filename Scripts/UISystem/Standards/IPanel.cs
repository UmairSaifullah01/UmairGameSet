using UnityEngine;


namespace UMUI
{


	public interface IPanel
	{

		string     Name        { get; }
		GameObject panelObject { get; }

		void Show(UMUI.Data data);

		void Hide();

	}


}