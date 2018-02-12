namespace Matisco.Core
{
    public interface ITranslationService
    {
        void SetDefaultLanguageCode(string languageCode);
        void SetDefaultTranslationFileDirectory(string path);
        string GetTranslation(string fullId);
    }
}
