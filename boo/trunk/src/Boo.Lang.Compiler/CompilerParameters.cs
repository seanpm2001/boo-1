#region license
// Copyright (c) 2004, Rodrigo B. de Oliveira (rbo@acm.org)
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
// 
//     * Redistributions of source code must retain the above copyright notice,
//     this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice,
//     this list of conditions and the following disclaimer in the documentation
//     and/or other materials provided with the distribution.
//     * Neither the name of Rodrigo B. de Oliveira nor the names of its
//     contributors may be used to endorse or promote products derived from this
//     software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
// THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#endregion

using System;
using System.Diagnostics;
using System.Collections;
using System.IO;
using System.Reflection;
using Boo.Lang;

namespace Boo.Lang.Compiler
{
	/// <summary>
	/// Compiler parameters.
	/// </summary>
	public class CompilerParameters : System.MarshalByRefObject
	{
		static private List _validFileExtensions = new List(new string[] { ".dll", ".exe" });
		
		TextWriter _outputWriter;
		
		CompilerPipeline _pipeline;

		CompilerInputCollection _input;
		
		CompilerResourceCollection _resources;

		AssemblyCollection _assemblyReferences;

		int _maxAttributeSteps;
		
		string _outputAssembly;
		
		CompilerOutputType _outputType;
		
		bool _debug;
		
		bool _ducky;
		
		bool _checked;
		
		bool _generateInMemory;
		
		bool _StdLib;
		
		string _keyFile;
		
		string _keyContainer;
		
		bool _delaySign;
		
		ArrayList _libpaths;
		
		string _systemDir;
		
		Assembly _booAssembly;
		
		public readonly TraceSwitch TraceSwitch = new TraceSwitch("booc", "boo compiler");
		
		public CompilerParameters(): this(true)
		{
		}
		
		public CompilerParameters(bool loadDefaultReferences)
		{
			_libpaths = new ArrayList();
			_systemDir = GetSystemDir();
			_libpaths.Add(_systemDir);
			_libpaths.Add(Directory.GetCurrentDirectory());
			
			_pipeline = null;
			_input = new CompilerInputCollection();
			_resources = new CompilerResourceCollection();
			_assemblyReferences = new AssemblyCollection();
			
			_maxAttributeSteps = 2;
			_outputAssembly = string.Empty;
			_outputType = CompilerOutputType.ConsoleApplication;
			_outputWriter = System.Console.Out;
			_debug = true;
			_checked = true;
			_generateInMemory = true;
			_StdLib = true;
			
			_delaySign = false;
			
			if (loadDefaultReferences) LoadDefaultReferences();
		}
		
		public void LoadDefaultReferences()
		{
			//mscorlib
			_assemblyReferences.Add(
				LoadAssembly("mscorlib", true)
				);
			//System
			_assemblyReferences.Add(
				LoadAssembly("System", true)
				);
			//boo.lang.dll
			_booAssembly = typeof(Boo.Lang.Builtins).Assembly;
			_assemblyReferences.Add(_booAssembly);
			//boo.lang.compiler.dll
			_assemblyReferences.Add(GetType().Assembly);
			
			if (TraceSwitch.TraceInfo)
			{
				Trace.WriteLine("BOO LANG DLL: "+_booAssembly.Location);
				Trace.WriteLine("BOO COMPILER DLL: "+GetType().Assembly.Location);
			}
		}
		
		public Assembly BooAssembly
		{
			get
			{
				return _booAssembly;
			}
			set
			{
				if (value != null)
				{
					(_assemblyReferences as IList).Remove(_booAssembly);
					_booAssembly = value;
					_assemblyReferences.Add(value);
				}
			}
		}
		
		public Assembly FindAssembly(string name)
		{
			return _assemblyReferences.Find(name);
		}
		
		public void AddAssembly(Assembly asm)
		{
			if (asm != null)
			{
				_assemblyReferences.Add(asm);
			}
		}
		
		public Assembly LoadAssembly(string assembly)
		{
			return LoadAssembly(assembly, true);
		}
		
		public Assembly LoadAssembly (string assembly, bool throwOnError)
		{
			if (TraceSwitch.TraceInfo)
			{
				Trace.WriteLine("ATTEMPTING LOADASSEMBLY: " + assembly);
			}
			
			Assembly a = null;
			try 
			{
				if (assembly.IndexOfAny(new char[] { '/', '\\' }) != -1)
				{

					//nant passes full path to gac dlls, which compiler doesn't like:
					if (assembly.ToLower().StartsWith(_systemDir.ToLower()))
					{
						return LoadAssemblyFromGac(Path.GetFileName(assembly));
					}
					else //load using path  
					{
						a = Assembly.LoadFrom(assembly);
					}
				}
				else
				{
					a = LoadAssemblyFromGac(assembly);
				}
			}
			catch (FileNotFoundException /*ignored*/)
			{
				return LoadAssemblyFromLibPaths(assembly, throwOnError);
			}
			catch (BadImageFormatException e)
			{
				if (throwOnError)
				{
					throw new ApplicationException(Boo.Lang.ResourceManager.Format(
						"BooC.BadFormat", 
						e.FusionLog), e);
				}
			} 
			catch (FileLoadException e)
			{
				if (throwOnError)
				{
					throw new ApplicationException(Boo.Lang.ResourceManager.Format(
						"BooC.UnableToLoadAssembly", 
						e.FusionLog), e);
				}
			} 
			catch (ArgumentNullException e)
			{
				if (throwOnError)
				{
					throw new ApplicationException(Boo.Lang.ResourceManager.Format(
						"BooC.NullAssembly"), e);
				}
			}
			if (a == null)
			{
				return LoadAssemblyFromLibPaths(assembly, throwOnError);
			}
			return a;
		}
		
