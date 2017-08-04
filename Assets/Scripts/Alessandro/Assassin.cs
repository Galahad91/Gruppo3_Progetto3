using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Assassin : Characters
{
    
	//Variabili per la decisione di priorità del personaggio
	[Header(" DEPRECATO! Gestione priorita")]
	//variabili di priorita
	[Range(1,5)]
	public int Nexus;
	[Range(1,5)]
	public int Torri;
	[Range(1,5)]
	public int Nemici;

	[Header("Costo del personaggio")]
	public int Costo = 1;

	[Header("Danno Nexus")]
	public int Danno = 5;

	//Variabile per il radar
	BoxCollider Radar;
	GameObject Radar_ogg;

	//Variabili per accedere a GestoreGioco
	public GameObject ogg_Gestore;
	private GestoreGioco gestore;

	//TEST
	//public Color color;

	#region LISTA_TO_DO

	//---------------------------------------------------------------------------------------------------- CODICE LISTA PRIORITA'
												
												private List<priorita> to_do;

												public class priorita
												{

													public string Nome_Priorita;
													public int Defcon_Priorita;
													public Transform Posizione_Priorita;

												}


		/// <summary>
		/// Aggiorno la lista delle priorita
		/// </summary>
		public void AggiornaListaPriorita()
		{

			//PRIORITA' NEXUS
			priorita dato = new priorita ();

			dato.Nome_Priorita = "Nexus";
			dato.Defcon_Priorita = Nexus;

			if (gameObject.layer == LayerMask.NameToLayer ("Player 1")) 
			{

				dato.Posizione_Priorita = gestore.Nexus_Player2;

			} 
			else if (gameObject.layer == LayerMask.NameToLayer ("Player 2")) 
			{

				dato.Posizione_Priorita = gestore.Nexus_Player1;

			} 
			else 
			{

				dato.Posizione_Priorita = null;
				Debug.LogError ("il nexus corrente is null");

			}

			//Aggiungo dato 
			to_do.Add (dato);

			dato = new priorita ();

			//PRIORITA' TORRI

			dato.Nome_Priorita = "Torri";
			dato.Defcon_Priorita = Torri;

			if (gameObject.GetComponent<Characters> ().TargetTorre != null) 
			{

				dato.Posizione_Priorita = gameObject.GetComponent<Characters> ().TargetTorre;

			} 
			else 
			{

				dato.Posizione_Priorita = to_do [0].Posizione_Priorita;

			}

			//Aggiungo dato
			to_do.Add (dato);

			//PRIORITA' NEMICO

			//Aggiungiamo nelle priorità il nemico 
			to_do.Add (GetNemico());
		}

		/// <summary>
		/// Metodo che ritorna il nemico come target
		/// </summary>
		/// <returns> Ritorna un oggetto di tipo priorità </returns>
		private priorita GetNemico()
		{

			List<string> listaNemici = new List<string>();
			priorita dato;

			/*//Passo la lista di nemici a una lista interna alla classe
			if (gameObject.GetComponentInChildren<Radar>().GetListaNemici().Count > 0) 
			{

				listaNemici = gameObject.GetComponentInChildren<Radar> ().GetListaNemici();


			} 
			else 
			{

				Debug.LogError ("Target nullo, nessun nemico in zona");

				dato = new priorita ();

				dato.Nome_Priorita = "Nemici";
				dato.Defcon_Priorita = Nemici;
				dato.Posizione_Priorita = null;

				//esco immediatamente dal metodo
				return dato;
			}

			//PARTE DEICISIONALE DELLA IA IN CUI VALUTA IL NEMICO MIGLIORE DA SELEZIONARE 
			//PER ORA FACCIAMO QUELLO PIU' VICINO 

			int pathVicino = 0;
			int indice = 0;
			Transform target = null;

			//calcoliamo il target più vicino
			for(int i = 0; i < listaNemici.Count; i++)
			{

				Path p = seek.StartPath (transform.position,gestore.CercaPerNome (listaNemici [i]).Oggetto.transform.position);
				p.BlockUntilCalculated ();

				if (p.vectorPath.Count < pathVicino && pathVicino != 0) 
				{

					pathVicino = p.vectorPath.Count;
					indice = i; //indice della lista in cui contiene il personaggio più vicino

				}
				else
				{ 
					//Assegno il primo valore della lista di nomi perchè se no rimane zero!!!
					pathVicino = p.vectorPath.Count;

				}
			}*/

			dato = new priorita ();

			dato.Nome_Priorita = "Nemici";
			dato.Defcon_Priorita = Nemici;
			//dato.Posizione_Priorita = gestore.CercaPerNome(listaNemici [indice]).Oggetto.transform;
			dato.Posizione_Priorita = null;

			Debug.LogError ("il target nemico : " + dato.Posizione_Priorita);

			//Resetto la lista di nemici nel radar
			//gameObject.GetComponentInChildren<Radar> ().ResetListaNemici();

			return dato;


		}
		
	//---------------------------------------------------------------------------------------------------- FINE

	#endregion

    public Assassin()
    {

        health = 100;
        currentHealth = health;
        steps = 3;
		currentSteps = steps;
        minDmg = 4;
        maxDmg = 6; 
        minMultiplier = 0.8f;
        maxMultiplier = 1.2f;
        critChance = 20;
        evasionChance = 20;
        maxCritMultiplier = 2.2f;
        minCritMultiplier = 1.8f;
        critMultiplier = 2f;
        sightRange = 2;
        weakAgainst = "Tower";
        strongAgainst = "Ranged";
        team = "Player 1";
		AttivoSetTarget = false;
		//abbiamo variabile torre anche se non la vediamo scritta qui

    }

	void Awake()
	{

		to_do = new List<priorita>();

		ogg_Gestore = GameObject.FindGameObjectWithTag ("GameManager");
		gestore = ogg_Gestore.GetComponent<GestoreGioco> ();

		seek = GetComponent<Seeker>();
		IA = gameObject.GetComponent<AILerp> ();

		Radar = gameObject.transform.GetChild (1).GetComponent<BoxCollider> ();



		//Cambiamo nome al personaggio
		AggiornoNome();

	}

	void Start()
	{

		//Quando istanziamo l'oggetto va aggiunto alla lista dei personaggi presenti in gioco
		if(gestore.GetTurno()%2 == 0)
		{
			//Turno Player 2
			gestore.AggiungereDatoListaPersonaggi (gameObject.tag, steps, currentSteps, gameObject.name, this.gameObject, true);
		}
		else
		{
			//Turno Player 1
			gestore.AggiungereDatoListaPersonaggi (gameObject.tag, steps, currentSteps, gameObject.name, this.gameObject, false );
		}

		//gameObject.transform.GetChild(1).gameObject.SetActive(true);
		Attendi ();
		//SetRadarSize (Radar);
		AggiornaListaPriorita ();
		SetTarget ();
		//gameObject.transform.GetChild(1).gameObject.SetActive(false);



	}


	void Update()
	{

		if (gestore.Turni % 2 == 0 ) 
		{

			if (gestore.IsFaseCombattimento () == true && gameObject.layer == LayerMask.NameToLayer("Player 2")) 
			{

				Avvia ();

				//Settiamo un nuovo target
				/*if (AttivoSetTarget == true) 
				{

					AttivoSetTarget = false;
					SetTarget ();

				}*/

			} else {

				Attendi ();

				//FASE DI PREPARAZIONE

				/*if (currentSteps <= 0) 
				{	
					
					ResetSteps ();

				}*/
					

			}
		} 
		else 
		{

			if (gestore.IsFaseCombattimento () == true && gameObject.layer == LayerMask.NameToLayer("Player 1")) 
			{

				Avvia ();

				//Settiamo un nuovo target
				/*if (AttivoSetTarget == true) 
				{

					AttivoSetTarget = false;
					SetTarget ();

				}*/

			} else {

				Attendi ();

				//FASE DI PREPARAZIONE

				/*if (currentSteps <= 0) 
				{	
					
					ResetSteps ();

				}*/


			}
		}


	}


	/// <summary>
	/// Aggiorniamo il nome del personaggio 
	/// </summary>
	private void AggiornoNome()
	{

		gameObject.name = gameObject.name + gestore.contatore_nemico.ToString ();
		gestore.contatore_nemico++;

	}
		
	/// <summary>
	/// Calcolo della priorita principale
	/// </summary>
	/// <returns> Ritorna il transform della priorità </returns>
	public Transform GetPriorita()
	{

		int priorita_corrente = 6;
		string nome_priorita = "Nemici"; //priorita massima
		Transform posizione_priorita = null;

		if (!isFighting) 
		{
			
			for (int i = 0; i < to_do.Count; i++) 
			{
				

				if (priorita_corrente > to_do [i].Defcon_Priorita && to_do [i].Posizione_Priorita != null ) 
				{

					priorita_corrente = to_do [i].Defcon_Priorita;
					nome_priorita = to_do [i].Nome_Priorita;
					posizione_priorita = to_do [i].Posizione_Priorita;

				}

			}
		}
			
		Debug.Log ("Priorita: " + posizione_priorita.position);

		return posizione_priorita;

	}


	/// <summary>
	/// Settiamo il target al personaggio date le sue priorità
	/// </summary>
	public void SetTarget()
	{

		IA.target = GetPriorita();

	}

	/// <summary>
	/// Override del metodo Death()
	/// </summary>
	public override void Death()
	{

		LiberaCasella (casella);
		gestore.RimuoviDatoListaPersonaggi (gameObject.name);
		IA.target = null;
		StartCoroutine (Explosion ());
		//Destroy (this.gameObject);

	}

	/// <summary>
	/// Metodo di suicidio per il quale il personaggio muore infliggendo danno al Nexus
	/// </summary>
	public void ToraToraTora()
	{

		//Controllo il turno per capire a quale Nexus fare dann
		if (this.gestore.Turni % 2 == 0) 
		{
			//Player 2 Togliere danno al Nexus1

			gestore.Nexus_Player1.gameObject.GetComponent<Structures> ().DamageTaken (Danno);

			//Mettere l'effetto esplosione
			StartCoroutine (Explosion ());


		} 
		else 
		{

			//Player 1 Togliere danno al Nexus2

			gestore.Nexus_Player2.gameObject.GetComponent<Structures> ().DamageTaken (Danno);

			StartCoroutine (Explosion ());

		}


	}

	/// <summary>
	/// Metodo che sottrae danno al nemico
	/// </summary>
	/// <param name="damage"> Quanto danno viene applicato </param>
	public void DamageTaken_Assassin (float damage, Color color)
	{

		StartCoroutine (ParticleDamage (color));

		currentHealth -= damage;
		if (currentHealth <= 0)
		{
			currentHealth = 0;
			Death();                
		}
	}


	IEnumerator Explosion()
	{
		//Posizione dell'effetto Explosion nella gerarchia dell'oggetto (Ste se sposti cose ti taglio le mani)
		this.transform.GetChild(2).GetComponent<ParticleSystem>().Play();
		//Posizione del personaggio nella gerarchia dell'oggetto (Ste anche qua sposti cose ti taglio le mani)
		this.transform.GetChild (4).gameObject.SetActive (false);
		yield return new WaitForSeconds (2f);
		Destroy (this.gameObject);

	}
	//TEST
	IEnumerator ParticleDamage(Color color)
	{

		ParticleSystem startingColor = this.transform.GetChild (5).GetComponent<ParticleSystem> ();
		var main = startingColor.main;
		main.startColor = new ParticleSystem.MinMaxGradient (color);


		yield return new WaitForSeconds (0.3f);

		this.transform.GetChild(5).GetComponent<ParticleSystem>().Play();

	}
}