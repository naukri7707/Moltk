﻿using System.Collections.Generic;
using UnityEngine;

namespace Naukri.Moltk.MeshDeformation
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public sealed class DeformableObject : MonoBehaviour
    {
        public MeshFilter MeshFilter { get; private set; }

        public MeshRenderer MeshRenderer { get; private set; }

        public DeformableParameters parameters;

        public VertexModifier[] vertexModifiers;

        public bool changeShaderPassDynamicly;

        public Material[] materials;

        public ShaderPassLayer[] shaderPassLayers;

        private HashSet<int> changedVertex = new();

        private Vector3[] originalVertices;

        public Vector3[] OriginalVertices => originalVertices;

        // Start is called before the first frame update
        private void Awake()
        {
            MeshFilter = GetComponent<MeshFilter>();
            MeshRenderer = GetComponent<MeshRenderer>();

            // 使用克隆體進行計算，避免多個 DeformableObject 同時使用一個 VertexModifier 的情況
            for (var i = 0; i < vertexModifiers.Length; i++)
            {
                vertexModifiers[i] = Instantiate(vertexModifiers[i]);
            }
            // 使用克隆體進行計算，避免多個 DeformableObject 同時使用一個 ShaderPassLayerCondition 的情況
            for (var i = 0; i < shaderPassLayers.Length; i++)
            {
                shaderPassLayers[i].condition = Instantiate(shaderPassLayers[i].condition);
            }
        }

        private void Start()
        {
            // 初始化 VertexModifier
            foreach (var vertexModifier in vertexModifiers)
            {
                vertexModifier.InitialImpl(this);
            }
            originalVertices = MeshFilter.mesh.vertices;

            if (changeShaderPassDynamicly)
            {
                MeshRenderer.materials = materials;
                MeshFilter.mesh.subMeshCount = materials.Length;
            }
        }

        public Vector3 ModifyVertex(VertexModifierArgs args)
        {
            // 紀錄被變更 vertex 的索引
            changedVertex.Add(args.vertexIndex);

            var current = args.newPosition;
            // 執行 VertexModifier
            foreach (var modifier in vertexModifiers)
            {
                modifier.ModifyVertex(ref current, args);
            }
            return current;
        }

        public void UpdateShaderPass()
        {
            // 如果偵測到有 vertex 變更，更新 face 材質
            if (changeShaderPassDynamicly && changedVertex.Count > 0)
            {
                var vertices = MeshFilter.mesh.vertices;
                var originalVertices = OriginalVertices;

                // 蒐集要切換 shader pass 的 triangle
                var modifiedRequest = new Queue<(int[] triangle, int shaderPass)>();

                // 取得各 shader pass 的 triangles
                var trianglesList = new List<int>[materials.Length];
                for (var i = 0; i < trianglesList.Length; i++)
                {
                    trianglesList[i] = new();
                    MeshFilter.mesh.GetTriangles(trianglesList[i], i);
                }

                // 遍歷所有 triangle
                for (var shaderPass = 0; shaderPass < trianglesList.Length; shaderPass++)
                {
                    var triangles = trianglesList[shaderPass];
                    for (var i = 0; i < triangles.Count; i += 3)
                    {
                        // 3 個頂點在 MeshFilter.mesh.vertices 陣列中的索引
                        var a = triangles[i];
                        var b = triangles[i + 1];
                        var c = triangles[i + 2];

                        // 如果任意頂點在此幀變更
                        if (changedVertex.Contains(a) || changedVertex.Contains(b) || changedVertex.Contains(c))
                        {
                            var targetShaderPass = shaderPass;
                            var triangle = new Triangle(vertices[a], vertices[b], vertices[c]);
                            var sharedTriangle = new Triangle(originalVertices[a], originalVertices[b], originalVertices[c]);
                            var args = new ShaderPassLayerCondition.Args(triangle, sharedTriangle);

                            // 找到首個符合條件得 shaderPassLayer
                            foreach (var layer in shaderPassLayers)
                            {
                                if (layer.condition.Evaluation(args))
                                {
                                    targetShaderPass = layer.shaderPass;
                                    break;
                                }
                            }
                            // 如果目標 shaderPass 與當前不同，新增切換 shaderPass 請求
                            if (targetShaderPass != shaderPass)
                            {
                                modifiedRequest.Enqueue((new[] { a, b, c }, targetShaderPass));
                                triangles.RemoveRange(i, 3);
                                // 修正移除 Item 產生的偏移
                                i -= 3;
                            }
                        }
                    }
                }
                // 執行切換 shaderPass 請求
                while (modifiedRequest.TryDequeue(out var request))
                {
                    trianglesList[request.shaderPass].AddRange(request.triangle);
                }

                // 更新 shaderPass
                for (var i = 0; i < trianglesList.Length; i++)
                {
                    MeshFilter.mesh.SetTriangles(trianglesList[i], i);
                }

                // 清除變更記錄
                changedVertex.Clear();
            }
        }
    }

}