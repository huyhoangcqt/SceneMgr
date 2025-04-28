using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snowwolf
{
    public static class Exporters
    {
        private static Exporter[] s_SupportedExporters = null;

        /// <summary>
        /// Get all supported exporters so far.
        /// </summary>
        public static Exporter[] GetSupportedExporters()
        {
            if (s_SupportedExporters != null) { return s_SupportedExporters; }

            //TODO: register your exporters here.
            s_SupportedExporters = new Exporter[]{
                new CSharpExporter(),
                new LuaExporter(),
                new JsonExporter(),
            };
            return s_SupportedExporters;
        }

        /// <summary>
        /// Get exporter by name.
        /// </summary>
        public static Exporter GetExporter(string exporterName)
        {
            if (s_SupportedExporters == null){ GetSupportedExporters(); }
            return System.Array.Find<Exporter>(s_SupportedExporters, (exporter) => exporter.name == exporterName);
        }
    }
}
