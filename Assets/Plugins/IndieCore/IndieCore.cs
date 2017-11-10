using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using NBitcoin;
using ZXing;
using ZXing.QrCode;

public class IndieCoreError {
	public string message;

	public IndieCoreError(string message) {
		this.message = message;
		 
	}

}

public class IndieCoreResult {
	public IndieCoreError error;
	public string method;
	public string result;
	public Texture2D qrcode;



}

public class IndieCore : MonoBehaviour {
	string baseurl = "https://indiesquare.me";
	string baseApiUrl = "https://api.indiesquare.me/v2/";
	public static IndieCore Instance;
	public string urlscheme;
	public string apiKey; 
	public bool testnet;
	public delegate void IndieCoreCallback(IndieCoreResult result);
	public event IndieCoreCallback indieCoreCallback;
	Dictionary<string, string> currentParams;
	string currentChannel;
	delegate void HttpCallback(bool error,string result);
	event HttpCallback pubSubCallback;

	event HttpCallback postAsyncCallback;

	string currentMethod = "";
	// Use this for initialization
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
	void Start () {

		pubSubCallback += HandlePubSubCallback;
		postAsyncCallback += HandleHttpAsyncCallback;
		if (testnet == true) {
			baseApiUrl =  "https://apitestnet.indiesquare.me/v2/";
		}
		 

	}

	public void getBalances(string address)
	{
		currentMethod = "getBalances";
		var url = baseApiUrl + "addresses/" + address + "/balances";
		StartCoroutine (getAsync (url, HandleHttpAsyncCallback)); 
	}

	public void getHistory(string address)
	{
		currentMethod = "getHistory";
		var url = baseApiUrl + "addresses/" + address + "/history";
		StartCoroutine (getAsync (url, HandleHttpAsyncCallback)); 
	}

	public void getOrderHistory(string address)
	{
		currentMethod = "getOrderHistory";
		var url = baseApiUrl + "addresses/" + address + "/orderhistory";
		StartCoroutine (getAsync (url, HandleHttpAsyncCallback)); 
	}

	public void getTokenInfo(string token)
	{
		currentMethod = "getTokenInfo";
		var url = baseApiUrl + "tokens/"+token;
		StartCoroutine (getAsync (url, HandleHttpAsyncCallback)); 
	}

	public void getTokenHolders(string token)
	{
		currentMethod = "getTokenHolders";
		var url = baseApiUrl + "tokens/"+token+"/holders";
		StartCoroutine (getAsync (url, HandleHttpAsyncCallback)); 
	}

	public void getTokenHistory(string token)
	{
		currentMethod = "getTokenHistory";
		var url = baseApiUrl + "tokens/"+token+"/history";
		StartCoroutine (getAsync (url, HandleHttpAsyncCallback)); 
	}

	public void getTokenDescription(string token)
	{
		currentMethod = "getTokenDescriptionPart1";
		var url = baseApiUrl + "tokens/"+token;
		StartCoroutine (getAsync (url, HandleHttpAsyncCallback)); 
	}

	public void createSend(string source,string token,string destination,double quantity,int fee = -1,int feePerKB = -1)
	{
		currentMethod = "createSend";
		var url = baseApiUrl + "transactions/send";

		Dictionary<string, string> parameters = new Dictionary<string, string>();
		if (quantity % 1 == 0) {

			parameters.Add("quantity",(int)quantity+"");
		} else {

			parameters.Add("quantity",quantity+"");
		}
		parameters.Add("source",source);
		parameters.Add("token",token);
		parameters.Add("destination",destination);
		 
		if (fee != -1) {
			parameters.Add("fee",fee+"");
		}
		if (feePerKB != -1) {
			parameters.Add("fee_per_kb",feePerKB+"");
		}
		string jsonString = JsonMapper.ToJson (parameters);
		 StartCoroutine (postAsyncJSON (url,jsonString, HandleHttpAsyncCallback)); 
	}
	 
