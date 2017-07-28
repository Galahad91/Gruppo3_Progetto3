using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Assassin : Characters
{
    
	[Header("Gestione priorita")]
	//variabili di priorita
	[Range(1,5)]
	public int Nexus;
	[Range(1,5)]
	public int Torri;
	[Range(1,5)]
	public int Nemici;

	private AILerp IA;
	public GameObject ogg_Gestore;
	private GestoreGioco gestore;
	private bool isFighting = false;
	private bool aggiorna = false;
	private Seeker seek;

	//variabile temp
	int t = 0;
	bool semaforo = true;

	//Variabile per il radar
	BoxCollider Radar;
	GameObject Radar_ogg;


	private List<priorita> to_do;

	public class priorita
	{

		public string Nome_Priorita;
		public int Defcon_Priorita;
		public Transform Posizione_Priorita;

	}
		
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
		targetDistance = 2.4f;
		attackCost = 2; 
		AttivoSetTarget = false;
		//abbiamo variabile torre anche se non la vediamo scritta qui

    }

	void Awake()
	{

		to_do = new List<priorita>();
		ogg_Gestore = GameObject.FindGameObjectWithTag ("GameManager");
		gestore = ogg_Gestore.GetComponent<GestoreGioco> ();
		seek = GetComponent<Seeker>();
		AggiornaListaPriorita ();
		IA = gameObject.GetComponent<AILerp> ();

		gameObject.name = gameObject.name + gestore.contatore_nemico.ToString ();
		gestore.contatore_nemico++;

		Radar = gameObject.transform.GetChild (1).GetComponent<BoxCollider> ();

	}

	void Start()
	{

		//Quando istanziamo l'oggetto va aggiunto alla lista dei personaggi presenti in gioco
		gestore.AggiungereDatoListaPersonaggi (gameObject.tag, currentSteps, gameObject.name, gameObject);

		Attendi ();
		//gameObject.transform.GetChild (1).gameObject.SetActive (true);
		SetRadarSize (Radar);
		//gameObject.transform.GetChild (1).gameObject.SetActive (false);
		SetTarget ();

	}


	void Update()
	{

		if (gestore.IsFaseCombattimento () == true && currentSteps > 1) 
		{

			//Avvia ();
			semaforo = true;

		} 
		else 
		{

			if (aggiorna == true) 
			{

				aggiorna = false;
				t = 0;
				currentSteps = steps;
				Debug.Log ("current Steps: " + currentSteps);


				//DEPRECATO
				//gestore.AggiornaDatoListaPersonaggi (gameObject.name, 0, gameObject.tag, steps);


			}
				

		}

		if (currentSteps == 1 && gestore.IsFaseCombattimento () == false ) 
		{

			if (gestore.GetTurno () % 2 != 0 && gameObject.layer == LayerMask.NameToLayer ("Player 1")) {

				aggiorna = true;

			}
			if (gestore.GetTurno () % 2 == 0 && gameObject.layer == LayerMask.NameToLayer ("Player 2")) {

				aggiorna = true;

			}

			//DEPRECATO
			//gestore.AggiornaDatoListaPersonaggi (gameObject.name, steps, gameObject.tag, currentSteps);

		}

		if (currentSteps == 1 && semaforo == true) {


			semaforo = false;

			gestore.AggiornaPersonaggiFermi ();


		}
			

		if (AttivoSetTarget == true) {

			AttivoSetTarget = false;
			SetTarget ();

		}

	}

	//Aggiorno la lista delle priorita
	public void AggiornaListaPriorita()
	{

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

		//Aggiungiamo nelle priorità il nemico 
		to_do.Add (GetNemico());
	}

	//Metodo che ritorna il nemico come target
	private priorita GetNemico()
	{
		
		List<string> listaNemici = new List<string>();

		//Attivo il radar
		//gameObject.GetComponentInChildren<Collider> ().gameObject.SetActive(true);

		//Passo la lista di nemici a una lista interna alla classe
		if (gameObject.GetComponentInChildren<Radar>().Lista_Nome_Nemico.Count > 0) 
		{

			listaNemici = gameObject.GetComponentInChildren<Radar> ().Lista_Nome_Nemico;
		

		} 
		else 
		{

			Debug.LogError ("Target nullo");

			priorita dato = new priorita ();

			dato.Nome_Priorita = "Nemici";
			dato.Defcon_Priorita = Nemici;
			dato.Posizione_Priorita = null;

			return dato;
		}

		//Disattivo il radar
		//gameObject.GetComponentInChildren<Collider> ().gameObject.SetActive(false);

		//PARTE DEICISIONALE DELLA IA IN CUI VALUTA IL NEMICO MIGLIORE DA SELEZIONARE 
		//PER ORA FACCIAMO QUELLO PIU' VICINO 

		int pathVicino = 0;
		Transform target = null;
		int j = 0;
		//calcoliamo il target più vicino
		for(int i = 0; i < listaNemici.Count; i++)
		{
			
			//IA.target.position = gestore.CercaPerNomeLaTransform (listaNemici [i]).position; 
			Path p = seek.StartPath (transform.position,gestore.CercaPerNomeLaTransform (listaNemici [i]).position);
			p.BlockUntilCalculated ();

			if (p.vectorPath.Count < pathVicino && pathVicino != 0) 
			{

				pathVicino = p.vectorPath.Count;
				j = i;

			}
			else
			{ 
				//Assegno il primo valore della lista di nomi perchè se no rimane zero!!!
				pathVicino = p.vectorPath.Count;

			}
		}

		//target.position = gestore.CercaPerNomeLaTransform (listaNemici [0]).position; 
		priorita dato2 = new priorita ();

		dato2.Nome_Priorita = "Nemici";
		dato2.Defcon_Priorita = Nemici;
		dato2.Posizione_Priorita = gestore.CercaPerNomeLaTransform (listaNemici [j]);


		Debug.LogError ("il target nemico : " + target);

		//Resetto la lista di nemici nel radar
		gameObject.GetComponentInChildren<Radar> ().ResetListaNemici();

		return dato2;


	}

	//calcolo della priorita principale
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

	//Settare il target dato dalla priorita
	public void SetTarget()
	{

		IA.target = GetPriorita();

	}


	//mettiamo in attesa il nostro personaggio
	public void Attendi()
	{

		IA.canMove = false;
		IA.canSearch = false;

	}

	public void Avvia()
	{

		IA.canMove = true;
		IA.canSearch = true;

	}

	//Metodo per aggiornare gli steps rimanenti 
	private void AggiornaStepsCorrenti()
	{

		/*usiamo la classe Path della libreria Pathfinding e assegnamo seek.StartPath per stabilire 
		 * un percorso iniziale e finale (transform), passiamo i nodi ad una lista inizializzandola a -1
		 * per non far contare la posizione di partenza, ad ogni nodo richiamiamo OnTargetReached()
		 */


		if (currentSteps > 1) 
		{
			
			currentSteps--;
			Debug.Log (currentSteps);


		}
		else 
		{
			Attendi ();
			Debug.Log ("ATTENDO");

		}

	}


	void OnTriggerEnter(Collider altro)
	{

		float distance = 0;

		if (to_do [2].Posizione_Priorita.position != null)
		{
			try
			{
			 	distance = Vector3.Distance (altro.transform.position, to_do [2].Posizione_Priorita.position);
		

				if (altro.tag == "Casella" ) 
				{
					if (distance <= targetDistance)
					{
						Attendi ();
					}
					//Debug.Log ("Sorry");
					//IA.OnTargetReached();
					AstarPath.active.Scan ();
					AggiornaStepsCorrenti ();
					altro.gameObject.layer = 8;
					Attendi ();

				}
			}
			catch 
			{

				Debug.LogError ("Operazione non consentita");

				if (altro.tag == "Casella" ) 
				{
					
					//Debug.Log ("Sorry");
					//IA.OnTargetReached();
					AstarPath.active.Scan ();
					AggiornaStepsCorrenti ();
					altro.gameObject.layer = 8;
					Attendi ();

				}

			}
		}

	}

	void OnTriggerExit(Collider altro)
	{


		altro.gameObject.layer = 0;

	}


}