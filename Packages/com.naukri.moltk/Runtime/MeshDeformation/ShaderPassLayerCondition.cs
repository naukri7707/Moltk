﻿namespace Naukri.Moltk.MeshDeformation
{
    public abstract class ShaderPassLayerCondition : DeformableObjectModifier
    {
        internal bool Evaluation(Args args)
        {
            return OnEvaluation(args);
        }

        protected abstract bool OnEvaluation(Args args);

        public class Args
        {

            public readonly Triangle triangle;

            public readonly Triangle originalTriangle;
            public Args(Triangle triangle, Triangle originalTriangle)
            {
                this.triangle = triangle;
                this.originalTriangle = originalTriangle;
            }
        }
    }

    public abstract class ShaderPassLayerCondition<TParameters> : ShaderPassLayerCondition, IWithParameter<TParameters>
    {
        private TParameters _parameters;

        public TParameters parameters => _parameters;

        internal override void InitialImpl(DeformableObject deformableObject)
        {
            if (deformableObject.parameters is TParameters parameters)
            {
                _parameters = parameters;
            }
            else
            {
                throw new System.Exception($"{deformableObject.name} required parameter {typeof(TParameters).Name}");
            }
            base.InitialImpl(deformableObject);
        }
    }
}