	public void createIssuance(string source,string token, double quantity,bool divisible,string description = null,int fee = -1,int feePerKB = -1)
	{
		currentMethod = "createIssuance";

		var url = baseApiUrl + "transactions/issuance";

		Dictionary<string, string> parameters = new Dictionary<string, string>();

		if (quantity % 1 == 0) {

			parameters.Add("quantity",(int)quantity+"");
		} else {

			parameters.Add("quantity",quantity+"");
		}

		parameters.Add("source",source);
		parameters.Add("token",token);
		if (divisible) {
			parameters.Add ("divisible", "true");
		} else {
			parameters.Add ("divisible", "false");
		}

		if (description != null) {
			parameters.Add("description",description);
		}
		if (fee != -1) {
			parameters.Add("fee",fee+"");
		}
		if (feePerKB != -1) {
			parameters.Add("fee_per_kb",feePerKB+"");
		}
		string jsonString = JsonMapper.ToJson (parameters);
		StartCoroutine (postAsyncJSON (url,jsonString, HandleHttpAsyncCallback)); 

	}
	 
 	public void createOrder(string source,double give_quantity, string give_token, double get_quantity, string get_token,  int expiration, int fee = -1, int feePerKB = -1)
	{
		currentMethod = "createOrder";

		var url = baseApiUrl + "transactions/order";

		Dictionary<string, string> parameters = new Dictionary<string, string>();

		if (give_quantity % 1 == 0) {

			parameters.Add("give_quantity",(int)give_quantity+"");
		} else {

			parameters.Add("give_quantity",give_quantity+"");
		}

		if (get_quantity % 1 == 0) {

			parameters.Add("get_quantity",(int)get_quantity+"");
		} else {

			parameters.Add("get_quantity",get_quantity+"");
		}

		parameters.Add("source",source);
	
		parameters.Add("give_token",give_token);

		parameters.Add("get_token",get_token);
		 
		parameters.Add ("expiration", expiration+"");
		 
		if (fee != -1) {
			parameters.Add("fee",fee+"");
		}
		if (feePerKB != -1) {
			parameters.Add("fee_per_kb",feePerKB+"");
		}
		string jsonString = JsonMapper.ToJson (parameters);
		StartCoroutine (postAsyncJSON (url,jsonString, HandleHttpAsyncCallback)); 

	}

	public void createCancel(string source,string offer_hash, int fee = -1, int feePerKB = -1)
	{
		currentMethod = "createCancel";

		var url = baseApiUrl + "transactions/cancel";

		Dictionary<string, string> parameters = new Dictionary<string, string>();

		parameters.Add("source",source);

		parameters.Add("offer_hash",offer_hash);
 
		if (fee != -1) {
			parameters.Add("fee",fee+"");
		}
		if (feePerKB != -1) {
			parameters.Add("fee_per_kb",feePerKB+"");
		}
		string jsonString = JsonMapper.ToJson (parameters);
		StartCoroutine (postAsyncJSON (url,jsonString, HandleHttpAsyncCallback)); 

	}

	public void broadcast(string tx)
	{
		currentMethod = "broadcast";

		var url = baseApiUrl + "transactions/broadcast";

		Dictionary<string, string> parameters = new Dictionary<string, string>();

		parameters.Add("tx",tx);

		string jsonString = JsonMapper.ToJson (parameters);
		StartCoroutine (postAsyncJSON (url,jsonString, HandleHttpAsyncCallback)); 

	}


	public void getAddress(){
		 
		currentMethod = "getAddress";
		string channel = "indie-"+ System.Guid.NewGuid();

		Dictionary<string, string> parameters = new Dictionary<string, string>();
		parameters.Add("request","getaddress");
		currentParams = parameters;
		currentChannel = channel;
		pubsub (channel, parameters);


	}

	public void signTransaction(string unsigned_tx){

		currentMethod = "signTransaction";
		string channel = "indie-"+ System.Guid.NewGuid();

		Dictionary<string, string> parameters = new Dictionary<string, string>();
		parameters.Add("request","sign");
		parameters.Add ("unsigned_hex", unsigned_tx);
		currentParams = parameters;
		currentChannel = channel;
		pubsub (channel, parameters);


	}

	void pubsub(string channel,Dictionary<string, string> parameters){
 
		Dictionary<string, string> newParameters = new Dictionary<string, string>();
		newParameters.Add("channel",channel);

		 
		StartCoroutine(postAsync(baseurl+"/pubsub/topic",newParameters,HandlePubSubCallback));


	}

 

