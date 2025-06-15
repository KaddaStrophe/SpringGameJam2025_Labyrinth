using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OutfitManager : MonoBehaviour {
    [SerializeField]
    bool useBloom;
    [SerializeField]
    Volume volume;

    bool bloomBefore;
    VolumeProfile volumeProfile;
    [SerializeField]
    Bloom bloom;

    protected void OnValidate() {
        if (useBloom != bloomBefore) {
            SetBloom(useBloom);
        }
        volumeProfile = volume.profile;
        volumeProfile.TryGet(out bloom);
    }

    protected void Start() {
        SetBloom(useBloom);
    }

    protected void Update() {
        if (useBloom != bloomBefore) {
            SetBloom(useBloom);
        }
    }

    void SetBloom(bool isActive) {
        bloomBefore = useBloom;
        bloom.active = isActive;
        if (useBloom) {
            //StartCoroutine(CirculateColor());
        }
    }

    IEnumerator CirculateColor() {
        Color.RGBToHSV((Color)bloom.tint, out float h, out float s, out float v);
        while (useBloom) {
            h += Time.deltaTime * .1f;
            h = h > 100 ? 0 : h;
            bloom.tint.Override(Color.HSVToRGB(h, s, v));
            
            yield return null;
        }
    }
}
