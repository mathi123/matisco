using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Matisco.Domain
{
    public class TranslationService : ITranslationService
    {
        private string _languageCode = "en";
        private Dictionary<string, string> _translations;
        private string _rootPath;

        public void SetDefaultLanguageCode(string languageCode)
        {
            _languageCode = languageCode;
        }

        public void SetDefaultTranslationFileDirectory(string path)
        {
            _rootPath = path;
        }

        public string GetTranslation(string fullId)
        {
            if(ReferenceEquals(_translations, null))
                LoadTranslations();

            string val = null;
            _translations?.TryGetValue(fullId, out val);

            if(!string.IsNullOrEmpty(val))
                return val;

            if (ContainsDotInBody(fullId))
                return $"[{fullId.Substring(fullId.LastIndexOf(".") + 1)}]";

            return fullId;
        }

        private static bool ContainsDotInBody(string fullId)
        {
            return fullId.Contains(".") && fullId.IndexOf(".") != fullId.Length - 1;
        }

        private void LoadTranslations()
        {
            if (ReferenceEquals(_rootPath, null))
                _rootPath = GetAssemblyDirectory();

            _translations = new Dictionary<string, string>();

            foreach (var file in GetTranslationFiles(new DirectoryInfo(_rootPath)))
            {
                using (var resourceStream = File.OpenRead(file))
                {
                    var xmlDocument = new XmlDocument();
                    xmlDocument.Load(resourceStream);

                    var fileTags = xmlDocument.GetElementsByTagName("file");
                    if (fileTags.Count == 0)
                    {
                        Debug.WriteLine($"Warning: xliff file {resourceStream} does not contain a file tag.");
                        continue;
                    }

                    ParseFileTags(file, fileTags);
                }
            }
        }

        private IEnumerable<string> GetTranslationFiles(DirectoryInfo info)
        {
            foreach (var file in info.GetFiles("*.xliff"))
            {
                yield return file.FullName;
            }

            foreach (var directoryInfo in info.GetDirectories())
            {
                foreach (var translationFile in GetTranslationFiles(directoryInfo))
                    yield return translationFile;
            }
        }

        private void ParseFileTags(string resource, XmlNodeList fileTags)
        {
            foreach (var fileTag in fileTags)
            {
                var xmlElement = fileTag as XmlElement;

                var originalAttribute = xmlElement?.GetAttribute("original");
                if (string.IsNullOrEmpty(originalAttribute))
                {
                    Debug.WriteLine(
                        $"Warning: xliff file {resource} does not contain a 'original' attribute in the file tag.");
                    continue;
                }

                var sourceLanguageAttribute = xmlElement?.GetAttribute("source-language");
                if (string.IsNullOrEmpty(sourceLanguageAttribute))
                {
                    Debug.WriteLine(
                        $"Warning: xliff file {resource} does not contain a 'source-language' attribute in the file tag.");
                    continue;
                }

                var targetLanguageAttribute = xmlElement?.GetAttribute("target-language");
                if (!(targetLanguageAttribute == _languageCode ||
                      (targetLanguageAttribute == "" && sourceLanguageAttribute == _languageCode)))
                {
                    continue;
                }

                var transUnitNodes = xmlElement.GetElementsByTagName("trans-unit");

                foreach (var transUnitNode in transUnitNodes)
                {
                    ParseTranslation(originalAttribute, transUnitNode);
                }
            }
        }

        private void ParseTranslation(string fileKey, object transUnitNode)
        {
            var transUnit = transUnitNode as XmlElement;

            var idAttribute = transUnit?.GetAttribute("id");

            var target = transUnit?.GetElementsByTagName("target");

            if (idAttribute != null && target.Count == 1)
            {
                var translation = target[0].InnerText;
                var key = $"{fileKey}.{idAttribute}";

                if (!_translations.ContainsKey(key))
                {
                    _translations.Add(key, translation);
                }
            }
        }

        private string GetAssemblyDirectory()
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
    }
}
