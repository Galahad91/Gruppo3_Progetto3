using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.UI;

public class GestoreGioco : MonoBehaviour {
	


	public int Turni;							//Numero di turni di gioco
	private int Energia1; 						//Energia spendibile dal Player 1
	private int Energia2;						//Energia spendibile dal Player 2					
	private bool FaseCombattimento = false;		//Variabile che indica se il gameplay si trova nella fase combattimento oppure no

	//---------------------------------------------------------------------------------- LISTA DI CLIP AUDIO 

	[Header("Musiche del gioco per i Designer")]
	public List<Brano> Musica; 
	[System.Serializable]
	public class Brano
	{

		[Header("Canzone/ Suono")]
		public AudioClip Clip_Audio;
		[Range(1,10)]
		public int Volume;
		[Header("Solo programmatori")]
		public int riferimento;
		[Header("Descrizione canzone")]
		public string Descrizione;

	}

	//-------------------------------- FINE

	private HUD schermata;	
	[Header("Oggetti padri per la Canvas")]
	public GameObject P1_HUD;
	public GameObject P2_HUD;

	[Header("Caselle spawn Player 1")]
	public GameObject[] Caselle_P1;

	[Header("Caselle spawn Player 2")]
	public GameObject[] Caselle_P2;

	//POSIZIONI FISSE ALL'INTERNO DELLA SCENA DELLE TORRI DEL PLAYER 1
	//[Header("Posizioni torri PLAYER 1")]
	//torri player 1
	private Transform Torre1_Player1;
	private Transform Torre2_Player1;
	private Transform Torre3_Player1;

	//POSIZIONI FISSE ALL'INTERNO DELLA SCENA DELLE TORRI DEL PLAYER 2
	//[Header("Posizioni torri PLAYER 2")]
	//torri player 2
	private Transform Torre1_Player2;
	private Transform Torre2_Player2;
	private Transform Torre3_Player2;

	//POSIZIONI FISSE ALL'INTERNO DELLA SCENA DEI NEXUS PLAYER 1 E PLAYER 2
	[Header("Nexus ambo PLAYERS")]
	//torri player 2
	public Transform Nexus_Player1;
	public Transform Nexus_Player2;

	//Timer per il movimento dei nemici casella per casella
	//private float timer;
	private float SetTimer = 0.3f; 

	private bool isActive;

	[Header("NON TOCCARE!")]
	public int contatore_nemico = 0;	//variabile per assegnare un nome univoco al giocatore che viene istanziato

	//VARIABILI PER MYDEBUG
	//public bool GestioneTurni_controllo = true;

	//THREAD
	private Thread T_Scan;
	private bool _t1Pausa = false;

	[Header("Variabile per la duarata del turno")]
	public float Durata_Turno = 20;
	private float timer = 0;
	[Header("TIMER A SCHERMO")]
	public Text Timer_Testo;


	//COROUTINE
	//[Header("Ogni quanto eseguire lo Scan della Mappa")]
	private float Tempo_Scan = 3;

	[Header("Energia da assegnare ad ogni passaggio di turno ad ambo i giocatori")]
	public int Energia;

	//VARIABILE TEMPORANEA PER FAR FUNZIONARE LO STOP
	private int NemiciFermi = 0;

	//---------------------------------------------------------------------------------------------------- CODICE LISTE PERSONAGGI

												//lista Personaggi Player 1
												public List<personaggio> Personaggi;
												private int Personaggi_fermiP1 = 0;

												//Classe che definisce tutte le informazioni di un personaggio
												public class personaggio
												{

													public string Tag; 				//tag del personaggio
													public int Steps;				//Steps Totali del personaggio
													public int Current_step;		//Steps Correnti del personaggio --- Vedere se è utile
													public string Nome;				//Nome del personaggio
													public GameObject Oggetto;		//GameOgject del personaggio

													//Parametro molto importante 
													public bool appartengo;         //	FALSO = Player1 - VERO = Player 2

												}

	#region METODI_LISTE

