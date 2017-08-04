using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nexus : Structures
{
	[Header("Radar del Nexus")]
	public BoxCollider ogg_Radar;
	private Radar Radar_torre;
    
    public Nexus()
    {
        health = 100;
        baseDmg = 5;
        sightRange = 2;
        weakAgainst = "Tank";
        strongAgainst = "Assassin";
        team = "Player 1";

	}

	//Settiamo la grandezza del radar come prima cosa da fare quando avviamo il gioco
	void Awake()
	{

		//AttivaRadar ();
		//SetRadarSize (ogg_Radar);
		//DisattivaRadar ();

	}
}