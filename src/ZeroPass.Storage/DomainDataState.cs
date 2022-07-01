using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZeroPass.Storage
{
    internal class DomainDataState : IDomainDataState
    {
        const int DataSyncLagInMsec = 1500;
        readonly ICache Cache;

        public DomainDataState(ICache cache)
        {
            Cache = cache;
        }

        public async Task SetDirty(int domainId, DomainDataType types)
        {
            await SetDirtyInCache(domainId, types);
        }

        public async Task<bool> IsDirty(int domainId, DomainDataType types)
        {
            return await IsDirtyInCache(domainId, types);
        }

        async Task SetDirtyInCache(int domainId, DomainDataType types)
        {
            var tasks = ExpandBasicTypes(types).Select(t
                => Cache.SetWithAbsoluteExpiration(GetCacheKey(domainId, t), DateTime.UtcNow.ToString(), DataSyncLagInMsec));
            await Task.WhenAll(tasks);
        }

        async Task<bool> IsDirtyInCache(int domainId, DomainDataType types)
        {
            var tasks = ExpandBasicTypes(types).Select(t => Cache.Get(GetCacheKey(domainId, t)));
            await Task.WhenAll(tasks);
            return tasks.Any(t => t.Result != null);
        }

        public IEnumerable<DomainDataType> ExpandBasicTypes(DomainDataType type)
        {
            if (type.IsBasic())
            {
                yield return type;
                yield break;
            }

            foreach (DomainDataType t in Enum.GetValues(typeof(DomainDataType)))
            {
                if (t.IsBasic() && type.HasFlag(t))
                {
                    yield return t;
                }
            }
        }

        static string GetCacheKey(int domainId, DomainDataType type) => $"DataDirty@{domainId}@{type}";
    }
}