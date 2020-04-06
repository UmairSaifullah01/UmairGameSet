using System.Linq;
using UnityEngine;

public abstract class UIHandler : MonoBehaviour
{

	public PanelBehaviour[] panels;
	public IPanel           currentPanel, previousPanel;

	public virtual void ActivatePanel(string panelName)
	{
		foreach (IPanel panel in panels)
		{
			if (panel.Name == panelName)
			{
				panel.Activate();
				previousPanel = currentPanel;
				currentPanel  = panel;
			}
			else
			{
				if (panel.panelObject.activeSelf)
					panel.Deactivate();
			}
		}
	}

	public virtual void Deactivate(string panelName)
	{
		foreach (IPanel panel in panels)
		{
			if (panel.Name == panelName && panel.panelObject.activeSelf)
			{
				panel.Deactivate();
				previousPanel = panel;
			}
		}
	}

	public virtual void DeactivateAll()
	{
		foreach (IPanel panel in panels)
		{
			if (panel.panelObject.activeSelf)
				panel.Deactivate();
		}
	}

	public PanelBehaviour GetPanel(string panelName)
	{
		return panels.FirstOrDefault(panel => panel.Name == panelName);
	}

	public void CallPreviousPanel()
	{
		var temp = currentPanel;
		currentPanel?.Deactivate();
		previousPanel?.Activate();
		currentPanel  = previousPanel;
		previousPanel = temp;
	}

	public virtual void ActivatePanelAdditive(string panelName)
	{
		foreach (var panel in panels)
		{
			if (panel.Name == panelName)
			{
				panel.Activate();
				currentPanel = panel;
			}
		}
	}

}