using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class TimedBaseGenericPanel : GenericPanel
{

	public  TextMeshProUGUI CounterText;
	private float           duration;

	public void Show(string title, string message, string cancelButtonText, UnityAction onCancel, bool btn1, UnityAction btn1Event, string btn1Text, float duration)
	{
		Show(title, message, cancelButtonText, onCancel, btn1, btn1Event, btn1Text);
		Buttons[0].onClick.AddListener(StopAllCoroutines);
		Buttons[1].gameObject.SetActive(false);
		this.duration     = duration;
		CounterText.text  = duration.ToString("00");
		CounterText.color = Color.green;
		StopAllCoroutines();
		StartCoroutine(Lerp());
	}

	IEnumerator Lerp()
	{
		for (var i = 0.0f; i < duration; i += Time.deltaTime)
		{
			CounterText.text  = Mathf.RoundToInt(duration - i).ToString("00");
			CounterText.color = Color.Lerp(Color.white, Color.red, Time.deltaTime);
			yield return new WaitForFixedUpdate();
		}

		Buttons[1].onClick.Invoke();
	}

}