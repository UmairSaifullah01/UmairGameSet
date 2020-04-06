using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

namespace UMGS
{
    public abstract class CurrencyBinding : MonoBehaviour
    {
        [SerializeField] protected Currency currencyName;
        private void OnDisable()
        {
            if (VCHandler.Instance != null)
                VCHandler.Instance.OnValueChangeUnregister (currencyName, ChangeEffect);

        }

        protected abstract void ChangeEffect(object sender,PropertyChangedEventArgs args);
        
        private void OnEnable()
        {
            if (VCHandler.Instance != null)
                VCHandler.Instance.OnValueChangeUnregister (currencyName, ChangeEffect);

        }
        
    }
}