	void HandlePubSubCallback(bool error, string res){
		IndieCoreResult result = new IndieCoreResult ();
		if (error) {
			
			result.error = new IndieCoreError (res);
			result.method = currentMethod;
			result.result = null;
				
			indieCoreCallback (result);
		} else {
 
			linkageWallet (currentParams, currentChannel);

		 

			Dictionary<string, string> newParameters = new Dictionary<string, string>();
			newParameters.Add("channel",currentChannel);
			newParameters.Add("type","1");
			StartCoroutine(postAsync(baseurl+"/pubsub/subscribe",newParameters, HandleHttpAsyncCallback));



		}
		 
	}
	void HandleHttpAsyncCallback (bool error, string res)
	{
 
		IndieCoreResult result = new IndieCoreResult ();
		if (error) {

			result.error = new IndieCoreError (res);
			result.method = currentMethod;
			result.result = null;

			indieCoreCallback (result);
			return;
		} 

		if (currentMethod == "getAddress") {

			JsonData data = JsonMapper.ToObject (res);

			JsonData data1 = data ["data"];
			string address = data1 ["address"].ToString ();
			string signature = data1 ["signature"].ToString ();

			if (verifySignature (address, currentChannel, signature)) {
				result.error = null;
				result.method = currentMethod;
				result.result = address;

				indieCoreCallback (result);
			} else {
				result.error = new IndieCoreError ("address not verified");
				result.method = currentMethod;
				result.result = null;

				indieCoreCallback (result);
			}


		
		} else if (currentMethod == "signTransaction") {
			
			JsonData data = JsonMapper.ToObject (res);

			JsonData data1 = data ["data"];
			string signedTx = data1 ["signed_tx"].ToString (); 
				
			result.error = null;
			result.method = currentMethod;
			result.result = signedTx;

			indieCoreCallback (result);

		}else if (currentMethod == "getTokenDescriptionPart1") {

			JsonData data = JsonMapper.ToObject (res);
 
			string description = data["description"].ToString (); 

			System.Uri uriResult;
			bool isURL = 	System.Uri.TryCreate(description, 	System.UriKind.Absolute, out uriResult) 
				&& (uriResult.Scheme == 	System.Uri.UriSchemeHttp || uriResult.Scheme == 	System.Uri.UriSchemeHttps);

			if (isURL) {

				currentMethod = "getTokenDescriptionPart2";
			 
				StartCoroutine (getAsync (description, HandleHttpAsyncCallback)); 

			} else {
				result.error = null;
				result.method = currentMethod;
				result.result = description;

				indieCoreCallback (result);
			}



		}else if (currentMethod == "getTokenDescriptionPart2") {

			 
				result.error = null;
				result.method = currentMethod;
				result.result = res;

				indieCoreCallback (result);
		 

		}


		else {
			result.error = null;
			result.method = currentMethod;
			result.result = res;

			indieCoreCallback (result);

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
 
	void linkageWallet(Dictionary<string, string> parameters, string channel){
		string request = parameters ["request"];
		string nonce = System.Guid.NewGuid () + "";;

		Dictionary<string, string> newParameters = new Dictionary<string, string>();
		newParameters.Add("channel",channel);
		newParameters.Add("nonce",nonce);
		newParameters.Add("x-success",urlscheme);

		if (request == "sign") {
			newParameters.Add ("unsigned_hex",parameters["unsigned_hex"]);
		}

		string jsonString = JsonMapper.ToJson (newParameters);
		string urlParams = request+"?params="+jsonString;

		if(request == "getaddress" || request == "verifyuser") {

			string xcallback_params = "channel="+channel+"&nonce="+nonce+"&x-success="+urlscheme+"&msg="+channel;
			urlParams = "x-callback-url/"+request+"?"+xcallback_params;
		}


		 
		Debug.Log ("urlparams: "+urlParams);
	 
		bool fail = false;
		#if UNITY_IPHONE
		Application.OpenURL("indiewallet://"+urlParams);
		#elif UNITY_ANDROID

		AndroidJavaClass up = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject ca = up.GetStatic<AndroidJavaObject> ("currentActivity");
		AndroidJavaObject packageManager = ca.Call<AndroidJavaObject> ("getPackageManager");
		AndroidJavaObject launchIntent = null;


		launchIntent = packageManager.Call<AndroidJavaObject> ("getLaunchIntentForPackage", "inc.lireneosoft.counterparty");
		launchIntent.Call<AndroidJavaObject> ("setClassName","inc.lireneosoft.counterparty", "inc.lireneosoft.counterparty.IntentActivity");
		try {

			AndroidJavaClass Uri = new AndroidJavaClass("android.net.Uri");
			AndroidJavaObject uri = Uri.CallStatic<AndroidJavaObject>("parse", "indiewallet://"+urlParams);
			launchIntent.Call<AndroidJavaObject> ("setData",uri);
			//launchIntent.Call<AndroidJavaObject>("setAction", "inc.lireneosoft.counterparty.IntentActivity"); 
			launchIntent.Call<AndroidJavaObject>("putExtra", "source", urlParams); 

		} catch (System.Exception e) {
			fail = true;
		}

		if (fail) {
			Debug.Log ("app not found");
		} else {
			ca.Call ("startActivity", launchIntent);
		}
		up.Dispose ();
		ca.Dispose ();
		packageManager.Dispose ();
		launchIntent.Dispose (); 
 
		#else
		string qrurl = "indiewallet://"+urlParams;

		IndieCoreResult result = new IndieCoreResult ();
	 

			result.error = null;
			result.method = currentMethod;
			result.result = qrurl;
			result.qrcode = generateQR(qrurl);
			indieCoreCallback (result);

		#endif

		 
	}
	private static Color32[] Encode(string textForEncoding, int width, int height) {
		var writer = new BarcodeWriter {
			Format = BarcodeFormat.QR_CODE,
			Options = new QrCodeEncodingOptions {
				Height = height,
				Width = width
			}
		};
		return writer.Write(textForEncoding);
	}
	public Texture2D generateQR(string text) {
		var encoded = new Texture2D (256, 256);

		var color32 = Encode(text, encoded.width, encoded.height);
		encoded.SetPixels32(color32);
		encoded.Apply();
		return encoded;
	}
	IEnumerator postAsyncJSON(string url,string parameters,HttpCallback callbackFunc )
	{

		WWW www;
		Hashtable postHeader = new Hashtable();
		postHeader.Add("Content-Type", "application/json");

		if (apiKey != "") {
			postHeader.Add("X-Api-Key", apiKey);
		}
		Debug.Log (url);
		Debug.Log (parameters);


		// convert json string to byte
		var formData = System.Text.Encoding.UTF8.GetBytes(parameters);

		www = new WWW(url, formData, postHeader);

		yield return www;
	 
		if (!string.IsNullOrEmpty(www.error))
		{

			Debug.Log(www.error);
			if (!string.IsNullOrEmpty (www.text)) {
				callbackFunc (true, www.text);
			} else {
				callbackFunc (true, www.error);
			}


		}
		else
		{
			Debug.Log (www.text);


			callbackFunc(false, www.text);



		}
	}

	IEnumerator postAsync(string urlString, Dictionary<string, string> parameters,HttpCallback callbackFunc )
	{
		 
		WWWForm form = new WWWForm();

		foreach(string paramKey in parameters.Keys){
 
			form.AddField(paramKey,parameters[paramKey]);
		}

		  
		var headers = form.headers;
		var rawData = form.data;
	 
		WWW www = new WWW(urlString, rawData, headers);


		yield return www;
	 
		if (!string.IsNullOrEmpty(www.error))
		{
	 
			Debug.Log(www.error);
			if (!string.IsNullOrEmpty (www.text)) {
				callbackFunc (true, www.text);
			} else {
				callbackFunc (true, www.error);
			}

		}
		else
		{
			Debug.Log (www.text);
		 
		 
			callbackFunc(false, www.text);
		 


		}
	}

	IEnumerator getAsync(string url, HttpCallback callbackFunc )
	{
		var api = "";
		if (apiKey != "") {
			api = "?X-Api-Key=" + apiKey;
		}

		url = url+api;
		Debug.Log (url);
		WWW www = new WWW(url);


		yield return www;

		if (!string.IsNullOrEmpty(www.error))
		{

			Debug.Log(www.error);
			callbackFunc(true,www.error);

		}
		else
		{
			Debug.Log (www.text);


			callbackFunc(false, www.text);



		}
	}

}
