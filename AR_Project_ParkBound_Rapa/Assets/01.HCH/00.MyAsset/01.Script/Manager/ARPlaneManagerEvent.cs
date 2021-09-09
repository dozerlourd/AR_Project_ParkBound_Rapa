using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlaneManagerEvent : MonoBehaviour
{
    public GameObject spawnObject;
    public ARPlaneManager arPlaneManager;
    static int spawnCount = 0;

    private Dictionary<TrackableId, GameObject> spawnObjects;

    void Start()
    {
        arPlaneManager.planesChanged += ArPlaneManager_planesChanged;
    }

    private void ArPlaneManager_planesChanged(ARPlanesChangedEventArgs obj)
    {
        List<ARPlane> addedPlanes = obj.added;

        if (addedPlanes.Count > 0)
        {
            foreach (ARPlane plane in addedPlanes)
            {
                if (spawnCount == 0)
                {
                    GameObject instance = Instantiate(spawnObject, plane.center + Vector3.up * 0.5f, plane.transform.rotation);
                    spawnCount++;
                }
                //spawnObjects.Add(plane.trackableId, instance);
            }
        }

        List<ARPlane> removedPlanes = obj.removed;

        if (removedPlanes.Count > 0)
        {
            foreach (ARPlane plane in removedPlanes)
            {
                GameObject destoryTarget = spawnObjects[plane.trackableId];
                Destroy(destoryTarget);
            }
        }

        //arPlaneManager.planesChanged -= ArPlaneManager_planesChanged;
        //Destroy(GetComponent<ARPlaneManager>());
        //Destroy(GetComponent<ARPointCloudManager>());
        //Destroy(GetComponent<ARRaycastManager>());
        //Destroy(this);
    }
}