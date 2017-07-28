using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

	public Text Energia_Player1;
	public Text Energia_Player2;
	public Text Turno;
	private GestoreGioco gestore;

	void Awake()
	{

		gestore = gameObject.GetComponent<GestoreGioco> ();

	}

	// Use this for initialization
	void Start () 
	{

		Energia_Player1.text = gestore.GetEnergiaPlayer1 ().ToString ();
		Energia_Player2.text = gestore.GetEnergiaPlayer2 ().ToString ();
		Turno.text = "Turno: 1";

	}

	//aggiorno l energia a schermo player 1
	public void AggiornaEnergiaPlayer1()
	{

		Energia_Player1.text = gestore.GetEnergiaPlayer1 ().ToString ();

	}

	//aggiorno l energia a schermo player 2
	public void AggiornaEnergiaPlayer2()
	{

		Energia_Player2.text = gestore.GetEnergiaPlayer2 ().ToString ();

	}

	public void AggiornaTurnoAschermo()
	{

		Turno.text = "Turno: " + gestore.GetTurno ().ToString();

	}

}
