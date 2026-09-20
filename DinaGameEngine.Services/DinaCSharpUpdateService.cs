using DinaGameEngine.Abstractions;
using DinaGameEngine.Common;
using DinaGameEngine.Models.Updater;

using System.Diagnostics;
using System.Text.Json;

namespace DinaGameEngine.Services
{
    public class DinaCSharpUpdateService : IDinaCSharpUpdateService
    {
        private readonly IFileService _fileService;
        private readonly ILogService _logService;
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly List<string> _filesToCopy = ["DinaCSharp.dll", "DinaCSharp.deps.json", "DinaCSharp.xml", "DLACrypto.dll"];

        public DinaCSharpUpdateService(IFileService fileService, ILogService logService)
        {
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "DinaGameEngine");

            _fileService = fileService;
            _logService = logService;
        }

        public async Task<DinaCSharpReleaseInfo?> GetLatestReleaseAsync()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("https://api.github.com/repos/Asthegor/DinaCSharp/releases/latest");

                if (!response.IsSuccessStatusCode)
                {
                    _logService.Error($"Impossible de récupérer la dernière version de la dll de DinaCSharp.");
                    return null;
                }

                string json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<DinaCSharpReleaseInfo>(json);
            }
            catch (HttpRequestException e)
            {
                _logService.Warning($"GetLatestReleaseAsync : {e.Message}");
                return null;
            }
        }
        public Version? GetLocalDllVersion(string dllFullName)
        {
            if (!_fileService.FileExists(dllFullName))
            {
                _logService.Info($"Fichier '{dllFullName}' inexistant.");
                return null;
            }

            var versionInfo = FileVersionInfo.GetVersionInfo(dllFullName);
            return ParseTagVersion(versionInfo.ProductVersion);
        }

        private static Version? ParseTagVersion(string? fileVersion)
        {
            if (fileVersion == null)
                return null;

            if (!Version.TryParse(fileVersion.TrimStart('v'), out var version))
                return null;

            return version;
        }
        private static bool IsNewerVersionAvailable(Version? local, Version? remote)
        {
            if (remote == null)
                return false;
            if (local == null)
                return true;

            return remote > local;
        }

        public async Task<bool> CheckAndUpdateLibsAsync(string libsFolder)
        {
            try
            {
                var release = await GetLatestReleaseAsync();
                if (release == null)
                {
                    _logService.Warning($"Release de DinaCSharp non trouvée.");
                    return false;
                }

                var remoteVersion = ParseTagVersion(release.TagName);
                if (remoteVersion == null)
                {
                    _logService.Warning($"Version de DinaCSharp incorrecte.");
                    return false;
                }

                string localDllPath = _fileService.Combine(libsFolder, "DinaCSharp.dll");
                var localVersion = GetLocalDllVersion(localDllPath);

                if (!IsNewerVersionAvailable(localVersion, remoteVersion))
                {
                    _logService.Warning($"Version locale de DinaCSharp à jour.");
                    return false;
                }

                string tempFolder = await DownloadAssetsToTempAsync(release);
                if (string.IsNullOrEmpty(tempFolder))
                {
                    _logService.Error($"Téléchargement des fichiers échoué.");
                    return false;
                }

                if (!ApplyToLibs(tempFolder, libsFolder))
                    return false;
    
                _logService.Info($"DinaCSharp mis à jour avec succès vers la version {remoteVersion}.");
                return true;
            }
            catch (Exception e)
            {
                _logService.Error($"CheckAndUpdateLibsAsync : {e.Message}");
                return false;
            }
        }

        public async Task<string> DownloadAssetsToTempAsync(DinaCSharpReleaseInfo release)
        {
            var tempDirectory = _fileService.Combine(_fileService.GetTempPath(), Guid.NewGuid().ToString());
            _fileService.CreateDirectory(tempDirectory);

            try
            {
                foreach (var asset in release.Assets)
                {
                    var bytes = await _httpClient.GetByteArrayAsync(asset.DownloadUrl);
                    await _fileService.WriteAllBytesAsync(_fileService.Combine(tempDirectory, asset.Name), bytes);
                }
            }
            catch (Exception e)
            {
                _logService.Error($"DownloadAssetsToTempAsync : {e.Message}");
                return string.Empty;
            }

            return tempDirectory;
        }
        public bool ApplyToLibs(string sourceFolder, string libsFolder)
        {
            try
            {
                _fileService.CreateDirectory(libsFolder);

                foreach (var file in _filesToCopy)
                {
                    var source = _fileService.Combine(sourceFolder, file);
                    var destination = _fileService.Combine(libsFolder, file);
                    _fileService.CopyFile(source, destination, overwrite: true);
                }
            }
            catch (Exception e)
            {
                _logService.Error($"ApplyToLibs : {e.Message}");
                return false;
            }
            return true;
        }
    }
}
