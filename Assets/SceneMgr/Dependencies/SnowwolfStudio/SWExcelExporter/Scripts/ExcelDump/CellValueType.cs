using UnityEngine;

namespace Snowwolf
{
    /// <summary>
    /// Cell value type in memory.
    /// </summary>
    public enum CellValueType
    {
        Invalid = 0,
        Byte,
        Int,
        UInt,
        Long,
        ULong,

        Float,
        Double,

        String,
        LocalizedString,

        Array,
    }
}
