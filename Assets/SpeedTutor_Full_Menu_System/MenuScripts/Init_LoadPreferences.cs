using UnityEngine;
using UnityEngine.UI;

public class Init_LoadPreferences : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log(message: "Loading player prefs test");

        if (canUse)
        {
            //BRIGHTNESS
            if (brightnessEffect != null)
            {
                if (PlayerPrefs.HasKey(key: "masterBrightness"))
                {
                    float localBrightness = PlayerPrefs.GetFloat(key: "masterBrightness");

                    brightnessText.text = localBrightness.ToString(format: "0.0");
                    brightnessSlider.value = localBrightness;
                    brightnessEffect.brightness = localBrightness;
                }

                else
                {
                    menuController.ResetButton(GraphicsMenu: "Brightness");
                }
            }

            //VOLUME
            if (PlayerPrefs.HasKey(key: "masterVolume"))
            {
                float localVolume = PlayerPrefs.GetFloat(key: "masterVolume");

                volumeText.text = localVolume.ToString(format: "0.0");
                volumeSlider.value = localVolume;
                AudioListener.volume = localVolume;
            }
            else
            {
                menuController.ResetButton(GraphicsMenu: "Audio");
            }

            //CONTROLLER SENSITIVITY
            if (PlayerPrefs.HasKey(key: "masterSen"))
            {
                float localSensitivity = PlayerPrefs.GetFloat(key: "masterSen");

                controllerText.text = localSensitivity.ToString(format: "0");
                controllerSlider.value = localSensitivity;
                menuController.controlSenFloat = localSensitivity;
            }
            else
            {
                menuController.ResetButton(GraphicsMenu: "Graphics");
            }

            //INVERT Y
            if (PlayerPrefs.HasKey(key: "masterInvertY"))
            {
                if (PlayerPrefs.GetInt(key: "masterInvertY") == 1)
                    invertYToggle.isOn = true;

                else
                    invertYToggle.isOn = false;
            }
        }
    }

    #region Variables

    //BRIGHTNESS
    [Space(height: 20)]
    [SerializeField]
    private Brightness brightnessEffect;

    [SerializeField]
    private Text brightnessText;

    [SerializeField]
    private Slider brightnessSlider;

    //VOLUME
    [Space(height: 20)]
    [SerializeField]
    private Text volumeText;

    [SerializeField]
    private Slider volumeSlider;

    //SENSITIVITY
    [Space(height: 20)]
    [SerializeField]
    private Text controllerText;

    [SerializeField]
    private Slider controllerSlider;

    //INVERT Y
    [Space(height: 20)]
    [SerializeField]
    private Toggle invertYToggle;

    [Space(height: 20)]
    [SerializeField]
    private bool canUse;

    [SerializeField]
    private bool isMenu;

    [SerializeField]
    private MenuController menuController;

    #endregion
}