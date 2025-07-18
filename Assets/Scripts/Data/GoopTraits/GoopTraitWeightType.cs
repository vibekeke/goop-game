using System;

namespace GoopGame.Data
{
    public enum GoopTraitWeightType
    {
        None = 0,
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

        [Obsolete]
        IsTrait = 1 << 5,
        [Obsolete]
        IsFloat = 1 << 6,
        [Obsolete]
        IsColor = 1 << 7
    }
}
