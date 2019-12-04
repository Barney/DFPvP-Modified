using System;

///<summary>Class used to define a player and some variables relating to a player. This class is used by the A_PvPClass and constructors
///initialized within SFSMultiplayer's parseData method.</summary>
public class A_PlayerClass
{
	///<summary>Constructor used to create a player.</summary>
	public A_PlayerClass(string name, int hp = 0, int armour = 0, int userID = -1)
	{
		this.name = name;
		this.hp = hp;
		this.armour = armour;
		this.userID = userID;
	}
	/*Setters for each datafield as accessing datafields directly in dnSpy is dangerous, this allows me to limit direct datafield
	//access to strictly within the class.*/
	public void setName(string name){ this.name = name; }
	public void setHP(int hp){ this.hp = hp; }
	public void setArmour(int armour){ this.armour = armour; }
	public void setUserID(int id) { this.userID = id; }

	//Getters for each datafield for the same reason as setters.
	public string getName(){ return this.name; }
	public int getHP(){ return this.hp; }
	public int getArmour(){ return this.armour; }
	public int getUserID(){ return this.userID; }

	//Datafields accessed directly only from this class.
	private string name;
	private int hp;
	private int armour;
	private int userID;
}
