using TMPro;
using UnityEngine;

namespace UniTime.Samples {
    [DisallowMultipleComponent]
    public class TextColorHelper : MonoBehaviour {
        [SerializeField] TMP_Text _text;
        [SerializeField] Color _isOff;
        [SerializeField] Color _isOn;

        public void IsOn(bool value) => _text.color = value ? _isOn : _isOff;
    }
}
