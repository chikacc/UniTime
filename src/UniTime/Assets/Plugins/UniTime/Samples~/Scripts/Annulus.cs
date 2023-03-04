using System;
using UnityEngine;
using UnityEngine.UI;

namespace UniTime.Samples {
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CanvasRenderer))]
    public class Annulus : MaskableGraphic {
        [SerializeField] Texture _texture;
        [SerializeField] float _innerRatio;
        [SerializeField] bool _fillClockwise = true;
        [SerializeField] Image.Origin360 _fillOrigin;
        [SerializeField] int _segments = 360;
        [SerializeField] float _fillAmount = 1f;

        readonly UIVertex[] _vertexes = new UIVertex[4];
        readonly Vector2[] _positions = new Vector2[4];
        readonly Vector2[] _uvs = { new(0, 1), new(1, 1), new(1, 0), new(0, 0) };

        public override Texture mainTexture => _texture ? _texture : s_WhiteTexture;

        public Texture Texture {
            get => _texture;
            set {
                if (_texture == value) return;
                _texture = value;
                SetVerticesDirty();
                SetMaterialDirty();
            }
        }

        public Image.Origin360 FillOrigin {
            get => _fillOrigin;
            set {
                if (_fillOrigin == value) return;
                _fillOrigin = value;
                SetVerticesDirty();
            }
        }

        public bool FillClockwise {
            get => _fillClockwise;
            set {
                if (_fillClockwise == value) return;
                _fillClockwise = value;
                SetVerticesDirty();
            }
        }

        public float FillAmount {
            get => _fillAmount;
            set {
                _fillAmount = value;
                SetVerticesDirty();
            }
        }

        float GetRadian() => _fillOrigin switch {
            Image.Origin360.Left => 0 * Mathf.Deg2Rad,
            Image.Origin360.Top => 90 * Mathf.Deg2Rad,
            Image.Origin360.Right => 180 * Mathf.Deg2Rad,
            Image.Origin360.Bottom => 270 * Mathf.Deg2Rad,
            _ => throw new ArgumentOutOfRangeException()
        };

        protected override void OnPopulateMesh(VertexHelper vh) {
            vh.Clear();
            if (_fillAmount == 0) return;
            var rt = (RectTransform)transform;
            var degrees = 360f / _segments;
            var count = (int)(_segments * _fillAmount);
            var radian = GetRadian();
            var cos = Mathf.Cos(radian);
            var sin = Mathf.Sin(radian);
            var radius = 0.5f * rt.rect.size;
            var originOuter = new Vector2(-radius.x * cos, radius.y * sin);
            var originInner = new Vector2(_innerRatio * -radius.x * cos, _innerRatio * radius.y * sin);
            for (var i = 1; i <= count; i++) {
                _positions[0] = originOuter;
                var endRadian = i * degrees * Mathf.Deg2Rad * (_fillClockwise ? 1 : -1) + radian;
                cos = Mathf.Cos(endRadian);
                sin = Mathf.Sin(endRadian);
                _positions[1] = new Vector2(-radius.x * cos, radius.y * sin);

                if (_innerRatio >= Mathf.Epsilon) {
                    _positions[2] = new Vector2(-_innerRatio * radius.x * cos, _innerRatio * radius.y * sin);
                    _positions[3] = originInner;
                } else {
                    _positions[2] = Vector2.zero;
                    _positions[3] = Vector2.zero;
                }

                for (var j = 0; j < 4; j++) {
                    _vertexes[j].color = color;
                    _vertexes[j].position = _positions[j];
                    _vertexes[j].uv0 = _uvs[j];
                }

                var vertCount = vh.currentVertCount;
                vh.AddVert(_vertexes[0]);
                vh.AddVert(_vertexes[1]);
                vh.AddVert(_vertexes[2]);
                vh.AddTriangle(vertCount, vertCount + 2, vertCount + 1);
                if (_innerRatio >= Mathf.Epsilon) {
                    vh.AddVert(_vertexes[3]);
                    vh.AddTriangle(vertCount, vertCount + 3, vertCount + 2);
                }

                originOuter = _positions[1];
                originInner = _positions[2];
            }
        }
    }
}
