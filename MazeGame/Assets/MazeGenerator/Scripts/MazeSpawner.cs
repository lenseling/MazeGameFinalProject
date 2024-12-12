using UnityEngine;
using System.Collections;
using Oculus.Interaction.Surfaces;
using UnityEditor;
using Unity.AI.Navigation;
using NavMeshSurface = Unity.AI.Navigation.NavMeshSurface;
using System.Collections.Generic;


//<summary>
//Game object, that creates maze and instantiates it in scene
//</summary>
public class MazeSpawner : MonoBehaviour {
	public enum MazeGenerationAlgorithm{
		PureRecursive,
		RecursiveTree,
		RandomTree,
		OldestTree,
		RecursiveDivision,
	}

	public MazeGenerationAlgorithm Algorithm = MazeGenerationAlgorithm.PureRecursive;
	public bool FullRandom = false;
	public int RandomSeed = 12345;
	public GameObject Floor = null;
	public GameObject Wall = null;
	public GameObject Pillar = null;
	public int Rows = 5;
	public int Columns = 5;
	public float CellWidth = 5;
	public float CellHeight = 5;
	public bool AddGaps = true;
	public GameObject GoalPrefab = null;
	public NavMeshSurface navMeshSurface = null;
	public EnemySpawner enemySpawner = null;

    public GameObject KeyPrefab = null;
    public int NumberOfKeys;
    private List<Vector3> keySpawnPositions = new List<Vector3>();


    private BasicMazeGenerator mMazeGenerator = null;

    public GameObject ExitPortalPrefab = null; // The exit portal prefab
    private GameObject exitPortalInstance = null; // Instance of the exit portal
    private Vector3 exitPortalPosition; // The spawn position for the exit portal
    public int keysCollected = 0; // Tracks the number of collected keys

	public GameObject StatuePrefab; // Prefab for the statues
	public GameObject spikePrefab; // Prefab for the swinging spikes
	public int NumberOfspikes; // Number of swinging spikes to spawn
    private List<Vector3> spikeSpawnPositions = new List<Vector3>();

