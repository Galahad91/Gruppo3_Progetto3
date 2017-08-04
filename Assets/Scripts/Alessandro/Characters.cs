using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

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


	//Casella corrente
	public GameObject casella;

	protected AILerp IA; 							//Variabile per accedere alle funzionalità dello script AILerp
	protected Seeker seek;							//Variabile utile per le operazioni di pathnode

	public bool isFighting = false; 			//Variabile che idnica se il personaggio è in combattimento oppure no

	//Lista di nodi del percorso corrente
	public List<Vector3> nodeList;

	//Distanza attacco
	public int attackRange;

	void Awake()
	{

		ogg_Gestore = GameObject.FindGameObjectWithTag ("GameManager");
		gestore = ogg_Gestore.GetComponent<GestoreGioco> ();

	}

	/// <summary>
	/// Settiamo la grandezza del radar
	/// </summary>
	/// <param name="Radar">Radar.</param>
	public void SetRadarSize(BoxCollider Radar)
	{

		Radar.size = new Vector3 (2.4f*sightRange, 1f, 2.4f*sightRange);

		//Debug.Log (Radar.size);

	}

 
	/// <summary>
	/// Metodo di gestione dei danni inflitti
	/// </summary>
	/// <param name="enemy">Nemico a cui faremo danni</param>
    public float Attack(GameObject enemy)
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

	/// <summary>
	/// Metodo che sottrae danno al nemico
	/// </summary>
	/// <param name="damage"> Quanto danno viene applicato </param>
	public void DamageTaken (float damage)
	{
		currentHealth -= damage;
		if (currentHealth <= 0)
		{
			currentHealth = 0;
			Death();                
		}
	}

	/// <summary>
	/// Metodo che causa la morte all'oggetto
	/// </summary>
	public virtual void Death()
    {

		//LiberaCasella (casella);
		//gestore.RimuoviDatoListaPersonaggi (gameObject.name);
		Destroy(this.gameObject);
    }
		

	//Ci scusiamo per sta porcata 
	public void AvviaPriorita()
	{

		AttivoSetTarget = true;

	}

	/// <summary>
	/// Aggiorniamo il numero di steps correnti
	/// </summary>
	public void AggiornaStepsCorrenti()
	{

		/*usiamo la classe Path della libreria Pathfinding e assegnamo seek.StartPath per stabilire 
		 * un percorso iniziale e finale (transform), passiamo i nodi ad una lista inizializzandola a -1
		 * per non far contare la posizione di partenza, ad ogni nodo richiamiamo OnTargetReached()
		 */

		Path p = seek.StartPath (transform.localPosition, IA.target.localPosition);
		p.BlockUntilCalculated ();
		nodeList = p.vectorPath;
		OnNodeReached ();

		if (currentSteps >= 1) 
		{

			currentSteps--;
			//Debug.Log (currentSteps);


		}
		else 
		{
			
			Attendi ();

			//BUGS BRUTTO

			//gestore.AggiornaPersonaggioFermo(gameObject.name, currentSteps);
			//this.gestore.NemiciFermi++;

			Debug.Log ("ATTENDO");

		}

	}

	/// <summary>
	/// Metodo che viene richiamato quando il personaggio è arrivato a destinazione. Setta anche IsFighting sia del personaggio che del nemico
	/// </summary>
	void OnNodeReached()
	{

		int startList = nodeList.Count - (nodeList.Count - 1);
		int endList = nodeList.Count - attackRange;

		if (startList >= endList) 
		{

			Debug.Log (gameObject.name + " in fase di combattimento");
			Attendi ();

			//essendo il nostro personaggio in fase di combattimento settiamo sia il target che sè stesso IsFighjting = true
			this.isFighting = true; //questo personaggio
			IA.target.GetComponent<Characters> ().isFighting = true; //il nemico 

		}

	}

	/// <summary>
	/// Metodo richiamto fine turno da ogni personaggio per resettare queste informazioni
	/// </summary>
	public void ResetSteps()
	{

		currentSteps = steps;

	}

	/// <summary>
	/// Attiva il radar del giocatore
	/// </summary>
	public void AttivaRadar()
	{

		gameObject.transform.GetChild (1).gameObject.SetActive (true);

	}


	/// <summary>
	/// Disattiva il radar del giocatore
	/// </summary>
	public void DisattivaRadar()
	{

		gameObject.transform.GetChild (1).gameObject.SetActive (false);

	}

	#region Gestione_Movimento 

		/// <summary>
		/// Metodo che mette in attesa il personaggio
		/// </summary>
		public void Attendi()
		{

			IA.canMove = false;
			IA.canSearch = false;

		}

		/// <summary>
		/// Metodo che avvia la camminata del persoanggio
		/// </summary>
		public void Avvia()
		{

			IA.canMove = true;
			IA.canSearch = true;

		}

	#endregion

	#region Gestione_Casella

		/// <summary>
		/// Metodo per il quale so che ho fatto un'altro passo e so in che casella sono
		/// </summary>
		/// <param name="altro"> Oggetto con cui abbiamo colliso </param>
		void OnTriggerEnter(Collider altro)
		{


			/*if (altro.tag == "Casella" ) 
			{

				casella = altro.gameObject;   //Assegniamo la casella corrente
				AstarPath.active.Scan (); 	
				AggiornaStepsCorrenti ();

				//Impostiamo la casella su cui siamo un'ostacolo
				altro.gameObject.layer = 8;

				Attendi ();

			}*/

		}

		/// <summary>
		/// Metodo per il quale so che sono uscito dalla casella vecchia e la posso liberare
		/// </summary>
		/// <param name="altro"> Oggetto con cui siamo usciti dalla collisione </param>
		void OnTriggerExit(Collider altro)
		{

			//Liberiamo la casella dcendo che non è più un ostacolo
			//LiberaCasella (altro.gameObject);

		}

		/// <summary>
		/// Andiamo a liberare la casella dicendo che non è più un ostacolo
		/// </summary>
		/// <param name="casella">Casella che vogliamo liberare</param>
		public void LiberaCasella(GameObject casella)
		{

			//Liberiamo la casella dcendo che non è più un ostacolo
			//casella.gameObject.layer = 0;

		}

	#endregion
  
}