		/// <summary>
		/// Aggiungiamo un personaggio alla lista dei Personaggi in gioco
		/// </summary>
		/// <param name="tag">Tag del personaggio</param>
		/// <param name="steps">Steps del personaggio </param>
		/// <param name="current_step">Current step del personaggio</param>
		/// <param name="nome">Nome del personaggio</param>
		/// <param name="oggetto">Il GameOject del personaggio stesso</param>
		/// <param name="appartengo">FALSE = Player 1 - TRUE = Player 2</param>
		public void AggiungereDatoListaPersonaggi(string tag, int steps ,int current_step, string nome, GameObject oggetto, bool appartengo)
		{

			personaggio dato = new personaggio ();

			dato.Tag = tag;
			dato.Current_step = current_step;
			dato.Steps = steps;
			dato.Nome = nome;
			dato.Oggetto = oggetto;
			dato.appartengo = appartengo;

			Personaggi.Add (dato);

		}

		/// <summary>
		/// Rimuoviamo un personaggio dalla lista dei personaggi dei singoli player
		/// </summary>
		/// <param name="nome">Nome del personaggio che vogliamo rimuovere</param>
		public void RimuoviDatoListaPersonaggi(string nome)
		{

			Personaggi.Remove(CercaPerNome(nome));

		}


		/// <summary>
		/// ritorna un oggetto della lista con il nome che gli passi
		/// </summary>
		/// <returns> Ritorna un oggetto di tipo personaggio</returns>
		/// <param name="nome">Nome del personaggio che stiamo cercando</param>
		public personaggio CercaPerNome(string nome)
		{

			for (int i = 0; i < Personaggi.Count; i++) {

				if (Personaggi[i].Nome == nome) {


					//Debug.Log ("Personaggio per nome trovato");

					return Personaggi[i];

				}

			}

			Debug.LogError ("non ho trovato il personaggio nelle liste");

			return null;



		}


		/// <summary>
		/// Aggiorniamo la lista dei personaggi
		/// </summary>
		public void AggiornaListaPersonaggi()
		{

			Personaggi.Clear ();
			Debug.Log ("Lista aggiornata");

		}



		/// <summary>
		/// ritorna il numero di personaggi correnti che sono fermi 
		/// </summary>
		/// <returns> numero dei personaggi fermi</returns>
		/// <param name="appartengo">FALSE = Player 1 - TRUE = Player 2</param>
		public int GetPersonaggiFermi(bool appartengo)
		{

			int PersonaggiFermi = 0;

			for (int i = 0; i < Personaggi.Count; i++) 
			{

				if (Personaggi[i].appartengo == appartengo && Personaggi[i].Current_step == 0) 
				{

					PersonaggiFermi++;

				}

			}

			if( PersonaggiFermi > 1)
			{

				return PersonaggiFermi;

			}

			Debug.Log ("Tutti i personaggi si devono ancora muovere o non ce ne sono");

			return 0;

		}


		/// <summary>
		/// Aggiorniamo il numero di personaggi che sono fermi
		/// </summary>
		/// <param name="nome">Nome del personaggio da aggiornare</param>
		/// <param name="currentSteps">Current steps del personaggio</param>
		public void AggiornaPersonaggioFermo(string nome, int currentSteps)
		{

			CercaPerNome (nome).Current_step = currentSteps;

			Debug.Log ("Aggiornato il personaggio: " + nome);


		}


		/// <summary>
		/// Ritorna il numero di personaggi in gioco di un Player
		/// </summary>
		/// <returns>The numero personaggi fazione.</returns>
		/// <param name="appartengo">If set to <c>true</c> appartengo.</param>
		public int GetNumeroPersonaggiFazione(bool appartengo)
		{

			int NumeroGiocatori = 0;

			for (int i = 0; i < Personaggi.Count; i++) 
			{

				if (Personaggi [i].appartengo == appartengo) 
				{

					NumeroGiocatori++;

				}

			}

			if (NumeroGiocatori > 0) 
			{

				return NumeroGiocatori;

			}

			if (appartengo == true) 
			{
				//Player2

				Debug.Log ("Il Player 2 non ha personaggi in gioco");

				return 0;
			}
			else 
			{

				//Player1

				Debug.Log ("Il Player 1 non ha personaggi in gioco");

				return 0;

			}

		}


