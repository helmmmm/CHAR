// using Niantic.ARDK.AR.LightEstimate;
// using Niantic.ARDK.Extensions;
// using Niantic.ARDK.AR;
// using Niantic.ARDK.Rendering;
// using Niantic.ARDK.AR.Configuration;
// using Niantic.ARDK.AR.Frame;
// using Niantic.ARDK.AR.ARSessionEventArgs;

// using TMPro;
// using UnityEngine.UI;
// using UnityEngine;
// using static UnityEngine.Mathf;

// public class LightEstimation : MonoBehaviour
// {
//     [SerializeField] private Light _directionalLight;
//     private IARLightEstimate _currentLightEstimate;
//     [SerializeField] private ARSessionManager _arSessionManager;
//     [SerializeField] private TMP_Text _ambientIntensityText;
//     [SerializeField] private TMP_Text _ambientColorTemperatureText;
//     [SerializeField] private TMP_Text _directionalIntensityText;
//     [SerializeField] private TMP_Text _directionalColorTemperatureText;


//     private void OnEnable()
//     {
//         _directionalLight = FindObjectOfType<Light>();
//         _arSessionManager = FindObjectOfType<ARSessionManager>();
//         _ambientIntensityText = GameObject.Find("Ambient Intensity").GetComponent<TMP_Text>();
//         _ambientColorTemperatureText = GameObject.Find("Ambient Color").GetComponent<TMP_Text>();
//         _directionalIntensityText = GameObject.Find("Directional Light Intensity").GetComponent<TMP_Text>();
//         _directionalColorTemperatureText = GameObject.Find("Directional Light Color").GetComponent<TMP_Text>();
//         _arSessionManager.ARSession.FrameUpdated += OnFrameUpdated;
//     }

//     private void OnDisable()
//     {
//         _arSessionManager.ARSession.FrameUpdated -= OnFrameUpdated;
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         // OnFrameUpdated(_arSessionManager.ARSession.FrameUpdated);

//     }

//     private void OnFrameUpdated(FrameUpdatedArgs args)
//     {
//         var frame = args.Frame;
//         _currentLightEstimate = frame.LightEstimate;
//         if (_currentLightEstimate == null)
//         {
//             _directionalLight.intensity = 1f;
//             _directionalLight.color = Color.white;
//         }
//         else
//         {
//             _directionalLight.intensity = _currentLightEstimate.AmbientIntensity;
//             _directionalLight.color = CorrelatedColorTemperatureToRGB(_currentLightEstimate.AmbientColorTemperature);
//         }
//         _ambientIntensityText.text = "Ambient Light Intensity: " + frame.LightEstimate.AmbientIntensity.ToString();
//         _ambientColorTemperatureText.text = "Ambient Light Color: " + frame.LightEstimate.AmbientColorTemperature.ToString();
//         _directionalIntensityText.text = "Directional Light Intensity: " + _currentLightEstimate.AmbientIntensity.ToString();
//         _directionalColorTemperatureText.text = "Directional Light Color: " + _currentLightEstimate.AmbientColorTemperature.ToString();
//     }
// }


using Niantic.ARDK.AR.LightEstimate;
using Niantic.ARDK.Extensions;
using Niantic.ARDK.AR;
using Niantic.ARDK.AR.ARSessionEventArgs;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine;
using static UnityEngine.Mathf;

public class LightEstimation : MonoBehaviour
{
    [SerializeField] private Light _directionalLight;
    private IARLightEstimate _currentLightEstimate;
    [SerializeField] private ARSessionManager _arSessionManager;

    // Store the text for GUI display
    private GUIStyle labelStyle;
    private string _ambientIntensityText;
    private string _ambientColorTemperatureText;
    private string _directionalIntensityText;
    private string _directionalColorTemperatureText;

    private void Start() 
    {
        labelStyle = new GUIStyle();
        labelStyle.fontSize = 48;
        labelStyle.normal.textColor = Color.white;    
    }

    private void OnEnable()
    {
        _directionalLight = FindObjectOfType<Light>();
        _arSessionManager = FindObjectOfType<ARSessionManager>();
        _arSessionManager.ARSession.FrameUpdated += OnFrameUpdated;
    }

    private void OnDisable()
    {
        _arSessionManager.ARSession.FrameUpdated -= OnFrameUpdated;
    }

    private void OnFrameUpdated(FrameUpdatedArgs args)
    {
        var frame = args.Frame;
        _currentLightEstimate = frame.LightEstimate;
        
        if (_currentLightEstimate != null)
        {
            // Update light settings
            _directionalLight.intensity = _currentLightEstimate.AmbientIntensity / 925f;
            _directionalLight.color = CorrelatedColorTemperatureToRGB(_currentLightEstimate.AmbientColorTemperature);

            // Update text values to display
            _ambientIntensityText = $"Ambient Light Intensity: {frame.LightEstimate.AmbientIntensity}";
            _ambientColorTemperatureText = $"Ambient Light Color: {frame.LightEstimate.AmbientColorTemperature}";
            _directionalIntensityText = $"Directional Light Intensity: {_currentLightEstimate.AmbientIntensity}";
            _directionalColorTemperatureText = $"Directional Light Color: {_currentLightEstimate.AmbientColorTemperature}";
        }
        else
        {
            // Set defaults if light estimate is not available
            _directionalLight.intensity = 1f;
            _directionalLight.color = Color.white;

            // Update text values to display
            _ambientIntensityText = "Ambient Light Intensity: N/A";
            _ambientColorTemperatureText = "Ambient Light Color: N/A";
            _directionalIntensityText = "Directional Light Intensity: N/A";
            _directionalColorTemperatureText = "Directional Light Color: N/A";
        }
    }

    void OnGUI()
    {
        // Ensure that the strings are initialized before attempting to display them
        if (_ambientIntensityText != null)
        {
            // Define the position and size of the GUI labels
            GUI.Label(new Rect(10, 10, 500, 20), _ambientIntensityText, labelStyle);
            GUI.Label(new Rect(10, 70, 500, 20), _ambientColorTemperatureText, labelStyle);
            GUI.Label(new Rect(10, 130, 500, 20), _directionalIntensityText, labelStyle);
            GUI.Label(new Rect(10, 190, 500, 20), _directionalColorTemperatureText, labelStyle);
        }
    }
}
