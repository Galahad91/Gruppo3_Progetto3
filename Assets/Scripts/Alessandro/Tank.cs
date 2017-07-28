using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Characters
{
    
    public Tank()
    {
        health = 300;
        currentHealth = health;
        steps = 1;
        minDmg = 4;
        maxDmg = 6;
        minMultiplier = 0.8f;
        minMultiplier = 1.2f;
        critChance = 10;
        evasionChance = 10;
        maxCritMultiplier = 2.2f;
        minCritMultiplier = 1.8f;
        critMultiplier = 2f;
        sightRange = 2;
        weakAgainst = "Ranged";
        strongAgainst = "Tower";
        team = "Player 1";
    }
}