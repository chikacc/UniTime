using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UniTime.Samples {
    [ExecuteAlways]
    [DisallowMultipleComponent]
    public sealed class ToggleMemoryHelper : MonoBehaviour {
        [SerializeField] Toggle[] _toggles;

        void Start() {
            var key = $"{nameof(ToggleMemoryHelper)}.{nameof(_toggles)}.{nameof(Toggle.isOn)}";
            var isOns = PlayerPrefs.GetString(key, "True").Split(',').ToArray();
            for (var i = 0; i < _toggles.Length && i < isOns.Length; ++i) _toggles[i].isOn = bool.Parse(isOns[i]);
        }

        void OnDestroy() {
            var key = $"{nameof(ToggleMemoryHelper)}.{nameof(_toggles)}.{nameof(Toggle.isOn)}";
            var isOns = _toggles.Select(x => x.isOn);
            PlayerPrefs.SetString(key, string.Join(',', isOns));
            PlayerPrefs.Save();
        }
    }
}
