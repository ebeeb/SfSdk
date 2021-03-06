﻿using System;
using System.Collections.Generic;
using System.Linq;
using SfSdk.Constants;

namespace SfSdk.Providers
{
    /// <summary>
    ///     A service to receive S&amp;F resources.
    /// </summary>
    internal abstract class ResourceProviderBase : IResourceProvider
    {
        /// <summary>
        ///     Caches the data which is downloaded.
        /// </summary>
        private static readonly Dictionary<string, Dictionary<SF, string>> ResourceDataDict =
            new Dictionary<string, Dictionary<SF, string>>();

        public abstract Dictionary<SF, string> GetResources(Uri serverUri);

        /// <summary>
        ///     The wrapper method for GetResources
        /// </summary>
        /// <param name="serverUrl">The server url.</param>
        /// <param name="fillResourceDict">The dictionary to be filled.</param>
        /// <returns>
        ///     A dictionary containing the language resources.
        /// </returns>
        protected Dictionary<SF, string> GetResources(string serverUrl, Func<string> fillResourceDict)
        {
            if (!ResourceDataDict.ContainsKey(serverUrl))
                ResourceDataDict.Add(serverUrl, new Dictionary<SF, string>());

            if (ResourceDataDict[serverUrl].Count > 0)
                return ResourceDataDict[serverUrl];

            var stringData = fillResourceDict();

            ProcessStringData(ResourceDataDict[serverUrl], stringData);
            return ResourceDataDict[serverUrl];
        }

        private static void ProcessStringData(Dictionary<SF, string> dictToFill, string stringData)
        {
            var lines = stringData
                .Split('\n')
                .Select(line => line.Split('\t'))
                .Where(lineParts => lineParts.Length == 2);

            foreach (var lineParts in lines)
            {
                var index = (SF) int.Parse(lineParts[0]);
                if (dictToFill.ContainsKey(index)) continue;
                dictToFill.Add(index, lineParts[1]);
            }
        }
    }
}