using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class IA_Torre : MonoBehaviour 
{

	[Header("Rateo di fuoco della torre")]
	public float Rateo_Fuoco = 1f;
	[Header("Durata espressa in turni della torre")]
	public int Durata = 3;
	[Header("Danni che effettua la torre")]
	public int Danno = 10;
	[Header("Costo della torre")]
	public int Costo = 1;
	[Header("Colore della torre")]
	public Color colore;

	[Header("VARIABILI DI DEBUG NON MODIFICABILI")]
	[SerializeField]
	public static List<GameObject> Lista_Nemici;

	[Header("VARIABILI MODIFICABILI SOLO DAI PROGRAMMATORI")]
	public string Tag = "Personaggio" ; //Variabile per il controllo dei tag per l'inserimento nella lista
	public GameObject Ogg_Gestore;
	private GestoreGioco gestore;
	private bool EseguoReset = false;

	private float timer; //timer di gioco

	//THREAD
	private Thread T_Scan;
	private bool _t1Pausa = false;

	//Casella su sui poggia la torre
	GameObject casella;

	void Awake()
	{

		Lista_Nemici = new List<GameObject>();
		Ogg_Gestore = GameObject.FindGameObjectWithTag ("GameManager");
		gestore = Ogg_Gestore.GetComponent<GestoreGioco> ();

	}

	void Start()
	{

		T_Scan = new Thread (PulisciLista) { Name = "Thread Pulizia" };
		T_Scan.Start ();

	}

	void Update()
	{

		if (gestore.IsFaseCombattimento () == true) 
		{

			EseguoReset = true;

			timer += Time.deltaTime;

			if (timer >= Rateo_Fuoco) 
			{
				timer = 0;

				if (gestore.Turni % 2 == 0) 
				{
					if(gameObject.layer == LayerMask.NameToLayer ("Player 1"))
					Attacco ();
				}
				else 
				{
					if(gameObject.layer == LayerMask.NameToLayer ("Player 2"))
					Attacco ();
				}

			}

		} 
		else 
		{

			if (EseguoReset == true) 
			{

				EseguoReset = false;

				if (gestore.Turni % 2 == 0) 
				{
					if(gameObject.layer == LayerMask.NameToLayer ("Player 2"))
						Reset ();
				}
				else 
				{
					if(gameObject.layer == LayerMask.NameToLayer ("Player 1"))
						Reset ();
				}
					
				//Se la torre ha terminato la durata la distruggo
				if (Durata == 0) 
				{

					DistruggiTorre ();

				}

			}

		}

	}

	/// <summary>
	/// Metodo che permette l'attacco della torre contro i nemici della lista
	/// </summary>
	private void Attacco()
	{

		for (int i = 0; i < Lista_Nemici.Count; i++) 
		{

			if (Lista_Nemici[i] != null) 
			{
				Lista_Nemici [i].gameObject.GetComponent<Assassin> ().DamageTaken_Assassin (Danno, colore);
				Debug.Log (gameObject.name + " ha attaccato");
			} 
			else 
			{

				Lista_Nemici[i] = null;

			}

		}

	}

	/// <summary>
	/// Trigger che riempie la lista di nemici
	/// </summary>
	void OnTriggerEnter(Collider other)
	{

		if (other.gameObject.tag == Tag) 
		{
			Debug.Log ("Entrato");

			Lista_Nemici.Add (other.gameObject);
		}

	}

	/// <summary>
	/// Trigger che toglie il nemico dalla lista
	/// </summary>
	void OnTriggerExit(Collider other)
	{
		try
		{
			if(other.gameObject.tag == Tag)
			{

				Debug.Log("Uscito");

				Lista_Nemici.Remove (other.gameObject);
			}
		}
		catch 
		{

			Debug.LogError ("Non riesco a rimuovere l'oggetto");
		}
	}

	/// <summary>
	/// Metodo che viene chiamato una sola volta per resettare la torre e per scalare il turno
	/// </summary>
	public void Reset()
	{

		Durata--;
		timer = 0;

	}


	/// <summary>
	/// Metodo che distrugge la torre
	/// </summary>
	private void DistruggiTorre()
	{

		casella.tag = "Libera";
		Destroy (gameObject);

	}

	private static void PulisciLista()
	{

		bool PossoPulire = true;

		while(true)
		{

			//Debug.Log ("Entrato");

			for (int i = 0; i < Lista_Nemici.Count; i++) 
			{

				if (Lista_Nemici [i].gameObject != null)
				{

					PossoPulire = false;

				}

			}

			if (PossoPulire == true) 
			{

				//Debug.Log ("PULISCO");
				Lista_Nemici.Clear ();

			} 
			else 
			{
				//Debug.Log ("Non pulisco");

			}

			PossoPulire = true;

			Thread.Sleep (3000);

		}

	}
		
	/// <summary>
	/// Metodo che assegna la casella su cui poggia la torre stessa
	/// </summary>
	/// <param name="casella_passata">Casella passata dal RayCast</param>
	public void SetCasella(GameObject casella_passata)
	{

		casella = casella_passata;

	}
}
