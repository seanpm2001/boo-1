﻿#region license
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

namespace Boo.NAnt

import System
import System.Diagnostics
import System.IO
import NAnt.Core
import NAnt.Core.Attributes
import NAnt.Core.Types
import Boo.Lang.Compiler

abstract class AbstractBooTask(Task):
	
	def constructor():
		baseAssemblyFolder = Path.GetDirectoryName(GetType().Assembly.Location)
		System.Reflection.Assembly.LoadFrom(
			Path.Combine(baseAssemblyFolder, "Boo.AntlrParser.dll"))
	
	protected def RunCompiler(compiler as BooCompiler):		
		result = compiler.Run()
		CheckCompilationResult(result)
		return result
	
	protected def CheckCompilationResult(context as CompilerContext):
		errors = context.Errors
		verbose = context.Parameters.TraceSwitch.TraceInfo
		for error in errors:
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
