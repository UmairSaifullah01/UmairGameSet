using System;
using System.ComponentModel;
using UnityEngine;
namespace UMGS
{

    public class VCHandler
    {
        #region Static Fields
        static VCHandler instance;
        public static VCHandler Instance
        {
            get
            {
                if (instance == null)
                    instance = new VCHandler ();
                return instance;
            }
        }

        [RuntimeInitializeOnLoadMethod]
        static void Initialize ()
        {
            Debug.Log ("Initiliazing");
            if (Instance == null)
            {
                instance = new VCHandler ();
            }
        }
        #endregion

        VirtualCurrency[] virtualCurrencies;


        // Constructor .....
        public VCHandler ()
        {
            //Get all Currencies values if saved
            var v = Enum.GetNames (typeof (Currency));
            virtualCurrencies = new VirtualCurrency[v.Length];
            for (int i = 0; i < v.Length; i++)
            {
                if (Enum.TryParse (v[i], out Currency currency))
                {
                    virtualCurrencies[i] = new VirtualCurrency () { Name = currency, value = 0 };
                }
            }
        }
        
        public void OnValueChangeRegister (Currency currencyName, PropertyChangedEventHandler changeEvent)
        {

            for (int i = 0; i < virtualCurrencies.Length; i++)
            {
                if (virtualCurrencies[i].Name == currencyName)
                {
                    virtualCurrencies[i].PropertyChanged += changeEvent;
                    changeEvent.Invoke (virtualCurrencies[i], null);
                    return;
                }
            }

        }
        public void OnValueChangeUnregister (Currency currencyName, PropertyChangedEventHandler changeEvent)
        {
            for (int i = 0; i < virtualCurrencies.Length; i++)
            {
                if (virtualCurrencies[i].Name == currencyName)
                {
                    virtualCurrencies[i].PropertyChanged -= changeEvent;
                    return;
                }
            }
        }
        

        public float GetValue (Currency currencyName)
        {
            for (int i = 0; i < virtualCurrencies.Length; i++)
            {
                if (virtualCurrencies[i].Name == currencyName)
                {
                    return virtualCurrencies[i].value;
                }
            }
            return 0.0f;
        }

        public float AddValue (Currency currencyName, float value)
        {
            for (int i = 0; i < virtualCurrencies.Length; i++)
            {
                if (virtualCurrencies[i].Name == currencyName)
                {
                    virtualCurrencies[i].value += value;
                    return virtualCurrencies[i].value;
                }
            }
            return 0.0f;
        }
        public void SetValue (Currency currencyName, float value)
        {
            for (int i = 0; i < virtualCurrencies.Length; i++)
            {
                if (virtualCurrencies[i].Name == currencyName)
                {
                    virtualCurrencies[i].value = value;
                    return;
                }
            }

        }
        public bool Buy (IPurchasable purchasable, Action OnPurchaseSuccess, Action OnPurchaseFailed)
        {

            for (int i = 0; i < virtualCurrencies.Length; i++)
            {
                if (virtualCurrencies[i].Name == purchasable.CurrencyName)
                {
                    if (virtualCurrencies[i].value >= purchasable.Price)
                    {
                        virtualCurrencies[i].value -= purchasable.Price;
                        purchasable.Purchased ();
                        OnPurchaseSuccess?.Invoke ();
                        return true;
                    }
                    else
                    {
                        OnPurchaseFailed?.Invoke ();
                        return false;
                    }
                }
            }
            OnPurchaseFailed?.Invoke ();
            return false;
        }
    }

    public enum Currency
    {
        Coin = ( 1 << 0 ),
        Cash = ( 1 << 1 )
        // Add New Name here like Gems = 1 << 2 
    }
}