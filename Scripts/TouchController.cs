using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace UMGS
{


	public class TouchController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{

		[SerializeField] string output = "";
//Static Fields
		public static float horizontalValue = 0;
		public static float verticalValue   = 0;

		//Events 
		public static event Action<PointerEventData> onTap, onDoubleTap, onTapRelease, onHoldRelease, onHold, onSwipeLeft, onSwipeRight, onSwipeUp, onSwipeDown;
//Serialize Fields

		public  float                  sensitivity   = 3f;
		public  float                  swipeDistance = 10;
		public  float                  holdDelay     = 1f;
		private float                  m_resolutionMultiplier;
		private List<PointerEventData> touchPointers = new List<PointerEventData>();
		//Properties
		public float ResolutionMultiplier
		{
			get
			{
				if (m_resolutionMultiplier <= 0f)
					m_resolutionMultiplier = 100f / (Screen.width + Screen.height);
				return m_resolutionMultiplier;
			}
		}

		//Functions
		public bool IsTouchInput(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
				return false;
			for (int i = Input.touchCount - 1; i >= 0; i--)
			{
				if (Input.GetTouch(i).fingerId == eventData.pointerId)
					return true;
			}

			return false;
		}

		public bool IsValid(PointerEventData eventData)
		{
			for (int i = Input.touchCount - 1; i >= 0; i--)
			{
				if (Input.GetTouch(i).fingerId == eventData.pointerId)
					return true;
			}

			return Input.GetMouseButton((int) eventData.button);
		}

		void OutPut(string value) => output = value;

		//Unity Defined

		void Update()
		{
			horizontalValue = 0;
			verticalValue   = 0;
			if (touchPointers.Count > 0)
			{
				var data  = touchPointers[touchPointers.Count - 1];
				var value = data.delta * (ResolutionMultiplier * sensitivity);
				horizontalValue = value.x;
				verticalValue   = value.y;
			}

			foreach (PointerEventData pointer in touchPointers)
			{
				var click = pointer.clickCount;
				if (click == 2)
				{
					onDoubleTap?.Invoke(pointer);
					OutPut("Double Tap");
				}
				else if (click == 1)
				{
					onTap?.Invoke(pointer);
					OutPut("Tap");
				}

				var pressedTime = Time.unscaledTime - pointer.clickTime;
				if (pressedTime > holdDelay)
				{
					Vector2 deltaPosition = pointer.position - pointer.pressPosition;
					if (deltaPosition.magnitude < swipeDistance)
					{
						onHold?.Invoke(pointer);
						OutPut("Hold");
					}
				}

				pointer.delta = new Vector2(0f, 0f);
			}
		}


		public void OnPointerDown(PointerEventData eventData)
		{
			var pointers = touchPointers;
			for (int i = 0; i < pointers.Count; i++)
			{
				if (pointers[i].pointerId == eventData.pointerId)
				{
					pointers[i] = eventData;
					return;
				}
			}

			pointers.Add(eventData);
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			for (int i = 0; i < touchPointers.Count; i++)
			{
				if (touchPointers[i].pointerId == eventData.pointerId)
				{
					var pressedTime = Time.unscaledTime - touchPointers[i].clickTime;
					if (pressedTime > holdDelay && touchPointers[i].clickCount == 1)
					{
						onHoldRelease?.Invoke(touchPointers[i]);
						OutPut("Hold Release");
					}

					onTapRelease?.Invoke(touchPointers[i]);
					OutPut("Tap Release");
					Vector2 deltaPosition = touchPointers[i].position - touchPointers[i].pressPosition;
					if (deltaPosition.y < deltaPosition.x && deltaPosition.x < 0 && Mathf.Abs(deltaPosition.x) >= swipeDistance)
					{
						onSwipeLeft?.Invoke(touchPointers[i]);
						OutPut("swipe => Left");
					}
					else if (deltaPosition.y < deltaPosition.x && deltaPosition.x > 0 && Mathf.Abs(deltaPosition.x) >= swipeDistance)
					{
						onSwipeRight?.Invoke(touchPointers[i]);
						OutPut("swipe => Right");
					}
					else if (deltaPosition.y > deltaPosition.x && deltaPosition.y > 0 && Mathf.Abs(deltaPosition.y) >= swipeDistance)
					{
						onSwipeUp?.Invoke(touchPointers[i]);
						OutPut("swipe => Up");
					}
					else if (deltaPosition.y > deltaPosition.x && deltaPosition.y < 0 && Mathf.Abs(deltaPosition.y) >= swipeDistance)
					{
						onSwipeDown?.Invoke(touchPointers[i]);
						OutPut("swipe => Down");
					}

					touchPointers.RemoveAt(i);
					break;
				}
			}
		}

		void OnDestroy()
		{
			onTap = onDoubleTap = onTapRelease = onHoldRelease = onHold = onSwipeLeft = onSwipeRight = onSwipeUp = onSwipeDown = null;
		}

	}


}