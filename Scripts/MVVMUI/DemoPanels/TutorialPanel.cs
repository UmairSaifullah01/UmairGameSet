using UMDataManagement;
using UMGS;
using UMUINew;
using UnityEngine;

public class TutorialPanel : UIPanel
{

	public int max;
	int        m_value = 0;

	public override void Init(IStateMachine stateMachine)
	{
		base.Init(stateMachine);
		EventHandler.TutorialPanel.SetListener(ActiveTutorial);
		DataManager.CanGet("Tutorial", out m_value);
	}

	void ActiveTutorial(int value)
	{
		var active = value > 0;
		if (m_value < max)
		{
			Enter();
		}

		switch (Mathf.Abs(value))
		{
			case 1:
				views["Click"].Active(active);
				if (active)
				{
					m_value++;
				}

				break;

			case 2:
				views["Hold"].Active(active);
				if (active)
				{
					m_value++;
				}

				break;

			case 3:
				views["Hold"].Active(active);
				if (active)
				{
					m_value++;
				}

				break;

			case 4:
				views["Drag"].Active(active);
				if (active)
				{
					m_value++;
				}

				break;

			case 5:
				views["BinacularDrag"].Active(active);
				if (active)
				{
					m_value++;
				}

				break;

			case 6:
				views["BenacularOpen"].Active(active);
				if (active)
				{
					m_value++;
				}

				break;

			default:
				Exit();
				break;
		}
	}

}