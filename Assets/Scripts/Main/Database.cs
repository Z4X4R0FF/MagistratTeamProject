using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Main
{
    public abstract class Database<T> : ScriptableObject, IDatabase<T> where T : IEntityData
    {
        [SerializeField]
        private List<T> entities = new List<T>();

        public T GetData(string name)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                T entityData = entities[i];
                if (entityData.GetName().Equals(name))
                {
                    return entityData;
                }
            }

            throw new KeyNotFoundException("No entity with name `" + name + "` in Database: " + GetType());
        }

        public List<T> GetEntities()
        {
            return entities;
        }
    }
}