// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;

namespace StarkPlatform.VisualStudio.LanguageServices
{
    internal static class Guids
    {
        // Deprecated, and can be removed once https://github.com/dotnet/roslyn/pull/9186 is merged
        // and the closed solution no longer depends on this.
        public const string CSharpOptionPageFormattingIdString = "17ABFCEB-2A0C-44F8-9A85-A92B08F6E3B3";

        public const string StarkPackageIdString = "17ABFCEB-2A0C-44F8-9A85-A92B08F6E3B3";
        public const string StarkProjectIdString = "0D82155C-3060-4DDC-9D49-06522FEDE816";
        public const string StarkLanguageServiceIdString = "79E2E70D-E9F9-475F-8F90-F18209CBDD00";
        public const string StarkEditorFactoryIdString = "5A3D871C-9840-46DA-9697-77776471C765";
        public const string StarkCodePageEditorFactoryIdString = "040E3492-803E-41F8-8883-31E6ED37F186";
        public const string StarkCommandSetIdString = "07433E79-1C5D-4CE0-9109-DFA80340657C";
        public const string StarkGroupIdString = "67DB5C4C-6484-4439-8FE9-57C6C854A334";
        public const string StarkRefactorIconIdString = "8E12B3CF-7DE0-4AD8-8B65-241D61DF2591";
        public const string StarkGenerateIconIdString = "6A6A1EF4-790F-48B6-BE12-171B81A86237";
        public const string StarkOrganizeIconIdString = "0B82213C-CFB7-42E1-975B-A0FA5463A022";
        public const string StarkLibraryIdString = "55362EC7-6C73-468B-BE07-87608661AE52";
        public const string StarkReplPackageIdString = "762A23B5-C4F3-4045-9863-4D1D8DD200CA";

        public const string StarkProjectRootIdString = "C7FEDB89-B36D-4a62-93F4-DC7A95999921";

        // from debugger\idl\makeapi\guid.c  
        public const string StarkDebuggerLanguageIdString = "3525D260-DE03-40A5-A4E5-E3650FB6AA16";

        public static readonly Guid StarkPackageId = new Guid(StarkPackageIdString);
        public static readonly Guid StarkProjectId = new Guid(StarkProjectIdString);
        public static readonly Guid StarkLanguageServiceId = new Guid(StarkLanguageServiceIdString);
        public static readonly Guid StarkEditorFactoryId = new Guid(StarkEditorFactoryIdString);
        public static readonly Guid StarkCodePageEditorFactoryId = new Guid(StarkCodePageEditorFactoryIdString);
        public static readonly Guid StarkCommandSetId = new Guid(StarkCommandSetIdString);     // guidCSharpCmdId
        public static readonly Guid StarkGroupId = new Guid(StarkGroupIdString);               // guidCSharpGrpId
        public static readonly Guid StarkRefactorIconId = new Guid(StarkRefactorIconIdString); // guidCSharpRefactorIcon
        public static readonly Guid StarkGenerateIconId = new Guid(StarkGenerateIconIdString); // guidCSharpGenerateIcon
        public static readonly Guid StarkOrganizeIconId = new Guid(StarkOrganizeIconIdString); // guidCSharpOrganizeIcon
        public static readonly Guid StarkDebuggerLanguageId = new Guid(StarkDebuggerLanguageIdString);
        public static readonly Guid StarkLibraryId = new Guid(StarkLibraryIdString);

        // option page guids from csharp\rad\pkg\guids.h
        public const string StarkOptionPageAdvancedIdString = "BE7D630F-2A60-425C-88A3-F98D5DBD9118";
        public const string StarkOptionPageNamingStyleIdString = "47E75C83-0EEE-4E67-B902-0E55FB88C9F1";
        public const string StarkOptionPageIntelliSenseIdString = "8C940E9A-4F9A-4472-8C7A-22B20B93E711";
        public const string StarkOptionPageCodeStyleIdString = "367ABEF5-72E6-4DC7-93FC-A18336671C60";
        public const string StarkOptionPageFormattingGeneralIdString = "FE7A04DC-16AB-49DA-A9D8-3B7049C25DA1";
        public const string StarkOptionPageFormattingIndentationIdString = "9AC3957C-3417-4F55-8904-DF98F1F78F13";
        public const string StarkOptionPageFormattingNewLinesIdString = "7C18F77B-1098-446A-AA14-AE399B04441D";
        public const string StarkOptionPageFormattingSpacingIdString = "2E7642ED-C558-400F-949B-2A7974D0FC92";
        public const string StarkOptionPageFormattingWrappingIdString = "1DBE2F1C-7BBF-4248-8BB9-AE10203BEB92";

        // from vscommon\inc\textmgruuids.h
        public const string TextManagerPackageString = "F5E7E720-1401-11D1-883B-0000F87579D2";

        // Roslyn guids
        public const string RoslynPackageIdString = "5617BD02-DB4F-4843-AF48-04134A7B438C";
        public const string RoslynCommandSetIdString = "1BD14407-118D-4AB6-A7D2-2B4374ADF03C";
        public const string RoslynGroupIdString = "3F0A6609-6135-4399-B1F7-22D780CD110B";

        public static readonly Guid RoslynPackageId = new Guid(RoslynPackageIdString);
        public static readonly Guid RoslynCommandSetId = new Guid(RoslynCommandSetIdString);
        public static readonly Guid RoslynGroupId = new Guid(RoslynGroupIdString);

        // TODO: Remove pending https://github.com/dotnet/roslyn/issues/8927 .
        // Interactive guids
        public const string InteractiveCommandSetIdString = "FD377604-F2C2-44B1-B893-FF3F51586464";
        public static readonly Guid InteractiveCommandSetId = new Guid(InteractiveCommandSetIdString);
    }
}
