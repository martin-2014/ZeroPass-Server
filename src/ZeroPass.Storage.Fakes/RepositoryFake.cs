using System.Collections.Generic;
using System.Reflection;
using ZeroPass.Storage.Entities;

namespace ZeroPass.Storage.Fakes
{
    public partial class RepositoryFake<TEntity> where TEntity : EntityBase, new()
    {
        public readonly List<TEntity> Entities;
        
        public RepositoryFake(List<TEntity> entities)
        {
            Entities = entities;
        }

        static void Merge(TEntity source, TEntity update)
        {
            var settableProperties = typeof(TEntity).GetProperties(
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.GetProperty |
                BindingFlags.SetProperty);
            foreach (var p in settableProperties)
            {
                var originValue = p.GetValue(source);
                var updateValue = p.GetValue(update);
                var value = update == null ? originValue : updateValue;
                p.SetValue(source, value);
            }
        }
    }
}