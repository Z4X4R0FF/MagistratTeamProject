using UnityEngine;

namespace Assets.Scripts.Main
{
    public abstract class EntityData : ScriptableObject, IEntityData
    {
        [SerializeField] protected string entityName;
        public string GetName()
        {
            return entityName;
        }
    }
}