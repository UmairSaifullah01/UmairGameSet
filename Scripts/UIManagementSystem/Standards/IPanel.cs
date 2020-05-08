using UnityEngine;

public interface IPanel
{

	string     Name        { get; }
	GameObject panelObject { get; }

	void Show(UMUI.Data data);

	void Hide();

}