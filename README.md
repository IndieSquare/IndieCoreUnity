# IndieCoreUnity

# Description
Unity SDK for IndieSquare API written in c# including [NBitcoin](https://github.com/MetacoSA/NBitcoin) Unity build, further api documentation can be found [here](https://developer.indiesquare.me/)
 
# Installation

Download the IndieCore.package file from the releases section and add it to your project. You can also download the source code and build/run files from the IndieCore folder.

 
# Basic Usage
Full usage can be seen in the example scene, results logged in console
 

# Example Usage

## Create a send transcation and request signing on the users IndieSquare Wallet

First initialize IndieCore in your start function and add the HandleIndieCoreCallback delegate
Set the your api key and url scheme in the editor view if needed

All functions return their result through the HandleIndieCoreCallback delegate, see the example scene for more info
```
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

 ```
Create a send transaction
```
  IndieCore.Instance.createSend("1965areciqapsuL2hsia2yKkRLfAsH1smG", "NETX", "12sWrxRY7E7Nhmuyjbz4TtGE9jRewGqEZD", 1.1);
 ```
Sign transaction with users installed IndieSquare Wallet
For mobile You must add your apps urlscheme (represented by MYURLSCHEME) in your plist (for iOS) or manifest (for android) file, so IndieSquare can return to your app after the user authorizes the signature.
for non mobile platforms the function returns a texure2D qrcode which the user can scan inorder to sign, (the user must scan within 90 seconds due to timeout)

 ```  
  IndieCore.Instance.signTransaction ("0100000001425fc12873a1a09c744ce7e782e95acc5cb69611d9f9b69550a45576383338f7020000001976a914edee861dff4de166683e4c54ae3869cd05c7ae0f88acffffffff0336150000000000001976a9141485d9d03b41aaa9dca7d70d7f63ff4a0826100e88ac00000000000000001e6a1cacd10644550a44ead1ce07effa7abcdd01911e197349b79630f48960b0561400000000001976a914edee861dff4de166683e4c54ae3869cd05c7ae0f88ac00000000");
	
   ``` 
Broadcast signed transaction
```
 IndieCore.Instance.broadcast("0100000001bf1f87c5041063d8353f3d8e109fb11405456d7972c5f401308ced36eb9e8fea010000001976a914e1869fa1cec7741a502e7a5bd938ed8f5e354b5488acffffffff0200000000000000002e6a2c0b0b8cb664864cdf2ff70668595e63567b9d8ece36b2383513b6eeab7f1c15e70466593f13bb49618b8afe7079e93a00000000001976a914e1869fa1cec7741a502e7a5bd938ed8f5e354b5488ac00000000");
	
    
 ```
 
## Request user's IndieSquare wallet address and verify user owns address through message signature

For mobile You must add your apps urlscheme (represented by MYURLSCHEME) in your plist (for iOS) or manifest (for android) file, so IndieSquare can return to your app after the user authorizes the address.
for non mobile platforms the function returns a texure2D qrcode which the user can scan inorder to sign, (the user must scan within 90 seconds due to timeout)
The address is verified by the SDK

```
 IndieCore.Instance.getAddress ();
```

# Further Reading

* [Official Project Documentation](https://developer.indiesquare.me)
