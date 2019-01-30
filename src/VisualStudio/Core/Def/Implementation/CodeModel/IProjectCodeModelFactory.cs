using System;
using System.Collections.Generic;
using System.Text;
using StarkPlatform.CodeAnalysis;

namespace StarkPlatform.VisualStudio.LanguageServices.Implementation.CodeModel
{
    internal interface IProjectCodeModelFactory
    {
        IProjectCodeModel CreateProjectCodeModel(ProjectId id, ICodeModelInstanceFactory codeModelInstanceFactory);
    }
}
