using GoopGame.Data;

namespace GoopGame.Engine
{
    /// <summary>
    /// An abstract generic class where <typeparamref name="T"/> is the 
    /// datatype of the trait, it represents the mutable trait of a goop 
    /// during runtime. Responsible for storing reference to the 
    /// immutable TraitData and its mutable value.
    /// </summary>
    public abstract class GoopTrait<T>
    {
        /// <summary>
        /// The immutable data related to this trait.
        /// </summary>
        public GoopTraitData<T> TraitData { get; protected set; }
        /// <summary>
        /// The trait's type, derived from its TraitData.
        /// </summary>
        public GoopTraitWeightType Type => TraitData.TraitType;
        /// <summary>
        /// The trait's runtime value.
        /// </summary>
        public virtual T Value { get; protected set; }
        /// <summary>
        /// Use TraitData to generate a random value.
        /// </summary>
        protected abstract T GenerateInitialValue();
        /// <summary>
        /// Use TraitData to generate two split values.
        /// </summary>
        public abstract (T, T) GenerateSplitValue(Goop goop);
        /// <summary>
        /// UseTraitData to generate a combined value.
        /// </summary>
        public abstract T GenerateCombineValue(Goop goop1, Goop goop2);
    }
}
