using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace UniTime.Samples.Editor {
    [CustomEditor(typeof(Annulus))]
    [CanEditMultipleObjects]
    public sealed class AnnulusEditor : UnityEditor.Editor {
        public override VisualElement CreateInspectorGUI() {
            var container = new VisualElement();
            var iterator = serializedObject.GetIterator();
            if (!iterator.NextVisible(true)) return container;
            do {
                var path = iterator.propertyPath;
                VisualElement child = path switch {
                    "_segments" => new SliderInt("Segments", 3, 720) { bindingPath = path, showInputField = true },
                    "_fillAmount" => new Slider("Fill Amount", 0f, 1f) { bindingPath = path, showInputField = true },
                    _ => new PropertyField(iterator) { name = "PropertyField:" + path }
                };
                if (path == "m_Script") child.SetEnabled(false);
                container.Add(child);
            } while (iterator.NextVisible(false));

            return container;
        }
    }
}
