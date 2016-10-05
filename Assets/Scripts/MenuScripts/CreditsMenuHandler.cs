using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using UnityEngine.Purchasing;

public class CreditsMenuHandler : MonoBehaviour, IStoreListener {
	private static IStoreController m_StoreController;
	private static IExtensionProvider m_StoreExtensionProvider;



	public Text watchVideoButtonText, mainMenuText, creditsTitleText, buyGameButtonText, watchVideoInfoText, buyGameInfoText, priceText;
	public Button watchVideoButton;
	private bool videoReady;
	LanguageHandler languageHandler;
	AudioHandler audioHandler;
	// Use this for initialization
	void Start () {
		
		languageHandler = (LanguageHandler)GameObject.FindGameObjectWithTag ("LanguageHandler").GetComponent (typeof(LanguageHandler));
		#if UNITY_ANDROID
		if (m_StoreController == null){
			InitializePurchasing ();
		}
		#endif
		audioHandler = (AudioHandler)GameObject.FindGameObjectWithTag ("MusicPlayer").GetComponent (typeof(AudioHandler));
		audioHandler.PlayMainMenu ();
		/*
		Product product = m_StoreController.products.WithID(kProductIDUnlockFullGame);
		if (product != null && product.hasReceipt)
		{
			// Owned Non Consumables and Subscriptions should always have receipts.
			// So here the Non Consumable product has already been bought.
			PlayerPrefs.SetInt (GloVar.GamePurchasedPrefName, 1);
		}
		else{
			PlayerPrefs.SetInt (GloVar.GamePurchasedPrefName, 0);
		}
		*/
		SetLabels ();
	}
	void SetLabels(){
		languageHandler.SetTextSettings (mainMenuText, "mainmenu", FontSize.Large);
		languageHandler.SetTextSettings (creditsTitleText, "credits", FontSize.Large);
		creditsTitleText.text += ": " + PlayerPrefs.GetInt (GloVar.CreditsPrefName, GloVar.InitialCredits);
		languageHandler.SetTextSettings (buyGameButtonText, "buygame", FontSize.ExtraLarge);
		languageHandler.SetTextSettings (buyGameInfoText, "buygameinfo", FontSize.Medium);
		languageHandler.SetTextSettings (watchVideoInfoText, "watchvideoinfo", FontSize.Medium);
		#if UNITY_ANDROID
		if (IsInitialized () && priceText.text.Equals ("")){
			languageHandler.SetTextSettings (priceText, "", FontSize.Medium);
			priceText.text = m_StoreController.products.WithID(GloVar.kProductIDUnlockFullGame).metadata.localizedPriceString;
		}
		#endif
	}
	void InitializePurchasing(){
		#if UNITY_ANDROID
		if(IsInitialized()){
			return;
		}
		var builder = ConfigurationBuilder.Instance (StandardPurchasingModule.Instance ());
		builder.AddProduct (GloVar.kProductIDUnlockFullGame, ProductType.NonConsumable);
		UnityPurchasing.Initialize (this, builder);
		#endif
	}
	bool IsInitialized(){
		#if UNITY_ANDROID
		return m_StoreController != null && m_StoreExtensionProvider != null;	
		#elif UNITY_IOS
		return false;
		#endif
	}
	public void PurchaseGameClick(){
		//PlayerPrefs.SetInt (GloVar.GamePurchasedPrefName, 1);
		//SceneManager.LoadScene (GloVar.MainMenuSceneName);
		BuyProductID (GloVar.kProductIDUnlockFullGame);
	}
	void BuyProductID(string productId){
		#if UNITY_ANDROID
		if (IsInitialized ()){
			Product product = m_StoreController.products.WithID (productId);

			if (product != null && product.hasReceipt)
			{
				PlayerPrefs.SetInt (GloVar.GamePurchasedPrefName, 1);
				SceneManager.LoadScene (GloVar.MainMenuSceneName);
				//SetLabels ();
			}

			if(product != null && product.availableToPurchase){
				Debug.Log (string.Format ("Purchasing product asynchronously: '{0}'", product.definition.id));
				m_StoreController.InitiatePurchase (product);
			}
			else{
				Debug.Log ("BuyProductID: FAIL. Not found or unavailable.");
			}
		}
		else{
			Debug.Log ("BuyProductID: FAIL. Not initialized.");
		}
		#endif
	}

