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

    protected void OnValidate() {
        if (useBloom != bloomBefore) {
            SetBloom(useBloom);
        }
        volumeProfile = volume.profile;
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
        var bloom = volumeProfile.components.Find(v => v is Bloom);
        bloom.active = isActive;
    }
}
