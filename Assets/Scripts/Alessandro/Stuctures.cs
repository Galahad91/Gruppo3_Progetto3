using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Structures : MonoBehaviour
{
    // Vita massima e attuale
    public float health;
    public float currentHealth;

    public float baseDmg;
    
    //Campo visivo
    public int sightRange;

    // Resistenze
    public string weakAgainst;
    public string strongAgainst;

    // distinzione Team
    public string team;
   
    // Metodo di gestione dei danni inflitti
    float Attack(GameObject enemy)
    {
		Characters currentEnemy = enemy.GetComponent<Characters>();

        float attackValue = baseDmg;
        int rand = Random.Range(1, 101);
       
        //check sul team
        if (enemy.layer != LayerMask.NameToLayer(team))
        {
            //Se evade
            if (rand <= currentEnemy.evasionChance)
            {
                return 0;
            }



            if (enemy.CompareTag(enemy.GetComponent<Characters>().weakAgainst))
            {
                attackValue = attackValue * currentEnemy.minMultiplier;
                return attackValue;
            }
            else if (enemy.CompareTag(currentEnemy.strongAgainst))
            {
                attackValue = attackValue * currentEnemy.maxMultiplier;
                return attackValue;
            }
            else
            {
                attackValue = attackValue * 1;
                return attackValue;
            }

        }

        else
        {
            return 0;
        }
    }

    // Metodo di sottrazione del danno subito
    void DamageTaken(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            //Death();                 // Metodo di distruzione personaggio
        }
    }

    void Death()
    {
        Destroy(this);
    }

    // check priorità nel raggio visivo
    void Radar(GameObject startPoint)
    {
        int length = 1;  //Getmappa();S



        for (int i = ((int)startPoint.transform.position.y - sightRange); i <= ((int)startPoint.transform.position.y + sightRange); i++)
            for (int j = ((int)startPoint.transform.position.x - sightRange); j <= ((int)startPoint.transform.position.x + sightRange); j++)
            {
                if (i >= 0 && i < length && j >= 0 && j < length)
                {

                }
            }

    }
}