		/// <summary>
		/// eseguo un controllo per il quale controllo se tutti i personaggi sono fermi oppure no
		/// </summary>
		/// <returns> Ritorna TRUE sono tutti i personaggi di un Player sono fermi </returns>
		/// <param name="appartengo">FALSE = Player 1 - TRUE = Player 2</param>
		public bool ControlloTuttiPersonaggiFermi(bool appartengo)
		{

			if (GetNumeroPersonaggiFazione(appartengo) == GetPersonaggiFermi(appartengo)) 
			{

				/*if (appartengo == true) 
				{
					//Player 2

					Debug.Log ("Tutti i personaggi del Player 2 sono fermi");

				} 
				else 
				{
					//Player1

					Debug.Log ("Tutti i personaggi del Player 1 sono fermi");
				}*/

				return true;

			}

			Debug.LogWarning ("Ci sono ancora personaggi che si devono muovere");

			return false;

		}

	#endregion  

	//---------------------------------------------------------------------------------------------------- FINE


	void Awake()
	{

		//INIZIALIZZO LE VARIABILI DI GIOCO
		Turni = 1;
		Energia1 = 10;
		Energia2 = 10;
		Personaggi = new List<personaggio> ();
		timer = SetTimer;
		schermata = gameObject.GetComponent<HUD> ();

	}

	void Start()
	{

		//AVVIO THREAD PER LO SCAN
		//AvvioThread_ScanObstacle ();

		//Avvio corotuine per scansionare la mappa all'infinito
		//StartCoroutine (ScanMappa ());

	}

	void Update()
	{
	
		//Avvio la fase di combattimento
		/*if (Input.GetKeyDown (KeyCode.Space)) 
		{

			FaseCombattimento = true;
			StartCoroutine (GestioneCombattimento ());


		}*/

		//Timer della durata del turno 
		if (FaseCombattimento == true) 
		{

			timer += Time.deltaTime;

			//Visualiziamo a schermo il timer del turno
			//Timer_Testo.text = "TIMER TURNO: " + timer.ToString ();

			//Quando scade il tempo passiamo il turno
			if (timer >= Durata_Turno) 
			{
				timer = 0;
				CambioTurno ();

			}

		}

		//DA USARE SOLO PER FORZARE
		//Avvio in MANUALE il cambio turno
		if(Input.GetKeyDown(KeyCode.Q))
		{

			Debug.Log ("FORZATURA: Fermiamo le coroutine e cambiamo turno");

			//Fermiamo tutte le coroutine
			StopAllCoroutines ();
			CambioTurno ();

		}



	}

	#region THREAD

		private void AvvioThread_ScanObstacle()
		{

			T_Scan = new Thread (ScanObstacle) { Name = "Thread Scan" };

			if (!T_Scan.IsAlive) 
			{

				Debug.Log ("THREAD SCAN AVVIATO");
				T_Scan.Start ();

			} 
			else 
			{

				Debug.LogError ("IMPOSSIBILE CHE IL TREAD: " + T_Scan.Name + " SIA AVVIATO 2 VOLTE");


			}

		}

		/// <summary>
		/// Thread per la scqansione costante della mappa.
		/// </summary>
		/// <returns>The obstacle.</returns>
		private static void ScanObstacle()
		{
			
			//contatore di volte 
			int i = 0;

			while(true)
			{

				Debug.Log ("SCAN n°"+i++);
				AstarPath.active.Scan ();

				Thread.Sleep (3000);

			}


		}

	#endregion

	#region COROUTINE

		/// <summary>
		/// Coroutine che si occupa della gestione compessiva di un singolo turno di combattimento 
		/// </summary>
		/// <returns>The combattimento.</returns>
		IEnumerator GestioneCombattimento()
		{

			if (FaseCombattimento == true) 
			{

				Debug.Log ("FASE DI COMBATTIMENTO");

				//StartCoroutine (AttaccoTorri ());
				//yield return new WaitForSeconds (7f);

				if (Turni % 2 == 0) 
				{
					//Player 2
					//StartCoroutine (AttaccoNemici (true));

				} 
				else 
				{
					//Player 1
					//StartCoroutine (AttaccoNemici (false));
				}
			} 
			else 
			{

				Debug.LogError ("Sono nella coroutine di combattimento senza che siamo in combattimento");
				

			}
			

			yield return null;
		}

