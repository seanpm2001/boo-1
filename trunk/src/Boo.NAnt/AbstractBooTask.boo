#region license
// boo - an extensible programming language for the CLI
// Copyright (C) 2004 Rodrigo B. de Oliveira
//
// Permission is hereby granted, free of charge, to any person 
// obtaining a copy of this software and associated documentation 
// files (the "Software"), to deal in the Software without restriction, 
// including without limitation the rights to use, copy, modify, merge, 
// publish, distribute, sublicense, and/or sell copies of the Software, 
// and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included 
// in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY 
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
// Contact Information
//
// mailto:rbo@acm.org
#endregion

namespace Boo.NAnt

import System
import System.Diagnostics
import System.IO
import NAnt.Core
import NAnt.Core.Attributes
import NAnt.Core.Types
import Boo.Lang.Compiler

abstract class AbstractBooTask(Task):
	
	_baseAssemblyFolder as string
	
	def constructor():
		_baseAssemblyFolder = Path.GetDirectoryName(GetType().Assembly.Location)	
	
	protected def RunCompiler(compiler as BooCompiler):
		
		domain = AppDomain.CurrentDomain
		try:
			domain.AssemblyResolve += AppDomain_AssemblyResolve
			result = compiler.Run()
			CheckCompilationResult(result)
			return result
		ensure:
			domain.AssemblyResolve -= AppDomain_AssemblyResolve
			
	private def AppDomain_AssemblyResolve(sender, args as ResolveEventArgs) as System.Reflection.Assembly:
		print("Looking for assembly: ${args.Name}")
		
		simpleName = /,\s+/.Split(args.Name)[0]
		fname = Path.Combine(_baseAssemblyFolder, simpleName + ".dll")
		if File.Exists(fname):
			print("${fname} was found!")
			return System.Reflection.Assembly.LoadFrom(fname)
		else:
			print("${fname} was NOT found!")
	
	protected def CheckCompilationResult(context as CompilerContext):
		errors = context.Errors
		verbose = context.Parameters.TraceSwitch.TraceInfo
		for error as CompilerError in errors:
			LogError(error.ToString(verbose))
			
		if len(errors):
			LogInfo("${len(errors)} error(s).")
			raise BuildException("boo compilation error", Location)

	def GetFrameworkDirectory():
		return Project.TargetFramework.FrameworkAssemblyDirectory.ToString()
		
	def print(message):
		LogVerbose(message)

	def LogInfo(message):
		self.Log(Level.Info, "${LogPrefix}${message}")
		
	def LogVerbose(message):
		self.Log(Level.Verbose, "${LogPrefix}${message}")
		
	def LogError(message):
		self.Log(Level.Error, "${LogPrefix}${message}")
