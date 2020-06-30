using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class LaserPointer : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean touchAction;    

    public enum AxisType
    {
        XAxis,
        ZAxis
    }
    public Color color;
    public float thickness = 0.002f;
    public AxisType facingAxis = AxisType.XAxis;
    public float length = 100f;
    public float cursorBuffer = 0.1f;
    public bool showCursor = true;
    public GameObject cursorPrefab;
    public GameObject pointerPrefab;

    private GameObject holder;
    private GameObject pointer;
    private GameObject cursor;
    private Vector3 cursorScale = new Vector3(0.05f, 0.05f, 0.05f);
    private float contactDistance = 0f;
    private Transform contactTarget = null;
    private MeshRenderer mr = new MeshRenderer();
    private SphereCollider sc;

    // Start is called before the first frame update
    void Start()
    {
        //Material newMaterial = new Material(Shader.Find("Unlit/Color"));
        //newMaterial.SetColor("_Color", color);

        holder = new GameObject();
        holder.transform.parent = this.transform;
        holder.transform.localPosition = Vector3.zero;

        pointer = Instantiate(pointerPrefab, holder.transform);
        //pointer.transform.parent = holder.transform;
        //pointer.GetComponent<MeshRenderer>().material = newMaterial;

        pointer.GetComponent<BoxCollider>().isTrigger = true;
        pointer.AddComponent<Rigidbody>().isKinematic = true;
        pointer.layer = 2;


        cursor = Instantiate(cursorPrefab, holder.transform);
        //cursor.transform.parent = holder.transform;
        cursor.transform.localScale = cursorScale;
        cursor.GetComponent<SphereCollider>().isTrigger = true;        
        cursor.AddComponent<Rigidbody>().isKinematic = true;
        cursor.layer = 2;

        DontDestroyOnLoad(cursor);

        SetPointerTransform(length, thickness);
    }


    float GetBeamLength(bool bHit, RaycastHit hit)
    {
        float actualLength = length;

        //reset if beam not hitting or hitting new target
        if (!bHit || (contactTarget && contactTarget != hit.transform))
        {
            contactDistance = 0f;
            contactTarget = null;
        }

        //check if beam has hit a new target
        if (bHit)
        {
            if (hit.distance <= 0)
            {

            }
            contactDistance = hit.distance;
            contactTarget = hit.transform;
        }

        //adjust beam length if something is blocking it
        if (bHit && contactDistance < length)
        {
            actualLength = contactDistance;
        }

        if (actualLength <= 0)
        {
            actualLength = length;
        }

        return actualLength; 
    }

    void Update()
    {
        if (touchAction.GetState(handType))
        {
            //Debug.Log($"Touch {handType}");
            Ray raycast = new Ray(transform.position, transform.forward);

            RaycastHit hitObject;
            bool rayHit = Physics.Raycast(raycast, out hitObject);

            float beamLength = GetBeamLength(rayHit, hitObject);
            cursor.SetActive(true);
            SetPointerTransform(beamLength, thickness);
            if (hitObject.transform.gameObject && hitObject.transform.gameObject.GetComponent<FlagSelectLanguage>())
            {
                hitObject.transform.gameObject.GetComponent<FlagSelectLanguage>().SelectLanguage();
                Debug.Log(hitObject.transform.gameObject.name);
            }
            else if (hitObject.transform.gameObject && hitObject.transform.gameObject.GetComponent<ButtonActions>())
            {
                hitObject.transform.gameObject.GetComponent<ButtonActions>().ExitApplication();
            }
            
        }
        else
        {
            cursor.SetActive(false);
            SetPointerTransform(0, 0);
        }
    }

    void SetPointerTransform(float setLength, float setThicknes)
    {
        //if the additional decimal isn't added then the beam position glitches
        float beamPosition = -setLength / (2 + 0.00001f);

        if (facingAxis == AxisType.XAxis)
        {
            pointer.transform.localScale = new Vector3(setLength, setThicknes, setThicknes);
            pointer.transform.localPosition = new Vector3(beamPosition, 0f, 0f);
            if (showCursor)
            {
                cursor.transform.localPosition = new Vector3(setLength - cursor.transform.localScale.x, 0f, 0f);
            }
        }
        else
        {
            pointer.transform.localScale = new Vector3(setThicknes, setThicknes, setLength);
            pointer.transform.localPosition = new Vector3(0f, 0f, beamPosition);

            if (showCursor)
            {
                cursor.transform.localPosition = new Vector3(0f, 0f, -setLength + cursorBuffer - cursor.transform.localScale.z);
            }
        }
    }
}
