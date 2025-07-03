using UnityEngine;

namespace GoopGame.Engine
{
    public class Goop : MonoBehaviour
    {
        /// <summary>
        /// Current FSM state.
        /// </summary>
        [field: SerializeField]
        public BaseState State { get; private set; }

        private Food _touchedFood;
        public Food TouchedFood => _touchedFood; //Makes it read-only

        public void SetState(BaseState state)
        {
            Debug.Log($"{name}: {this.State?.name ?? "NULL"} → {state.name}");
            State.ExitState(this); // Exit the current state
            State = state;
            State.EnterState(this); // Enter the new state
        }

        private void Awake()
        {
            if (State != null)
                State.EnterState(this);
        }

        private void Update()
        {
            if (State == null)
                return;

            State.Execute(this);
        }

        public Transform CurrentFoodTarget { get; private set; }

        public void FindNearestFood()
        {
            Food[] foods = Object.FindObjectsByType<Food>(FindObjectsSortMode.None);

            float closestDistance = Mathf.Infinity;
            Transform closestFood = null;

            foreach (Food food in foods)
            {
                float dist = Vector2.Distance(transform.position, food.transform.position);
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    closestFood = food.transform;
                }
            }

            CurrentFoodTarget = closestFood;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Food>(out var food))
            {
                _touchedFood = food;
                Debug.Log($"{name} touched {food.name}");
            }
        }

        public void ClearTouchedFood()
        {
            _touchedFood = null;
        }

        public void ClearFoodTarget()
        {
            CurrentFoodTarget = null;
        }
    }
}