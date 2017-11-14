using UnityEngine;

public class ObjectGrabber : MonoBehaviour {

    public Transform mainTransform;
    public Transform grabber;

    private LayerMask grabbableMask;
    private LayerMask environmentMask;

    void Awake() {
        environmentMask = LayerMask.GetMask("Environment");
        grabbableMask = LayerMask.GetMask("Grabbable");
    }

    Transform currentObject = null;

    void Update () {
        if (Input.GetMouseButtonDown(0)) {
            if (currentObject == null) {
                RaycastHit hit;
                if (Physics.Raycast(mainTransform.position, mainTransform.forward, out hit, 1000, grabbableMask.value)) {
                    grabber.position = hit.point;
                    hit.transform.SetParent(grabber, true);
                    currentObject = hit.transform;
                    Destroy(hit.rigidbody);
                    //currentObject.GetComponentInChildren<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                }
            } else {
                currentObject.gameObject.AddComponent<Rigidbody>();
                //currentObject.GetComponentInChildren<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                currentObject.SetParent(null, true);
                currentObject = null;
            }
        }
        if(currentObject != null) {
            RaycastHit hit;
            if (Physics.Raycast(mainTransform.position, mainTransform.forward, out hit, 1000, environmentMask.value)) {
                float previousDistance = Vector3.Distance(grabber.position, mainTransform.position);
                Vector3 newPosition = hit.point;

                float newDistance = Vector3.Distance(newPosition, mainTransform.position);
                float distanceChange = newDistance / previousDistance;
                Vector3 newScale = currentObject.transform.localScale * distanceChange;

                newPosition = mainTransform.position + ShortenByUnits(newPosition - mainTransform.position, newScale.x);

                newDistance = Vector3.Distance(newPosition, mainTransform.position);
                distanceChange = newDistance / previousDistance;
                currentObject.transform.localScale *= distanceChange;

                grabber.position = newPosition;
                currentObject.localPosition *= distanceChange;
            }
        }
	}

    private static Vector3 ShortenByUnits(Vector3 vec, float units) {
        Vector3 normalized = vec.normalized;
        Vector3 subtractionVec = normalized * units;
        return vec - subtractionVec;
    }
}
