using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class sun : MonoBehaviour {

	public float rotationSpeed = 0.3f;
	// suns information
	public Vector3? sunSpawnPosition = null;
	public Quaternion? sunSpawnRotation = null;

	public GameObject SunObject;
	// moons information
	public Vector3? moonSpawnPosition = null;
	public Quaternion? moonSpawnRotation = null;
	public GameObject moonObject;

	void Start() 
	{
		sunSpawnPosition = SunObject.transform.position;
		sunSpawnRotation = SunObject.transform.rotation;

		moonSpawnPosition = moonObject.transform.position;
		moonSpawnRotation = moonObject.transform.rotation;
	}
 	
	void Update () 
	{

		// Rotates the Sun in a certain rotatien, in a given speed
		transform.RotateAround(Vector3.zero,Vector3.right, rotationSpeed * Time.deltaTime);
		transform.LookAt(Vector3.zero);

	}
	
	// Suns spawn position
	internal void SaveSunSpawnPoint()
    {
        sunSpawnPosition = SunObject.transform.position;
        
    }
	// Suns spawn rotation
	internal void SaveSunSpawnRotation()
	{
		sunSpawnRotation = SunObject.transform.rotation;
	}


	// Sets the suns respawn position (after save file load)
    public void RespawnSun()
    {
        if (sunSpawnPosition != null)
        {
            TeleportSunTo(sunSpawnPosition.Value + Vector3.up, sunSpawnRotation.Value);
        }
    }

	public void TeleportSunTo(Vector3 position, Quaternion rotation)
    {
        SunObject.transform.position = position;
		SunObject.transform.rotation = rotation;
    }

	// Same code but to the moons position

	internal void SaveMoonSpawnPoint()
    {
        moonSpawnPosition = moonObject.transform.position;
        
    }
	// Suns spawn rotation
	internal void SaveMoonSpawnRotation()
	{
		moonSpawnRotation = moonObject.transform.rotation;
	}


	// Sets the suns respawn position (after save file load)
    public void RespawnMoon()
    {
        if (moonSpawnPosition != null)
        {
            TeleportMoonTo(moonSpawnPosition.Value + Vector3.up, moonSpawnRotation.Value);
        }
    }

	public void TeleportMoonTo(Vector3 position, Quaternion rotation)
    {
        moonObject.transform.position = position;
		moonObject.transform.rotation = rotation;
    }

}
