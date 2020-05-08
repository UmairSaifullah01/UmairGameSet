using System.Linq;
using UnityEngine;

public abstract class UIHandler : MonoBehaviour
{

	public PanelBehaviour[] panels;
	public IPanel           currentPanel, previousPanel;

	public virtual void ShowPanel(string panelName, UMUI.Data data = null)
	{
		foreach (IPanel panel in panels)
		{
			if (panel.Name == panelName)
			{
				panel.Show(data);
				previousPanel = currentPanel;
				currentPanel  = panel;
				
			}
			else
			{
				if (panel.panelObject.activeSelf)
					panel.Hide();
			}
		}
	}

	public virtual void Hide(string panelName)
	{
		foreach (IPanel panel in panels)
		{
			if (panel.Name == panelName && panel.panelObject.activeSelf)
			{
				panel.Hide();
				previousPanel = panel;
			}
		}
	}

	public virtual void HideAll()
	{
		foreach (IPanel panel in panels)
		{
			if (panel.panelObject.activeSelf)
				panel.Hide();
		}
	}

	public PanelBehaviour GetPanel(string panelName)
	{
		return panels.FirstOrDefault(panel => panel.Name == panelName);
	}

	public void CallPreviousPanel(UMUI.Data data)
	{
		var temp = currentPanel;
		currentPanel?.Hide();
		previousPanel?.Show(data);
		currentPanel  = previousPanel;
		previousPanel = temp;
	}

	public virtual void ShowAdditivePanel(string panelName, UMUI.Data data)
	{
		foreach (var panel in panels)
		{
			if (panel.Name == panelName)
			{
				panel.Show(data);
				currentPanel = panel;
			}
		}
	}

	private void OnValidate()
	{
		if (gameObject.name != this.GetType().Name)
			gameObject.name = this.GetType().Name;
	}

}