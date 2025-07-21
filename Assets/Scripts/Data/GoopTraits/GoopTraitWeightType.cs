using System;

namespace GoopGame.Data
{
    /// <summary>
    /// Enum to track any value relevant to trait weights.
    /// This includes traits themselves.
    /// Setup with bitflag values to quickly compare and assign different
    /// categories.
    /// </summary>
    public enum GoopTraitWeightType
    {
        None = 0,

        //suppress warnings due to [Obsolete]
#pragma warning disable CS0612 // Type or member is obsolete
        AverageHunger = 1 | IsFloat,
        AverageTemperature = 2 | IsFloat,
        AverageMood = 3 | IsFloat,
        AverageEnergy = 4 | IsFloat,
        Age = 5 | IsFloat,
        Color = 6 | IsColor | IsTrait,
        Size = 7 | IsFloat | IsTrait,
        Negative = 8 | IsTrait,
        Positive = 9 | IsTrait,
        Speed = 10 | IsFloat | IsTrait,
        Diet = 11 | IsTrait,
        UniqueTraits = 12 | IsTrait,
#pragma warning restore CS0612 // Type or member is obsolete

        //First flag is bit-shifted by 5 bits.
        //Bits 0-4 are dedicated to index of enums above (Up to 31).

        //[Obsolete] attribute is to hide this enum in the Unity Inspector.
        //requires us to suppress warnings in code.
        [Obsolete]
        IsTrait = 1 << 5,
        [Obsolete]
        IsFloat = 1 << 6,
        [Obsolete]
        IsColor = 1 << 7
    }
}
