using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Structures : MonoBehaviour
{
	
    // Vita massima e attuale
    public float health;
    public float currentHealth;

    public float baseDmg;


    // Resistenze
    public string weakAgainst;
    public string strongAgainst;

    // distinzione Team
    public string team;

	//Campo visivo
	[Header("Campo visivo della struttura")]
	[Range(1,40)]
	public int sightRange;

	#region Metodi_Gestione_Danno
   
	    /// <summary>
	    /// Metodo che permette l'attacco da parte della struttura
	    /// </summary>
	    /// <param name="enemy">Enemy.</param>
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
			

		/// <summary>
		/// Metodo che infligge danno alla struttura
		/// </summary>
		/// <param name="damage"> Quantità di danno</param>
	    public void DamageTaken(float damage)
	    {
	        currentHealth -= damage;
	        if (currentHealth <= 0)
	        {
	            currentHealth = 0;
	            Death();                 // Metodo di distruzione personaggio
	        }
	    }

		/// <summary>
		/// Morte della struttura
		/// </summary>
	    void Death()
	    {
			Destroy(this.gameObject);
	    }

		/// <summary>
		/// Metodo che viene richiamato dal GestoreGioco ogni inzio turno per attaccare tutti i personaggi in zona
		/// </summary>
		public void Attacco()
		{

			GameObject[] Vettore_nemici;

			Vettore_nemici = gameObject.GetComponentInChildren<Radar> ().GetVettoreNemici();

			for (int i = 0; i < Vettore_nemici.Length - 1; i++) 
			{

				//Togliamo la vita al nemico selezionato in questo ciclo di for
				if (Vettore_nemici [i] != null) 
				{
					Vettore_nemici [i].GetComponent<Characters> ().DamageTaken (Attack (Vettore_nemici [i]));
				}

			}
			
			gameObject.GetComponentInChildren<Radar> ().ResetVettoreNemici ();
			Debug.Log (gameObject.name + " ha attaccato");

		}

	#endregion

	#region Metodi_Generici

		/// <summary>
		/// Settiamo la grandezza del radar
		/// </summary>
		/// <param name="Radar">Radar.</param>
		public void SetRadarSize(BoxCollider Radar)
		{

			//Radar.size = new Vector3 (0.5f*sightRange, 0.2f, 0.5f*sightRange);

			//Debug.Log (Radar.size);

		}

		/// <summary>
		/// Attiva il radar
		/// </summary>
		public void AttivaRadar()
		{

			gameObject.transform.GetChild (0).gameObject.SetActive (true);

		}

		/// <summary>
		/// Disattiva il radar
		/// </summary>
		public void DisattivaRadar()
		{

			gameObject.transform.GetChild (0).gameObject.SetActive (false);

		}

	#endregion

}