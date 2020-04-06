using UMGS;
using UnityEngine.Events;

public class PopupManager : UIHandler
{

	private GenericPanel          genericPanel;
	private GenericPanel          genericPanelSmall;
	private TimedBaseGenericPanel timedBaseGenericPanel;
	public static PopupManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<PopupManager>();
			}

			return instance;
		}
	}

	private static PopupManager instance;

	protected void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
			SceneLoader.OnSceneStartLoadEvent += () => ActivatePanel("Loading");
			SceneLoader.OnSceneEndLoadEvent   += () => Deactivate("Loading");
		}
		else
		{
			Destroy(gameObject);
		}
		DeactivateAll();
	}

	private void OnValidate()
	{
		gameObject.name = $"{this.GetType().ToString()}";
	}

	public void BuyInapp(string value)
	{
		EventHandler.OnBuyINAPP.Send(value);
	}

	public void RemoveAds()
	{
		ActivatePanel("RemoveAds");
	}

	public void UnlockAllGame()
	{
		ActivatePanel("UnlockAllGame");
	}

	public void SoundButton()
	{
		EventHandler.ButtonSound.Send();
	}

	public void UnlockAllGameReward()
	{
		EventHandler.OnAllGameUnlockPurchaseSuccess.Send();
	}

	public void RemoveAdsReward()
	{
		EventHandler.OnRemoveAdsPurchaseSuccess.Send();
	}

	public void DialogueBoxWithYesNo(string title, string message, string cancelButtonText, UnityAction onCancel, bool btn1, UnityAction btn1Event, string btn1Text)
	{
		Show(title, message, cancelButtonText, onCancel, btn1, btn1Event, btn1Text);
	}

	private void Show(string title, string message, string cancelButtonText, UnityAction onCancel, bool btn1, UnityAction btn1Event, string btn1Text, bool smallSize = true)
	{
		DeactivateAll();
		((!smallSize) ? genericPanel : genericPanelSmall).Show(title, message, cancelButtonText, onCancel, btn1, btn1Event, btn1Text);
	}

	public void DialogueBoxWithOkOrCancel(string title, string message, string cancelButtonText, UnityAction onCancel, bool smallSize = true)
	{
		Show(title, message, cancelButtonText, onCancel, false, null, null);
	}

	public void DialogueBox(string message)
	{
		Show("Note", message, "OK", null, false, null, null, true);
	}

	public bool IsCurrencyValid(string name, int value)
	{
		if (CurrencyManager.Instance && CurrencyManager.Instance.GetValue(name) >= value) return true;
		ActivatePanel("NotEnoughCash");
		return false;
	}

	public void DialogueBoxError(string message)
	{
		Show("Note", $"<color=red>{message}</color>", "OK", null, false, null, null, true);
	}

	public void ShowRewardedAd(string title, string message, UnityAction onClosePanel, UnityAction onReward)
	{
		#if !UNITY_EDITOR
		if (EventHandler.IsRewaredAvailable.Send(6))
		{
			EventHandler.RewaredVideoRewardEvent.RemoveAllListener();
			EventHandler.RewaredVideoRewardEvent.AddListener(onReward.Invoke);
			Show(title, message, "Close", onClosePanel, true, () => { EventHandler.ShowRewaredVideo.Send(6); }, "Watch Ad");
		}
		else
		{
			onClosePanel?.Invoke();
			EventHandler.LoadRewaredVideo.Send(6);
		}
		#else
		Show(title, message, "Close", onClosePanel, true, onReward, "Watch Ad");
		#endif
	}

	public void TimedBaseGeneric(string title, string message, UnityAction onClosePanel, UnityAction onReward, float duration)
	{
		#if !UNITY_EDITOR
		if (EventHandler.IsRewaredAvailable.Send(6))
		{
			if (timedBaseGenericPanel != null)
				timedBaseGenericPanel.Show(title, message, "Close", onClosePanel, true, () =>
				{
					EventHandler.ShowRewaredVideo.Send(6);
					this.AfterWait(onReward.Invoke, 2, true);
				}, "Watch Ad", duration);
		}
		else
		{
			onClosePanel?.Invoke();
			EventHandler.LoadRewaredVideo.Send(6);
		}
		#else
		if (timedBaseGenericPanel != null)
			timedBaseGenericPanel.Show(title, message, "Close", onClosePanel, true, onReward, "Watch Ad", duration);
		#endif
	}

	private void Start()
	{
		foreach (PanelBehaviour panel in panels)
		{
			switch (panel.Name)
			{
				case "GenericPanel":
					genericPanel = (GenericPanel) panel;
					break;

				case "GenericPanelSmall":
					genericPanelSmall = (GenericPanel) panel;
					break;

				case "TimedBaseGenericPanel":
					timedBaseGenericPanel = (TimedBaseGenericPanel) panel;
					break;
			}
		}
	}

}