using System;
using UMDataManagement;
using UMGS;
using UnityEngine;


[System.Serializable]
public class CustomizeItem : IPurchasable
{

	public                   string                 ID;
	[SerializeField] private string                 virtualCurrencyID;
	[SerializeField] private int                    price;
	[SerializeField] private bool                   isPurchased;
	public                   CustomizationInfluence influence;

	public int Price
	{
		get => price;
		set => price = value;
	}


	public bool IsPurchased
	{
		get => isPurchased;
		set => isPurchased = value;
	}

	public string VirtualCurrencyID
	{
		get => virtualCurrencyID;
		set => virtualCurrencyID = value;
	}


	public bool Purchase(Action OnPurchaseSuccess, Action OnPurchaseFailed, bool isFree = false)
	{
		if (isPurchased)
		{
			Debug.Log("Product Already Purchased");
			return true;
		}

		if (CurrencyManager.Instance.GetValue(VirtualCurrencyID) >= Price)
		{
			isPurchased = true;
			CurrencyManager.Instance.AddValue(virtualCurrencyID, -Price);
			DataPersistSystem.Instance.Add(ID, new Data<int>(1));
			OnPurchaseSuccess.Invoke();
			return true;
		}
		else
		{
			OnPurchaseFailed.Invoke();
			return false;
		}
	}

	private void GetData()
	{
		var data = DataPersistSystem.Instance.Get<Data<int>>(ID);
		if (data != null)
		{
			isPurchased = data.value == 1;
		}
	}

}

[System.Serializable]
public class WheelCustomizeItem : CustomizeItem
{

	public Texture wheelTexture;

}

[System.Serializable]
public class ColorCustomizeItem : CustomizeItem
{

	public Color color;

}

[System.Serializable]
public class TextureCustomizeItem : CustomizeItem
{

	public Texture2D texture;

}

[System.Serializable]
public class StickerCustomizeItem : CustomizeItem
{

	public GameObject StickerObject;

}