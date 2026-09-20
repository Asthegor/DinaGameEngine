using DinaGameEngine.Models.Updater;

namespace DinaGameEngine.Abstractions
{
    public interface IDinaCSharpUpdateService
    {
        public Task<DinaCSharpReleaseInfo?> GetLatestReleaseAsync();
        public Version? GetLocalDllVersion(string dllFullName);
        public Task<bool> CheckAndUpdateLibsAsync(string libsFolder);
        public Task<string> DownloadAssetsToTempAsync(DinaCSharpReleaseInfo release);
        public bool ApplyToLibs(string sourceFolder, string libsFolder);
    }
}
