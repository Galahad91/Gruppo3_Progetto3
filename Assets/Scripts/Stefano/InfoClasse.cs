using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoClasse : MonoBehaviour {

	public GameObject personaggio;
	private GameObject ogg_Gestore;
	private IstanziareOggetto istanzia;
	private GestoreGioco gestore;

	void Awake()
	{

		ogg_Gestore = GameObject.FindGameObjectWithTag ("GameManager");
		istanzia = ogg_Gestore.GetComponent<IstanziareOggetto> ();
		gestore = ogg_Gestore.GetComponent<GestoreGioco> ();

	}
		
	/// <summary>
	/// Premiamo l`icona per prendere l`oggetto da istanziare sulla plancia di gioco
	/// </summary>
	void OnMouseUp()
	{

		if (gestore.GetTurno () % 2 == 0 && gameObject.layer == LayerMask.NameToLayer ("Player 2")) 
		{
			if (personaggio != null) 
			{

				istanzia.SetOggettoIstanzia (personaggio);

			} 
			else 
			{

				Debug.LogError ("Personaggio non assegnato all`icona");

			}

		}
		else if (gestore.GetTurno () % 2 != 0 && gameObject.layer == LayerMask.NameToLayer ("Player 1")) 
		{
			
			if (personaggio != null) 
			{

				istanzia.SetOggettoIstanzia (personaggio);

			} 
			else 
			{

				Debug.LogError ("Personaggio non assegnato all`icona");

			}

		}

	}


	/// <summary>
	/// Stesso metodo di quello sopra solo che viene richiamato da bottone
	/// </summary>
	/// <param name="oggetto">Oggetto.</param>
	public void ScegliClasse(GameObject oggetto)
	{

		if (gestore.GetTurno () % 2 == 0 && (gameObject.layer == LayerMask.NameToLayer ("Player 2") || gameObject.layer == LayerMask.NameToLayer("Torre P2"))) {
			if (oggetto != null) {

				istanzia.SetOggettoIstanzia (oggetto);
				Debug.Log ("Assegnato personaggio");

			} else {

				Debug.LogError ("Personaggio non assegnato all`icona");

			}

		} else if (gestore.GetTurno () % 2 != 0 && (gameObject.layer == LayerMask.NameToLayer ("Player 1") || gameObject.layer == LayerMask.NameToLayer("Torre P1"))) {

			if (oggetto != null) {

				istanzia.SetOggettoIstanzia (oggetto);
				Debug.Log ("Assegnato personaggio");

			} else {

				Debug.LogError ("Personaggio non assegnato all`icona");

			}

		} 

		//Debug.Log ("Premuto");

	}

}
