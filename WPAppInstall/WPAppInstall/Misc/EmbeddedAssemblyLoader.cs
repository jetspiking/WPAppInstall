using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WPAppInstall.Misc
{
    /// <summary>
    /// Class to load embedded assemblies, so it is not required to provide these alongside the executable.
    /// </summary>
    public class EmbeddedAssemblyLoader
    {
        private static readonly String projectName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
        private static readonly String assemblyPathPrefix = projectName + '.';
        private const String dllExtension = ".dll";
        private const String assemblyLoaded = "Log: Loaded (Previously Unresolved) Embedded Assembly: ";
        private const String pathVariable = "PATH";
        private String temporaryFolder = String.Empty;

        /// <summary>
        /// Load a library module or executable module.
        /// </summary>
        /// <param name="lpFileName"></param>
        /// <returns></returns>
        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern IntPtr LoadLibrary(String lpFileName);

        public EmbeddedAssemblyLoader()
        {
            AppDomain.CurrentDomain.AssemblyLoad += new AssemblyLoadEventHandler(CurrentDomain_AssemblyLoad);
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
        }

        /// <summary>
        /// Called when an assembly was loaded.
        /// </summary>
        private static void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            Console.WriteLine(assemblyLoaded + args.LoadedAssembly.FullName);
        }

        /// <summary>
        /// Load the assembly Microsoft.WindowsAPICodePack.Shell.dll which is configured to be embedded in the target executable (Build Action: "Embedded Resource").   
        /// </summary>
        /// <returns>Assembly from Taskbar11.Microsoft.WindowsAPICodePack.Shell.dll</returns>
        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {

            int charLocation = args.Name.IndexOf(',');

            if (charLocation > 0)
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(assemblyPathPrefix + args.Name.Substring(0, charLocation) + dllExtension))
                {
                    if (stream == null) return null;
                    Byte[] assemblyData = new Byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);
                    return Assembly.Load(assemblyData);
                }
            return null;
        }

        /// <summary>
        /// Load an unmanaged (embedded) dll.
        /// </summary>
        /// <param name="dllName">Name of the dll.</param>
        public void LoadUnmanagedLibraryFromResource(String dllName)
        {
            LoadAssembly(ExtractEmbeddedAssemblies(dllName, ReadFromEmbededResource(dllName)));
        }

        /// <summary>
        /// Read an input stream as bytes.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Input stream used in the application.</returns>
        public Byte[] ReadBytes(Stream input)
        {
            Byte[] buffer = new Byte[16 * 1024];
            using (MemoryStream memoryStream = new MemoryStream())
            {
                Int32 read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    memoryStream.Write(buffer, 0, read);
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Read an assembly resource as stream and return as bytes.
        /// </summary>
        /// <param name="resourceName">Assembly name.</param>
        /// <returns>Assembly as bytes.</returns>
        public Byte[] ReadFromEmbededResource(String resourceName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            String[] resourceNames = assembly.GetManifestResourceNames();
            String matchingResourceName = resourceNames.First(str => str.Contains(resourceName));

            if (matchingResourceName == null)
                return null;

            Stream assemblyStream = assembly.GetManifestResourceStream(matchingResourceName);
            return ReadBytes(assemblyStream);
        }

        /// <summary>
        /// Extract an embedded assembly.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <param name="assemblyBytes">Bytes of the assembly.</param>
        /// <returns>Path to assembly.</returns>
        public String ExtractEmbeddedAssemblies(String assemblyName, Byte[] assemblyBytes)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            String[] resourceNames = assembly.GetManifestResourceNames();
            AssemblyName assemblyNameObject = assembly.GetName();

            temporaryFolder = $"{assemblyNameObject.Name}.{assemblyNameObject.ProcessorArchitecture}.{assemblyNameObject.Version}";
            String directoryName = Path.Combine(Path.GetTempPath(), temporaryFolder);

            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);

            String path = Environment.GetEnvironmentVariable(pathVariable);
            String[] separatedPath = path.Split(';');
            Boolean found = false;

            foreach (String pathPiece in separatedPath)
                if (pathPiece == directoryName)
                {
                    found = true;
                    break;
                }
            
            if (!found)
                Environment.SetEnvironmentVariable(pathVariable, directoryName + ";" + path);

            String assemblyPath = Path.Combine(directoryName, assemblyName);
            Boolean rewrite = true;

            if (File.Exists(assemblyPath))
            {
                Byte[] existing = File.ReadAllBytes(assemblyPath);
                if (assemblyBytes.SequenceEqual(existing))
                    rewrite = false;
            }

            if (rewrite)
                File.WriteAllBytes(assemblyPath, assemblyBytes);

            return assemblyPath;
        }

        /// <summary>
        /// Load an assembly by using the LoadLibrary function from the kernel32 dll. 
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <exception cref="DllNotFoundException">Exception when a problem locating the assembly occured.</exception>
        public void LoadAssembly(String assemblyName)
        {
            if (temporaryFolder == String.Empty)
                return;
            IntPtr assembly = LoadLibrary(assemblyName);
            if (assembly == IntPtr.Zero)
                throw new DllNotFoundException();
        }
    }
}