	public void RestorePurchases(){
		#if UNITY_ANDROID
		if (!IsInitialized ()){
			Debug.Log ("Restore purchases: FAIL. Not initialized.");
			return;
		}
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer){
			Debug.Log ("Restore purchases started");
			var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions> ();
			apple.RestoreTransactions ((result) => {
				Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore");
			});
		}
		else{
			Debug.Log ("No restore necessary (not Apple platform).");
		}
		#endif
	}

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions){
		#if UNITY_ANDROID
		Debug.Log ("OnInitialized: PASS");
		m_StoreController = controller;
		m_StoreExtensionProvider = extensions;
		Product product = controller.products.WithID(GloVar.kProductIDUnlockFullGame);
		if (product != null && product.hasReceipt)
		{
			PlayerPrefs.SetInt (GloVar.GamePurchasedPrefName, 1);
			SceneManager.LoadScene (GloVar.MainMenuSceneName);
		}
		else{
			PlayerPrefs.SetInt (GloVar.GamePurchasedPrefName, 0);
		}
		languageHandler.SetTextSettings (priceText, "", FontSize.Medium);
		priceText.text = product.metadata.localizedPriceString;
		#endif
	}
	public void OnInitializeFailed(InitializationFailureReason error){
		Debug.Log ("OnInitializeFailed Reason: " + error);
	}

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args){
		#if UNITY_ANDROID
		if (String.Equals (args.purchasedProduct.definition.id, GloVar.kProductIDUnlockFullGame, StringComparison.Ordinal)){
			Debug.Log (string.Format ("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
			PlayerPrefs.SetInt (GloVar.GamePurchasedPrefName, 1);
			SceneManager.LoadScene (GloVar.MainMenuSceneName);
		}
		else{
			Debug.Log (string.Format ("ProcessPurchase: FAIL. Unrecognized product: '{0}", args.purchasedProduct.definition.id));
		}
		return PurchaseProcessingResult.Complete;
		#elif UNITY_IOS
		return PurchaseProcessingResult.Complete;
		#endif
	}
	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason){
		#if UNITY_ANDROID
		Debug.Log (string.Format ("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailedReason: '{1}'", product.definition.storeSpecificId, failureReason));
		#endif
	}

	// Update is called once per frame
	void Update () {
		#if UNITY_ANDROID
		if (Input.GetKeyDown(KeyCode.Escape)) { SceneManager.LoadScene(GloVar.MainMenuSceneName);; }
		videoReady = Advertisement.IsReady ();
		WatchButtonManipulation ();
		SetLabels ();
		#endif
	}

	void WatchButtonManipulation(){
		#if UNITY_ANDROID
		if (videoReady){
			languageHandler.SetTextSettings (watchVideoButtonText, "watchvideo", FontSize.Large);
		}
		else{
			languageHandler.SetTextSettings (watchVideoButtonText, "loadingvideo", FontSize.Large); 
		}
		#endif
	}

	public void WatchButtonClicked(){
		#if UNITY_ANDROID
		if (videoReady){
			ShowOptions options = new ShowOptions();
			options.resultCallback = HandleShowResult;
			Advertisement.Show ("rewardedVideo", options);
		}
		#endif
	}

	private void HandleShowResult(ShowResult result){
		switch(result){
		case ShowResult.Finished:
			CreditsHandler.IncreaseByX (GloVar.CreditsPerViewing);
			break;
		case ShowResult.Skipped:
			Debug.Log ("SKIPPU DESU");
			break;
		case ShowResult.Failed:
			Debug.Log ("WHAT IS FUDGE!? FAIRU!?");
			break;
		}
	}
	public void MainMenuClick(){
		SceneManager.LoadScene (GloVar.MainMenuSceneName);
	}
}
