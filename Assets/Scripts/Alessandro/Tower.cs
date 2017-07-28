using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Structures
{
    
    public Tower()
    {
        health = 100;
        baseDmg = 5;
        sightRange = 1;
        weakAgainst = "Tank";
        strongAgainst = "Assassin";
        team = "Player 1";
    }
}