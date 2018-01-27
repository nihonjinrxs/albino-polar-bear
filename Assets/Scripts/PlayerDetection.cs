using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    private const float RAYCASTDIST = 5f;
    private RaycastHit2D visionhit;
    private Transform target;

    void Start ()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
	
	// Update is called once per frame
	void Update ()
    {
        Vector2 dir = this.transform.TransformDirection(Vector3.down) * RAYCASTDIST;
        visionhit = Physics2D.Raycast(this.transform.position, Vector3.down, RAYCASTDIST);
        Debug.DrawRay(this.transform.position, dir, Color.blue);
        if (visionhit.collider != null && visionhit.collider.gameObject.name == "Player")
        {
            Debug.Log("I see you");
        }
    }
}
