using System.Collections.Generic;

namespace Assets.Scripts.Main
{
    public interface IDatabase<T> where T : IEntityData
    {
        T GetData(string name);
        List<T> GetEntities();
    }
}