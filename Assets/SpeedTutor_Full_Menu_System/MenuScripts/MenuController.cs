using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    #region Initialisation - Button Selection & Menu Order

    private void Start()
    {
        menuNumber = 1;
    }

    #endregion

    //MAIN SECTION

    #region Confrimation Box

    public IEnumerator ConfirmationBox()
    {
        confirmationMenu.SetActive(value: true);
        yield return new WaitForSeconds(seconds: 2);
        confirmationMenu.SetActive(value: false);
    }

    #endregion

    private void ClickSound()
    {
        GetComponent<AudioSource>().Play();
    }

    #region Menu Mouse Clicks

    public void MouseClick(string buttonType)
    {
        if (buttonType == "Controls")
        {
            gameplayMenu.SetActive(value: false);
            controlsMenu.SetActive(value: true);
            menuNumber = 6;
        }

        if (buttonType == "Graphics")
        {
            GeneralSettingsCanvas.SetActive(value: false);
            graphicsMenu.SetActive(value: true);
            menuNumber = 3;
        }

        if (buttonType == "Sound")
        {
            GeneralSettingsCanvas.SetActive(value: false);
            soundMenu.SetActive(value: true);
            menuNumber = 4;
        }

        if (buttonType == "Gameplay")
        {
            GeneralSettingsCanvas.SetActive(value: false);
            gameplayMenu.SetActive(value: true);
            menuNumber = 5;
        }

        if (buttonType == "Exit")
        {
            Debug.Log(message: "YES QUIT!");
            Application.Quit();
        }

        if (buttonType == "Options")
        {
            menuDefaultCanvas.SetActive(value: false);
            GeneralSettingsCanvas.SetActive(value: true);
            menuNumber = 2;
        }

        if (buttonType == "LoadGame")
        {
            menuDefaultCanvas.SetActive(value: false);
            loadGameDialog.SetActive(value: true);
            menuNumber = 8;
        }

        if (buttonType == "NewGame")
        {
            menuDefaultCanvas.SetActive(value: false);
            newGameDialog.SetActive(value: true);
            menuNumber = 7;
        }
    }

    #endregion

    #region Controller Sensitivity

    public void ControllerSen()
    {
        controllerSenText.text = controllerSenSlider.value.ToString(format: "0");
        controlSenFloat = controllerSenSlider.value;
    }

    #endregion

    public void GameplayApply()
    {
        #region Invert

        if (invertYToggle.isOn) //Invert Y ON
        {
            PlayerPrefs.SetInt(key: "masterInvertY", value: 1);
            Debug.Log(message: "Invert" + " " + PlayerPrefs.GetInt(key: "masterInvertY"));
        }

        else if (!invertYToggle.isOn) //Invert Y OFF
        {
            PlayerPrefs.SetInt(key: "masterInvertY", value: 0);
            Debug.Log(message: PlayerPrefs.GetInt(key: "masterInvertY"));
        }

        #endregion

        #region Controller Sensitivity

        PlayerPrefs.SetFloat(key: "masterSen", value: controlSenFloat);
        Debug.Log(message: "Sensitivity" + " " + PlayerPrefs.GetFloat(key: "masterSen"));

        #endregion

        StartCoroutine(routine: ConfirmationBox());
    }

    #region ResetButton

    public void ResetButton(string GraphicsMenu)
    {
        if (GraphicsMenu == "Brightness")
        {
            brightnessEffect.brightness = defaultBrightness;
            brightnessSlider.value = defaultBrightness;
            brightnessText.text = defaultBrightness.ToString(format: "0.0");
            BrightnessApply();
        }

        if (GraphicsMenu == "Audio")
        {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeText.text = defaultVolume.ToString(format: "0.0");
            VolumeApply();
        }

        if (GraphicsMenu == "Graphics")
        {
            controllerSenText.text = defaultSen.ToString(format: "0");
            controllerSenSlider.value = defaultSen;
            controlSenFloat = defaultSen;

            invertYToggle.isOn = false;

            GameplayApply();
        }
    }

    #endregion

    #region Default Values

    [Header(header: "Default Menu Values")]
    [SerializeField]
    private float defaultBrightness;

    [SerializeField]
    private float defaultVolume;

    [SerializeField]
    private int defaultSen;

    [SerializeField]
    private bool defaultInvertY;

    [Header(header: "Levels To Load")]
    public string level;

    private string levelToLoad;

    [SerializeField]
    private int menuNumber;

    #endregion

    #region Menu Dialogs

    [Header(header: "Main Menu Components")]
    [SerializeField]
    private GameObject menuDefaultCanvas;

    [SerializeField]
    private GameObject GeneralSettingsCanvas;

    [SerializeField]
    private GameObject graphicsMenu;

    [SerializeField]
    private GameObject soundMenu;

    [SerializeField]
    private GameObject gameplayMenu;

    [SerializeField]
    private GameObject controlsMenu;

    [SerializeField]
    private GameObject confirmationMenu;

    [Space(height: 10)]
    [Header(header: "Menu Popout Dialogs")]
    [SerializeField]
    private GameObject noSaveDialog;

    [SerializeField]
    private GameObject newGameDialog;

    [SerializeField]
    private GameObject loadGameDialog;

    #endregion

    #region Slider Linking

    [Header(header: "Menu Sliders")]
    [SerializeField]
    private Text controllerSenText;

    [SerializeField]
    private Slider controllerSenSlider;

    public float controlSenFloat = 2f;

    [Space(height: 10)]
    [SerializeField]
    private Brightness brightnessEffect;

    [SerializeField]
    private Slider brightnessSlider;

    [SerializeField]
    private Text brightnessText;

    [Space(height: 10)]
    [SerializeField]
    private Text volumeText;

    [SerializeField]
    private Slider volumeSlider;

    [Space(height: 10)]
    [SerializeField]
    private Toggle invertYToggle;

    #endregion

    #region Volume Sliders Click

    public void VolumeSlider(float volume)
    {
        AudioListener.volume = volume;
        volumeText.text = volume.ToString(format: "0.0");
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat(key: "masterVolume", value: AudioListener.volume);
        Debug.Log(message: PlayerPrefs.GetFloat(key: "masterVolume"));
        StartCoroutine(routine: ConfirmationBox());
    }

    #endregion

    #region Brightness Sliders Click

    public void BrightnessSlider(float brightness)
    {
        brightnessEffect.brightness = brightness;
        brightnessText.text = brightness.ToString(format: "0.0");
    }

    public void BrightnessApply()
    {
        PlayerPrefs.SetFloat(key: "masterBrightness", value: brightnessEffect.brightness);
        Debug.Log(message: PlayerPrefs.GetFloat(key: "masterBrightness"));
        StartCoroutine(routine: ConfirmationBox());
    }

    #endregion

    #region Dialog Options

    public void ClickNewGameDialog(string ButtonType)
    {
        if (ButtonType == "Yes") SceneManager.LoadScene(sceneName: level);

        if (ButtonType == "No") GoBackToMainMenu();
    }

    public void ClickLoadGameDialog(string ButtonType)
    {
        if (ButtonType == "Yes")
        {
            if (PlayerPrefs.HasKey(key: "SavedLevel"))
            {
                Debug.Log(message: "I WANT TO LOAD THE SAVED GAME");
                //LOAD LAST SAVED SCENE
                levelToLoad = PlayerPrefs.GetString(key: "SavedLevel");
                SceneManager.LoadScene(sceneName: levelToLoad);
            }

            else
            {
                Debug.Log(message: "Load Game Dialog");
                menuDefaultCanvas.SetActive(value: false);
                loadGameDialog.SetActive(value: false);
                noSaveDialog.SetActive(value: true);
            }
        }

        if (ButtonType == "No") GoBackToMainMenu();
    }

    #endregion

    #region Back to Menus

    public void GoBackToOptionsMenu()
    {
        GeneralSettingsCanvas.SetActive(value: true);
        graphicsMenu.SetActive(value: false);
        soundMenu.SetActive(value: false);
        gameplayMenu.SetActive(value: false);

        GameplayApply();
        BrightnessApply();
        VolumeApply();

        menuNumber = 2;
    }

    public void GoBackToMainMenu()
    {
        menuDefaultCanvas.SetActive(value: true);
        newGameDialog.SetActive(value: false);
        loadGameDialog.SetActive(value: false);
        noSaveDialog.SetActive(value: false);
        GeneralSettingsCanvas.SetActive(value: false);
        graphicsMenu.SetActive(value: false);
        soundMenu.SetActive(value: false);
        gameplayMenu.SetActive(value: false);
        menuNumber = 1;
    }

    public void GoBackToGameplayMenu()
    {
        controlsMenu.SetActive(value: false);
        gameplayMenu.SetActive(value: true);
        menuNumber = 5;
    }

    public void ClickQuitOptions()
    {
        GoBackToMainMenu();
    }

    public void ClickNoSaveDialog()
    {
        GoBackToMainMenu();
    }

    #endregion
}