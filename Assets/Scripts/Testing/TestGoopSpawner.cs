using GoopGame.Engine;
using System.Collections.Generic;
using UnityEngine;

namespace GoopGame.Testing
{
    /// <summary>
    /// A class for quick prototyping and testing of Goop traits.
    /// </summary>
    public class TestGoopSpawner : MonoBehaviour
    {
        /// <summary>
        /// Grid to spawn goops in.
        /// </summary>
        [SerializeField]
        private Transform _goopParent;

        /// <summary>
        /// Test goop prefab.
        /// </summary>
        [SerializeField]
        private Goop _goopPrefab;

        private List<Goop> _goopList;

        private void Awake()
        {
            _goopList = new List<Goop>();

            SpawnRandomGoop();
        }

        /// <summary>
        /// Spawn random goop at end of list.
        /// </summary>
        public void SpawnRandomGoop()
        {
            AddAt(_goopList.Count);
        }

        /// <summary>
        /// Remove goop from index in list.
        /// Does not destroy the GameObject.
        /// </summary>
        private Goop RemoveAt(int index)
        {
            Goop goopToRemove = _goopList[index];
            _goopList.RemoveAt(index);
            return goopToRemove;
        }

        /// <summary>
        /// Adds and initializes a goop at an index, pushes elements
        /// ahead of it. If <paramref name="traits"/> is unset, traits
        /// are set to random.
        /// </summary>
        private Goop AddAt(int index, GoopTraits traits = null)
        {
            //Create gameObject.
            Goop goopToAdd = Instantiate(_goopPrefab, _goopParent);

            //Set traits if provided.
            if (traits != null)
                goopToAdd.SetTraits(traits);

            //Set index.
            goopToAdd.transform.SetSiblingIndex(index);
            _goopList.Insert(index, goopToAdd);
            InitializeGoop(_goopList[index]);
            return goopToAdd;
        }

        private void InitializeGoop(Goop goop)
        {
            //Initialize the goop.
            goop.Initialize();
            TestGoopVisual test = goop.GetComponent<TestGoopVisual>();

            //Set visual to match goop data.
            test.SetVisual();

            //Add anonymous functions to buttons of TestGoopVisual.
            test.CombineWithNextButton.onClick.AddListener(
                () => {
                    int index = goop.transform.GetSiblingIndex();
                    //Exit if last goop.
                    if (index >= _goopList.Count - 1)
                        return;

                    //Get goops and remove them.
                    Goop goop1 = RemoveAt(index);
                    Goop goop2 = RemoveAt(index);
                    
                    //Combine.
                    GoopTraits traits = 
                        GoopTraits.GetGoopTraitsFromCombine(goop1, goop2);

                    //Destroy old goops.
                    Destroy(goop1.gameObject);
                    Destroy(goop2.gameObject);

                    //Add new goop.
                    AddAt(index, traits);
                });
            test.RemoveButton.onClick.AddListener(
                () => {
                    int index = goop.transform.GetSiblingIndex();

                    //Remove
                    RemoveAt(index);

                    //...and destroy goop.
                    Destroy(goop.gameObject);
                });
            test.SplitButton.onClick.AddListener(
                () =>{
                    int index = goop.transform.GetSiblingIndex();

                    //Remove goop.
                    RemoveAt(index);

                    //Split.
                    (GoopTraits traits1, GoopTraits traits2) =
                        GoopTraits.GetGoopTraitsFromSplit(goop);

                    //Destroy old goop.
                    Destroy(goop.gameObject);

                    //Add two new goops.
                    AddAt(index, traits1);
                    AddAt(index, traits2);
                });
        }
    }
}
