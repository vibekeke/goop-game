using System.Collections.Generic;
using UnityEngine;

namespace GoopGame.Data
{
    /// <summary>
    /// A ScriptableObject containing a collection of
    /// <seealso cref="GoopTraitClassification{T}"/> objects used to
    /// determine if a goop falls under a specific classification.
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Goop Classification",
        menuName = "GoopGame/Goops/Classifications/Create new Classification")]
    public class GoopClassificationData : ScriptableObject
    {
        /// <summary>
        /// A bool determining if all classifications in the list
        /// <seealso cref="TraitClassifications"/> must be met.
        /// If set to true, traits are compared with AND.
        /// If set to false, traits are compared with OR.
        /// </summary>
        public bool MustFulfillAll;
        /// <summary>
        /// A list of trait classifications that set the criteria for
        /// this goop classification.
        /// </summary>
        public List<GoopTraitClassification> TraitClassifications;
    }
}
