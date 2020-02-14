using UnityEngine;
using System.Collections;

[System.Serializable]
public class Game 
{

	public static Game current;
	private static int gid = 0;
	private int id;

	public Character melee = new Character ();
	public Character defender;
	public Character ranged;

	public Game (Character melee, Character defender, Character ranged)
	{
		this.id = ++gid;
		this.melee = melee;
		this.defender = defender;
		this.ranged = ranged;
		Debug.Log ("Game");
	}
	public Game () {
		this.id = ++gid;
		this.melee = new Character();
		this.defender = new Character();
		this.ranged = new Character();
	}

	public int Id {
		get {
			return this.id;
		}
	}

	public Character Melee {
		get {
			return this.melee;
		}
		set {
			melee = value;
		}
	}

	public Character Defender {
		get {
			return this.defender;
		}
		set {
			defender = value;
		}
	}

	public Character Ranged {
		get {
			return this.ranged;
		}
		set {
			ranged = value;
		}
	}

	public void LoadGame(Character melee, Character defender, Character ranged)
	{
		melee = this.melee;
		defender = this.defender;
		ranged = this.ranged;
	}

	public void LoadGame(PlayerController melee)
	{
		Debug.Log (this.melee);
		this.melee.LoadPlayerData (melee);
	}
		
}
