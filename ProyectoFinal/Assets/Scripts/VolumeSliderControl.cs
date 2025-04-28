using UnityEngine;
using UnityEngine.UIElements;

public class VolumeSliderControl : VisualElement
{
    public new class UxmlFactory : UxmlFactory<VolumeSliderControl, UxmlTraits> { }
    public new class UxmlTraits : VisualElement.UxmlTraits { }

    private Slider _slider;
    private Label _valueLabel;

    private const string VOLUME_KEY = "MasterVolume";

    public VolumeSliderControl()
    {
       
        style.flexDirection = FlexDirection.Column;

       
        _valueLabel = new Label("100%")
        {
            name = "volume-value",
            style =
            {
                fontSize = 14,
                color = Color.white,
                unityTextAlign = TextAnchor.MiddleCenter
            }
        };
        Add(_valueLabel);

        
        _slider = new Slider(0, 100)
        {
            name = "volume-slider",
            value = LoadVolume(), 
            style =
            {
                height = 20,
                marginTop = 5
            }
        };
        _slider.RegisterValueChangedCallback(UpdateVolume);
        Add(_slider);

       
        UpdateLabel(_slider.value);

        
        _slider.Q("unity-tracker").style.backgroundColor = new Color(0.3f, 0.3f, 0.3f);
        _slider.Q("unity-dragger").style.backgroundColor = Color.cyan;
    }

    private float LoadVolume()
    {
        
        float savedVolume = PlayerPrefs.GetFloat(VOLUME_KEY, 0.7f);
        AudioListener.volume = savedVolume; 
        return savedVolume * 100; 
    }

    private void UpdateVolume(ChangeEvent<float> evt)
    {
        float volume = evt.newValue / 100f;
        UpdateLabel(evt.newValue);
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat(VOLUME_KEY, volume);
        PlayerPrefs.Save(); 
    }

    private void UpdateLabel(float value)
    {
        _valueLabel.text = $"{Mathf.RoundToInt(value)}%";
    }
}