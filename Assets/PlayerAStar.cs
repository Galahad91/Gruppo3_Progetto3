using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAStar : MonoBehaviour {

	public GameObject Ogg_Game;
	private MyGameManager Game;
	public MyPathNode nextNode;
	bool gray = false;
	public MyPathNode[,] grid;


	public gridPosition currentGridPosition = new gridPosition();
	public gridPosition startGridPosition = new gridPosition();
	public gridPosition endGridPosition = new gridPosition();

	private Orientation gridOrientation = Orientation.Vertical;
	private bool allowDiagonals = false;
	private bool correctDiagonalSpeed = true;
	private Vector3 input;
	private bool isMoving = true;
	private Vector3 startPosition;
	private Vector3 endPosition;
	private float t;
	private float factor;
	private Color myColor;


	void Awake()
	{

		Ogg_Game = GameObject.FindGameObjectWithTag ("GameManager");
		Game = Ogg_Game.GetComponent<MyGameManager> ();

	}

	public class MySolver<TPathNode, TUserContext> : SettlersEngine.SpatialAStar<TPathNode, 
	TUserContext> where TPathNode : SettlersEngine.IPathNode<TUserContext>
	{
		protected override Double Heuristic(PathNode inStart, PathNode inEnd)
		{


			int formula = GameManager.distance;
			int dx = Math.Abs (inStart.X - inEnd.X);
			int dy = Math.Abs(inStart.Z - inEnd.Z);

			if(formula == 0)
				return Math.Sqrt(dx * dx + dy * dy); //Euclidean distance

			else if(formula == 1)
				return (dx * dx + dy * dy); //Euclidean distance squared

			else if(formula == 2)
				return Math.Min(dx, dy); //Diagonal distance

			else if(formula == 3)
				return (dx*dy)+(dx + dy); //Manhatten distance



			else 
				return Math.Abs (inStart.X - inEnd.X) + Math.Abs (inStart.Z - inEnd.Z);

			//return 1*(Math.Abs(inStart.X - inEnd.X) + Math.Abs(inStart.Y - inEnd.Y) - 1); //optimized tile based Manhatten
			//return ((dx * dx) + (dy * dy)); //Khawaja distance

		}

		protected override Double NeighborDistance(PathNode inStart, PathNode inEnd)
		{
			return Heuristic(inStart, inEnd);
		}

		public MySolver(TPathNode[,] inGrid)
			: base(inGrid)
		{
		}
	} 



	// Use this for initialization
	void Start () {

		//startGridPosition = new gridPosition(UnityEngine.Random.Range(1,Game.gridWidth-1),UnityEngine.Random.Range(0,Game.gridHeight-1));
		//endGridPosition = new gridPosition(UnityEngine.Random.Range(1,Game.gridWidth-1),UnityEngine.Random.Range(0,Game.gridHeight-1));

		startGridPosition = new gridPosition (1, 1);
		endGridPosition = new gridPosition (9,14); 

		//Aggiungiamo il nemico alla lista dei nemici
		Game.AggiungiNemicoLista (this.gameObject);

		Game.StampaListaNemici ();

		initializePosition ();


		MySolver<MyPathNode, System.Object> aStar = new MySolver<MyPathNode, System.Object>(Game.grid);
		IEnumerable<MyPathNode> path = aStar.Search(new Vector3(startGridPosition.x, 0 ,startGridPosition.z), new Vector3(endGridPosition.x, 0 ,endGridPosition.z), null);

	
		updatePath();

	}



	public void findUpdatedPath(int currentX,int currentZ)
	{


		MySolver<MyPathNode, System.Object> aStar = new MySolver<MyPathNode, System.Object>(Game.grid);
		IEnumerable<MyPathNode> path = aStar.Search(new Vector3(currentX,0 ,currentZ), new Vector3(endGridPosition.x, 0 ,endGridPosition.z), null);


		int x = 0;

		if (path != null) {

			foreach (MyPathNode node in path)
			{
				if(x==1)
				{
					nextNode = node;
					break;
				}

				x++;

			}
				
		}


	}

	// Update is called once per frame
	void Update () {

		if (!isMoving) {
			StartCoroutine(move());
		}
	}



	public float moveSpeed;

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


	private enum Orientation {
		Horizontal,
		Vertical
	};


	//coroutine per il movimento 
	public IEnumerator move() 
	{
		isMoving = true;
		startPosition = transform.position;
		t = 0;

		if(gridOrientation == Orientation.Horizontal) {
			endPosition = new Vector3(startPosition.x + System.Math.Sign(input.x) * Game.gridSize*10,0,
				startPosition.z);
			currentGridPosition.x += System.Math.Sign(input.x);
		} else {
			endPosition = new Vector3(startPosition.x + System.Math.Sign(input.x) * Game.gridSize*10,0,
				startPosition.z + System.Math.Sign(input.z) * Game.gridSize*10);

			currentGridPosition.x += System.Math.Sign(input.x);
			currentGridPosition.z += System.Math.Sign(input.z);
		}

		if(allowDiagonals && correctDiagonalSpeed && input.x != 0 && input.z != 0) {
			factor = 0.9071f;
		} else {
			factor = 1f;
		}


		while (t < 1f) {
			t += Time.deltaTime * (moveSpeed/Game.gridSize*10) * factor;
			transform.position = Vector3.Lerp(startPosition, endPosition, t);
			yield return null;
		}



		isMoving = false;

		getNextMovement ();

		yield return 0;

	}

	void updatePath()
	{
		findUpdatedPath (currentGridPosition.x, currentGridPosition.z);
	}

	void getNextMovement()
	{
		updatePath();


		input.x = 0;
		input.z = 0;
		if (nextNode.X > currentGridPosition.x) {
			input.x = 1;
			//this.GetComponent<SpriteRenderer>().sprite = Game.carFront;
		}
		if (nextNode.Z > currentGridPosition.z) {
			input.z = 1;
			//this.GetComponent<SpriteRenderer>().sprite = Game.carUp;
		}
		if (nextNode.Z < currentGridPosition.z) {
			input.z = -1;
			//this.GetComponent<SpriteRenderer>().sprite = Game.carDown;
		}
		if (nextNode.X < currentGridPosition.x) {
			input.x = -1;
			//this.GetComponent<SpriteRenderer>().sprite = Game.carBack;
		}

		StartCoroutine (move ());
	}

	public Vector3 getGridPosition(int x, int z)
	{
		float contingencyMargin = Game.gridSize*10f; //prima c'era .10f0
		float posX = Game.gridBox.transform.position.x + (Game.gridSize*x*10+10) - contingencyMargin;
		float posZ = Game.gridBox.transform.position.y + (Game.gridSize*z*10-10) + contingencyMargin;
		return new Vector3(posX,0,posZ);	

	}


	public void initializePosition()
	{
		this.gameObject.transform.position = getGridPosition (startGridPosition.x, startGridPosition.z);
		currentGridPosition.x = startGridPosition.x;
		currentGridPosition.z = startGridPosition.z;
		isMoving = false;
		//GameObject.Find(startGridPosition.x + "," + startGridPosition.z).GetComponent<Renderer>().material.color = Color.black; 

	}

	//mi faccio tornare indietro la posizione dell oggetto sulla griglia
	/*public gridPosition GetCurrentPosition(GameObject oggetto)
	{

		return ;

	}*/

	private void ControlloPriorita()
	{

		//calcolo la destinazione del personaggio


	}

	private void ControlloNemiciInZona()
	{



	}

}
