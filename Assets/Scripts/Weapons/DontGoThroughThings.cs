// Script from Daniel Brauer, Adrian
// http://wiki.unity3d.com/index.php?title=DontGoThroughThings

using UnityEngine;

public class DontGoThroughThings : MonoBehaviour
{
    // Careful when setting this to true - it might cause double
    // events to be fired - but it won't pass through the trigger
    public bool sendTriggerMessage = false;

    public LayerMask layerMask = -1; //make sure we aren't in this layer
    public float skinWidth = 0.1f; //probably doesn't need to be changed

    [SerializeField] private Transform[] _offsets = null;
    private Vector2[] _previousPositions;

    private float minimumExtent;
    private float partialExtent;
    private float sqrMinimumExtent;
    //private Vector3 previousPosition;
    private Vector3 movementThisStep;
    private Rigidbody2D myRigidbody;
    private Collider2D myCollider;

    //initialize values
    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
        //previousPosition = myRigidbody.position;
        minimumExtent = Mathf.Min(Mathf.Min(myCollider.bounds.extents.x, myCollider.bounds.extents.y), myCollider.bounds.extents.z);
        partialExtent = minimumExtent * (1.0f - skinWidth);
        sqrMinimumExtent = minimumExtent * minimumExtent;

        _previousPositions = new Vector2[_offsets.Length];
        for (byte i = 0; i < _offsets.Length; i++)
            _previousPositions[i] = _offsets[i].position;

    }

    void FixedUpdate()
    {
        //have we moved more than our minimum extent?

        for (byte i = 0; i < _offsets.Length; i++)
        {
            movementThisStep = _offsets[i].position - (Vector3)_previousPositions[i];
            float movementSqrMagnitude = movementThisStep.sqrMagnitude;
            if (movementSqrMagnitude > sqrMinimumExtent)
            {
                float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);
                RaycastHit2D hitInfo = Physics2D.Linecast(_previousPositions[i], _offsets[i].position, layerMask.value);

                //check for obstructions we might have missed
                if (hitInfo)
                {
                    if (!hitInfo.collider) return;

                    if (hitInfo.collider.isTrigger)
                    {
                        myCollider.SendMessage("OnTriggerEnter2D", hitInfo.collider);
                        Debug.Log("Trigger HIT");
                    }

                    if (!hitInfo.collider.isTrigger)
                        myRigidbody.position = (Vector3)hitInfo.point - (movementThisStep / movementMagnitude) * partialExtent;
                }
            }

            _previousPositions[i] = _offsets[i].position;
        }

        //previousPosition = myRigidbody.position;
    }

    private void OnDrawGizmos()
    {
        if (enabled)
        {
            Gizmos.color = Color.green;
            if (_previousPositions != null)
                for (byte i = 0; i < _previousPositions.Length; i++)
                {
                    Gizmos.DrawLine(_previousPositions[i], _offsets[i].position);
                }
        }
    }
}