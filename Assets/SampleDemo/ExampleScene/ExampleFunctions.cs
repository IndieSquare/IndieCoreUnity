using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NBitcoin;
using System;
using UnityEngine.UI;
using LitJson;
public class ExampleFunctions : MonoBehaviour {
	public Text resultsText;
	public Image qrCode;
 
	void Start () {
	
		IndieCore.Instance.indieCoreCallback += HandleIndieCoreCallback;
		 
	}
	void HandleIndieCoreCallback(IndieCoreResult data){

		if (data.error != null) {
			Debug.Log ("error " + data.method + ": " + data.error.message);
			resultsText.text = data.method + ": " +data.error.message;
		} else {

			Debug.Log (data.method+": " + data.result);
			resultsText.text = data.method + ": " + data.result;
			if (data.qrcode != null) {
				
				Rect rec = new Rect(0, 0, data.qrcode.width, data.qrcode.height);

				qrCode.sprite =Sprite.Create(data.qrcode,rec,new Vector2(0.5f,0.5f),500);

			}

		}
			
	}
	public void getAddress(){
		resultsText.text = "please wait...";
		IndieCore.Instance.getAddress ();
	}
	public void signTransaction(){
		resultsText.text = "please wait...";
		IndieCore.Instance.signTransaction ("0100000001425fc12873a1a09c744ce7e782e95acc5cb69611d9f9b69550a45576383338f7020000001976a914edee861dff4de166683e4c54ae3869cd05c7ae0f88acffffffff0336150000000000001976a9141485d9d03b41aaa9dca7d70d7f63ff4a0826100e88ac00000000000000001e6a1cacd10644550a44ead1ce07effa7abcdd01911e197349b79630f48960b0561400000000001976a914edee861dff4de166683e4c54ae3869cd05c7ae0f88ac00000000");
	}
	public void getBalances(){
		resultsText.text = "please wait...";
		IndieCore.Instance.getBalances("1MZUJyKLHsSthHY3z68NxLFPhnrDcsPaDk");
	}
	public void getHistory(){
		resultsText.text = "please wait...";
		IndieCore.Instance.getHistory("1MZUJyKLHsSthHY3z68NxLFPhnrDcsPaDk");
	}
	public void getOrderHistory(){
		resultsText.text = "please wait...";
		IndieCore.Instance.getOrderHistory("1MZUJyKLHsSthHY3z68NxLFPhnrDcsPaDk");
	}
	public void getTokenInfo(){
		resultsText.text = "please wait...";
		IndieCore.Instance.getTokenInfo("SARUTOBI");
	}
	public void getTokenHolders(){
		resultsText.text = "please wait...";
		IndieCore.Instance.getTokenHolders("SARUTOBI");
	}
	public void getTokenHistory(){
		resultsText.text = "please wait...";
		IndieCore.Instance.getTokenHistory("SARUTOBI");
	}
	public void getTokenDescription(){
		resultsText.text = "please wait...";
		IndieCore.Instance.getTokenDescription("SARUTOBI");
	}
	public void createSend(){
		resultsText.text = "please wait...";
		IndieCore.Instance.createSend("1965areciqapsuL2hsia2yKkRLfAsH1smG", "NETX", "12sWrxRY7E7Nhmuyjbz4TtGE9jRewGqEZD", 1.1);
	}
	public void createIssuance(){
		resultsText.text = "please wait...";
		IndieCore.Instance.createIssuance("12sWrxRY7E7Nhmuyjbz4TtGE9jRewGqEZD", "NETX", 1000, true);
	}
	public void createOrder(){
		resultsText.text = "please wait...";
		IndieCore.Instance.createOrder("12sWrxRY7E7Nhmuyjbz4TtGE9jRewGqEZD",10,"NETX",1,"XCP",100);
	}
	public void createCancel(){
		resultsText.text = "please wait...";
		IndieCore.Instance.createCancel("1MZUJyKLHsSthHY3z68NxLFPhnrDcsPaDk","ea8f9eeb36ed8c3001f4c572796d450514b19f108e3d3f35d8631004c5871fbf");
	}
	public void broadcast(){
		resultsText.text = "please wait...";
		IndieCore.Instance.broadcast("0100000001bf1f87c5041063d8353f3d8e109fb11405456d7972c5f401308ced36eb9e8fea010000001976a914e1869fa1cec7741a502e7a5bd938ed8f5e354b5488acffffffff0200000000000000002e6a2c0b0b8cb664864cdf2ff70668595e63567b9d8ece36b2383513b6eeab7f1c15e70466593f13bb49618b8afe7079e93a00000000001976a914e1869fa1cec7741a502e7a5bd938ed8f5e354b5488ac00000000");
	}

		
}