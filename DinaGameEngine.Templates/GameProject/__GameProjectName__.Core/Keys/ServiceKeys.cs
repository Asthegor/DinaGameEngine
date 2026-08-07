using DinaCSharp.Services.Keys;

namespace __RootNamespace__.Core.Keys
{
    public sealed class ServiceKeys : KeyResolver<ServiceKeys, ServiceTag>
    {
        public static readonly Key<ServiceTag> Config = Key<ServiceTag>.FromString("Config.dat");
        public static readonly Key<ServiceTag> DefaultConfig = Key<ServiceTag>.FromString("DefaultConfig");
        public static readonly Key<ServiceTag> PlayerController = Key<ServiceTag>.FromString("PlayerController");
        public static readonly Key<ServiceTag> SoundManager = Key<ServiceTag>.FromString("SoundManager");
        public static readonly Key<ServiceTag> AssetsResourceManager = Key<ServiceTag>.FromString("AssetsResourceManager");
    }
}
