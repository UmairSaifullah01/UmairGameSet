using System;
using UMGS;
using UMUINew;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public class PerfactMeter : ViewBase, IMeter
{

	[SerializeField] Image fillBar;
	[SerializeField] float m_speed = 1;
	public float value
	{
		get => m_value;
		set => m_value = value;
	}
	public float speed
	{
		get => m_speed;
		set => m_speed = value;
	}
	public AnimationCurve curve;
	float                 maxTime = 0;
	float                 time;
	float                 m_value = 0;
	float sliderValue
	{
		set =>
			// if (!_slider) _slider = GetComponent<Slider>();
			fillBar.fillAmount = value;
	}

	// void SetRangeView(float lower, float higher)
	// {
	// 	// positiveRangeImage.anchorMin        = new Vector2(lower,  0);
	// 	// positiveRangeImage.anchorMax        = new Vector2(higher, 1);
	// 	// positiveRangeImage.anchoredPosition = Vector2.zero;
	// }

	public override void Init(IViewModel viewModel)
	{
		base.Init(viewModel);
		gameObject.SetActive(false);
	}


	void Update()
	{
		sliderValue = Evaluate(speed * Time.deltaTime);
	}


	public void InitMeter(float speed, float lowerValue, float higherValue)
	{
		maxTime    = curve[curve.length - 1].time;
		this.speed = speed;
		gameObject.SetActive(true);
		// SetRangeView(lowerValue, higherValue);
	}

	public float Evaluate(float deltaTime)
	{
		time += deltaTime;
		if (time >= maxTime) time = 0;
		m_value = curve.Evaluate(time);
		return m_value;
	}

}

