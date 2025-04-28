using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Snowwolf
{
    public abstract class Exporter
    {
        public const string localizedStringPrefix = "__lKey_";

        /// <summary>
        /// The name of the exporter.
        /// </summary>
        public abstract string name { get; }

        /// <summary>
        /// The name use for display UI.
        /// </summary>
        public abstract string displayName { get;  }

        /// <summary>
        /// Export the datasheet to targetPath in specified format.
        /// </summary>
        /// <param name="targetPath">The path to export the files.</param>
        /// <param name="dataSheet">The dataSheet to export.</param>
        /// <param name="forServer">Export for server or client.</param>
        public abstract void Export(string targetPath, DataSheet dataSheet, bool forServer);

        public static bool ShouldExcludeColumn(bool forServer, SheetHeader.ItemExclusiveType exclusiveType)
        {
            return ((forServer && (exclusiveType == SheetHeader.ItemExclusiveType.ClientOnly))
                || (!forServer && (exclusiveType == SheetHeader.ItemExclusiveType.ServerOnly)));
        }

        public override string ToString()
        {
            return string.Format("{0}({1})", displayName, name);
        }
    }
}
