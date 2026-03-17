using DinaCSharp.Services;
using DinaCSharp.Services.Fonts;

namespace __RootNamespace__.Core.Keys
{
    public static class ResolutionKeys
    {
        public readonly static Key<ResolutionTag> R720p = Key<ResolutionTag>.FromString("720p");
        public readonly static Key<ResolutionTag> R900p = Key<ResolutionTag>.FromString("900p");
        public readonly static Key<ResolutionTag> R1080p = Key<ResolutionTag>.FromString("1080p");
        public readonly static Key<ResolutionTag> R1440p = Key<ResolutionTag>.FromString("1440p");
        public readonly static Key<ResolutionTag> R2160p = Key<ResolutionTag>.FromString("2160p");
    }
}