		/// <summary>
		/// coroutine che gestisce l'attacco delle torri
		/// </summary>
		/// <returns>The torri.</returns>
		IEnumerator AttaccoTorri()
		{

			Debug.Log ("INIZIO ATTACCO TORRI");

			if (Turni % 2 != 0) 
			{
				//PLAYER 2

				//Controllo se il Nexus e le Torri sono ancora in scena 
				//Eseguiamo i vari attacchi
				if (Torre1_Player2.gameObject.scene != null || Torre1_Player2.gameObject.GetComponent<Structures>().currentHealth > 0) 
				{

					Structures T1;
					T1 = Torre1_Player2.gameObject.GetComponent<Structures> ();

					T1.AttivaRadar();
					yield return new WaitForSeconds (1f);
					T1.gameObject.GetComponent<Structures> ().Attacco ();
					T1.DisattivaRadar ();


				}
				if (Torre2_Player2.gameObject.scene != null || Torre2_Player2.gameObject.GetComponent<Structures>().currentHealth > 0) 
				{

					Structures T2;
					T2 = Torre2_Player2.gameObject.GetComponent<Structures> ();

					T2.AttivaRadar();
					yield return new WaitForSeconds (1f);
					T2.gameObject.GetComponent<Structures> ().Attacco ();
					T2.DisattivaRadar ();

				}
				if (Torre3_Player2.gameObject.scene != null || Torre3_Player2.gameObject.GetComponent<Structures>().currentHealth > 0) 
				{

					Structures T3;
					T3 = Torre3_Player2.gameObject.GetComponent<Structures> ();

					T3.AttivaRadar();
					yield return new WaitForSeconds (1f);
					T3.gameObject.GetComponent<Structures> ().Attacco ();
					T3.DisattivaRadar ();

				}
				if (Nexus_Player2.gameObject.scene != null || Nexus_Player2.gameObject.GetComponent<Structures>().currentHealth > 0) 
				{

					Structures N;
					N = Nexus_Player1.gameObject.GetComponent<Structures> ();

					N.AttivaRadar();
					yield return new WaitForSeconds (1f);
					N.gameObject.GetComponent<Structures> ().Attacco ();
					N.DisattivaRadar ();

				}
				


			} 
			else 
			{

				//PLAYER 1

				//Controllo se il Nexus e le Torri sono ancora in scena 
				//Eseguiamo i vari attacchi
				if (Torre1_Player1.gameObject.scene != null || Torre1_Player1.gameObject.GetComponent<Structures>().currentHealth > 0) 
				{

					Structures T1;
					T1 = Torre1_Player1.gameObject.GetComponent<Structures> ();

					T1.AttivaRadar();
					yield return new WaitForSeconds (1f);
					T1.gameObject.GetComponent<Structures> ().Attacco ();
					T1.DisattivaRadar ();


				}
				if (Torre2_Player1.gameObject.scene != null || Torre2_Player1.gameObject.GetComponent<Structures>().currentHealth > 0) 
				{

					Structures T2;
					T2 = Torre2_Player1.gameObject.GetComponent<Structures> ();

					T2.AttivaRadar();
					yield return new WaitForSeconds (1f);
					T2.gameObject.GetComponent<Structures> ().Attacco ();
					T2.DisattivaRadar ();

				}
				if (Torre3_Player1.gameObject.scene != null || Torre3_Player1.gameObject.GetComponent<Structures>().currentHealth > 0) 
				{

					Structures T3;
					T3 = Torre3_Player1.gameObject.GetComponent<Structures> ();

					T3.AttivaRadar();
					yield return new WaitForSeconds (1f);
					T3.gameObject.GetComponent<Structures> ().Attacco ();
					T3.DisattivaRadar ();

				}
				if (Nexus_Player1.gameObject.scene != null || Nexus_Player1.gameObject.GetComponent<Structures>().currentHealth > 0) 
				{

					Structures N;
					N = Nexus_Player2.gameObject.GetComponent<Structures> ();

					N.AttivaRadar();
					yield return new WaitForSeconds (1f);
					N.gameObject.GetComponent<Structures> ().Attacco ();
					N.DisattivaRadar ();

				}

			}

			Debug.Log ("FINE ATTACCO TORRI");
			yield return new WaitForSeconds (0.3f);

		}

