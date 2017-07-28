using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MyGameManager : MonoBehaviour {

	public MyPathNode[,] grid;
	public GameObject enemy;
	public GameObject gridBox;
	public int gridWidth;
	public int gridHeight;
	public gridPosition currentGridPosition = new gridPosition();
	public float gridSize;

	public static string distanceType;


	//This is what you need to show in the inspector.
	public static int distance = 2;


	public List<dati> lista_nemici;
	public class dati
	{

		public GameObject oggetto;
		public MyPathNode path;

	}


	public class gridPosition{
		public int x =0;
		public int z =0;

		public gridPosition()
		{
		}

		public gridPosition (int x, int z)
		{
			this.x = x;
			this.z = z;
		}
	};

	public PlayerAStar star;


	void Awake ()
	{

		//inizializiamo la lista dei nemici
		lista_nemici = new List<dati> ();

	}

	void Start () {


		//Generate a grid - nodes according to the specified size
		grid = new MyPathNode[gridWidth, gridHeight];

		for (int x = 0; x < gridWidth; x++) {
			for (int y = 0; y < gridHeight; y++) {
				//Boolean isWall = ((y % 2) != 0) && (rnd.Next (0, 10) != 8);
				Boolean isWall = false;
				grid [x, y] = new MyPathNode ()
				{
					IsWall = isWall,
					X = x,
					Z = y,
				};
			}
		}
			

		//instantiate enemy object
		createEnemy ();


	}

	void Update()
	{

		if (Input.GetKeyDown (KeyCode.Space)) {

			createEnemy ();


		}

	}


	void createGrid()
	{
		//Generate Gameobjects of GridBox to show on the Screen
		for (int i =0; i<gridHeight; i++) {
			for (int j =0; j<gridWidth; j++) {
				GameObject nobj = (GameObject)GameObject.Instantiate(gridBox);
				nobj.transform.position = new Vector3(gridBox.transform.position.x + (10*j),0,gridBox.transform.position.z + (10f*i));
				nobj.name = j+","+i;

				nobj.gameObject.transform.parent = gridBox.transform.parent;
				nobj.SetActive(true);

			}
		}
	}

	void createEnemy()
	{
		GameObject nb = (GameObject)GameObject.Instantiate (enemy, new Vector3(0,enemy.transform.localScale.y/2,0),Quaternion.identity);
		nb.SetActive (true);
	}


	public void addWall (int x, int y)
	{
		grid [x, y].IsWall = true;
	}

	public void removeWall (int x, int y)
	{
		grid [x, y].IsWall = false;
	}

	#region Lista Nemici

	//Aggiungiamo nemico alla lista dei nemici
	public void AggiungiNemicoLista(GameObject ogg)
	{

		//lista_nemici.Add (ogg);

	}

	//Stampiamo la lista dei nemici correnti
	public void StampaListaNemici()
	{

		for (int i = 0; i < lista_nemici.Count; i++) 
		{

			Debug.Log (lista_nemici[i].oggetto.transform.position);


		}

	}

	//Rimuovo un nemico dalla lista
	public void RimuoviNemicoLista(GameObject ogg)
	{

		//lista_nemici.Remove (ogg);

	}

	//Aggiorniamo il nemico dentro alle liste
	public void ModificaNemicoLista(List<dati> lista)
	{

		for (int i = 0; i < lista_nemici.Count; i++) 
		{
			
			GameObject ogg;

			//cerchiamo un oggetto per nome
			ogg = GameObject.Find (lista[i].oggetto.name);

			if (ogg != null)  //se esiste aggiorno il gameobject
			{

				lista [i].oggetto = ogg;


				//lista [i].path = ogg;

			} 
			else //se non esiste lo rimuovo
			{

				//lista.Remove (ogg);

			}

		}

	}

	#endregion
}
