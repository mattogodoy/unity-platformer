﻿using UnityEngine;
using System.Collections;
using Pathfinding;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(Seeker))]
public class EnemyAI : MonoBehaviour {

	// What to chase
	public Transform target;

	// Times per second to update the path
	public float updateRate = 2f;

	private Seeker seeker;
	private Rigidbody2D rb;

	// Calculated path
	public Path path;

	// AI's speed per second
	public float speed = 300f;
	public ForceMode2D fMode;

	[HideInInspector]
	public bool pathIsEnded = false;

	// Max distance from the AI to the waypoint for it to continue to the next waypoint
	public float nextWaypointDistance = 3f;

	// The waypoint we are currently moving towards
	private int currentWaypoint = 0;

	private bool searchingForPlayer = false;

	void Start () {
		seeker = GetComponent<Seeker> ();
		rb = GetComponent<Rigidbody2D> ();

		if(target == null){
			if (!searchingForPlayer) {
				searchingForPlayer = true;
				StartCoroutine (SearchForPlayer ());
			}

			return;
		}

		StartCoroutine (UpdatePath ());
	}

	IEnumerator SearchForPlayer() {
		GameObject sResult = GameObject.FindGameObjectWithTag ("Player");
		if (sResult == null) {
			yield return new WaitForSeconds (0.5f);
			StartCoroutine (SearchForPlayer ());
		} else {
			target = sResult.transform;
			searchingForPlayer = false;
			StartCoroutine (UpdatePath ());

			return false;
		}
	}

	IEnumerator UpdatePath () {
		if(target == null){
			if (!searchingForPlayer) {
				searchingForPlayer = true;
				StartCoroutine (SearchForPlayer ());
			}

			return false;
		}

		// Start a new path to the target position and retur the result to OnPathComplete
		seeker.StartPath (transform.position, target.position, OnPathComplete);

		yield return new WaitForSeconds (1f / updateRate);
		StartCoroutine (UpdatePath ());
	}

	public void OnPathComplete(Path p){
		if (!p.error) {
			path = p;
			currentWaypoint = 0;
		}
	}

	void FixedUpdate () {
		if(target == null){
			if (!searchingForPlayer) {
				searchingForPlayer = true;
				StartCoroutine (SearchForPlayer ());
			}

			return;
		}

		// TODO: Always look at player?

		if (path == null) {
			return;
		}

		if (currentWaypoint >= path.vectorPath.Count) {
			if (pathIsEnded) {
				return;
			} else {
				Debug.Log ("End of path reached");
				pathIsEnded = true;
				return;
			}
		}

		pathIsEnded = false;

		// Direction to the next waypoint
		Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
		dir *= speed * Time.fixedDeltaTime;

		// Move the AI
		rb.AddForce(dir, fMode);
		float dist = Vector3.Distance (transform.position, path.vectorPath [currentWaypoint]);

		if (dist < nextWaypointDistance) {
			currentWaypoint++;
			return;
		}
	}
}
