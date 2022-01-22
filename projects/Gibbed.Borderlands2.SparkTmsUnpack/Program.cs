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
using System.IO;
using System.Text;
using Gibbed.IO;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using NDesk.Options;

// Unpacks 'WillowTMS.cfg', found at URL:
//   http://cdn.services.gearboxsoftware.com/sparktms/willow2/pc/steam/WillowTMS.cfg

namespace Gibbed.Borderlands2.SparkTmsUnpack
{
    internal class Program
    {
        private static string GetExecutableName()
        {
            return Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
        }

        private static void Main(string[] args)
        {
            bool showHelp = false;
            bool overwriteFiles = false;
            bool verbose = false;

            var options = new OptionSet()
            {
                { "o|overwrite", "overwrite existing files", v => overwriteFiles = v != null },
                { "v|verbose", "be verbose", v => verbose = v != null },
                { "h|help", "show this message and exit", v => showHelp = v != null },
            };

            List<string> extras;

            try
            {
                extras = options.Parse(args);
            }
            catch (OptionException e)
            {
                Console.Write("{0}: ", GetExecutableName());
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `{0} --help' for more information.", GetExecutableName());
                return;
            }

            if (extras.Count < 1 || extras.Count > 2 || showHelp == true)
            {
                Console.WriteLine("Usage: {0} [OPTIONS]+ input_tms [output_dir]", GetExecutableName());
                Console.WriteLine();
                Console.WriteLine("Options:");
                options.WriteOptionDescriptions(Console.Out);
                return;
            }

            string inputPath = extras[0];
            string outputPath = extras.Count > 1 ? extras[1] : Path.ChangeExtension(inputPath, null) + "_unpack";

            using (var input = File.OpenRead(inputPath))
            {
                const uint signature = 0x9E2A83C1u;

                input.Position = 8;
                var magic = input.ReadValueU32(Endian.Little);
                if (magic != signature && magic.Swap() != signature)
                {
                    throw new FormatException("bad magic");
                }
                var endian = magic == signature ? Endian.Little : Endian.Big;

                input.Position = 0;
                var uncompressedSize3 = input.ReadValueU32(endian);
                var fileCount = input.ReadValueU32(endian);
                input.Position += 4; // skip magic
                var version = input.ReadValueU32(endian);

                if (version != 0x00020000)
                {
                    throw new FormatException("bad version");
                }

                var compressedSize1 = input.ReadValueU32(endian);
                var uncompressedSize1 = input.ReadValueU32(endian);
                var compressedSize2 = input.ReadValueU32(endian);
                var uncompressedSize2 = input.ReadValueU32(endian);

                if (compressedSize1 != compressedSize2 ||
                    uncompressedSize1 != uncompressedSize2 ||
                    uncompressedSize1 != uncompressedSize3)
                {
                    throw new FormatException();
                }

                var compressedBytes = input.ReadBytes((int)compressedSize1);
                var uncompressedBytes = new byte[uncompressedSize1];
                var actualUncompressedSize = (int)uncompressedSize1;
                var result = MiniLZO.LZO.DecompressSafe(
                    compressedBytes,
                    0,
                    (int)compressedSize1,
                    uncompressedBytes,
                    0,
                    ref actualUncompressedSize);
                if (result != MiniLZO.ErrorCode.Success)
                {
                    // LZO failed, try Zlib.
                    using (var temp = new MemoryStream(compressedBytes))
                    {
                        var zlib = new InflaterInputStream(temp);
                        actualUncompressedSize = zlib.Read(uncompressedBytes, 0, (int)uncompressedSize1);
                    }
                }

                if (actualUncompressedSize != uncompressedSize1)
                {
                    throw new FormatException();
                }

                using (var data = new MemoryStream(uncompressedBytes))
                {
                    for (uint i = 0; i < fileCount; i++)
                    {
                        var entryNameLength = data.ReadValueS32(endian);
                        if (entryNameLength < 0)
                        {
                            throw new NotImplementedException();
                        }

                        var entryName = data.ReadString(entryNameLength, true, Encoding.ASCII);
                        var entryTextLength = data.ReadValueS32(endian);

                        Encoding entryTextEncoding;
                        if (entryTextLength >= 0)
                        {
                            entryTextEncoding = Encoding.GetEncoding(1252);
                        }
                        else
                        {
                            entryTextEncoding = Encoding.Unicode;
                            entryTextLength = (-entryTextLength) * 2;
                        }
                        var entryText = data.ReadString(entryTextLength, true, entryTextEncoding);

                        var entryPath = Path.Combine(outputPath, entryName);
                        if (overwriteFiles == false &&
                            File.Exists(entryPath) == true)
                        {
                            continue;
                        }

                        if (verbose == true)
                        {
                            Console.WriteLine(entryName);
                        }

                        var entryParentPath = Path.GetDirectoryName(entryPath);
                        if (string.IsNullOrEmpty(entryParentPath) == false)
                        {
                            Directory.CreateDirectory(entryParentPath);
                        }

                        File.WriteAllText(entryPath, entryText, Encoding.UTF8);
                    }
                }
            }
        }
    }
}