		/// <summary>
		/// Coroutine per la gestione dell'attacco dei personaggi
		/// </summary>
		/// <returns>The nemici.</returns>
		/// <param name="appartengo"> FALSE = Player 1 - TRUE = Player 2 </param>
		/*IEnumerator AttaccoNemici(bool appartengo)
		{
			GameObject Personaggio;

			int NumeroPersonaggiFermi = 0;  //Essendo l'inizio del ciclo ho ragione di pensare che nessuno è fermo
			NemiciFermi = 0; //Resetto il numero di personaggi fermi

			//Continuo a ciclare fino a quando non mi restituisce true il metodo nel While
			//ControlloTuttiPersonaggiFermi(appartengo) != true --------> CONDIZIONE GIUSTA IN TEORIA
			while (GetNumeroPersonaggiFazione(appartengo) != NemiciFermi) 
			{
				int indiceLista = 0;
				

				while (indiceLista <= Personaggi.Count - 1) 
				{
					
					if (Personaggi [indiceLista].appartengo == appartengo) 
					{
						Personaggio = GameObject.Find (Personaggi [indiceLista].Nome);

						//Attivo il radar per cercare nemici in zona
						Personaggio.GetComponent<Characters> ().AttivaRadar ();
						yield return new WaitForSeconds (1f);

						Personaggio.GetComponent<Assassin> ().AggiornaListaPriorita ();
						Personaggio.GetComponent<Assassin> ().SetTarget ();
						AstarPath.active.Scan ();

						for (int i = 0; i < Personaggi [indiceLista].Current_step; i++) 
						{
							
							if (Personaggio.GetComponent<Characters> ().isFighting != true) {
								Personaggio.GetComponent<AILerp> ().canMove = true;
								Personaggio.GetComponent<AILerp> ().canSearch = true;
								yield return new WaitForSeconds (0.3f);
							} 
							else 
							{
							

								Debug.Log (Personaggio.name + " ATTACCA");
								// attacco
								//istanziare coroutine
								yield return StartCoroutine (AttaccoPersonaggio (Personaggio));

								//distruggere coroutine
								//StopCoroutine(AttaccoPersonaggio);
			

							}

						}
						
						//Disattivo il radar
						Personaggio.GetComponent<Characters> ().DisattivaRadar ();
						NemiciFermi++;
					}

					indiceLista++;

				}
			}

			yield return new WaitForSeconds (0.3f);

			CambioTurno ();

		}*/

		/// <summary>
		/// Esegue lo scan della mappa ogni X secondi
		/// </summary>
		/// <returns></returns>
		IEnumerator ScanMappa()
		{

			int i = 0;
			
			while (true) 
			{
				
				AstarPath.active.Scan ();
				//Debug.Log("SCAN n°"+i++);

				yield return new WaitForSeconds (Tempo_Scan);

			}

		}
		

		/// <summary>
		/// Coroutine che gestiscono il combattimento del personaggio
		/// </summary>
		/// <returns>The personaggio.</returns>
		/// <param name="oggetto">Personaggio che deve attaccare</param>
		IEnumerator AttaccoPersonaggio(GameObject oggetto)
		{

			Animator anim = oggetto.GetComponentInChildren<Animator>();
			GameObject enemy = oggetto.GetComponent<AILerp>().target.gameObject;

			anim.SetBool("isAttacking", true);

			if (anim.GetBool("isAttacking"))
			{

				float danno = oggetto.GetComponent<Characters> ().Attack (enemy);
				enemy.GetComponent<Characters>().DamageTaken(danno);
				//oggetto.GetComponent<Characters>().currentSteps -= oggetto.GetComponent<Characters>().attackCost;
				Debug.Log("Attacco");
				Debug.Log("DANNO " + danno);
				Debug.Log(enemy.GetComponent<Characters>().currentHealth);
				//anim.SetBool("isAttacking", false);
			}

			yield return null;


		}

	#endregion
		

	#region METODI_GAMEPLAY