    void Start() {


		if (!FullRandom) {
			Random.seed = RandomSeed;
		}
		switch (Algorithm) {
		case MazeGenerationAlgorithm.PureRecursive:
			mMazeGenerator = new RecursiveMazeGenerator (Rows, Columns);
			break;
		case MazeGenerationAlgorithm.RecursiveTree:
			mMazeGenerator = new RecursiveTreeMazeGenerator (Rows, Columns);
			break;
		case MazeGenerationAlgorithm.RandomTree:
			mMazeGenerator = new RandomTreeMazeGenerator (Rows, Columns);
			break;
		case MazeGenerationAlgorithm.OldestTree:
			mMazeGenerator = new OldestTreeMazeGenerator (Rows, Columns);
			break;
		case MazeGenerationAlgorithm.RecursiveDivision:
			mMazeGenerator = new DivisionMazeGenerator (Rows, Columns);
			break;
		}
		mMazeGenerator.GenerateMaze ();
		for (int row = 0; row < Rows; row++) {
			for(int column = 0; column < Columns; column++){
				float x = column*(CellWidth+(AddGaps?.2f:0));
				float z = row*(CellHeight+(AddGaps?.2f:0));

				Vector3 cellCenter = new Vector3(x, 1, z); // Adjust y if necessary
				keySpawnPositions.Add(cellCenter);
				spikeSpawnPositions.Add(cellCenter);

				MazeCell cell = mMazeGenerator.GetMazeCell(row,column);
				GameObject tmp;
				tmp = Instantiate(Floor,new Vector3(x,0,z), Quaternion.Euler(0,0,0)) as GameObject;
				tmp.transform.parent = transform;
				if(cell.WallRight){
					tmp = Instantiate(Wall,new Vector3(x+CellWidth/2,0,z)+Wall.transform.position,Quaternion.Euler(0,90,0)) as GameObject;// right
					tmp.transform.parent = transform;
				}
				if(cell.WallFront){
					tmp = Instantiate(Wall,new Vector3(x,0,z+CellHeight/2)+Wall.transform.position,Quaternion.Euler(0,0,0)) as GameObject;// front
					tmp.transform.parent = transform;
				}
				if(cell.WallLeft){
					tmp = Instantiate(Wall,new Vector3(x-CellWidth/2,0,z)+Wall.transform.position,Quaternion.Euler(0,270,0)) as GameObject;// left
					tmp.transform.parent = transform;
				}
				if(cell.WallBack){
					tmp = Instantiate(Wall,new Vector3(x,0,z-CellHeight/2)+Wall.transform.position,Quaternion.Euler(0,180,0)) as GameObject;// back
					tmp.transform.parent = transform;
				}
				if(cell.IsGoal && GoalPrefab != null){
					tmp = Instantiate(GoalPrefab,new Vector3(x,1,z), Quaternion.Euler(0,0,0)) as GameObject;
					tmp.transform.parent = transform;
				}
				// set layer for Nav Mesh
                tmp.layer = LayerMask.NameToLayer("Maze");
				//GameObjectUtility.SetStaticEditorFlags(tmp, StaticEditorFlags.NavigationStatic);

				

			}
		}

        if (KeyPrefab != null && NumberOfKeys > 0)
{
			// Shuffle the positions
			for (int i = keySpawnPositions.Count - 1; i > 0; i--)
			{
				int randomIndex = Random.Range(0, i + 1);
				Vector3 temp = keySpawnPositions[i];
				keySpawnPositions[i] = keySpawnPositions[randomIndex];
				keySpawnPositions[randomIndex] = temp;
			}

			// Spawn keys at random positions
			for (int i = 0; i < Mathf.Min(NumberOfKeys, keySpawnPositions.Count); i++)
			{
				GameObject key = Instantiate(KeyPrefab, keySpawnPositions[i], Quaternion.identity, transform);

				// Spawn the statue to cover the key
				if (StatuePrefab != null && i < NumberOfKeys)
				{
					Vector3 statuePosition = keySpawnPositions[i];
					statuePosition.y = Floor.transform.position.y + 1; // Adjust Y position so the statue doesn't overlap the key visually
					GameObject statue = Instantiate(StatuePrefab, statuePosition, Quaternion.identity, transform);

					// Attach the key to the statue
					key.transform.parent = statue.transform;
					key.transform.localPosition = Vector3.zero; // Center the key inside the statue
					key.SetActive(false); // Hide the key until the statue is moved
				}
			}
		}

		if (spikePrefab != null && NumberOfspikes > 0)
		{
			for (int i = spikeSpawnPositions.Count - 1; i > 0; i--)
			{
				int randomIndex = Random.Range(0, i + 1);
				Vector3 temp = spikeSpawnPositions[i];
				spikeSpawnPositions[i] = spikeSpawnPositions[randomIndex];
				spikeSpawnPositions[randomIndex] = temp;
			}
			for (int i = 0; i < Mathf.Min(NumberOfspikes, spikeSpawnPositions.Count); i++)
			{
                Vector3 spawnPosition = spikeSpawnPositions[i];
                spawnPosition.y = Mathf.Max(spawnPosition.y - 1.5f, 0); // Lower the spike closer to the floor, clamping at 0 to avoid underground placement

                // Instantiate the spike
                GameObject spike = Instantiate(spikePrefab, spawnPosition, Quaternion.identity, transform);
                // Instantiate the spike

                // Scale adjustment (if not already set in the prefab)
                spike.transform.localScale = new Vector3(1f, 1f, 1f); // Adjust scale as needed

                
            }
		
		}
        if (ExitPortalPrefab != null)
        {
            // Choose a random position for the portal from the key spawn positions
            int randomIndex = Random.Range(0, keySpawnPositions.Count);
            exitPortalPosition = keySpawnPositions[randomIndex];

            // Instantiate the exit portal but keep it inactive initially
            exitPortalInstance = Instantiate(ExitPortalPrefab, exitPortalPosition, Quaternion.identity, transform);
            exitPortalInstance.SetActive(false);
        }


        if (Pillar != null){
			for (int row = 0; row < Rows+1; row++) {
				for (int column = 0; column < Columns+1; column++) {
					float x = column*(CellWidth+(AddGaps?.2f:0));
					float z = row*(CellHeight+(AddGaps?.2f:0));
					GameObject tmp = Instantiate(Pillar,new Vector3(x-CellWidth/2,0,z-CellHeight/2),Quaternion.identity) as GameObject;
					tmp.transform.parent = transform;
                    tmp.layer = LayerMask.NameToLayer("Maze");
                }
            }
		}

        if (navMeshSurface != null)
        {
            navMeshSurface.BuildNavMesh();
        }
        Debug.Log("Maze Initialization Finished");
        // Call Enemy Spawner to generate enemies in the maze
        enemySpawner.Init();
        // find all enemies in the scene and notify them
        EnemyAI[] enemies = FindObjectsOfType<EnemyAI>();

        foreach (EnemyAI enemy in enemies)
        {
            enemy.Init();
        }
		Debug.Log("Enemy Initialization Finished");
    }

    public void OnKeyCollected()
    {
        keysCollected++;
        if (keysCollected >= NumberOfKeys && exitPortalInstance != null)
        {
            // Activate the exit portal when all keys are collected
            exitPortalInstance.SetActive(true);
            Debug.Log("Exit Portal Activated!");
        }
    }

}