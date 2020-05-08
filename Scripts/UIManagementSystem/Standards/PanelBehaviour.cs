using UMGS;
using UnityEngine;
using UnityEngine.UI;

public class PanelBehaviour : MonoBehaviour, IPanel
{

	[SerializeField] private string     m_name;
	public                   string     Name        => m_name;
	public                   GameObject panelObject => gameObject;
	public                   Button[]   Buttons;
	[HideInInspector] public UIHandler  handler;

	protected virtual void Start()
	{
		for (int i = 0; i < Buttons.Length; i++)
		{
			int cache = i;
			Buttons[cache].onClick.AddListener(() => OnButtonEvent(cache));
		}
	}

	public virtual void Show(UMUI.Data data)
	{
		panelObject.SetActive(true);
		panelObject.transform.SetAsLastSibling();
		EventHandler.AnalyticsEvent.Send($"{Name}_Opened");
	}

	public virtual void Hide()
	{
		EventHandler.AnalyticsEvent.Send($"{Name}_Closed");
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