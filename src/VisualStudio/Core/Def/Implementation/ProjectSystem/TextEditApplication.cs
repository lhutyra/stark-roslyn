using StarkPlatform.CodeAnalysis.Editor.Shared.Extensions;
using StarkPlatform.CodeAnalysis.Editor.Undo;
using StarkPlatform.CodeAnalysis.Text;
using Microsoft.VisualStudio.Text;

namespace StarkPlatform.VisualStudio.LanguageServices.Implementation.ProjectSystem
{
    internal static class TextEditApplication
    {
        internal static void UpdateText(SourceText newText, ITextBuffer buffer, EditOptions options)
        {
            using (var edit = buffer.CreateEdit(options, reiteratedVersionNumber: null, editTag: null))
            {
                var oldSnapshot = buffer.CurrentSnapshot;
                var oldText = oldSnapshot.AsText();
                var changes = newText.GetTextChanges(oldText);
                if (CodeAnalysis.Workspace.TryGetWorkspace(oldText.Container, out var workspace))
                {
                    var undoService = workspace.Services.GetService<ISourceTextUndoService>();
                    undoService.BeginUndoTransaction(oldSnapshot);
                }

                foreach (var change in changes)
                {
                    edit.Replace(change.Span.Start, change.Span.Length, change.NewText);
                }

                edit.ApplyAndLogExceptions();
            }
        }
    }
}
