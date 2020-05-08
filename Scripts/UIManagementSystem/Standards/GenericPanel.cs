using TMPro;
using UMGS;
using UnityEngine;
using UnityEngine.Events;


public class GenericPanel : PanelBehaviour
{

	[SerializeField] protected TextMeshProUGUI positiveText, negativeText, titleText, descriptionText;

	public void Show(string title, string message, string cancelButtonText, UnityAction onCancel, bool btn1, UnityAction btn1Event, string btn1Text)
	{
		Buttons[1].onClick.RemoveAllListeners(); // negative
		Buttons[0].onClick.RemoveAllListeners(); //Position
		Buttons[1].gameObject.SetActive(true);
		panelObject.SetActive(true);
		titleText.text       = title;
		descriptionText.text = message;
		negativeText.text    = cancelButtonText;
		Buttons[1].onClick.AddListener(Hide);
		if (onCancel != null)
		{
			Buttons[1].onClick.AddListener(EventHandler.ButtonSound.Send);
			Buttons[1].onClick.AddListener(onCancel);
		}

		if (btn1)
		{
			Buttons[0].gameObject.SetActive(true);
			if (btn1Event != null)
			{
				Buttons[0].onClick.AddListener(EventHandler.ButtonSound.Send);
				Buttons[0].onClick.AddListener(Hide);
				Buttons[0].onClick.AddListener(btn1Event);
			}

			positiveText.text = btn1Text;
		}

		EventHandler.AnalyticsEvent.Send($"{title}_Panel_Opened");
	}

	public override void Hide()
	{
		panelObject.SetActive(false);
		EventHandler.AnalyticsEvent.Send($"{titleText.text}_Panel_Opened");
		Buttons[0].gameObject.SetActive(false);
		Buttons[1].gameObject.SetActive(false);
	}

}