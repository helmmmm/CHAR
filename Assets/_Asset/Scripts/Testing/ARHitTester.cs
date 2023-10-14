// Copyright 2022 Niantic, Inc. All Rights Reserved.

using System.Collections.Generic;

using Niantic.ARDK.AR;
using Niantic.ARDK.AR.ARSessionEventArgs;
using Niantic.ARDK.AR.HitTest;
using Niantic.ARDK.External;
using Niantic.ARDK.Utilities;
using Niantic.ARDK.Utilities.Input.Legacy;

using UnityEngine;

// namespace Niantic.ARDKExamples.Helpers
// {
//   //! A helper class that demonstrates hit tests based on user input
//   /// <summary>
//   /// A sample class that can be added to a scene and takes user input in the form of a screen touch.
//   ///   A hit test is run from that location. If a plane is found, spawn a game object at the
//   ///   hit location.
//   /// </summary>
  public class ARHitTester: MonoBehaviour
  {
    /// The camera used to render the scene. Used to get the center of the screen.
    public Camera Camera;

    /// The types of hit test results to filter against when performing a hit test.
    [EnumFlagAttribute]
    public ARHitTestResultType HitTestType = ARHitTestResultType.ExistingPlane;

    /// The object we will place when we get a valid hit test result!
    public GameObject PlacementObjectPf;

    public ARCursorRenderer _cursorRenderer;

    /// A list of placed game objects to be destroyed in the OnDestroy method.
    private List<GameObject> _placedObjects = new List<GameObject>();

    /// Internal reference to the session, used to get the current frame to hit test against.
    private IARSession _session;
    public bool _levelPlaced = false;
    
    private void Start()
    {
      ARSessionFactory.SessionInitialized += OnAnyARSessionDidInitialize;
      _cursorRenderer = GetComponent<ARCursorRenderer>();
    }

    private void OnAnyARSessionDidInitialize(AnyARSessionInitializedArgs args)
    {
      _session = args.Session;
      _session.Deinitialized += OnSessionDeinitialized;
    }

    private void OnSessionDeinitialized(ARSessionDeinitializedArgs args)
    {
      ClearObjects();
    }

    private void OnDestroy()
    {
      ARSessionFactory.SessionInitialized -= OnAnyARSessionDidInitialize;

      _session = null;

      ClearObjects();
    }

    private void ClearObjects()
    {
      foreach (var placedObject in _placedObjects)
      {
        Destroy(placedObject);
      }

      _placedObjects.Clear();
    }

    private void Update()
    {
      if (_session == null)
      {
        return;
      }

      if (PlatformAgnosticInput.touchCount <= 0)
      {
        return;
      }

      var touch = PlatformAgnosticInput.GetTouch(0);
      if (touch.phase == TouchPhase.Began && !_levelPlaced)
      {
        TouchBegan(touch);
      }
    }

    private void TouchBegan(Touch touch)
    {
      var currentFrame = _session.CurrentFrame;
      if (currentFrame == null)
      {
        return;
      }
      
      // if(touch.IsTouchOverUIObject())
      //   return;

      var results = currentFrame.HitTest
      (
        Camera.pixelWidth,
        Camera.pixelHeight,
        touch.position,
        HitTestType
      );

      int count = results.Count;
      Debug.Log("Hit test results: " + count);

      if (count <= 0)
        return;

      // Get the closest result
      var result = results[0];

      // var hitPosition = result.WorldTransform.ToPosition();
      Vector3 cursorPosition = _cursorRenderer.CursorPosition;
      Quaternion cursorRotation = _cursorRenderer.CursorRotation;

      _placedObjects.Add(Instantiate(PlacementObjectPf, cursorPosition, cursorRotation));

      if (_placedObjects.Count > 0)
      {
        _levelPlaced = true;
        SM_Game.Instance.TryChangeState(SM_Game.Instance.GSM_State_CursorPlaced);
        _cursorRenderer.SetCursorVisibility(false); // Hide the cursor
      }
      
      var anchor = result.Anchor;
      Debug.LogFormat
      (
        "Spawning cube at {0} (anchor: {1})",
        cursorPosition.ToString("F4"),
        anchor == null
          ? "none"
          : anchor.AnchorType + " " + anchor.Identifier
      );
    }
  }
// }
