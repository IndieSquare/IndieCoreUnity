public class Token {
	public string name;
	public double balance;
	public double unconfirmed_balance;

	public Token(string name, double balance, double unconfirmed_balance) {
		this.name = name;
		this.balance = balance;
		this.unconfirmed_balance = unconfirmed_balance;
	}

  
}