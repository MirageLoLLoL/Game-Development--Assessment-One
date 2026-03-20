using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public OrbitCamera cameraSettings;
    public TMP_Text sensValue;
    Slider sensitivitySlider;
    private void Awake()
    {
        sensitivitySlider = GetComponent<Slider>();
        int tempNumber = Mathf.RoundToInt(cameraSettings.rotationSpeed);
        sensValue.text = tempNumber.ToString();
        sensitivitySlider.value = cameraSettings.rotationSpeed;
    }
    public void SetSensitivity()
    {
        cameraSettings.rotationSpeed = sensitivitySlider.value;
        int tempNumber = Mathf.RoundToInt(cameraSettings.rotationSpeed);

        sensValue.text = tempNumber.ToString();

    }
}
