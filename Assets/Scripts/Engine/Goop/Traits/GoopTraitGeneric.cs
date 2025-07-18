using GoopGame.Data;

namespace GoopGame.Engine
{
    public abstract class GoopTrait<T>
    {
        public GoopTraitData<T> TraitData { get; protected set; }
        public GoopTraitWeightType Type => TraitData.TraitType;
        public virtual T Value { get; protected set; }
        protected abstract T GenerateInitialValue();
        public abstract (T, T) GenerateSplitValue(Goop goop);
        public abstract T GenerateCombineValue(Goop goop1, Goop goop2);
    }
}
