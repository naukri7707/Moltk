﻿using UnityEngine;

namespace Naukri.Moltk.MeshDeformation.Condition
{
    public class DeformedCondition : ShaderPassLayerCondition
    {
        public float triggerDistance;

        protected override bool OnEvaluation(Args args)
        {
            var disA = Vector3.Distance(args.triangle.a, args.originalTriangle.a);
            var disB = Vector3.Distance(args.triangle.b, args.originalTriangle.b);
            var disC = Vector3.Distance(args.triangle.c, args.originalTriangle.c);
            var distance = (disA + disB + disC) / 3;
            return distance > triggerDistance;
        }
    }
}