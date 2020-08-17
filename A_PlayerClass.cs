using System;

public class A_PlayerClass
{
	public A_PlayerClass(string name, int hp, int armour, int userID)
	{
		this.name = name;
		this.hp = hp;
		this.armour = armour;
		this.userID = userID;
	}

	public string name;

	public int hp;

	public int armour;

	public int userID;
}
