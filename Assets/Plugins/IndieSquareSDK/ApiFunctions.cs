using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using NBitcoin;

public class ApiFunctions : MonoBehaviour {
	public static ApiFunctions Instance;
	public string apiKey;
	public List<Token> userTokens;
	public delegate void OnBalanceLoaded();
	public event OnBalanceLoaded onBalanceLoaded;
	void Awake()
	{
		if (Instance == null)
			Instance = this;

	}

	void OnDestroy()
	{
		if (Instance == this)
			Instance = null;
	}

	public void getAddressBalance(string address)
	{
		StartCoroutine (getAddressBalanceCo (address));

	}

	public IEnumerator getAddressBalanceCo(string address)
	{

		var url = "https://api.indiesquare.me/v2/addresses/"+ address + "/balances";

		if (apiKey != null) {

			url = "https://api.indiesquare.me/v2/addresses/" + address + "/balances?X-Api-Key=" + apiKey;


		} 

		WWW www = new WWW(url);

		yield return www;
 
		if (www.error != null)
		{


			Debug.Log(www.error);

		}
		else
		{

			userTokens = new List<Token> ();
			JsonData data = JsonMapper.ToObject(www.text);
			foreach (JsonData aToken in data)
			{

				string tokenName = aToken["token"].ToString(); 
				double balance = double.Parse(aToken["balance"].ToString());
				double unconfirmed_balance = double.Parse(aToken ["unconfirmed_balance"].ToString());

				Token token = new Token(tokenName,balance,unconfirmed_balance);

				userTokens.Add (token);
				 


			}

			if (onBalanceLoaded != null) {
				onBalanceLoaded ();
			}
			 

		}
	}

	 
	public double getBalanceForToken(string token){
 
	 
		foreach (Token aToken in userTokens)
		{

			if(token == aToken.name){
				return aToken.balance;


			}


		}
		return 0.0;
	}


		public double getUnconfirmedBalanceForToken(string token){


			foreach (Token aToken in userTokens)
			{

				if(token == aToken.name){
				return aToken.unconfirmed_balance;


				}

			}
		return 0.0;
		}

	public bool userHasToken(string token, bool unconfirmedOk){


		foreach (Token aToken in userTokens)
		{

			if(token == aToken.name){
				if (aToken.balance > 0) {

					return true;
				} else {

					if (unconfirmedOk) {

						if (aToken.unconfirmed_balance > 0) {
							return true;

						} else {

							return false;
						}

					} else {
						return false;
					}

				}
					


			}

		}
		return false;

	}


	public bool checkAddressValid(string address){

		try
		{
			BitcoinPubKeyAddress addrs = new BitcoinPubKeyAddress (address);

			return true;
		}
		catch(System.FormatException e)
		{
			return false;


		}

	}



	public bool verifySignature(string address, string message, string signature){


		if (checkAddressValid(address)) {

			BitcoinPubKeyAddress addrs = new BitcoinPubKeyAddress (address);

			if (addrs.VerifyMessage (message, signature)) {

				return true;

			} else{

				return false;

			}

		}
		return false;

	}


}
