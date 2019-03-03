using System.Reflection;

namespace StarkPlatform.CodeAnalysis
{
    public static class TypeAttributesExt
    {
        public const TypeAttributes ClassSemanticsMask = (TypeAttributes)0x00000060;
        public const TypeAttributes Struct = (TypeAttributes)0x00000040;
    }
}
