using UnityEngine;
using Niantic.ARDK.AR;
using Niantic.ARDK.AR.ARSessionEventArgs;
using Niantic.ARDK.Utilities;
using Niantic.ARDK.AR.HitTest;
using Niantic.ARDK.Utilities.Input.Legacy;

public class ARManager : MonoBehaviour
{
    /*
        
    */
    public Camera _mainCamera;
    IARSession _ARsession;

    // Start is called before the first frame update
    void Start()
    {
        ARSessionFactory.SessionInitialized += OnSessionInitialized;
        // SM_Game.Instance.GSM_State_CursorPlaced.OnEnter += DestroyAllCursors;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DestroyAllCursors()
    {
        Destroy(GameObject.Find("Level Cursor(Clone)"));
    }

    private void OnSessionInitialized(AnyARSessionInitializedArgs args)
    {
        ARSessionFactory.SessionInitialized -= OnSessionInitialized;
        _ARsession = args.Session;
    }
}
