using System;
using System.Collections;
using System.Reflection;
using List=Boo.Lang.List;
using Boo.Ast;
using Boo.Ast.Compilation;
using Boo.Ast.Compilation.Binding;

namespace Boo.Ast.Compilation.Steps
{	
	// todo: CompilerParameters.References.Changed += OnChanged
	// recalculate namespaces on reference changes
	// todo: optimize this class so it only reescans
	// the references when they change
	public class UsingResolutionStep : AbstractCompilerStep, INamespace
	{
		static object GlobalNamespaceKey = new object();
		
		static object BooLangNamespaceKey = new object();
		
		Hashtable _namespaces = new Hashtable();
		
		Hashtable _externalTypes = new Hashtable();
		
		public static INamespace GetGlobalNamespace(CompilerContext context)
		{
			return (INamespace)context.CompileUnit[GlobalNamespaceKey];
		}		
		
		public static INamespace GetBooLangNamespace(CompilerContext context)
		{
			return (INamespace)context.CompileUnit[BooLangNamespaceKey];
		}
		
		public override void Run()
		{
			ResolveNamespaces();
			CompileUnit[GlobalNamespaceKey] = this;
			CompileUnit[BooLangNamespaceKey] = ResolveQualifiedName("Boo.Lang");
		}
		
		public override void Dispose()
		{
			base.Dispose();
			_namespaces.Clear();
			_externalTypes.Clear();
		}
		
		void ResolveNamespaces()
		{				
			ResolveUsingAssemblyReferences();
			OrganizeNamespaces();
			
			foreach (Module module in CompileUnit.Modules)
			{
				foreach (Using using_ in module.Using)
				{
					IBinding binding = ResolveQualifiedName(using_.Namespace);					
					if (null == binding)
					{
						binding = ErrorBinding.Default;
						Errors.InvalidNamespace(using_);
					}
					else
					{
						if (null != using_.AssemblyReference)
						{	
							NamespaceBinding nsBinding = binding as NamespaceBinding;
							if (null == nsBinding)
							{
								Errors.NotImplemented(using_, "assembly qualified type references");
							}
							else
							{								
								binding = new AssemblyQualifiedNamespaceBinding(GetBoundAssembly(using_.AssemblyReference), nsBinding);
							}
						}
						if (null != using_.Alias)
						{
							binding = new AliasedNamespaceBinding(using_.Alias.Name, binding);
							BindingManager.Bind(using_.Alias, binding);
						}
					}
					
					_context.TraceVerbose("Node {0} at {1} bound to {2}", using_, using_.LexicalInfo, binding.Name);
					BindingManager.Bind(using_, binding);
				}
			}			
		}
		
		void ResolveUsingAssemblyReferences()
		{
			foreach (Boo.Ast.Module module in CompileUnit.Modules)
			{
				UsingCollection usingCollection = module.Using;
				Using[] usingArray = usingCollection.ToArray();
				for (int i=0; i<usingArray.Length; ++i)
				{
					Using u = usingArray[i];
					ReferenceExpression reference = u.AssemblyReference;
					if (null != reference)
					{
						try
						{
							Assembly asm = Assembly.LoadWithPartialName(reference.Name);
							CompilerParameters.References.Add(asm);
							BindingManager.Bind(reference, new Binding.AssemblyBinding(asm));
						}
						catch (Exception x)
						{
							Errors.UnableToLoadAssembly(reference, reference.Name, x);
							usingCollection.RemoveAt(i);							
						}
					}
				}
			}
		}
		
		Assembly GetBoundAssembly(ReferenceExpression reference)
		{
			return ((AssemblyBinding)BindingManager.GetBinding(reference)).Assembly;
		}
		
		public IBinding Resolve(string name)
		{
			IBinding binding = (IBinding)_namespaces[name];
			if (null == binding)
			{
				// todo: resolve from externalTypes here
			}			
			return binding;
		}
		
		IBinding ResolveQualifiedName(string name)
		{
			string[] parts = name.Split('.');
			string topLevel = parts[0];
			
			INamespace ns = (INamespace)_namespaces[topLevel];
			for (int i=1; i<parts.Length; ++i)
			{
				ns = (INamespace)ns.Resolve(parts[i]);
				if (null == ns)
				{
					break;
				}
			}
			return (IBinding)ns;
		}
		
		void OrganizeNamespaces()
		{
			foreach (Assembly asm in CompilerParameters.References)
			{
				Type[] types = asm.GetTypes();
				foreach (Type type in types)
				{
					string ns = type.Namespace;
					if (null == ns)
					{
						ns = string.Empty;
					}
					
					string[] namespaceHierarchy = ns.Split('.');
					string topLevelName = namespaceHierarchy[0];
					Binding.NamespaceBinding topLevel = GetNamespaceBinding(topLevelName);
					Binding.NamespaceBinding current = topLevel;
					for (int i=1; i<namespaceHierarchy.Length; ++i)
					{
						current = current.GetChildNamespace(namespaceHierarchy[i]);
						// Trace(current);
					}					
					current.Add(type);
					
					List typeList = GetList(_externalTypes, type.FullName);
					typeList.Add(type);
				}				
			}
		}
		
		Binding.NamespaceBinding GetNamespaceBinding(string name)
		{
			Binding.NamespaceBinding binding = (Binding.NamespaceBinding)_namespaces[name];	
			if (null == binding)
			{
				_namespaces[name] = binding = new Binding.NamespaceBinding(BindingManager, name);
			}
			return binding;
		}
		
		List GetList(Hashtable hash, string key)
		{
			List list = (List)hash[key];
			if (null == list)
			{
				list = new List();
				hash[key] = list;
			}
			return list;
		}
	}
}
