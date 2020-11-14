using System;
using TMPro;
using UMGS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using EventHandler = UMGS.EventHandler;


namespace UMUINew
{


	
	public class AdvanceButtonBinder : Intractable, IView
	{

		public string     Id        => gameObject.name;
		public IViewModel viewModel { get; set; }

		public virtual void Init(IViewModel viewModel)
		{
			this.viewModel         =  viewModel;
			viewModel.eventBinder  += ClickEvent;
			viewModel.stringBinder += TextUpdater;
		}

		void TextUpdater(string id, string value)
		{
			if (id == Id)
			{
				text.text = value;
			}
		}

		protected virtual void ClickEvent(string id, Action click)
		{
			if (id == $"{Id}")
			{
				onClick.AddListener(click.Invoke);
			}
			else if (id == $"{Id}Up")
			{
				onUp.AddListener(click.Invoke);
			}
			else if (id == $"{Id}Down")
			{
				onDown.AddListener(click.Invoke);
			}
			else if (id == $"{Id}Hold")
			{
				onHold.AddListener(click.Invoke);
			}
		}

		public Transform GetTransform() => transform;

		public void Active(bool value) => gameObject.SetActive(value);

	}


}

[RequireComponent(typeof(UnityEngine.UI.Image))]
public  class Intractable : UIBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler, IPointerClickHandler
{

	[Serializable]
	public class ButtonClickedEvent : UnityEvent
	{

	}

	[SerializeField]                                                               protected TextMeshProUGUI    text;
	[SerializeField]                                                               protected ButtonClickedEvent onClick = new ButtonClickedEvent();
	[SerializeField]                                                               protected ButtonClickedEvent onUp    = new ButtonClickedEvent();
	[SerializeField]                                                               protected ButtonClickedEvent onDown  = new ButtonClickedEvent();
	[SerializeField] [Tooltip("I will update every frame when button is pressed")] protected ButtonClickedEvent onHold  = new ButtonClickedEvent();

	protected bool m_pointerInside  = false;
	protected bool m_pointerPressed = false;

	// protected override void OnValidate()
	// {
	// 	base.OnValidate();
	// 	if (!text)
	// 	{
	// 		text = GetComponentInChildren<TextMeshProUGUI>();
	// 		if (text) text.text = Id;
	// 	}
	// }

	protected override void Start()
	{
		if (!text)
		{
			if (string.IsNullOrEmpty(text.text))
				text = GetComponentInChildren<TextMeshProUGUI>();
		}
	}

	void Update()
	{
		if (m_pointerPressed)
		{
			onHold?.Invoke();
		}
	}


	public virtual void OnPointerEnter(PointerEventData eventData)
	{
		if (eventData.button != PointerEventData.InputButton.Left)
			return;
		m_pointerInside = true;
		if (m_pointerPressed)
			Press();
	}

	public virtual void OnPointerExit(PointerEventData eventData)
	{
		if (eventData.button != PointerEventData.InputButton.Left)
			return;
		m_pointerInside = false;
		Unpress();
	}

	public virtual void OnPointerUp(PointerEventData eventData)
	{
		if (eventData.button != PointerEventData.InputButton.Left)
			return;
		m_pointerPressed = false;
		Unpress();
		if (m_pointerInside)
			onUp?.Invoke();
	}

	public virtual void OnPointerDown(PointerEventData eventData)
	{
		if (eventData.button != PointerEventData.InputButton.Left)
			return;
		onDown?.Invoke();
		m_pointerPressed = true;
		if (m_pointerPressed) Press();
	}

	public virtual void OnPointerClick(PointerEventData eventData)
	{
		if (m_pointerInside)
		{
			onClick.Invoke();
			
		}
	}

	protected virtual void Press()
	{
	}

	protected virtual void Unpress()
	{
	}

}