using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class GestoreGioco : MonoBehaviour {

	private int Turni;
	private int Energia1;
	private int Energia2;
	private HUD schermata;
	private bool FaseCombattimento = false;

	[Header("Posizioni torri PLAYER 1")]
	//torri player 1
	public Transform Torre1_Player1;
	public Transform Torre2_Player1;
	public Transform Torre3_Player1;

	[Header("Posizioni torri PLAYER 2")]
	//torri player 2
	public Transform Torre1_Player2;
	public Transform Torre2_Player2;
	public Transform Torre3_Player2;

	[Header("Nexus ambo PLAYERS")]
	//torri player 2
	public Transform Nexus_Player1;
	public Transform Nexus_Player2;

	//lista Personaggi Player 1
	public List<personaggio> Personaggi_P1;
	private int Personaggi_fermiP1 = 0;

	//lista Personaggi Player 2
	public List<personaggio> Personaggi_P2;
	private int Personaggi_fermiP2 = 0;

	//Contatore per il nome dei nemici
	public int contatore_nemico = 0;

	//Timer per il movimento dei nemici casella per casella
	private float timer;
	public float SetTimer = 0.3f;

	private bool isActive;

	private int indiceLista_P1 =0;

	private int indiceLista_P2 =0;

	//VARIABILI PER MYDEBUG
	public bool GestioneTurni_controllo = true;

	public class personaggio
	{

		public string Tag;
		public int Current_step;
		public string Nome;
		public GameObject Oggetto;

	}


	void Awake()
	{

		Turni = 1;
		Energia1 = 10;
		Energia2 = 10;
		contatore_nemico = 0;
		Personaggi_P1 = new List<personaggio> ();
		Personaggi_P2 = new List<personaggio> ();
		timer = SetTimer;

		schermata = gameObject.GetComponent<HUD> ();

	}

	void Start()
	{





	}

	void Update()
	{

		//Serve per resettare gli indici
		if (indiceLista_P1 > Personaggi_P1.Count) 
		{
			indiceLista_P1 = 0;
		}

		if (indiceLista_P2 > Personaggi_P2.Count) 
		{
			indiceLista_P2 = 0;
		}

	
		//Avvio la fase di combattimento
		if (Input.GetKeyDown (KeyCode.Space)) 
		{

			FaseCombattimento = true;

			if(Turni%2 == 0)
			{ //Player 2

				 
				//Avvio coroutine per la gestione del movimento del player 2
				StartCoroutine (GestioneTurniP2 ());

			}
			else
			{//Player 1

				//Avvio coroutine per la gestione del movimento del player 1
				StartCoroutine (GestioneTurniP1 ());

			}


		}

		//Avvio in MANUALE il cambio turno
		if(Input.GetKeyDown(KeyCode.Q))
		{

			CambioTurno ();
			StopCoroutine (GestioneTurniP1 ());
			StopCoroutine (GestioneTurniP2 ());

		}



	}

	//Permette il movimento dei personaggi P1 all'interno della scena di gioco 
	IEnumerator GestioneTurniP1()
	{
		GameObject oggetto;

		while (Personaggi_fermiP1 <= Personaggi_P1.Count-1) 
		{
			indiceLista_P1 = 0;

			while (indiceLista_P1 <= Personaggi_P1.Count - 1) 
			{
				AstarPath.active.Scan ();
				oggetto = GameObject.Find (Personaggi_P1 [indiceLista_P1].Nome);
				oggetto.GetComponent<Assassin> ().AggiornaListaPriorita();
				oggetto.GetComponent<Assassin> ().SetTarget ();
				AstarPath.active.Scan ();

				for(int i = 0; i < Personaggi_P1 [indiceLista_P1].Current_step; i++) 
				{
					float distance = Vector3.Distance (oggetto.GetComponent<Characters> ().transform.position, oggetto.GetComponent<AILerp> ().target.position);
					if (distance > oggetto.GetComponent<Characters> ().targetDistance) 
					{
						Debug.Log ("Sono entrato nell'if");
						oggetto.GetComponent<AILerp> ().canMove = true;
						oggetto.GetComponent<AILerp> ().canSearch = true;
						yield return new WaitForSeconds (0.3f);
					} 
					else 
					{

						Debug.Log ("Sono entrato nell'else");

						if (oggetto.GetComponent<Characters> ().currentSteps >= oggetto.GetComponent<Characters> ().attackCost)
						{

							Debug.Log ("Sono entrato nell'if per eseguire l'attacco");
							// attacco
							//istanziare coroutine
							yield return StartCoroutine(AttaccoPersonaggio(oggetto));

							//distruggere coroutine
							//StopCoroutine(AttaccoPersonaggio);
						}
						yield return new WaitForSeconds (0.3f);
					}
				}

				indiceLista_P1++;

			}
		}

		indiceLista_P1 = 0;

		yield return new WaitForSeconds (0.3f);

	}

	//Permette il movimento dei personaggi P2 all'interno della scena di gioco 
	IEnumerator GestioneTurniP2()
	{
		GameObject oggetto;

		while (Personaggi_fermiP2 <= Personaggi_P2.Count-1) 
		{
			indiceLista_P2 = 0;

			while (indiceLista_P2 <= Personaggi_P2.Count - 1) 
			{

				AstarPath.active.Scan ();
				oggetto = GameObject.Find (Personaggi_P2 [indiceLista_P2].Nome);
				oggetto.GetComponent<Assassin> ().AggiornaListaPriorita();
				oggetto.GetComponent<Assassin> ().SetTarget ();
				AstarPath.active.Scan ();

				for(int i = 0; i < Personaggi_P2 [indiceLista_P2].Current_step; i++) 
				{

					float distance = Vector3.Distance (oggetto.GetComponent<Characters> ().transform.position, oggetto.GetComponent<AILerp> ().target.position);
					if (distance > oggetto.GetComponent<Characters> ().targetDistance) {
						oggetto.GetComponent<AILerp> ().canMove = true;
						oggetto.GetComponent<AILerp> ().canSearch = true;
						yield return new WaitForSeconds (0.3f);
					} 
					else 
					{
						if (oggetto.GetComponent<Characters> ().currentSteps >= oggetto.GetComponent<Characters> ().attackCost)
						{
							// attacco
							//istanziare coroutine
							yield return StartCoroutine(AttaccoPersonaggio(oggetto));

							//distruggere coroutine
							//StopCoroutine(AttaccoPersonaggio);
						}
						yield return new WaitForSeconds (0.3f);
					}
				}

				indiceLista_P2++;

			}
		}

		indiceLista_P2 = 0;

		yield return new WaitForSeconds (0.3f);

	}

	//Coroutine per l'attacco del personaggio 
	IEnumerator AttaccoPersonaggio(GameObject oggetto)
	{

		oggetto.GetComponent<Characters> ().currentSteps--;
		Debug.Log ("Attacco");

		yield return null;


	}

	//Coroutine per la scqansione costante della mappa
	IEnumerator ScanObstacle()
	{

		while (true) {

			AstarPath.active.Scan ();
			yield return null;

		}

	}

	// metodo che cambia il turno 
	public void CambioTurno()
	{     

		Debug.Log ("cambio turno");
		StopCoroutine (ScanObstacle ());

		Turni++;
		schermata.AggiornaTurnoAschermo ();
		Energia1 = 10;
		schermata.AggiornaEnergiaPlayer1 ();
		Energia2 = 10;
		schermata.AggiornaEnergiaPlayer2 ();
		FaseCombattimento = false;
		//Resettiamo il numero di personaggi fermi 
		ResetPersonaggiFermi ();

	}

	//ritorna il turno corrente
	public int GetTurno()
	{

		return Turni;

	}

	//ritorna energia player 1
	public int GetEnergiaPlayer1()
	{

		return Energia1;

	}

	//ritorna energia player2
	public int GetEnergiaPlayer2()
	{

		return Energia2;

	}

	public void SottraiEnergia(int consumo)
	{

		if ( Turni % 2 == 0) 
		{//pari

			Energia2 = Energia2 - consumo;
			schermata.AggiornaEnergiaPlayer2 ();

		} 
		else //dispari
		{

			Energia1 = Energia1 - consumo;
			schermata.AggiornaEnergiaPlayer1 ();

		}

	}

	//ci dice se siamo in fase di combattimento oppure no
	public bool IsFaseCombattimento()
	{

		return FaseCombattimento;

	}

	//Aggiungiamo un personaggio alla lista dei player a seconda del turno
	public void AggiungereDatoListaPersonaggi(string tag, int current_step, string nome, GameObject oggetto)
	{

		personaggio dato = new personaggio ();

		dato.Current_step = current_step;
		dato.Tag = tag;
		dato.Nome = nome;
		dato.Oggetto = oggetto;

		if (Turni % 2 == 0) 
		{//player 2

			Personaggi_P2.Add (dato);

		} 
		else //player 1
		{

			Personaggi_P1.Add (dato);

		}

	}

	//Aggiorniamo le liste dei personaggi dei singoli player a seconda del turno
	/*public void AggiornaDatoListaPersonaggi(string nome, int steps, string tag, int current_steps)
	{
		
		personaggio dato = new personaggio ();

		dato.Current_step = steps;
		dato.Nome = nome;
		dato.Tag = tag;

		//azione pericolosa 
		try
		{
			if (Turni % 2 == 0) 
			{//player 2

				int indice;

				dato = new personaggio();

				dato.Current_step = current_steps;
				dato.Nome = nome;
				dato.Tag = tag;

				indice = Personaggi_P2.IndexOf (dato);
				Personaggi_P2 [indice] = dato;



			} 
			else //player 1
			{

				int indice;

				indice = Personaggi_P1.IndexOf (dato);

				dato = new personaggio();

				dato.Current_step = current_steps;
				dato.Nome = nome;
				dato.Tag = tag;

				Personaggi_P1 [indice] = dato;

			}
		}
		catch
		{

			Debug.LogError ("Errore nell'aggiornamento dei dati della lista");

		}

	}*/

	//Rimuoviamo un personaggio dalla lista dei personaggi dei singoli player
	public void RimuoviDatoListaPersonaggi(string nome)
	{

		if (Turni % 2 == 0) 
		{//player 2

			Personaggi_P2.Remove(CercaPerNome(nome));

		}   
		else //player 1
		{

			Personaggi_P1.Remove(CercaPerNome(nome));

		}

	}

	//ritorna un oggetto della lista con il nome che gli passi
	public personaggio CercaPerNome(string nome)
	{

		if (Turni % 2 == 0) 
		{//player 2

			for (int i = 0; i < Personaggi_P2.Count; i++) {

				if (Personaggi_P2 [i].Nome == nome) {


					return Personaggi_P2 [i];

				}

			}
				
		} 
		else //player 1
		{
			
			for (int i = 0; i < Personaggi_P1.Count; i++) {

				if (Personaggi_P1 [i].Nome == nome) {


					return Personaggi_P1 [i];

				}

			}

		}

		Debug.LogError ("non ho trovato il personaggio nelle liste");

		return null;



	}

	//ritorna un oggetto della lista con il nome che gli passi
	public Transform CercaPerNomeLaTransform(string nome)
	{

		if (Turni % 2 == 0) 
		{//player 2

			for (int i = 0; i < Personaggi_P1.Count; i++) {

				if (Personaggi_P1 [i].Nome == nome) {


					return Personaggi_P1 [i].Oggetto.transform;

				}

			}

		} 
		else //player 1
		{

			for (int i = 0; i < Personaggi_P2.Count; i++) {

				if (Personaggi_P2 [i].Nome == nome) {


					return Personaggi_P2 [i].Oggetto.transform;

				}

			}

		}

		Debug.LogError ("non ho trovato il personaggio nelle liste");

		return null;



	}

	//Aggiorniamo la lista dei personaggi a secondo del turno corrente 
	public void AggiornaListaPersonaggi()
	{

		if (Turni % 2 == 0) 
		{
			//Player 2
		
			Personaggi_P2.Clear ();

			Debug.Log ("Lista P1 Aggiornata");
		} 
		else 
		{
			//Player 1

			Personaggi_P1.Clear ();

			Debug.Log ("Lista P2 Aggiornata");

		}

	}

	//Muoviamo il singolo nemico alla volta a secondo del turno
	/*public void MuoviSingoloNemico()
	{


		GameObject oggetto;

		if (Turni % 2 == 0) 
		{
			//Player 2

			for (int i = 0; i < Personaggi_P2.Count; i++) 
			{

				oggetto = GameObject.Find (Personaggi_P2 [i].Nome);

				//WaitForSeconds (0.1f);

				if (oggetto.GetComponent<Characters> ().currentSteps > 1) 
				{

					oggetto.GetComponent<AILerp> ().canMove = true;
					oggetto.GetComponent<AILerp> ().canSearch = true;
					isActive = false;

				}

			}

		} 
		else 
		{
			//Player 1


			for (int i = 0; i < Personaggi_P1.Count; i++) 
			{

				oggetto = GameObject.Find (Personaggi_P1 [i].Nome);

				//WaitForSeconds (0.1f);

				if (oggetto.GetComponent<Characters> ().currentSteps > 1) 
				{

					oggetto.GetComponent<AILerp> ().canMove = true;
					oggetto.GetComponent<AILerp> ().canSearch = true;
					isActive = false;

				}

			}


		}

	}*/


	//ritorna il numero di personaggi correnti che sono fermi P1
	public int GetPersonaggiP1Fermi()
	{

		return Personaggi_fermiP1;

	}

	//ritorna il numero di personaggi correnti che sono fermi P1
	public int GetPersonaggiP2Fermi()
	{

		return Personaggi_fermiP2;

	}

	//Aggiorniamo il numero di personaggi che sono fermi P1
	public void AggiornaPersonaggiFermi()
	{

		if (Turni % 2 == 0) 
		{//player 2

		
			Personaggi_fermiP2++;

			//dopo l'aggiornamento contorllo se tutti i personaggi sono fermi 
			if (ControlloPersonaggiFermi () == true) {

				CambioTurno ();
			}

		} 
		else //player 1
		{


			Personaggi_fermiP1++;

			//dopo l'aggiornamento contorllo se tutti i personaggi sono fermi 
			if (ControlloPersonaggiFermi () == true) {

				CambioTurno ();

			}


		}

	}

	public void ResetPersonaggiFermi()
	{

		if (Turni % 2 == 0) 
		{//player 2


			Personaggi_fermiP2 = 0;

		} 
		else //player 1
		{


			Personaggi_fermiP1 = 0;


		}

	}

	//eseguo un controllo per il quale controllo se tutti i personaggi sono fermi oppure no
	public bool ControlloPersonaggiFermi()
	{

		if (Turni % 2 == 0) 
		{//player 2

			if (Personaggi_P2.Count == Personaggi_fermiP2) {


				Debug.Log ("Tutti i personaggi P1 sono fermi");

				return true;

			}

			return false;

		} 
		else //player 1
		{

			if (Personaggi_P1.Count == Personaggi_fermiP1) {


				Debug.Log ("Tutti i personaggi P2 sono fermi");

				return true;

			}

			return false;

		}

		Debug.LogError ("non ho trovato il personaggio nelle liste");

		return false;

	}

	/*IEnumerator GestioneTurni()
	{
		GameObject oggetto;

		//try
		//{
		//Player 1
		if (Turni % 2 != 0)
		{
			while (Personaggi_fermiP1 <= Personaggi_P1.Count-1) 
			{
				indiceLista_P1 = 0;

				while (indiceLista_P1 <= Personaggi_P1.Count - 1) 
				{

					oggetto = GameObject.Find (Personaggi_P1 [indiceLista_P1].Nome);

					if (oggetto.GetComponent<Characters> ().currentSteps > 1) {

						AstarPath.active.Scan ();
						yield return new WaitForSeconds (0.2f);
						oggetto.GetComponent<AILerp> ().canMove = true;
						oggetto.GetComponent<AILerp> ().canSearch = true;
						//ScanObstacle ();
						indiceLista_P1++;
						yield return new WaitForSeconds (0.6f);

					}

				}
			}

			yield return new WaitForSeconds (0.6f);
		}
		//Player 2
		if (Turni % 2 == 0)
		{
			while (Personaggi_fermiP2 <= Personaggi_P2.Count-1) 
			{
				indiceLista_P2 = 0;

				while (indiceLista_P2 <= Personaggi_P2.Count - 1) 
				{

					oggetto = GameObject.Find (Personaggi_P2 [indiceLista_P2].Nome);

					if (oggetto.GetComponent<Characters> ().currentSteps > 1) {

						AstarPath.active.Scan ();
						yield return new WaitForSeconds (0.2f);
						oggetto.GetComponent<AILerp> ().canMove = true;
						oggetto.GetComponent<AILerp> ().canSearch = true;
						//ScanObstacle ();
						indiceLista_P2++;
						yield return new WaitForSeconds (0.6f);

					}

				}
			}

			yield return new WaitForSeconds (0.6f);

		}

		//}
		catch 
		{

			Debug.LogError ("ERRORE COROUTINE GestioneTurni");
			GestioneTurni_controllo = false;


		}


		//yield return new WaitForSeconds (0.3f);

	}*/
}
