using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Characters : MonoBehaviour
{

    // Vita massima e attuale
    public float health;
    public float currentHealth;
    // Range di movimento
    public int steps;
	public int currentSteps;
    // Range danno base
    public float minDmg;
    public float maxDmg;
    // Moltiplicatore danno in base alle resistenze
    public float maxMultiplier;
    public float minMultiplier;
    // % di effettuare un critico o di evadere un attacco
    public int critChance;
    public int evasionChance;
    // Moltiplicatore danno critico in base alle resistenze
    public float maxCritMultiplier;
    public float minCritMultiplier;
    public float critMultiplier;
    // Resistenze
    public string weakAgainst;
    public string strongAgainst;
	public Transform TargetTorre;
	//distanza dal target
	[Range(1,10)]
	public float targetDistance;
	//costo attacco
	public int attackCost;

    // distinzione Team
    public string team;

	//Campo visivo
	[Header("Campo visivo della classe")]
	[Range(1,40)]
	public int sightRange = 5;

	private GameObject ogg_Gestore;
	private GestoreGioco gestore;

	//Variabile per attivare la priorità e il target
	public bool AttivoSetTarget = false;



	void Awake()
	{

		ogg_Gestore = GameObject.FindGameObjectWithTag ("GameManager");
		gestore = ogg_Gestore.GetComponent<GestoreGioco> ();

	}
		

	//settiamo la grandezza del radar
	public void SetRadarSize(BoxCollider Radar)
	{

		Radar.size = new Vector3 (2.4f*sightRange, 1f, 2.4f*sightRange);

		//Debug.Log (Radar.size);

	}

  // Metodo di gestione dei danni inflitti
  float Attack(GameObject enemy)
  {

		Characters currentEnemy = enemy.GetComponent<Characters>();

        float attackValue;
        int rand =  Random.Range(1, 101);

        // check sul team
        if (enemy.layer != LayerMask.NameToLayer(team))
        {
            if (!enemy.CompareTag("Tower") || !enemy.CompareTag("Nexus"))
            {
                //Se evade
				if (rand <= currentEnemy.evasionChance)
                {
                    return 0;
                }
            }


            rand = Random.Range(1, 101);
            //Se è critico
            if (rand <= critChance)
            {
                attackValue = Random.Range(minDmg, maxDmg);

                if (enemy.CompareTag(currentEnemy.weakAgainst))
                {
                    attackValue = attackValue * minCritMultiplier;
                    return attackValue;
                }
                else if (enemy.CompareTag(currentEnemy.strongAgainst))
                {
                    attackValue = attackValue * maxCritMultiplier;
                    return attackValue;
                }
                else
                {
                    attackValue = attackValue * critMultiplier;
                    return attackValue;
                }
            }
            //Se non lo è
            else
            {
                attackValue = Random.Range(minDmg, maxDmg);

                if (enemy.CompareTag(currentEnemy.weakAgainst))
                {
                    attackValue = attackValue * minMultiplier;
                    return attackValue;
                }
                else if (enemy.CompareTag(currentEnemy.strongAgainst))
                {
                    attackValue = attackValue * maxMultiplier;
                    return attackValue;
                }
                else
                {
                    attackValue = attackValue * 1;
                    return attackValue;
                }
            }

        }   
       
        else
        {
            return 0;
        }
  }

  // Metodo di sottrazione del danno subito
  void DamageTaken (float damage)
  {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Death();                
        }
  }

  void Death()
  {

		gestore.RimuoviDatoListaPersonaggi (gameObject.name);
        Destroy(this);
  }


	//Ci scusiamo per sta porcata 
	public void AvviaPriorita()
	{

		AttivoSetTarget = true;

	}
  
}