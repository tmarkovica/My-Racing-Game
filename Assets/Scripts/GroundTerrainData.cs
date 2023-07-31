using UnityEngine;

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
	
	public float GetPercentageOfMaxSpeedAllowedAt(Vector3 playerPosition) 
	{
		int textureIndex = GetDominantTextureIndex(playerPosition);

		if (textureIndex == 0) // grass
		{
			return 0.4f;
		}
		else
		{
			return 1f; // road
		}
	}
	
	private int GetDominantTextureIndex(Vector3 playerPosition) 
	{
		// Get the player's position in world space.
		// Ensure the player's position is within the bounds of the terrain. 
		if (_terrain.terrainData.bounds.Contains(playerPosition))
		{
			// Get the normalized position of the player within the terrain.
			Vector3 normalizedPosition = GetNormalizedTerrainPosition(playerPosition);

			// Get the alphamap data at the player's position.
			float[,,] alphamapData = _terrain.terrainData.GetAlphamaps(
				(int)(normalizedPosition.x * _terrain.terrainData.alphamapWidth),
				(int)(normalizedPosition.z * _terrain.terrainData.alphamapHeight),
				1, 1
			);
			
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
		return -1;
	}
	
	private Vector3 GetNormalizedTerrainPosition(Vector3 position)
	{
		// Convert world position to normalized position within the terrain.
		Vector3 normalizedPosition = new Vector3(
			(position.x - _terrain.terrainData.bounds.min.x) / _terrain.terrainData.size.x,
			0,
			(position.z - _terrain.terrainData.bounds.min.z) / _terrain.terrainData.size.z
		);

		return normalizedPosition;
	}

	/* private int GetDominantTextureIndex(float[,,] alphamapData)
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
	} */
	
	public bool IsGrounded(Vector3 vehiclePosition, float checkRadius) 
	{
		RaycastHit hit;
		return Physics.Raycast(vehiclePosition, Vector3.down, out hit, checkRadius +  0.1f, _tag);
	}
}