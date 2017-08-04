using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyDebug : MonoBehaviour {

	GestoreGioco gestore;
	public Text fase;
	public InputField input_rimuovi;
	public Text testo_rimuovi;
	public Text turno;

	public Button GestoreTurni_coroutine;


	void Awake()
	{


		gestore = gameObject.GetComponent<GestoreGioco> ();

	}

	/*void Update()
	{

		if (gestore.IsFaseCombattimento() == true) {

			fase.text = "COMBATTIMENTO";
			fase.color = Color.red;

		} else {

			fase.text = "PREPARAZIONE";
			fase.color = Color.green;

		}

		turno.text = "Turno: " + gestore.GetTurno ().ToString ();

	}*/

	/*public void AttivaDebugMode(GameObject finestra)
	{

		if (finestra.activeSelf == true) {


			finestra.SetActive (false);

		} else {


			finestra.SetActive (true);

		}

	}

	//Metodo di Debug per stampare la lista a secondo del turno
	public void StampaLista(int lista)
	{

		if (lista == 2) 
		{//player 2

			Debug.Log ("STAMPO LISTA P2");

			for (int i = 0; i < gestore.Personaggi_P2.Count; i++) {


				Debug.Log("nemico ["+i+"] = "+gestore.Personaggi_P2[i].Nome+" - "+gestore.Personaggi_P2[i].Tag+" - "+gestore.Personaggi_P2[i].Current_step);

			}

		} 
		else if(lista == 1)//player 1
		{

			Debug.Log ("STAMPO LISTA P1");

			for (int i = 0; i < gestore.Personaggi_P1.Count; i++) {


				Debug.Log("nemico ["+i+"] = "+ gestore.Personaggi_P1[i].Nome+" - "+gestore.Personaggi_P1[i].Tag+" - "+gestore.Personaggi_P1[i].Current_step);

			}

		}

	}

	//Puliamo la console di debug
	public void PulisciConsole()
	{

		Debug.ClearDeveloperConsole ();

	}

	//Controllo quanti personaggi sono fermi in questo dato istante
	public void ControlloPersonaggiFermi(Text testo)
	{
		if (gestore.GetTurno() % 2 == 0) 
		{//player 2

			testo.text = "P2 Fermi = " + gestore.GetPersonaggiP2Fermi ().ToString();

		} 
		else //player 1
		{


			testo.text = "P1 Fermi = " + gestore.GetPersonaggiP1Fermi ().ToString();

		}


	}

	//Rimuove un personaggio da una lista DA NON USARE IN FASE DI COMBATTIMENTO
	public void RimuoviPersonaggioLista()
	{

		if (gestore.IsFaseCombattimento () == false) 
		{

			int count_old_p1 = gestore.Personaggi_P1.Count;
			int count_old_p2 = gestore.Personaggi_P2.Count;
			
			gestore.RimuoviDatoListaPersonaggi (input_rimuovi.text);

			testo_rimuovi.text = "Attenzione!";
			testo_rimuovi.color = Color.magenta;

			if (count_old_p1 != gestore.Personaggi_P1.Count || count_old_p2 != gestore.Personaggi_P2.Count) { 

				testo_rimuovi.text = "OK!";
				testo_rimuovi.color = Color.green;

			} 
			else 
			{

				testo_rimuovi.text = "ERRORE!!!";
				testo_rimuovi.color = Color.red;

			}

		}
		else 
		{

			testo_rimuovi.text = "Furbo eh!?";
			testo_rimuovi.color = Color.red;

		}



	}


	//metodo per forzare l'aggiornamento delle liste dei personaggi sia P1 che P2 a secondo del turno 
	public void AggiornaListe()
	{

		gestore.AggiornaListaPersonaggi ();

	}

	public void GestioneCoroutine()
	{

		if (gestore.GestioneTurni_controllo == false) 
		{



		}

	}*/
		

}
