using Naukri.InspectorMaid;
using Naukri.InspectorMaid.Layout;
using Naukri.Moltk.Core;
using UnityEngine;
using UnityEngine.UIElements;
using UnityFx.Outline;

namespace Naukri.Moltk.Outline
{
    public enum OutlineLayer
    {
        Highlight,

        Select,

        Hover,
    }

    public partial class OutlineService : MoltkService
    {
        public OutlineEffect outlineEffect;

        public OutlineLayerCollection outlineLayers;

        public RenderPipeline renderPipeline = RenderPipeline.BuiltIn;

        private bool IsRuntime => Application.isPlaying;

        public void Highlight(GameObject gameObject, bool highlight = true)
        {
            var layer = outlineLayers[(int)OutlineLayer.Highlight];

            if (highlight && !layer.Contains(gameObject))
            {
                layer.Add(gameObject);
            }
            else if (!highlight && layer.Contains(gameObject))
            {
                layer.Remove(gameObject);
            }
        }

        public void Select(GameObject gameObject, bool select = true)
        {
            var layer = outlineLayers[(int)OutlineLayer.Select];
            if (select && !layer.Contains(gameObject))
            {
                layer.Add(gameObject);
            }
            else if (!select && layer.Contains(gameObject))
            {
                layer.Remove(gameObject);
            }
        }

        public void Hover(GameObject gameObject, bool hover = true)
        {
            var layer = outlineLayers[(int)OutlineLayer.Hover];
            if (hover && !layer.Contains(gameObject))
            {
                layer.Add(gameObject);
            }
            else if (!hover && layer.Contains(gameObject))
            {
                layer.Remove(gameObject);
            }
        }

        public bool IsHighlighted(GameObject gameObject)
        {
            var layer = outlineLayers[(int)OutlineLayer.Highlight];
            return layer.Contains(gameObject);
        }

        public bool IsSelected(GameObject gameObject)
        {
            var layer = outlineLayers[(int)OutlineLayer.Select];
            return layer.Contains(gameObject);
        }

        public bool IsHovered(GameObject gameObject)
        {
            var layer = outlineLayers[(int)OutlineLayer.Hover];
            return layer.Contains(gameObject);
        }

        [Template]
        public void ToggleHighlight(GameObject gameObject)
        {
            var toggle = !IsHighlighted(gameObject);
            Highlight(gameObject, toggle);
        }

        [Template]
        public void ToggleSelect(GameObject gameObject)
        {
            var toggle = !IsSelected(gameObject);
            Select(gameObject, toggle);
        }

        [Template]
        public void ToggleHover(GameObject gameObject)
        {
            var toggle = !IsHovered(gameObject);
            Hover(gameObject, toggle);
        }

        protected override void Start()
        {
            outlineEffect.OutlineLayers = outlineLayers;
        }

        private void AddOutlineEffectToMainCamera()
        {
            var mainCamera = Camera.main;

            if (mainCamera.TryGetComponent<OutlineEffect>(out var effect))
            {
                outlineEffect = effect;
            }
            else
            {
                outlineEffect = Camera.main.gameObject.AddComponent<OutlineEffect>();
            }
        }

        private bool IsLayerCollectionExists()
        {
            if (outlineEffect == null)
            {
                return false;
            }

            return outlineEffect.OutlineLayers != null;
        }

        public enum RenderPipeline
        {
            BuiltIn,

            Universal,

            HighDefinition,
        }
    }

    [
        ScriptField,
        Base,
        Slot(nameof(renderPipeline)),
        HelpBox("This pipeline is not supported yet.", HelpBoxMessageType.Error),
            ShowIf(nameof(renderPipeline), RenderPipeline.Universal, RenderPipeline.HighDefinition),
        ColumnScope, ShowIf(nameof(renderPipeline), RenderPipeline.BuiltIn),
            Slot(nameof(outlineEffect)),
            RowScope, ShowIf(nameof(outlineEffect), null),
                HelpBox("You may need to add OutlineEffect to Camera.", HelpBoxMessageType.Warning), Style(flexGrow: "0.8"),
                Button("Add to MainCamera", binding: nameof(AddOutlineEffectToMainCamera)), Style(flexGrow: "0.2"),
            EndScope,
            Slot(nameof(outlineLayers)),
            HelpBox(@"You assign a value to the OutlineEffect's outlineLayers, but OutlineService will replace it.", HelpBoxMessageType.Warning), ShowIf(nameof(IsLayerCollectionExists)),
        EndScope,
        ColumnScope, EnableIf(nameof(IsRuntime)),
            Slot(nameof(ToggleHighlight), nameof(ToggleSelect), nameof(ToggleHover)),
        EndScope,
    ]
    partial class OutlineService
    {
    }
}
