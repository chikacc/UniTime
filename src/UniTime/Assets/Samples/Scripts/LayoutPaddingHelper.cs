using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UniTime.Samples {
    [ExecuteAlways]
    [DisallowMultipleComponent]
    public class LayoutPaddingHelper : UIBehaviour, ICanvasElement {
        static readonly PropertyInfo RectChildrenProperty =
            typeof(LayoutGroup).GetProperty("rectChildren", BindingFlags.Instance | BindingFlags.NonPublic);

        [SerializeField] RectTransform _rectTransform;
        [SerializeField] ScrollRect _scrollRect;
        [SerializeField] LayoutGroup _layoutGroup;
        [SerializeField] RectMask2D _mask;

        bool _layoutBuilt;

        void LateUpdate() => EnsureLayoutRebuilt();

        protected override void OnEnable() {
            base.OnEnable();
            CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
            SetDirty();
        }

        protected override void OnDisable() {
            CanvasUpdateRegistry.UnRegisterCanvasElementForRebuild(this);
            _layoutBuilt = false;
            LayoutRebuilder.MarkLayoutForRebuild(_rectTransform);
            base.OnDisable();
        }

        protected override void OnDidApplyAnimationProperties() => Rebuild(CanvasUpdate.PostLayout);
        protected override void OnRectTransformDimensionsChange() => Rebuild(CanvasUpdate.PostLayout);
#if UNITY_EDITOR
        protected override void OnValidate() => SetDirtyCaching();
#endif
        public void Rebuild(CanvasUpdate executing) {
            if (executing != CanvasUpdate.PostLayout) return;
            UpdateLayoutGroupPadding();
            UpdateMaskSoftness();
            _layoutBuilt = true;
        }

        public void LayoutComplete() { }
        public void GraphicUpdateComplete() { }

        void UpdateLayoutGroupPadding() {
            var viewportSize = Vector2Int.RoundToInt(0.5f * _scrollRect.viewport.rect.size);
            if (_scrollRect.horizontal) {
                _layoutGroup.padding.left = viewportSize.x;
                _layoutGroup.padding.right = viewportSize.x;
            } else {
                _layoutGroup.padding.top = viewportSize.y;
                _layoutGroup.padding.bottom = viewportSize.y;
            }

            if (RectChildrenProperty.GetValue(_layoutGroup) is not List<RectTransform> {
                    Count: > 0
                } rectChildren) return;
            var childSize = Vector2Int.RoundToInt(0.5f * rectChildren[0].rect.size);
            if (_scrollRect.horizontal) {
                _layoutGroup.padding.left -= childSize.x;
                _layoutGroup.padding.right -= childSize.x;
            } else {
                _layoutGroup.padding.top -= childSize.y;
                _layoutGroup.padding.bottom -= childSize.y;
            }
        }

        void UpdateMaskSoftness() =>
            _mask.softness = Vector2Int.RoundToInt(_rectTransform.rect.size - GetRectChildrenSize());

        Vector2 GetRectChildrenSize() {
            if (RectChildrenProperty.GetValue(_layoutGroup) is not List<RectTransform> { Count: > 0 } rectChildren)
                return Vector2.zero;
            return rectChildren[0].rect.size;
        }

        void EnsureLayoutRebuilt() {
            if (_layoutBuilt) return;
            if (CanvasUpdateRegistry.IsRebuildingLayout()) return;
            Canvas.ForceUpdateCanvases();
        }

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
    }
}
