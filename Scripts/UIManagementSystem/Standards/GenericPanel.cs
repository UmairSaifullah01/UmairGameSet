using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class GenericPanel : PanelBehaviour
{
    [SerializeField] protected Button positivebtn, negativebtn;
    [SerializeField] protected TextMeshProUGUI positiveText, negativeText, titleText, descriptionText;

    public void Show(string title, string message, string cancelButtonText, UnityAction onCancel, bool btn1,
        UnityAction btn1Event, string btn1Text)
    {
        negativebtn.onClick.RemoveAllListeners();
        positivebtn.onClick.RemoveAllListeners();
        negativebtn.gameObject.SetActive(true);
        Activate();
        titleText.text = title;
        descriptionText.text = message;
        negativeText.text = cancelButtonText;
        negativebtn.onClick.AddListener(Deactivate);
        if (onCancel != null)
        {
            negativebtn.onClick.AddListener(EventHandler.ButtonSound.Send);
            negativebtn.onClick.AddListener(onCancel);
        }

        if (btn1)
        {
            positivebtn.gameObject.SetActive(true);
            if (btn1Event != null)
            {
                positivebtn.onClick.AddListener(EventHandler.ButtonSound.Send);
                positivebtn.onClick.AddListener(Deactivate);
                positivebtn.onClick.AddListener(btn1Event);
            }

            positiveText.text = btn1Text;
        }
    }

    public override void Deactivate()
    {
        base.Deactivate();
        positivebtn.gameObject.SetActive(false);
        negativebtn.gameObject.SetActive(false);
    }
}