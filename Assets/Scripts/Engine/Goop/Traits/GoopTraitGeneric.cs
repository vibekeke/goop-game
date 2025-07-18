using GoopGame.Data;

namespace GoopGame.Engine
{
    public abstract class GoopTrait<T>
    {
        protected GoopTraitData<T> TraitData;
        public GoopTraitWeightType Type => TraitData.TraitType;
        public abstract T GenerateInitialValue();
        public abstract T Value { get; }
        public abstract (T, T) GenerateSplitValue(Goop goop);
        public abstract T GenerateCombineValue(Goop goop1, Goop goop2);
    }
}
