using EnvDTE;
using EnvDTE80;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.Interop;

namespace StarkPlatform.VisualStudio.LanguageServices.Implementation.CodeModel
{
    internal interface IProjectCodeModel
    {
        EnvDTE.FileCodeModel GetOrCreateFileCodeModel(string filePath);
        EnvDTE.FileCodeModel GetOrCreateFileCodeModel(string filePath, object parent);
        EnvDTE.CodeModel GetOrCreateRootCodeModel(Project parent);
        void OnSourceFileRemoved(string fileName);
        void OnSourceFileRenaming(string filePath, string newFilePath);
        void OnProjectClosed();
    }
}
