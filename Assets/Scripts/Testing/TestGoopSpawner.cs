using GoopGame.Engine;
using System.Collections.Generic;
using UnityEngine;

namespace GoopGame.Testing
{
    public class TestGoopSpawner : MonoBehaviour
    {
        [SerializeField]
        private Transform _goopParent;

        [SerializeField]
        private Goop _goopPrefab;

        private List<Goop> _goopList;

        private void Awake()
        {
            _goopList = new List<Goop>();

            SpawnRandomGoop();
        }

        public void SpawnRandomGoop()
        {
            AddAt(_goopList.Count);
        }

        private Goop RemoveAt(int index)
        {
            Goop goopToRemove = _goopList[index];
            _goopList.RemoveAt(index);
            return goopToRemove;
        }

        private Goop AddAt(int index, GoopTraits traits = null)
        {
            Goop goopToAdd = Instantiate(_goopPrefab, _goopParent);

            if (traits != null)
                goopToAdd.SetTraits(traits);

            goopToAdd.transform.SetSiblingIndex(index);
            _goopList.Insert(index, goopToAdd);
            InitializeGoop(_goopList[index]);
            return goopToAdd;
        }

        private void InitializeGoop(Goop goop)
        {
            goop.Initialize();
            TestGoopVisual test = goop.GetComponent<TestGoopVisual>();

            test.SetVisual();
            test.CombineWithNextButton.onClick.AddListener(
                () => {
                    int index = goop.transform.GetSiblingIndex();
                    if (index >= _goopList.Count - 1)
                        return;

                    Goop goop1 = RemoveAt(index);
                    Goop goop2 = RemoveAt(index);
                    
                    GoopTraits traits = 
                        GoopTraits.GetGoopTraitsFromCombine(goop1, goop2);

                    Destroy(goop1.gameObject);
                    Destroy(goop2.gameObject);

                    AddAt(index, traits);
                });
            test.RemoveButton.onClick.AddListener(
                () => {
                    int index = goop.transform.GetSiblingIndex();

                    RemoveAt(index);

                    Destroy(goop.gameObject);
                });
            test.SplitButton.onClick.AddListener(
                () =>{
                    int index = goop.transform.GetSiblingIndex();

                    RemoveAt(index);

                    (GoopTraits traits1, GoopTraits traits2) =
                        GoopTraits.GetGoopTraitsFromSplit(goop);

                    Destroy(goop.gameObject);

                    AddAt(index, traits1);
                    AddAt(index, traits2);
                });
        }
    }
}
