using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Main;

namespace Assets.Scripts.SpawnSystem
{
    public interface ISpawnManager
    {
        bool Spawn(IEntity entity, IEntityData entityData, Vector3 position, Quaternion rotation);
    }
}