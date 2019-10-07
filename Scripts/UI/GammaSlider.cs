using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class GammaSlider : MonoBehaviour
{
    public Slider _GammaSlider;

    [SerializeField]
    private PostProcessProfile _Profile;
    private ColorGrading _ColorGrading;

    private void Awake()
    {
        _GammaSlider = gameObject.GetComponent<Slider>();
        _GammaSlider.onValueChanged.AddListener(delegate {SetGamma(); });
        _ColorGrading = _Profile.GetSetting<ColorGrading>();
    }
    
    public void SetGamma()
    {
        _ColorGrading.gamma.value = new Color(_GammaSlider.value,_GammaSlider.value,_GammaSlider.value,_GammaSlider.value);
    }
}