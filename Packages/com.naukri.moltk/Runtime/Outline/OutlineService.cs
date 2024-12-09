﻿using System;
using Naukri.InspectorMaid;
using Naukri.InspectorMaid.Layout;
using Naukri.Moltk.Utility;
using Naukri.Physarum;
using UnityEngine;
using UnityEngine.UIElements;
using UnityFx.Outline;

namespace Naukri.Moltk.Outline
{
    public enum OutlineLayer
    {
        Highlight,

        Hover,

        Select,
    }

    public record OutlineCollectionChangedEvent(GameObject target) : IElementEvent;

    public partial class OutlineService : Provider.Behaviour
    {
        public enum RenderPipeline
        {
            BuiltIn,

            Universal,

            HighDefinition,
        }

        [SerializeField]
        private OutlineEffect outlineEffect;

        [SerializeField]
        private OutlineLayerCollection outlineLayers;

        [SerializeField]
        private RenderPipeline renderPipeline = RenderPipeline.BuiltIn;

        public Color HighlightColor
        {
            get => outlineLayers[(int)OutlineLayer.Highlight].OutlineColor;
            set => outlineLayers[(int)OutlineLayer.Highlight].OutlineColor = value;
        }

        public Color HoverColor
        {
            get => outlineLayers[(int)OutlineLayer.Hover].OutlineColor;
            set => outlineLayers[(int)OutlineLayer.Hover].OutlineColor = value;
        }

        public Color SelectColor
        {
            get => outlineLayers[(int)OutlineLayer.Select].OutlineColor;
            set => outlineLayers[(int)OutlineLayer.Select].OutlineColor = value;
        }

        private bool IsRuntime => Application.isPlaying;

        public void Highlight(GameObject go, bool highlight = true)
        {
            var layer = outlineLayers[(int)OutlineLayer.Highlight];

            if (highlight && !layer.Contains(go))
            {
                layer.Add(go);
                ctx.DispatchListeners(new OutlineCollectionChangedEvent(go));
            }
            else if (!highlight && layer.Contains(go))
            {
                layer.Remove(go);
                ctx.DispatchListeners(new OutlineCollectionChangedEvent(go));
            }
            else
            {
                // Do nothing
            }
        }

        public void Select(GameObject go,  bool select = true)
        {
            var layer = outlineLayers[(int)OutlineLayer.Select];

            if (select && !layer.Contains(go))
            {
                layer.Add(go);
                ctx.DispatchListeners(new OutlineCollectionChangedEvent(go));
            }
            else if (!select && layer.Contains(go))
            {
                layer.Remove(go);
                ctx.DispatchListeners(new OutlineCollectionChangedEvent(go));
            }
            else
            {
                // Do nothing
            }
        }

        public void Hover(GameObject go,  bool hover = true)
        {
            var layer = outlineLayers[(int)OutlineLayer.Hover];

            if (hover && !layer.Contains(go))
            {
                layer.Add(go);
                ctx.DispatchListeners(new OutlineCollectionChangedEvent(go));
            }
            else if (!hover && layer.Contains(go))
            {
                layer.Remove(go);
                ctx.DispatchListeners(new OutlineCollectionChangedEvent(go));
            }
            else
            {
                // Do nothing
            }
        }

        public bool IsHighlighted(GameObject go)
        {
            var layer = outlineLayers[(int)OutlineLayer.Highlight];

            return layer.Contains(go);
        }

        public bool IsSelected(GameObject go)
        {
            var layer = outlineLayers[(int)OutlineLayer.Select];

            return layer.Contains(go);
        }

        public bool IsHovered(GameObject go)
        {
            var layer = outlineLayers[(int)OutlineLayer.Hover];

            return layer.Contains(go);
        }



        [Template]
        public void ToggleHighlight(GameObject go)
        {
            var toggle = !IsHighlighted(go);

            Highlight(go, toggle);
        }

        [Template]
        public void ToggleSelect(GameObject go)
        {
            var toggle = !IsSelected(go);

            Select(go, toggle);
        }

        [Template]
        public void ToggleHover(GameObject go)
        {
            var toggle = !IsHovered(go);

            Hover(go, toggle);
        }

        protected override void Start()
        {
            base.Start();
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
                outlineEffect = mainCamera.gameObject.AddComponent<OutlineEffect>();
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
    }

    [
        ScriptField,
        Base,
        Slot(nameof(renderPipeline)),
        HelpBox("This pipeline is not supported yet.", HelpBoxMessageType.Error),
        ShowIf(nameof(renderPipeline), RenderPipeline.Universal, RenderPipeline.HighDefinition),
        ColumnScope,
        ShowIf(nameof(renderPipeline), RenderPipeline.BuiltIn),
        Slot(nameof(outlineEffect)),
        RowScope,
        ShowIf(nameof(outlineEffect), null),
        HelpBox("You may need to add OutlineEffect to Camera.", HelpBoxMessageType.Warning),
        Style(flexGrow: "0.8"),
        Button("Add to MainCamera", binding: nameof(AddOutlineEffectToMainCamera)),
        Style(flexGrow: "0.2"),
        EndScope,
        Slot(nameof(outlineLayers)),
        HelpBox(
            @"You assign a value to the OutlineEffect's outlineLayers, but OutlineService will replace it.",
            HelpBoxMessageType.Warning
        ),
        ShowIf(nameof(IsLayerCollectionExists)),
        EndScope,
        ColumnScope,
        EnableIf(nameof(IsRuntime)),
        Slot(nameof(ToggleHighlight), nameof(ToggleSelect), nameof(ToggleHover)),
        EndScope,
    ]
    partial class OutlineService { }
}
