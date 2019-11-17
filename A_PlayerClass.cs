using System;

//A simple data object class to keep some information about a player.
public class A_PlayerClass
{
	//Note: lastHitBy returns a player ID, currently no way to parse it to a username so it goes unused.
	public A_PlayerClass(string name, int hp = 0, int armour = 0, string lastHitBy = "Nobody", int userID = -1)
	{
		this.name = name;
		this.hp = hp;
		this.armour = armour;
		this.lastHitBy = lastHitBy;
		this.userID = userID;
	}

	public string name;
  
	public int hp;

	public int armour;

	public string lastHitBy;

	public int userID;
}
