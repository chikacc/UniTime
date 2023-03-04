using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UniTime.Samples {
    [ExecuteAlways]
    [DisallowMultipleComponent]
    public class SnapScrollRect : ScrollRect, IPointerDownHandler, IPointerUpHandler {
        static readonly PropertyInfo RectChildrenProperty =
            typeof(LayoutGroup).GetProperty("rectChildren", BindingFlags.Instance | BindingFlags.NonPublic);

        [SerializeField] float _snapDuration = 0.1f;
        [SerializeField] int _index;
        [SerializeField] bool _interactable = true;
        [SerializeField] OnChangeEvent _onValueChanged;

        float[] _childPositions = Array.Empty<float>();
        bool _dragging;
        bool _snapping;

        public float SnapDuration { get => _snapDuration; set => _snapDuration = value; }
        public int Index { get => _index; set => SetIndex(value); }
        public bool Interactable { get => _interactable; set => _interactable = value; }
        public OnChangeEvent OnValueChanged { get => _onValueChanged; set => _onValueChanged = value; }

        protected override void LateUpdate() {
            if (!_interactable) {
                velocity = Vector2.zero;
                return;
            }

            base.LateUpdate();
            if (_snapping) return;
            if (_dragging) return;
            var axis = horizontal ? 0 : 1;
            if (Mathf.Abs(velocity[axis]) < 1f) SnapTo(_index);
            else Snap();
        }

        public override void Rebuild(CanvasUpdate executing) {
            base.Rebuild(executing);
            if (executing != CanvasUpdate.PostLayout) return;
            EnsureSnapStopped();
            UpdateChildPositions();
            UpdateIndex();
            UpdateNormalizedPosition();
        }

        public void OnPointerDown(PointerEventData eventData) {
            EnsureSnapStopped();
            if (eventData.button != PointerEventData.InputButton.Left) return;
            if (!IsActive()) return;
            _dragging = true;
        }

        public void OnPointerUp(PointerEventData eventData) {
            if (eventData.button != PointerEventData.InputButton.Left) return;
            if (!IsActive()) return;
            _dragging = false;
        }

        void UpdateChildPositions() {
            var childCount = GetContentChildCount();
            _childPositions = childCount > 0 ? new float[childCount] : Array.Empty<float>();
            if (childCount == 0) return;
            var spacing = 1f / (childCount - 1);
            for (var i = 0; i < childCount; ++i) _childPositions[i] = (childCount - 1 - i) * spacing;
        }

        void UpdateIndex() {
            SetIndex(Mathf.Clamp(_index, 0, _childPositions.Length - 1));
            _onValueChanged?.Invoke(_index);
        }

        void UpdateNormalizedPosition() {
            var position = _childPositions.Length > 0 ? _childPositions[_index] : 1f;
            if (horizontal) horizontalNormalizedPosition = position;
            else verticalNormalizedPosition = position;
        }

        int GetContentChildCount() {
            if (!content.TryGetComponent(out LayoutGroup layoutGroup)) return content.childCount;
            if (RectChildrenProperty.GetValue(layoutGroup) is not List<RectTransform> { Count: var childCount })
                return content.childCount;
            return childCount;
        }

        void SetIndex(int value) {
            if (value == _index) return;
            _index = value;
            _onValueChanged?.Invoke(value);
        }

        void EnsureSnapStopped() {
            StopAllCoroutines();
            _snapping = false;
        }

        void Snap() {
            EnsureSnapStopped();
            if (_childPositions.Length < 1) return;
            StartCoroutine(SnapRoutine());
        }

        public void SnapTo(int index) {
            EnsureSnapStopped();
            if (_childPositions.Length < 1) return;
            var axis = horizontal ? 0 : 1;
            StartCoroutine(SnapToIndexRoutine(index, axis));
        }

        IEnumerator SnapRoutine() {
            _snapping = true;
            var axis = horizontal ? 0 : 1;
            var snapThreshold = content.rect.size[axis] / _childPositions.Length;
            if (Mathf.Abs(velocity[axis]) <= 0.5f * snapThreshold) {
                yield return SnapToIndexRoutine(_index, axis);
                yield break;
            }

            while (Mathf.Abs(velocity[axis]) > snapThreshold) {
                SetIndex(CalculateClosetIndex(axis));
                yield return null;
            }

            yield return SnapToIndexRoutine(CalculateNextIndex(axis), axis);
        }

        IEnumerator SnapToIndexRoutine(int index, int axis) {
            var current = axis == 0 ? horizontalNormalizedPosition
                : verticalNormalizedPosition;
            if (current < 0 || current >= _childPositions.Length) yield break;
            _snapping = true;
            var target = _childPositions[index];
            var maxSpeed = Mathf.Max(Mathf.Abs(velocity[axis] / content.rect.size[axis]), 1f / _snapDuration);
            var speed = 0f;
            while (Mathf.Abs(target - current) > float.Epsilon) {
                current = Mathf.SmoothDamp(current, target, ref speed, _snapDuration, maxSpeed, Time.unscaledDeltaTime);
                if (axis == 0) horizontalNormalizedPosition = current;
                else verticalNormalizedPosition = current;
                SetIndex(CalculateClosetIndex(axis));
                yield return null;
            }

            if (axis == 0) horizontalNormalizedPosition = target;
            else verticalNormalizedPosition = target;
            SetIndex(CalculateClosetIndex(axis));
            _snapping = false;
        }

        float CalculateClosetPosition(int axis) {
            if (_childPositions is not { Length: > 1 and var childCount }) return 0;
            return (1f - Mathf.Clamp01(normalizedPosition[axis])) * (childCount - 1);
        }

        int CalculateClosetIndex(int axis) => Mathf.RoundToInt(CalculateClosetPosition(axis));

        int CalculateNextIndex(int axis) {
            var value = CalculateClosetPosition(axis);
            return velocity[axis] > 0f ? Mathf.CeilToInt(value) : Mathf.FloorToInt(value);
        }

        [Serializable]
        public class OnChangeEvent : UnityEvent<int> { }
    }
}