		private Assembly LoadAssemblyFromLibPaths(string assembly, bool throwOnError)
		{
			Assembly a = null;
			string fullLog = "";
			foreach (string dir in _libpaths)
			{
				string full_path = Path.Combine(dir, assembly);
				FileInfo file = new FileInfo(full_path);
				if (!_validFileExtensions.Contains(file.Extension.ToLower()))
					full_path += ".dll";

				try 
				{
					a = Assembly.LoadFrom(full_path);
					if (a != null)
					{
						return a;
					}
				} 
				catch (FileNotFoundException ff)
				{
					fullLog += ff.FusionLog;
					continue;
				}
			}
			if (throwOnError)
			{
				throw new ApplicationException(Boo.Lang.ResourceManager.Format(
					"BooC.CannotFindAssembly", 
					assembly)); 
					//assembly, total_log)); //total_log contains the fusion log
			}
			return a;
		}
		
		private Assembly LoadAssemblyFromGac(string assemblyName)
		{
			assemblyName = NormalizeAssemblyName(assemblyName);
			Assembly assembly = Assembly.LoadWithPartialName(assemblyName);
			if (assembly != null) return assembly;
			return Assembly.Load(assemblyName);
		}

		private static string NormalizeAssemblyName(string assembly)
		{
			if (assembly.EndsWith(".dll") || assembly.EndsWith(".exe"))
			{
				assembly = assembly.Substring(0, assembly.Length - 4);
			}
			return assembly;
		}

		private string GetSystemDir()
		{
			return Path.GetDirectoryName(typeof(string).Assembly.Location);
		}
		
		/// <summary>
		/// Max number of steps for the resolution of AST attributes.		
		/// </summary>
		public int MaxAttributeSteps
		{
			get
			{
				return _maxAttributeSteps;
			}

			set
			{
				_maxAttributeSteps = value;
			}
		}

		public CompilerInputCollection Input
		{
			get
			{
				return _input;
			}
		}
		
		public ArrayList LibPaths
		{
			get
			{
				return _libpaths;
			}
		}
		
		public CompilerResourceCollection Resources
		{
			get
			{
				return _resources;
			}
		}

		public AssemblyCollection References
		{
			get
			{
				return _assemblyReferences;
			}
			
			set
			{
				if (null == value)
				{
					throw new ArgumentNullException("References");
				}
				_assemblyReferences = value;
			}
		}

		/// <summary>
		/// The compilation pipeline.
		/// </summary>
		public CompilerPipeline Pipeline
		{
			get
			{
				return _pipeline;
			}
			
			set
			{
				_pipeline = value;
			}
		}
		
		/// <summary>
		/// The name (full or partial) for the file
		/// that should receive the resulting assembly.
		/// </summary>
		public string OutputAssembly
		{
			get
			{
				return _outputAssembly;
			}
			
			set
			{
				if (null == value)
				{
					throw new ArgumentNullException("OutputAssembly");
				}
				if (0 == value.Length)
				{
					throw new ArgumentException("OutputAssembly");
				}
				_outputAssembly = value;
			}
		}
		
		/// <summary>
		/// Type and execution subsystem for the generated portable
		/// executable file.
		/// </summary>
		public CompilerOutputType OutputType
		{
			get
			{
				return _outputType;
			}
			
			set
			{
				_outputType = value;
			}
		}
		
		public bool GenerateInMemory
		{
			get
			{
				return _generateInMemory;
			}
			
			set
			{
				_generateInMemory = value;
			}
		}
		
		public bool StdLib
		{
			get
			{
				return _StdLib;
			}
			
			set
			{
				_StdLib = value;
			}
		}
		
		public TextWriter OutputWriter
		{
			get
			{
				return _outputWriter;
			}
			
			set
			{
				if (null == value)
				{
					throw new ArgumentNullException("OutputWriter");
				}
				_outputWriter = value;
			}
		}
		
		public bool Debug
		{
			get
			{
				return _debug;
			}
			
			set
			{
				_debug = value;
			}
		}
		
		/// <summary>
		/// Use duck instead of object as the most generic type.
		/// </summary>
		public bool Ducky
		{
			get
			{
				return _ducky;
			}
			
			set
			{
				_ducky = value;
			}
		}

		public bool Checked
		{
			get
			{
				return _checked;
			}
			
			set
			{
				_checked = value;
			}
		}

		public string KeyFile
		{
			get
			{
				return _keyFile;
			}
			
			set
			{
				_keyFile = value;
			}
		}
		
		public string KeyContainer
		{
			get
			{
				return _keyContainer;
			}
			
			set
			{
				_keyContainer = value;
			}
		}
		
		public bool DelaySign
		{
			get
			{
				return _delaySign;
			}
			
			set
			{
				_delaySign = value;
			}
		}
	}
}
