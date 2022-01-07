using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A collecting game object
/// </summary>
public class Collector : MonoBehaviour
{
	#region Fields

    // targeting support
    [SerializeField] SortedList<Target> targets = new SortedList<Target>();
    Target targetPickup = null;

    // movement support
	const float BaseImpulseForceMagnitude = 2.0f;
    const float ImpulseForceIncrement = 0.3f;
	
	// saved for efficiency
    Rigidbody2D rb2d;

    #endregion

    #region Methods

    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Start()
    {
		// center collector in screen
		Vector3 position = transform.position;
		position.x = 0;
		position.y = 0;
		position.z = 0;
		transform.position = position;

		// save reference for efficiency
		rb2d = GetComponent<Rigidbody2D>();

        // add as listener for pickup spawned event
        EventManager.AddListener(OnPickupSpawned);
	}

    void OnPickupSpawned(GameObject pickup)
    {
	    targets.Add(new Target(pickup, transform.position));
	    if (targets.Count == 1)
	    {
		    SetTarget(targets[0]);
		    return;
	    }

	    Target lastTargetInList = targets[targets.Count - 1];
	    Debug.Log($"OnPickupSpawned: distance is {lastTargetInList}");
	    if (targetPickup.Distance > lastTargetInList.Distance)
	    {
		    SetTarget(lastTargetInList);
	    }
    }
    
    /// <summary>
    /// Called when another object is within a trigger collider
    /// attached to this object
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerStay2D(Collider2D other)
    {
        // only respond if the collision is with the target pickup
		if (other.gameObject == targetPickup.GameObject)
        {
            // remove collected pickup from list of targets and game
            //targets.RemoveAt(targets.IndexOf(targetPickup));

			// go to next target if there is one

		}
	}

	/// <summary>
	/// Sets the target pickup to the provided pickup
	/// </summary>
	/// <param name="pickup">Pickup.</param>
	void SetTarget(Target pickup)
    {
		targetPickup = pickup;
		Debug.Log($"SetTarget: distance is {targetPickup.Distance}");
		GoToTargetPickup();
	}

	/// <summary>
	/// Starts the teddy bear moving toward the target pickup
	/// </summary>
	void GoToTargetPickup()
    {
        // calculate direction to target pickup and start moving toward it
		Vector2 direction = new Vector2(
			targetPickup.GameObject.transform.position.x - transform.position.x,
			targetPickup.GameObject.transform.position.y - transform.position.y);
		direction.Normalize();
		rb2d.velocity = Vector2.zero;
		rb2d.AddForce(direction * BaseImpulseForceMagnitude, 
			ForceMode2D.Impulse);
	}
	
	#endregion
}
