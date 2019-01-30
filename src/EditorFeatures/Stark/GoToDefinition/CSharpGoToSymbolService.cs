using System;
using System.Composition;
using StarkPlatform.CodeAnalysis.Editor.GoToDefinition;
using StarkPlatform.CodeAnalysis.Editor.Shared.Utilities;
using StarkPlatform.CodeAnalysis.Host.Mef;

namespace StarkPlatform.CodeAnalysis.Editor.CSharp.GoToDefinition
{
    [ExportLanguageService(typeof(IGoToSymbolService), LanguageNames.Stark), Shared]
    internal class CSharpGoToSymbolService : AbstractGoToSymbolService
    {
        [ImportingConstructor]
        [Obsolete(MefConstruction.ImportingConstructorMessage, error: true)]
        public CSharpGoToSymbolService(IThreadingContext threadingContext)
            : base(threadingContext)
        {
        }
    }
}
