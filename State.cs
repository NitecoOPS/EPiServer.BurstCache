using EPiServer.Configuration;
using System;
using System.Threading;
using System.Web;

namespace EPiServer.BurstCache
{
    /// <summary>
    /// Stores state to be passed to the Output Cache Validation Callback.
    /// You pass it to
    /// <see cref="HttpCachePolicyBase.AddValidationCallback(HttpCacheValidateHandler, object)"/> when you register
    /// <see cref="ValidateOutputCache(HttpContext, object, ref HttpValidationStatus)"/>.
    /// </summary>
    public class State
    {
        public Settings Config { get; private set; }
        public long VersionInCache { get; private set; }
        public DateTime RefreshAfter { get; private set; }

        private long versionRendering;
        public long VersionRendering { get => versionRendering; set => versionRendering = value; }

        public State(Settings settings, long currentVersion, DateTime refreshAfter)
        {
            Config = settings;
            VersionInCache = currentVersion;
            RefreshAfter = refreshAfter;
            VersionRendering = -1;
        }

        public bool ExchangeVersion(long currentVersion)
        {
            return Interlocked.Exchange(ref versionRendering, currentVersion) == currentVersion;
        }
    }
}