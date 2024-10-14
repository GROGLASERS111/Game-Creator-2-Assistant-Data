using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class GraphicsSettingsManager : MonoBehaviour
{
    public Camera mainCamera;
    public Volume postProcessProfile;

    public Button resetButton;

    public Dropdown resolutionDropdown;
    public Toggle fullScreenToggle;
    public Toggle vsyncToggle;
    public Dropdown graphicsQualityDropdown;
    public Dropdown antiAliasingDropdown;
    public Dropdown textureQualityDropdown;
    public Dropdown refreshRateDropdown;
    public Slider shadowDistanceSlider;
    public Text shadowDistanceText;
    public Dropdown softShadowQualityDropdown;

    public Toggle bloomToggle;
    public Toggle motionBlurToggle;
    public Toggle depthOfFieldToggle;

    private Bloom bloom;
    private MotionBlur motionBlur;
    private DepthOfField depthOfField;

    private void Start()
    {
        // Populate dropdowns with options
        resolutionDropdown.AddOptions(GetAvailableResolutions());
        graphicsQualityDropdown.AddOptions(GetAvailableQualities());
        antiAliasingDropdown.AddOptions(GetAvailableAntiAliasingOptions());
        textureQualityDropdown.AddOptions(GetAvailableTextureQualities());
        refreshRateDropdown.AddOptions(GetAvailableRefreshRatesDistinct());
        softShadowQualityDropdown.AddOptions(GetAvailableSoftShadowQualities());

        // Set initial dropdown values to current settings
        resolutionDropdown.value = GetCurrentResolutionIndex();
        fullScreenToggle.isOn = Screen.fullScreen;
        vsyncToggle.isOn = QualitySettings.vSyncCount != 0;
        graphicsQualityDropdown.value = QualitySettings.GetQualityLevel();

        // Update anti-aliasing dropdown value based on QualitySettings
        switch (QualitySettings.antiAliasing)
        {
            case 0:
                antiAliasingDropdown.value = 0;
                break;
            case 2:
                antiAliasingDropdown.value = 1;
                break;
            case 4:
                antiAliasingDropdown.value = 2;
                break;
            case 8:
                antiAliasingDropdown.value = 3;
                break;
            default:
                antiAliasingDropdown.value = 0; // Default to 'Disabled' if an unrecognized value is found
                break;
        }

        textureQualityDropdown.value = QualitySettings.globalTextureMipmapLimit;
        refreshRateDropdown.value = GetCurrentRefreshRateIndex();
        shadowDistanceSlider.value = QualitySettings.shadowDistance;
        shadowDistanceText.text = QualitySettings.shadowDistance.ToString();

        // Update soft shadow quality dropdown based on QualitySettings
        softShadowQualityDropdown.value = QualitySettings.shadowCascades switch
        {
            0 => 0,
            2 => 1,
            4 => 2,
            _ => 0, // Default to 'No Cascades' if an unrecognized value is found
        };

        // Set up dropdown onChange events
        resolutionDropdown.onValueChanged.AddListener(index => SetResolution(index));
        fullScreenToggle.onValueChanged.AddListener(value => SetFullScreen(value));
        vsyncToggle.onValueChanged.AddListener(value => SetVSync(value));
        graphicsQualityDropdown.onValueChanged.AddListener(index => SetGraphicsQuality(index));
        antiAliasingDropdown.onValueChanged.AddListener(index => SetAntiAliasingQuality(index));
        textureQualityDropdown.onValueChanged.AddListener(index => SetTextureQuality(index));
        refreshRateDropdown.onValueChanged.AddListener(index => SetRefreshRate(index));
        shadowDistanceSlider.onValueChanged.AddListener(value => SetShadowDistance(value));
        softShadowQualityDropdown.onValueChanged.AddListener(index => SetShadowDistance(index));
        resetButton.onClick.AddListener(ResetToDefaults);

        // Get current post-process settings
        if (postProcessProfile.profile.TryGet(out bloom))
            bloomToggle.isOn = bloom.active;

        if (postProcessProfile.profile.TryGet(out motionBlur))
            motionBlurToggle.isOn = motionBlur.active;

        if (postProcessProfile.profile.TryGet(out depthOfField))
            depthOfFieldToggle.isOn = depthOfField.active;

        // Set up toggle onChange events
        bloomToggle.onValueChanged.AddListener(value => SetBloom(value));
        motionBlurToggle.onValueChanged.AddListener(value => SetMotionBlur(value));
        depthOfFieldToggle.onValueChanged.AddListener(value => SetDepthOfField(value));


        // Set initial toggle values
        bloomToggle.isOn = bloom != null && bloom.active;
        motionBlurToggle.isOn = motionBlur != null && motionBlur.active;
        depthOfFieldToggle.isOn = depthOfField != null && depthOfField.active;

        // Set up toggle onChange events
        bloomToggle.onValueChanged.AddListener(value => SetBloom(value));
        motionBlurToggle.onValueChanged.AddListener(value => SetMotionBlur(value));
        depthOfFieldToggle.onValueChanged.AddListener(value => SetDepthOfField(value));
    }

    private int GetCurrentResolutionIndex()
    {
        string currentResolution = Screen.width + "x" + Screen.height;
        return resolutionDropdown.options.FindIndex(option => option.text == currentResolution);
    }

    private int GetCurrentRefreshRateIndex()
    {
        string currentRefreshRate = Screen.currentResolution.refreshRateRatio.ToString() + "Hz";
        return refreshRateDropdown.options.FindIndex(option => option.text == currentRefreshRate);
    }

    private List<string> GetAvailableResolutions()
    {
        HashSet<string> uniqueResolutions = new HashSet<string>();
        foreach (var res in Screen.resolutions)
        {
            uniqueResolutions.Add(res.width + "x" + res.height);
        }
        return uniqueResolutions.ToList();
    }

    private List<string> GetAvailableQualities()
    {
        return new List<string>(QualitySettings.names);
    }

    private List<string> GetAvailableAntiAliasingOptions()
    {
        return new List<string>() { "Disabled", "FXAA", "SMAA", "TSAA" };
    }

    private List<string> GetAvailableTextureQualities()
    {
        return new List<string>() { "Low", "Medium", "High" };
    }

    private List<string> GetAvailableRefreshRatesDistinct()
    {
        HashSet<string> refreshRates = new HashSet<string>();
        foreach (var rate in Screen.resolutions)
        {
            string refreshRate = rate.refreshRateRatio.ToString() + "Hz";
            refreshRates.Add(refreshRate);
        }
        return new List<string>(refreshRates);
    }

    private List<string> GetAvailableSoftShadowQualities()
    {
        return new List<string>() { "No Cascades", "Two Cascades", "Four Cascades" };
    }

    private void SetResolution(int index)
    {
        var parts = resolutionDropdown.options[index].text.Split('x');
        int width = int.Parse(parts[0]);
        int height = int.Parse(parts[1]);
        Screen.SetResolution(width, height, Screen.fullScreen);

        PlayerPrefs.SetInt("ResolutionWidth", width);
        PlayerPrefs.SetInt("ResolutionHeight", height);
    }

    private void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        PlayerPrefs.SetInt("FullScreen", isFullScreen ? 1 : 0);
    }

    private void SetVSync(bool isEnabled)
    {
        QualitySettings.vSyncCount = isEnabled ? 1 : 0;
        PlayerPrefs.SetInt("VSync", isEnabled ? 1 : 0);
    }

    private void SetGraphicsQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("GraphicsQuality", qualityIndex);
    }

    private void SetAntiAliasingQuality(int optionIndex)
    {
        switch (optionIndex)
        {
            case 0:
                QualitySettings.antiAliasing = 0;
                break;
            case 1:
                QualitySettings.antiAliasing = 2;
                break;
            case 2:
                QualitySettings.antiAliasing = 4;
                break;
            case 3:
                QualitySettings.antiAliasing = 8;
                break;
            default:
                Debug.LogError("Invalid Anti-Aliasing option index: " + optionIndex);
                break;
        }

        PlayerPrefs.SetInt("AntiAliasingQuality", optionIndex);
    }

    private void SetRefreshRate(int index)
    {
        string refreshRateString = refreshRateDropdown.options[index].text;
        int refreshRate = int.Parse(refreshRateString.Substring(0, refreshRateString.Length - 2));

        PlayerPrefs.SetInt("RefreshRate", refreshRate);
    }

    private void SetTextureQuality(int qualityIndex)
    {
        QualitySettings.globalTextureMipmapLimit = qualityIndex;
        PlayerPrefs.SetInt("TextureQuality", qualityIndex);
    }

    private void SetShadowDistance(float value)
    {
        QualitySettings.shadowDistance = value;
        shadowDistanceText.text = value.ToString();
        PlayerPrefs.SetFloat("ShadowDistance", value);
    }

    private void SetBloom(bool isEnabled)
    {
        if (bloom != null)
        {
            bloom.active = isEnabled;
        }
        PlayerPrefs.SetInt("Bloom", isEnabled ? 1 : 0);
    }

    private void SetMotionBlur(bool isEnabled)
    {
        if (motionBlur != null)
        {
            motionBlur.active = isEnabled;
        }
        PlayerPrefs.SetInt("MotionBlur", isEnabled ? 1 : 0);
    }

    private void SetDepthOfField(bool isEnabled)
    {
        if (depthOfField != null)
        {
            depthOfField.active = isEnabled;
        }
        PlayerPrefs.SetInt("DepthOfField", isEnabled ? 1 : 0);
    }

    private void ResetToDefaults()
    {
        // Set default screen resolution and other settings
        Screen.SetResolution(1920, 1080, true);
        QualitySettings.vSyncCount = 1;
        QualitySettings.SetQualityLevel(2);
        QualitySettings.globalTextureMipmapLimit = 0; // 0 for full resolution
        QualitySettings.antiAliasing = 2; // For example, 2x MSAA
        QualitySettings.shadowDistance = 150f;
        QualitySettings.shadowCascades = 2; // For example, 2 cascades

        // Update UI elements to reflect these default values
        resolutionDropdown.value = GetCurrentResolutionIndex();
        fullScreenToggle.isOn = Screen.fullScreen;
        vsyncToggle.isOn = QualitySettings.vSyncCount != 0;
        graphicsQualityDropdown.value = QualitySettings.GetQualityLevel();
        textureQualityDropdown.value = QualitySettings.globalTextureMipmapLimit;

        // Update dropdowns and sliders for anti-aliasing and shadows
        antiAliasingDropdown.value = 1; // Corresponds to 2x MSAA
        shadowDistanceSlider.value = QualitySettings.shadowDistance;
        shadowDistanceText.text = QualitySettings.shadowDistance.ToString();
        softShadowQualityDropdown.value = QualitySettings.shadowCascades == 2 ? 1 : 0;

        // Save these default values to PlayerPrefs
        PlayerPrefs.SetInt("ResolutionWidth", 1920);
        PlayerPrefs.SetInt("ResolutionHeight", 1080);
        PlayerPrefs.SetInt("FullScreen", 1);
        PlayerPrefs.SetInt("VSync", 1);
        PlayerPrefs.SetInt("GraphicsQuality", 2);
        PlayerPrefs.SetInt("TextureQuality", 0); // 0 for full resolution
        PlayerPrefs.SetInt("AntiAliasingQuality", 1); // Corresponds to 2x MSAA
        PlayerPrefs.SetFloat("ShadowDistance", 150f);
        PlayerPrefs.SetInt("SoftShadowQuality", QualitySettings.shadowCascades == 2 ? 1 : 0);
    }
}
