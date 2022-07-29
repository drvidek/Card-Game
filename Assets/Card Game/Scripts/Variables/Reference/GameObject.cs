using UnityEngine;

namespace Variable
{
    [CreateAssetMenu(fileName = "New GameObject", menuName = "Variable/Reference/GameObject")]
    public class GameObject : GenericVariable<UnityEngine.GameObject> { }
}