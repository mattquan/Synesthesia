using UnityEngine;
using System.Collections;

using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

// Change the material when certain poses are made with the Myo armband.
// Vibrate the Myo armband when a fist pose is made.
public class ColorBoxByPosePrimary : MonoBehaviour {
	// Myo game object to connect with.
	// This object must have a ThalmicMyo script attached.
	public GameObject myo = null;
	public GameObject boxSecondary;

	// Materials to change to when poses are made.
	public Material doubleTapMaterial;

	// The pose from the last update. This is used to determine if the pose has changed
	// so that actions are only performed upon making them rather than every frame during
	// which they are active.
	private Pose _lastPose = Pose.Unknown;
	//private int gesture = 0;
	// Update is called once per frame.
	void Update ()
	{
		// Access the ThalmicMyo component attached to the Myo game object.
		ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo> ();




		// Check if the pose has changed since last update.
		// The ThalmicMyo component of a Myo game object has a pose property that is set to the
		// currently detected pose (e.g. Pose.Fist for the user making a fist). If no pose is currently
		// detected, pose will be set to Pose.Rest. If pose detection is unavailable, e.g. because Myo
		// is not on a user's arm, pose will be set to Pose.Unknown.
		if (thalmicMyo.pose != _lastPose) {
			_lastPose = thalmicMyo.pose;

			// Vibrate the Myo armband when a fist is made.
			if (thalmicMyo.pose == Pose.Fist) {
				thalmicMyo.Vibrate (VibrationType.Medium);

				ExtendUnlockAndNotifyUserAction (thalmicMyo);

				// Change material when wave in, wave out or double tap poses are made.
			} else if (thalmicMyo.pose == Pose.WaveIn) {
				//GetComponent<Renderer> ().material.color = Color.green;
				GetComponent<Renderer> ().transform.position = new Vector3(GetComponent<Renderer>().transform.position.x, GetComponent<Renderer>().transform.position.y, GetComponent<Renderer>().transform.position.z - .2f);
				boxSecondary.transform.position = new Vector3(boxSecondary.transform.position.x, boxSecondary.transform.position.y, GetComponent<Renderer>().transform.position.z - .2f);

				ExtendUnlockAndNotifyUserAction (thalmicMyo);
			} else if (thalmicMyo.pose == Pose.WaveOut) {
				//GetComponent<Renderer> ().material.color = Color.yellow;
				GetComponent<Renderer> ().transform.position = new Vector3 (GetComponent<Renderer>().transform.position.x, GetComponent<Renderer>().transform.position.y, GetComponent<Renderer> ().transform.position.z + .2f);
				boxSecondary.transform.position = new Vector3(boxSecondary.transform.position.x, boxSecondary.transform.position.y, GetComponent<Renderer>().transform.position.z + .2f);


				ExtendUnlockAndNotifyUserAction (thalmicMyo);
			} else if (thalmicMyo.pose == Pose.DoubleTap) {
				//GetComponent<Renderer> ().material = doubleTapMaterial;
				ExtendUnlockAndNotifyUserAction (thalmicMyo);
			}
		} else {
			//it's the same pose
			if (thalmicMyo.pose == Pose.WaveIn) {
				//GetComponent<Renderer> ().material.color = Color.green;

				GetComponent<Renderer> ().transform.position = new Vector3 (GetComponent<Renderer>().transform.position.x, GetComponent<Renderer>().transform.position.y, GetComponent<Renderer> ().transform.position.z - .2f);
				boxSecondary.transform.position = new Vector3(boxSecondary.transform.position.x, boxSecondary.transform.position.y, GetComponent<Renderer>().transform.position.z - .2f);



				ExtendUnlock (thalmicMyo);
			} else if (thalmicMyo.pose == Pose.WaveOut) {
				//GetComponent<Renderer> ().material.color = Color.yellow;
				GetComponent<Renderer> ().transform.position = new Vector3(GetComponent<Renderer>().transform.position.x, GetComponent<Renderer>().transform.position.y, GetComponent<Renderer>().transform.position.z + .2f);
				boxSecondary.transform.position = new Vector3(boxSecondary.transform.position.x, boxSecondary.transform.position.y, GetComponent<Renderer>().transform.position.z + .2f);


				ExtendUnlock(thalmicMyo);
			}
		}
	}

	// Extend the unlock if ThalmcHub's locking policy is standard, and notifies the given myo that a user action was
	// recognized.
	void ExtendUnlockAndNotifyUserAction (ThalmicMyo myo)
	{
		ThalmicHub hub = ThalmicHub.instance;

		if (hub.lockingPolicy == LockingPolicy.Standard) {
			myo.Unlock (UnlockType.Timed);
		}

		myo.NotifyUserAction ();
	}
	void ExtendUnlock (ThalmicMyo myo)
	{
		ThalmicHub hub = ThalmicHub.instance;

		if (hub.lockingPolicy == LockingPolicy.Standard) {
			myo.Unlock (UnlockType.Timed);
		}
	}
}
