using System.Globalization;
using System.Reflection;
using System.Resources;

namespace DinaGameEngine.Common
{
    /// <summary>
    /// Gère la traduction des chaînes de texte dans l'application, permettant de récupérer des traductions pour différentes langues.
    /// </summary>
    public static class LocalizationManager
    {
        private static readonly Dictionary<string, string> _cache = [];
        private static readonly List<ResourceManager> _resourceManagers = [];
        private static readonly List<Assembly> _assemblies = [];
        private static string[] _availableLanguages = [];
        private static bool _loaded;
        private static bool _updated;

        //private static readonly List<Type> _listStrings = [];

        private static CultureInfo _currentCulture = CultureInfo.CurrentUICulture;

        /// <summary>
        /// Vérifie si les valeurs de traduction ont été chargées.
        /// </summary>
        public static bool IsLoaded => _loaded;

        /// <summary>
        /// Vérifie si les traductions ont été mises à jour.
        /// </summary>
        public static bool IsUpdated => _updated;

        /// <summary>
        /// Obtient ou définit la langue actuelle.
        /// </summary>
        public static string CurrentLanguage
        {
            get => _currentCulture.TwoLetterISOLanguageName;
            set
            {
                _currentCulture = new CultureInfo(value);
                _updated = true;
                _cache.Clear();
                LanguageChanged?.Invoke(null, new LocalizationEventArgs());
            }
        }

        /// <summary>
        /// Ajoute une classe contenant des traductions.
        /// </summary>
        /// <param name="resourceClass">La classe contenant les traductions à ajouter.</param>
        public static void Register(Type resourceClass)
        {
            ArgumentNullException.ThrowIfNull(resourceClass);

            var prop = resourceClass.GetProperty("ResourceManager", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public) ?? throw new InvalidOperationException($"La classe {resourceClass.Name} ne contient pas de ResourceManager.");
            var rm = prop.GetValue(null) as ResourceManager
                ?? throw new InvalidOperationException($"La classe {resourceClass.Name} ne fournit pas de ResourceManager valide.");
            _resourceManagers.Add(rm);

            _assemblies.Add(resourceClass.Assembly);
            _loaded = true;
            _availableLanguages = SearchAvailableLanguages();
        }

        /// <summary>
        /// Récupère la traduction pour une clé donnée dans la langue actuelle.
        /// </summary>
        /// <param name="key">La clé de la traduction.</param>
        /// <param name="values">Liste des </param>
        /// <returns>La traduction correspondant à la clé, ou la clé elle-même si non trouvée.</returns>
        public static string GetTranslation(string key, params string[] values)
        {
            if (_cache.TryGetValue(key, out var cached))
            {
                if (values?.Length > 0)
                    return string.Format(CultureInfo.InvariantCulture, cached, values);
                return cached;
            }

            foreach (var rm in _resourceManagers)
            {
                try
                {
                    var translation = rm.GetString(key, _currentCulture);
                    if (!string.IsNullOrEmpty(translation))
                    {
                        _cache[key] = translation;
                        if (values?.Length > 0)
                            return string.Format(CultureInfo.InvariantCulture, translation, values);
                        return translation;
                    }
                }
                catch (MissingManifestResourceException)
                {
                    // Ignore cette exception si la ressource n'est pas trouvée
                }
            }
            return key;
        }

        /// <summary>
        /// Récupère la traduction pour une clé donnée dans une culture spécifique.
        /// </summary>
        /// <param name="key">La clé de la traduction.</param>
        /// <param name="culture">Le code de la culture.</param>
        /// <returns>La traduction pour la culture spécifiée.</returns>
        public static string GetTranslationForCulture(string key, string culture)
        {
            var previous = _currentCulture;
            _currentCulture = new CultureInfo(culture);
            var translation = GetTranslation(key);
            _currentCulture = previous;
            return translation;
        }

        /// <summary>
        /// Récupère la liste des cultures disponibles via les assemblies satellites.
        /// </summary>
        /// <returns>Un tableau des langues disponibles.</returns>
        public static string[] GetAvailableLanguages() => _availableLanguages ?? [];

        private static string[] SearchAvailableLanguages()
        {
            var cultures = new HashSet<string>();
            foreach (var assembly in _assemblies)
            {
                var dir = Path.GetDirectoryName(assembly.Location);
                if (dir != null)
                    AddCulturesFromSubdirectories(dir, cultures, assembly);
            }
            return [.. cultures];

        }

        private static void AddCulturesFromSubdirectories(string directoryPath, HashSet<string> cultures, Assembly assembly)
        {
            foreach (var directory in Directory.GetDirectories(directoryPath))
            {
                var cultureName = Path.GetFileName(directory);
                //try
                //{
                //    var culture = new CultureInfo(cultureName);
                //    var satellitePath = Path.Combine(directory, $"{assembly.GetName().Name}.resources.dll");
                //    if (File.Exists(satellitePath))
                //        cultures.Add(cultureName);
                //}
                //catch (CultureNotFoundException)
                //{
                //    // Ignorer les dossiers non valides, mais ne pas bloquer la recherche des autres sous-répertoires
                //}
                if (CultureInfo.GetCultures(CultureTypes.AllCultures)
                    .Any(c => c.Name.Equals(cultureName, StringComparison.OrdinalIgnoreCase)))
                {
                    var satellitePath = Path.Combine(directory, $"{assembly.GetName().Name}.resources.dll");
                    if (File.Exists(satellitePath))
                        cultures.Add(cultureName);
                }
                AddCulturesFromSubdirectories(directory, cultures, assembly);
            }
        }

        public static event EventHandler<LocalizationEventArgs>? LanguageChanged;
    }
}
