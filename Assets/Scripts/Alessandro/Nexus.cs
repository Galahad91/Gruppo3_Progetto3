using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nexus : Structures
{
    
    public Nexus()
    {
        health = 100;
        baseDmg = 5;
        sightRange = 2;
        weakAgainst = "Tank";
        strongAgainst = "Assassin";
        team = "Player 1";

}
}