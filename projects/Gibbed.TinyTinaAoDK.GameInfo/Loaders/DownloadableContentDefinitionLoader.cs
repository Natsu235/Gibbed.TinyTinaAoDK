﻿/* Copyright (c) 2019 Rick (rick 'at' gibbed 'dot' us)
 *
 * This software is provided 'as-is', without any express or implied
 * warranty. In no event will the authors be held liable for any damages
 * arising from the use of this software.
 *
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 *
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would
 *    be appreciated but is not required.
 *
 * 2. Altered source versions must be plainly marked as such, and must not
 *    be misrepresented as being the original software.
 *
 * 3. This notice may not be removed or altered from any source
 *    distribution.
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace Gibbed.TinyTinaAoDK.GameInfo.Loaders
{
    internal static class DownloadableContentDefinitionLoader
    {
        public static InfoDictionary<DownloadableContentDefinition> Load(
            InfoDictionary<DownloadablePackageDefinition> packages)
        {
            try
            {
                var raws = LoaderHelper.DeserializeDump<Dictionary<string, Raw.DownloadableContentDefinition>>(
                    "Downloadable Contents");
                return new InfoDictionary<DownloadableContentDefinition>(
                    raws.ToDictionary(
                        kv => kv.Key,
                        kv => CreateDownloadableContent(packages, kv)));
            }
            catch (Exception e)
            {
                throw new InfoLoadException("failed to load downloadable contents", e);
            }
        }

        private static DownloadableContentDefinition CreateDownloadableContent(
            InfoDictionary<DownloadablePackageDefinition> packages,
            KeyValuePair<string, Raw.DownloadableContentDefinition> kv)
        {
            var raw = kv.Value;

            DownloadablePackageDefinition package = null;
            if (string.IsNullOrEmpty(raw.Package) == false)
            {
                if (packages.TryGetValue(raw.Package, out package) == false)
                {
                    throw ResourceNotFoundException.Create("downloadable package", kv.Value.Package);
                }
            }

            return new DownloadableContentDefinition()
            {
                ResourcePath = kv.Key,
                Id = raw.Id,
                Name = raw.Name,
                Package = package,
                Type = raw.Type,
            };
        }
    }
}
