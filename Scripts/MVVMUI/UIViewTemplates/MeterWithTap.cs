using System;
using UMGS;
using UMUINew;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public class MeterWithTap : ViewBase, IMeter
{

	[SerializeField] Image         startImage, orangeStartImage, orangeEndImage, greenEndImage;
	[SerializeField] RectTransform handle;
	[SerializeField] float         m_speed = 1;

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
		set
		// if (!_slider) _slider = GetComponent<Slider>();
			=>
				handle.localEulerAngles = handle.localEulerAngles.SetVector3Axis(56 - 112 * value, Axis.z);
	}

	void SetRangeView(Range greenRange, Range orangeRange)
	{
		startImage.fillAmount       = orangeRange.min;
		orangeStartImage.fillAmount = greenRange.min;
		greenEndImage.fillAmount    = greenRange.max;
		orangeEndImage.fillAmount   = orangeRange.max;
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
		SetRangeView(new Range(0.4f, 0.6f), new Range(0.3f, 0.7f));
	}

	public float Evaluate(float deltaTime)
	{
		time += deltaTime;
		if (time >= maxTime) time = 0;
		m_value = curve.Evaluate(time);
		return m_value;
	}

}

public interface IMeter
{

	float value { get; set; }
	float speed { get; set; }

	void InitMeter(float speed, float lowerValue, float higherValue);

	float Evaluate(float deltaTime);

}