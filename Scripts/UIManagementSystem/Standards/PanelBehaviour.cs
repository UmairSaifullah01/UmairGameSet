using UMGS;
using UnityEngine;
using UnityEngine.UI;

public class PanelBehaviour : MonoBehaviour, IPanel
{

	[SerializeField] private string     m_name;
	public                   string     Name        => m_name;
	public                   GameObject panelObject => gameObject;
	public                   Button[]   Buttons;

	protected virtual  void Start()
	{
		for (int i = 0; i < Buttons.Length; i++)
		{
			int cache = i;
			Buttons[cache].onClick.AddListener(() => OnButtonEvent(cache));
		}
	}

	public virtual void Activate()
	{
		EventHandler.AnalyticsEvent.Send($"{Name}_Panel_Opened");
		panelObject.SetActive(true);
	}

	public virtual void Deactivate()
	{
		EventHandler.AnalyticsEvent.Send($"{Name}_Panel_Closed");
		panelObject.SetActive(false);
	}

	protected virtual void OnButtonEvent(int i)
	{
		EventHandler.ButtonSound.Send();
	}

	private void OnValidate()
	{
		if (string.IsNullOrEmpty(m_name))
			m_name = gameObject.name;
	}

}