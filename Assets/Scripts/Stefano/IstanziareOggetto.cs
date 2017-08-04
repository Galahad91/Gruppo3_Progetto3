using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IstanziareOggetto : MonoBehaviour {

	private GestoreGioco gestore;		//GestoreGioco
	private GameObject oggetto;			//oggetto che andremo a istanziare sulla plancia
	public float x;						//coordinata X per istanziare il personaggio	
	public float z;						//coordinata Z per istanziare il personaggio	
	private float x_old;				//coordinata X vecchia per controllare che non ven ga istanziato un personaggio sulla stessa casella	
	private float z_old;				//coordinata Z vecchia per controllare che non ven ga istanziato un personaggio sulla stessa casella	
	private Transform TorreTarget;		//Transform della Torre 


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

			if (oggetto != null) 
			{

				if (oggetto.layer == LayerMask.NameToLayer("Torre P2") || oggetto.layer ==  LayerMask.NameToLayer("Torre P1")) 
				{

					IstanziaTorre (oggetto, x, z);

					//togliamo energia ogni volta che instanziamo oggetti
					if (gestore.Turni % 2 == 0) 
					{
						//Player 2
						gestore.SottraiEnergia (oggetto.GetComponent<IA_Torre>().Costo, true);

					} 
					else 
					{
						//Player 1
						gestore.SottraiEnergia (oggetto.GetComponent<IA_Torre>().Costo, false );
					}

				} 
				else 
				{
					

					if (oggetto != null && gestore.GetTurno () % 2 == 0) 
					{//player 2

						if (gestore.GetEnergiaPlayer2 () > 0) {
							IstanzioOggetto (oggetto, x, z, GetTorreTarget ());

							//togliamo energia ogni volta che instanziamo oggetti
							gestore.SottraiEnergia (oggetto.GetComponent<Assassin>().Costo, true);
						}

					} 
					else 
					{ //player 1

						if (gestore.GetEnergiaPlayer1 () > 0) {

							IstanzioOggetto (oggetto, x, z, GetTorreTarget ());

							//togliamo energia ogni volta che instanziamo oggetti
							gestore.SottraiEnergia (oggetto.GetComponent<Assassin>().Costo, false);

						}

					}

				}

			}


		}

	}

	/// <summary>
	/// Determiniamo dove istanziare un personaggio nella plancia di gioco
	/// </summary>
	/// <returns></returns>
	/// <param name="oggetto">Oggetto da istanziare</param>
	/// <param name="x"> X coordinate per istanziare</param></param>
	/// <param name="z"> Z coordinate per istanziare</param>
	/// <param name="Torre">La torre target </param>
	void IstanzioOggetto(GameObject oggetto, float x, float z, Transform Torre)
	{

		Instantiate (oggetto, new Vector3 (x, 1 , z), Quaternion.identity);

		oggetto.GetComponent<Characters> ().TargetTorre = Torre;

	}


	/// <summary>
	/// Determiniamo dove istanziare un personaggio nella plancia di gioco
	/// </summary>
	/// <param name="oggetto">Oggetto da istanziare</param>
	/// <param name="x">X dove istanziare l'oggetto</param>
	/// <param name="z">Z dove istanziare l'oggetto</param>
	void IstanziaTorre(GameObject oggetto, float x, float z)
	{

		Instantiate (oggetto, new Vector3 (x, 1 , z), Quaternion.identity);

	}
		

	/// <summary>
	/// Ritorna il transform della torre
	/// </summary>
	/// <returns> Ritorna il transform della torre</returns>
	public Transform GetTorreTarget()
	{

		return TorreTarget;

	}

	/// <summary>
	/// Diamo il target della torre al personaggio
	/// </summary>
	/// <param name="arrivo">Arrivo.</param>
	public void SetTorreTarget(Transform arrivo)
	{

		TorreTarget = arrivo;

	}


	/// <summary>
	/// Resettiamo l'ggetto da istanziare
	/// </summary>
	public void Reset()
	{

		oggetto = null;

	}

	/// <summary>
	/// Settiamo il personaggio da istanziare
	/// </summary>
	/// <param name="personaggio"> Personaggio che andremo a istanziare</param>
	public void SetOggettoIstanzia(GameObject personaggio)
	{

		oggetto = personaggio;

	}

	/// <summary>
	/// Ritorna l'oggetto che andremo a istanziare
	/// </summary>
	/// <returns> Ritorna l'oggetto che andremo a istanziare.</returns>
	public GameObject GetOggettoIstanzia()
	{

		return oggetto;

	}

}