		/// <summary>
		/// Metodo che cambia il turno
		/// </summary>
		public void CambioTurno()
		{    

			Debug.Log ("CAMBIO TURNO");
			Turni++;
			schermata.AggiornaTurnoAschermo ();

			if (Turni % 2 == 0) 
			{ //Player 2
				
				Energia1 += 10;
				schermata.AggiornaEnergiaPlayer1 ();
				P1_HUD.SetActive (false);
				P2_HUD.SetActive (true);

			} 
			else 
			{ //Player 1


				Energia2 += 10;
				schermata.AggiornaEnergiaPlayer2 ();
				P1_HUD.SetActive (true);
				P2_HUD.SetActive (false);

			}

			//Resettiamo i punti di spawn dei personaggi 
			ResetVettoreSpawnPersonaggi ();

			FaseCombattimento = false;

		}


		/// <summary>
		/// Sottrai energia ad un giocatore
		/// </summary>
		/// <param name="consumo"> Concumo dell'energia </param>
		/// <param name="giocatore"> FALSE = Player 1 - TRUE = Player 2 </param>
		public void SottraiEnergia(int consumo, bool giocatore)
		{

			if ( giocatore == true) 
			{
				//player 2

				Energia2 = Energia2 - consumo;
				//Aggiorniamo a schemro il numero di energia del player 2
				schermata.AggiornaEnergiaPlayer2 ();

			} 
			else 
			{

				//Player 1

				Energia1 = Energia1 - consumo;
				//Aggiorniamo a schemro il numero di energia del player 1
				schermata.AggiornaEnergiaPlayer1 ();

			}

		}

		/// <summary>
		/// Aumenta energia del giocatore
		/// </summary>
		/// <param name="consumo"> Quantità energia </param>
		/// <param name="giocatore"> FALSE = Player 1 - TRUE = Player 2 </param>
		public void AggiungiEnergia(int consumo, bool giocatore)
		{

			if ( giocatore == true) 
			{
				//player 2

				Energia2 = Energia2 + consumo;
				//Aggiorniamo a schemro il numero di energia del player 2
				schermata.AggiornaEnergiaPlayer2 ();

			} 
			else 
			{

				//Player 1

				Energia1 = Energia1 + consumo;
				//Aggiorniamo a schemro il numero di energia del player 1
				schermata.AggiornaEnergiaPlayer1 ();

			}

		}

		public void AvvioFaseCombattimento()
		{

			FaseCombattimento = true;
			StartCoroutine (GestioneCombattimento ());

		}

	/// <summary>
	/// Metodo per resettare le caselle di Spawn dei personaggi da occupate a libere
	/// </summary>
	public void ResetVettoreSpawnPersonaggi()
	{

		Debug.Log ("Resettiamo i punti Spawn");

		if (Turni % 2 == 0) 
		{

			//Resettare il vettore caselle personaggi Player 2

			for (int i = 0; i < Caselle_P2.Length; i++) 
			{

				Caselle_P2 [i].tag = "Libera";

			}

		} 
		else 
		{

			//Resettare il vettore caselle personaggi Player 1

			for (int i = 0; i < Caselle_P1.Length; i++) 
			{

				Caselle_P1 [i].tag = "Libera";

			}

		}


	}

	#endregion

	#region GET_REGION

		/// <summary>
		/// Ritorna il turno corrente
		/// </summary>
		/// <returns>Ritorna il numero corrente del turo</returns>
		public int GetTurno()
		{

			return Turni;

		}


		/// <summary>
		///	Ritorna l'energia del Player 1
		/// </summary>
		/// <returns> Ritorna l'energia del Player 1</returns>
		public int GetEnergiaPlayer1()
		{

			return Energia1;

		}

		/// <summary>
		///	Ritorna l'energia del Player 2
		/// </summary>
		/// <returns> Ritorna l'energia del Player 2</returns>
		public int GetEnergiaPlayer2()
		{

			return Energia2;

		}

		/// <summary>
		/// ci dice se siamo in fase di combattimento oppure no
		/// </summary>
		/// <returns>Ritorna TRUE se siamo in fase di combattimento, mentre FALSO se non lo siamo</returns>
		public bool IsFaseCombattimento()
		{

			return FaseCombattimento;

		}

	#endregion	

}
