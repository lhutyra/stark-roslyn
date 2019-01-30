using System.ComponentModel.Composition;
using StarkPlatform.CodeAnalysis.Editor.Commanding.Commands;
using Microsoft.VisualStudio.Editor.Commanding;
using StarkPlatform.VisualStudio.LanguageServices;

namespace Microsoft.VisualStudio.Editor.Implementation
{
    internal sealed class CommandBindings
    {
        [Export]
        [CommandBinding(Guids.RoslynGroupIdString, ID.RoslynCommands.GoToImplementation, typeof(GoToImplementationCommandArgs))]
        internal CommandBindingDefinition gotoImplementationCommandBinding;

        [Export]
        [CommandBinding(Guids.StarkGroupIdString, ID.CSharpCommands.OrganizeRemoveAndSort, typeof(SortAndRemoveUnnecessaryImportsCommandArgs))]
        internal CommandBindingDefinition organizeRemoveAndSortCommandBinding;

        [Export]
        [CommandBinding(Guids.StarkGroupIdString, ID.CSharpCommands.ContextOrganizeRemoveAndSort, typeof(SortAndRemoveUnnecessaryImportsCommandArgs))]
        internal CommandBindingDefinition contextOrganizeRemoveAndSortCommandBinding;
    }
}
