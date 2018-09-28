﻿using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.IO;
using SharpOpenGL.StaticMesh;
using System.Xml.Linq;
using Core.OpenGLShader;
using OpenTK.Graphics.OpenGL;
using System.Threading.Tasks;
using ZeroFormatter;
using Core;

namespace SharpOpenGL.Asset
{
    public class AssetManager : Singleton<AssetManager>
    {
        private static object AssetMapLock = new object();

        protected static ConcurrentDictionary<string, AssetBase> AssetMap = new ConcurrentDictionary<string, AssetBase>();

        protected static string ImportedDirectory = "./Resources/Imported";

        public static T GetAsset<T>(string name) where T : AssetBase
        {
            if (AssetMap.ContainsKey(name))
            {
                return (T) AssetMap[name];
            }

            return null;
        }

        public static T LoadAssetSync<T>(string path) where T : AssetBase
        {
            var cachedAsset = GetAsset<T>(Path.GetFileName(path));

            if (cachedAsset != null)
            {
                return cachedAsset;
            }

            byte[] data = File.ReadAllBytes(path);
            T asset = ZeroFormatter.ZeroFormatterSerializer.Deserialize<T>(data);
            AssetMap.TryAdd(Path.GetFileName(path), asset);
            asset.OnPostLoad();
            return asset;
        }

        public static async Task<T> LoadAssetAsync<T>(string path) where T : AssetBase
        {
            return await Task.Factory.StartNew(() =>
            {
                var cachedAsset = GetAsset<T>(Path.GetFileName(path));

                if (cachedAsset != null)
                {
                    return cachedAsset;
                }

                byte[] data = File.ReadAllBytes(path);
                T asset = ZeroFormatter.ZeroFormatterSerializer.Deserialize<T>(data);
                AssetMap.TryAdd(Path.GetFileName(path), asset);

                MainThreadQueue.Get().Enqueue(() =>
                {
                    asset.OnPostLoad();
                });
                
                return asset;
            });
        }

        public void DiscoverShader()
        {
            if(File.Exists("./Resources/Shader/MaterialList.xml") == false)
            {
                return;
            }

            if(Directory.Exists("./Resources/Imported/Shader") == false)
            {
                Directory.CreateDirectory("./Resources/Imported/Shader");
            }

            var root = XElement.Load("./Resources/Shader/MaterialList.xml");

            // import first
            foreach (var node in root.Descendants("Material"))
            {
                string materialName = node.Attribute("name").Value;

                string importedFileName = string.Format("./Resources/Imported/Shader/{0}.material", materialName);

                if(File.Exists(importedFileName))
                {
                    continue;
                }

                string vsPath = Path.Combine("./Resources", "Shader", node.Attribute("vertexShader").Value);
                string fsPath = Path.Combine("./Resources", "Shader", node.Attribute("fragmentShader").Value);
                var vs = new VertexShader();
                var fs = new FragmentShader();

                vs.CompileShader(File.ReadAllText(vsPath));
                fs.CompileShader(File.ReadAllText(fsPath));

                var program = new ShaderProgram(vs, fs);

                byte[] data = new byte[1024 * 1024 * 512]; // 512kb

                int binaryLength = 0;

                program.GetProgramBinary(ref data, out binaryLength);

                using (var filestream = new FileStream(importedFileName, FileMode.CreateNew))
                {
                    filestream.Write(data, 0, binaryLength);
                }
            }
        }


        public void DiscoverStaticMesh()
        {
            List<string> objFileList = new List<string>();
            List<string> mtlFileList = new List<string>();

            if (Directory.Exists("./Resources/Imported/StaticMesh") == false)
            {
                Directory.CreateDirectory("./Resources/Imported/StaticMesh");
            }

            // discover importable obj , mtl files first
            foreach (var file in Directory.EnumerateFiles("./Resources/ObjMesh"))
            {
                if (file.EndsWith(".obj"))
                {
                    objFileList.Add(file);
                }
                else if (file.EndsWith(".mtl"))
                {
                    mtlFileList.Add(file);
                }
            }

            foreach (var file in Directory.EnumerateFiles(Path.Combine(ImportedDirectory, "StaticMesh")))
            {
                var staticMeshAsset = AssetManager.LoadAssetSync<StaticMeshAsset>(file);
                var name = Path.GetFileName(file);
                Console.WriteLine("[AssetManager] Found {0}...", name);
                AssetMap.TryAdd(name, staticMeshAsset);
            }

            foreach (var objfile in objFileList)
            {
                var filename = Path.GetFileNameWithoutExtension(objfile);

                var importedPath = Path.GetFullPath (Path.Combine(ImportedDirectory, "StaticMesh", filename + ".staticmesh"));

                if (File.Exists(importedPath))
                {
                    continue;
                }
                
                var mtlFileName = mtlFileList.Where(x => Path.GetFileNameWithoutExtension(x).StartsWith(filename)).FirstOrDefault();
                
                if (mtlFileName != null)
                {
                    var staticMesh = new StaticMeshAsset(objfile, mtlFileName);
                    staticMesh.ImportAssetSync();

                    string importedAssetPath = Path.Combine("./Resources/Imported/StaticMesh", Path.GetFileNameWithoutExtension(objfile) + ".staticmesh");
                    staticMesh.SaveImportedAsset(importedAssetPath);
                    AssetMap.TryAdd(filename + ".staticmesh", staticMesh);
                }
                else
                {
                    var staticMesh = new StaticMeshAsset(objfile);
                    staticMesh.ImportAssetSync();

                    if (Directory.Exists("./Resources/Imported/StaticMesh") == false)
                    {
                        Directory.CreateDirectory("./Resources/Imported/StaticMesh");
                    }

                    string importedAssetPath = Path.Combine("./Resources/Imported/StaticMesh", Path.GetFileNameWithoutExtension(objfile) + ".staticmesh");
                    staticMesh.SaveImportedAsset(importedAssetPath);
                    AssetMap.TryAdd(filename + ".staticmesh", staticMesh);
                }
            }
        }

    }
}
