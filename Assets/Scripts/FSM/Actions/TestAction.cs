using GoopGame.Engine;
using UnityEngine;
using UnityEngine.Rendering;

namespace GoopGame.FSM
{
    [CreateAssetMenu(fileName = "TestAction", menuName = "GoopGame/FSM/Actions/Create new TestAction")]
    public class TestAction : StateAction
    {
        public string Message;
        public override void Execute(Goop goop)
        {
            Debug.Log($"Executing TestAction: {Message}");
        }
    }
}
