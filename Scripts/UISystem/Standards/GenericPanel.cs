using TMPro;
using UMUI;
using UMGS;
using UnityEngine;
using UnityEngine.Events;


namespace UMUI
{


	public class GenericPanel : PanelBehaviour
	{

		[SerializeField] protected TextMeshProUGUI positiveText, negativeText, titleText, descriptionText;

		public override void Show(UMUI.Data data)
		{
			if (data == null) return;
			var d = (GenericPanelData) data;
			panelObject.transform.SetAsLastSibling();
			Buttons[1].onClick.RemoveAllListeners(); // negative
			Buttons[0].onClick.RemoveAllListeners(); //Position
			Buttons[1].gameObject.SetActive(true);
			panelObject.SetActive(true);
			titleText.text       = d.title;
			descriptionText.text = d.message;
			negativeText.text    = d.negativeText;
			Buttons[1].onClick.AddListener(Hide);
			if (d.negativeEvent != null)
			{
				Buttons[1].onClick.AddListener(EventHandler.ButtonSound.Send);
				Buttons[1].onClick.AddListener(d.negativeEvent);
			}

			if (d.isPositiveButton)
			{
				Buttons[0].gameObject.SetActive(true);
				if (d.positiveEvent != null)
				{
					Buttons[0].onClick.AddListener(EventHandler.ButtonSound.Send);
					Buttons[0].onClick.AddListener(Hide);
					Buttons[0].onClick.AddListener(d.positiveEvent);
				}

				positiveText.text = d.positiveText;
			}

			EventHandler.AnalyticsEvent.Send($"{d.title}_Panel_Opened");
		}

		public void Show(string title, string message, string cancelButtonText, UnityAction onCancel, bool btn1, UnityAction btn1Event, string btn1Text)
		{
			panelObject.transform.SetAsLastSibling();
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


	


}