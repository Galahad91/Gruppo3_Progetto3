using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged : Characters
{
    
    public Ranged()
    {
        health = 100;
        currentHealth = health;
        steps = 3;
        minDmg = 4;
        maxDmg = 6;
        minMultiplier = 0.8f;
        minMultiplier = 1.2f;
        critChance = 20;
        evasionChance = 20;
        maxCritMultiplier = 2.2f;
        minCritMultiplier = 1.8f;
        critMultiplier = 2;
        sightRange = 2;
        weakAgainst = "Assassin";
        strongAgainst = "Tank";
        team = "Player 1";
    }
}