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
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Gibbed.TinyTinaAoDK.GameInfo.Loaders
{
    internal static class LoaderHelper
    {
        public static UnmanagedMemoryStream GetUnmanagedMemoryStream(string embeddedResourceName)
        {
            if (embeddedResourceName == null)
            {
                throw new ArgumentNullException(nameof(embeddedResourceName));
            }

            var path = "Gibbed.TinyTinaAoDK.GameInfo.Resources." + embeddedResourceName + ".json";

            var assembly = Assembly.GetExecutingAssembly();
            var stream = (UnmanagedMemoryStream)assembly.GetManifestResourceStream(path);
            if (stream == null)
            {
                throw new ArgumentException(
                    $"The embedded resource '{path}' could not be found.",
                    nameof(embeddedResourceName));
            }
            return stream;
        }

        public static TType Deserialize<TType>(string embeddedResourceName)
        {
            var settings = new JsonSerializerSettings()
            {
                MissingMemberHandling = MissingMemberHandling.Error,
                TypeNameHandling = TypeNameHandling.Auto,
                SerializationBinder = new TypeNameSerializationBinder(
                    "Gibbed.TinyTinaAoDK.GameInfo.Raw.{0}, Gibbed.TinyTinaAoDK.GameInfo")
            };
            settings.Converters.Add(new StringEnumConverter());

            try
            {
                var serializer = JsonSerializer.Create(settings);

                using (var input = GetUnmanagedMemoryStream(embeddedResourceName))
                using (var textReader = new StreamReader(input))
                using (var jsonReader = new JsonTextReader(textReader))
                {
                    return serializer.Deserialize<TType>(jsonReader);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static TType DeserializeDump<TType>(string embeddedResourceName)
        {
            return Deserialize<TType>("Dumps." + embeddedResourceName);
        }
    }
}
