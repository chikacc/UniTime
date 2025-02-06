using TMPro;
using UnityEngine;

namespace UniTime.Samples {
    [DisallowMultipleComponent]
    public class LapElement : MonoBehaviour {
        [SerializeField] TMP_Text _lap;
        [SerializeField] TMP_Text _elapsed;

        public string Lap { get => _lap.text; set => _lap.text = value; }
        public string Elapsed { get => _elapsed.text; set => _elapsed.text = value; }
        public double StartTime { get; set; }
    }
}
