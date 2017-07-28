using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IstanziareOggetto : MonoBehaviour {

	private GestoreGioco gestore;
	private GameObject oggetto;
	public float x;
	public float z;
	private float x_old;
	private float z_old;
	private Transform TorreTarget;


	void Awake()
	{

		x_old = 0;
		z_old = 0;
		gestore = gameObject.GetComponent<GestoreGioco> ();

	}
	

	void Update () 
	{

		if (x!= x_old || z!= z_old) 
		{
			x_old = x;
			z_old = z;

			if (oggetto != null && gestore.GetTurno () % 2 == 0) 
			{//player 2

				if (gestore.GetEnergiaPlayer2 () > 0) 
				{
					IstanzioOggetto (oggetto, x, z, GetTorreTarget());

					//togliamo energia ogni volta che instanziamo oggetti
					gestore.SottraiEnergia (1);
				}

			} 
			else //player 1
			{

				if (gestore.GetEnergiaPlayer1 () > 0) 
				{
					
					IstanzioOggetto (oggetto, x, z, GetTorreTarget());

					//togliamo energia ogni volta che instanziamo oggetti
					gestore.SottraiEnergia (1);

				}

			}

		}

	}

	void IstanzioOggetto(GameObject oggetto, float x, float z, Transform Torre)
	{

		Instantiate (oggetto, new Vector3 (x, 1 , z), Quaternion.identity);

		oggetto.GetComponent<Characters> ().TargetTorre = Torre;

	}

	//diamo il target al nemico 
	public void SetTorreTarget(Transform arrivo)
	{

		TorreTarget = arrivo;

	}

	public Transform GetTorreTarget()
	{

		return TorreTarget;

	}

	public void Reset()
	{

		oggetto = null;

	}

	//Settiamo il personaggio da istanziare 
	public void SetOggettoIstanzia(GameObject personaggio)
	{

		oggetto = personaggio;

	}

	//Ritorna l`oggetto che stiamo per istanziare in una casella
	public GameObject GetOggettoIstanzia()
	{

		return oggetto;

	}

}
