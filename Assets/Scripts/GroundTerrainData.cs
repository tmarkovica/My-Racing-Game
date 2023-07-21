using UnityEngine;
using UnityEngine.UI;

public class GroundTerrainData : MonoBehaviour
{
	public static GroundTerrainData Instance;
	
	[SerializeField] private LayerMask _tag;	
	public LayerMask Tag { get { return _tag; } set {} }
	[SerializeField] private Terrain _terrain;
	public Terrain Terrain { get { return _terrain; } set {} }
	
	void Awake() 
	{
		Instance = this;
	}
	
	public string execute() 
	{
		return this.Tag.ToString();
		
	}
	
	public float GetMaxSpeedForTerrainAtPlayersPosition(Vector3 playerPosition) 
	{
		int textureIndex = DetectTexture(playerPosition);

		if (textureIndex == 0) // grass
		{
			return 400;
		}
		else // road
		{
			return 700; // max speed
		}	
	}
	
	private int DetectTexture(Vector3 playerPosition) 
	{
		// Get the player's position in world space.
		//Vector3 playerPosition = carRigidBodyRef.transform.position;

		// Ensure the player's position is within the bounds of the terrain.
		if (Terrain.terrainData.bounds.Contains(playerPosition))
		{
			// Get the normalized position of the player within the terrain.
			Vector3 normalizedPosition = GetNormalizedTerrainPosition(playerPosition);

			// Get the alphamap data at the player's position.
			float[,,] alphamapData = Terrain.terrainData.GetAlphamaps(
				(int)(normalizedPosition.x * Terrain.terrainData.alphamapWidth),
				(int)(normalizedPosition.z * Terrain.terrainData.alphamapHeight),
				1, 1
			);
			
			return GetDominantTextureIndex(alphamapData);
		}
		return -1;
	}
	
	private Vector3 GetNormalizedTerrainPosition(Vector3 position)
	{
		// Convert world position to normalized position within the terrain.
		Vector3 normalizedPosition = new Vector3(
			(position.x - Terrain.terrainData.bounds.min.x) / Terrain.terrainData.size.x,
			0,
			(position.z - Terrain.terrainData.bounds.min.z) / Terrain.terrainData.size.z
		);

		return normalizedPosition;
	}

	private int GetDominantTextureIndex(float[,,] alphamapData)
	{
		// Find the index of the highest value in the alphamap data.
		int dominantIndex = 0;
		float maxAlpha = 0f;

		for (int i = 0; i < alphamapData.GetLength(2); i++)
		{
			if (alphamapData[0, 0, i] > maxAlpha)
			{
				maxAlpha = alphamapData[0, 0, i];
				dominantIndex = i;
			}
		}

		return dominantIndex;
	}
}