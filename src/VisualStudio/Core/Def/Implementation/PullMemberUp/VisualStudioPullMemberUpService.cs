// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.  

using System.Composition;
using System.Threading;
using StarkPlatform.CodeAnalysis;
using StarkPlatform.CodeAnalysis.CodeRefactorings.PullMemberUp.Dialog;
using StarkPlatform.CodeAnalysis.Editor.Host;
using StarkPlatform.CodeAnalysis.Host.Mef;
using StarkPlatform.CodeAnalysis.PullMemberUp;
using StarkPlatform.CodeAnalysis.Shared.Extensions;
using StarkPlatform.CodeAnalysis.Shared.Utilities;
using Microsoft.VisualStudio.Language.Intellisense;
using StarkPlatform.VisualStudio.LanguageServices.Implementation.PullMemberUp.MainDialog;
using Roslyn.Utilities;

namespace StarkPlatform.VisualStudio.LanguageServices.Implementation.PullMemberUp
{
    [ExportWorkspaceService(typeof(IPullMemberUpOptionsService), ServiceLayer.Host), Shared]
    internal class VisualStudioPullMemberUpService : IPullMemberUpOptionsService
    {
        private readonly IGlyphService _glyphService;
        private readonly IWaitIndicator _waitIndicator;

        [ImportingConstructor]
        public VisualStudioPullMemberUpService(IGlyphService glyphService, IWaitIndicator waitIndicator)
        {
            _glyphService = glyphService;
            _waitIndicator = waitIndicator;
        }

        public PullMembersUpOptions GetPullMemberUpOptions(Document document, ISymbol selectedMember)
        {
            var membersInType = selectedMember.ContainingType.GetMembers().
                WhereAsArray(member => MemberAndDestinationValidator.IsMemberValid(member));
            var memberViewModels = membersInType
                .SelectAsArray(member =>
                    new PullMemberUpSymbolViewModel(member, _glyphService)
                    {
                        // The member user selected will be checked at the begining.
                        IsChecked = SymbolEquivalenceComparer.Instance.Equals(selectedMember, member),
                        MakeAbstract = false,
                        IsMakeAbstractCheckable = !member.IsKind(SymbolKind.Field) && !member.IsAbstract,
                        IsCheckable = true
                    });

            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var baseTypeRootViewModel = BaseTypeTreeNodeViewModel.CreateBaseTypeTree(
                    _glyphService,
                    document.Project.Solution,
                    selectedMember.ContainingType,
                    cancellationTokenSource.Token).BaseTypeNodes;
                var memberToDependentsMap = SymbolDependentsBuilder.FindMemberToDependentsMap(membersInType, document.Project, cancellationTokenSource.Token);
                var viewModel = new PullMemberUpDialogViewModel(_waitIndicator, memberViewModels, baseTypeRootViewModel, memberToDependentsMap);
                var dialog = new PullMemberUpDialog(viewModel);
                var result = dialog.ShowModal();

                // Dialog has finshed its work, cancel finding dependents task.
                cancellationTokenSource.Cancel();
                if (result.GetValueOrDefault())
                {
                    return dialog.ViewModel.CreatePullMemberUpOptions();
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
