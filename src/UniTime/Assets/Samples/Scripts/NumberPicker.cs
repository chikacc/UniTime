using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UniTime.Samples {
    [ExecuteAlways]
    [DisallowMultipleComponent]
    public class NumberPicker : UIBehaviour, ICanvasElement {
        [SerializeField] RectTransform _rectTransform;
        [SerializeField] SnapScrollRect _helper;
        [SerializeField] RectTransform _content;
        [SerializeField] int _value;
        [SerializeField] bool _interactable = true;
        [SerializeField] string _monospace = "0.6em";
        [SerializeField] OnChangeEvent _onValueChanged;

        public int Value { get => _value; set => SetValue(value); }
        public bool Interactable { get => _interactable; set => SetInteractable(value); }

        protected override void OnEnable() {
            base.OnEnable();
            CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
            SetDirty();
        }

        protected override void OnDisable() {
            CanvasUpdateRegistry.UnRegisterCanvasElementForRebuild(this);
            LayoutRebuilder.MarkLayoutForRebuild(_rectTransform);
            base.OnDisable();
        }

#if UNITY_EDITOR
        protected override void OnValidate() => SetDirtyCaching();
#endif

        public void Rebuild(CanvasUpdate executing) {
            if (executing != CanvasUpdate.LatePreRender) return;
            UpdateNumbers();
            UpdateInteractable();
        }

        public void LayoutComplete() { }
        public void GraphicUpdateComplete() { }

        void SetDirty() {
            if (!IsActive()) return;
            if (!CanvasUpdateRegistry.IsRebuildingLayout()) LayoutRebuilder.MarkLayoutForRebuild(_rectTransform);
            else StartCoroutine(DelayedSetDirty(_rectTransform));
        }

        static IEnumerator DelayedSetDirty(RectTransform rectTransform) {
            yield return null;
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        }

        void SetDirtyCaching() {
            if (!IsActive()) return;
            CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
            LayoutRebuilder.MarkLayoutForRebuild(_rectTransform);
        }

        void UpdateNumbers() {
            for (var i = 0; i < _content.childCount; ++i) {
                var text = _content.GetChild(i).GetComponent<TMP_Text>();
                text.text = $"<mspace={_monospace}>{i}</mspace>";
            }

            _helper.Index = _value;
        }

        void UpdateInteractable() => _helper.Interactable = _interactable;

        void SetValue(int value) {
            if (!_interactable) return;
            if (value == _value) return;
            _value = value;
            _helper.Index = value;
            _onValueChanged?.Invoke(value);
        }

        void SetInteractable(bool value) {
            if (_interactable == value) return;
            _interactable = value;
            _helper.Interactable = value;
        }

        [Serializable]
        public class OnChangeEvent : UnityEvent<int> { }
    }
}
