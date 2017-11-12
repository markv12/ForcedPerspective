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
                    currentObject.GetComponentInChildren<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                }
            } else {
                currentObject.gameObject.AddComponent<Rigidbody>();
                currentObject.GetComponentInChildren<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                currentObject.SetParent(null, true);
                currentObject = null;
            }
        }
        if(currentObject != null) {
            RaycastHit hit;
            if (Physics.Raycast(mainTransform.position, mainTransform.forward, out hit, 1000, environmentMask.value)) {
                float previousDistance = Vector3.Distance(grabber.position, mainTransform.position);
                Vector3 newPostion = hit.point;
                Vector3 relativePosition = newPostion - mainTransform.position;
                relativePosition *= 0.6f;
                newPostion = mainTransform.position + relativePosition;

                float newDistance = Vector3.Distance(newPostion, mainTransform.position);
                float distanceChange = newDistance / previousDistance;
                grabber.position = newPostion;
                currentObject.localPosition *= distanceChange;
                currentObject.transform.localScale *= distanceChange;
            }
        }
	}
}
