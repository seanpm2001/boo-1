using System;
using System.Text;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using Boo;
using Boo.Ast;
using Boo.Ast.Compilation;
using Boo.Ast.Compilation.Binding;

namespace Boo.Ast.Compilation.Steps
{		
	/// <summary>
	/// Step 4.
	/// </summary>
	public class SemanticStep : AbstractCompilerStep
	{
		ModuleBuilder _moduleBuilder;
		
		TypeBuilder _typeBuilder;
		
		Method _method;
		
		INameSpace _namespace;
		
		public override void Run()
		{
			_moduleBuilder = AssemblySetupStep.GetModuleBuilder(CompilerContext);
			
			Switch(CompileUnit);
		}
		
		public override bool EnterCompileUnit(CompileUnit cu)
		{			
			// builtins resolution at the highest level
			_namespace = new TypeNameSpace(BindingManager, null, typeof(Boo.Lang.Builtins));
			return true;
		}
		
		public override bool EnterModule(Boo.Ast.Module module)
		{
			TypeAttributes attributes = TypeAttributes.Public|TypeAttributes.Sealed;
			_typeBuilder = _moduleBuilder.DefineType(module.FullyQualifiedName, attributes);
			
			BindingManager.Bind(module, _typeBuilder);
			
			_namespace = new TypeDefinitionNameSpace(BindingManager, _namespace, module);
			return true;
		}
		
		public override void OnMethod(Method method)
		{
			_method = method;
			
			ProcessParameters(method);
			ProcessReturnType(method);
			
			_namespace = new MethodNameSpace(BindingManager, _namespace, _method);			
			
			MethodBuilder mbuilder = _typeBuilder.DefineMethod(method.Name,
				                     MethodAttributes.Static|MethodAttributes.Private,
				                     BindingManager.GetBoundType(method.ReturnType),
				                     GetParameterTypes(method));
			BindingManager.Bind(method, mbuilder);
			
			method.Body.Switch(this);
		}
		
		public override void OnTypeReference(TypeReference node)
		{
			IBinding info = ResolveName(node, node.Name);
			if (null != info)
			{
				if (BindingType.Type != info.BindingType)
				{
					Errors.NameNotType(node, node.Name);
				}
				else
				{
					BindingManager.Bind(node, info);
				}
			}
		}
		
		void ProcessParameters(Method method)
		{
			ParameterDeclarationCollection parameters = method.Parameters;
			for (int i=0; i<parameters.Count; ++i)
			{
				ParameterDeclaration parameter = parameters[i];
				if (null == parameter.Type)
				{
					parameter.Type = new TypeReference("object");
					BindingManager.Bind(parameter.Type, BindingManager.ToTypeBinding(BindingManager.ObjectType));
				}		
				else
				{
					parameter.Type.Switch(this);
				}
				Binding.ParameterBinding info = new Binding.ParameterBinding(parameter, BindingManager.GetTypeBinding(parameter.Type), i);
				BindingManager.Bind(parameter, info);
			}
		}
		
		void ProcessReturnType(Method method)
		{
			if (null == method.ReturnType)
			{
				// Por enquanto, valor de retorno apenas void
				method.ReturnType = new TypeReference("void");
				BindingManager.Bind(method.ReturnType, BindingManager.ToTypeBinding(BindingManager.VoidType));
			}
			else
			{
				if (!BindingManager.IsBound(method.ReturnType))
				{
					method.ReturnType.Switch(this);
				}
			}
		}
		
		public override void OnStringLiteralExpression(StringLiteralExpression node)
		{
			BindingManager.Bind(node, BindingManager.StringType);
		}
		
		public override void LeaveStringFormattingExpression(StringFormattingExpression node)
		{
			BindingManager.Bind(node, BindingManager.StringType);
		}
		
		public override void OnReferenceExpression(ReferenceExpression node)
		{
			IBinding info = ResolveName(node, node.Name);
			if (null != info)
			{
				BindingManager.Bind(node, info);
			}
		}
		
		public override void OnBinaryExpression(BinaryExpression node)
		{
			if (node.Operator == BinaryOperatorType.Assign)
			{
				// Auto local declaration:
				// assign to unknown reference implies local
				// declaration
				ReferenceExpression reference = node.Left as ReferenceExpression;
				if (null != reference)
				{
					node.Right.Switch(this);
					
					ITypeBinding expressionTypeInfo = BindingManager.GetTypeBinding(node.Right);
					
					IBinding info = _namespace.Resolve(reference.Name);					
					if (null == info)
					{
						Local local = new Local(reference);
						LocalBinding localInfo = new LocalBinding(local, expressionTypeInfo);
						BindingManager.Bind(local, localInfo);
						
						_method.Locals.Add(local);
						BindingManager.Bind(reference, localInfo);
					}
					else
					{
						// default reference resolution
						if (CheckNameResolution(reference, reference.Name, info))
						{
							BindingManager.Bind(reference, info);
						}
					}
					
					LeaveBinaryExpression(node);
				}
				else
				{
					throw new NotImplementedException();
				}
			}
			else
			{
				base.OnBinaryExpression(node);
			}
		}
		
		public override void LeaveBinaryExpression(BinaryExpression node)
		{
			// expression type is the same as the right expression's
			BindingManager.Bind(node, BindingManager.GetBinding(node.Right));
		}
		
		public override void LeaveMethodInvocationExpression(MethodInvocationExpression node)
		{			
			IBinding targetType = BindingManager.GetBinding(node.Target);			
			if (targetType.BindingType == BindingType.Method)
			{				
				IMethodBinding targetMethod = (IMethodBinding)targetType;
				CheckParameters(targetMethod, node);			
				// todo: if not CheckParameter
				// Bind(node, BindingManager.UnknownType)
				BindingManager.Bind(node, targetMethod.ReturnType);
			}
			else
			{
				throw new NotImplementedException();
			}
		}
		
		IBinding ResolveName(Node node, string name)
		{
			IBinding info = null;
			switch (name)
			{
				case "void":
				{
					info = BindingManager.ToTypeBinding(BindingManager.VoidType);
					break;
				}
				
				case "string":
				{
					info = BindingManager.ToTypeBinding(BindingManager.StringType);
					break;
				}
				
				default:
				{
					info = _namespace.Resolve(name);
					CheckNameResolution(node, name, info);
					break;
				}
			}			
			return info;
		}
		
		bool CheckNameResolution(Node node, string name, IBinding info)
		{
			if (null == info)
			{
				Errors.UnknownName(node, name);			
				return false;
			}
			else
			{
				if (info.BindingType == BindingType.Ambiguous)
				{
					//Errors.AmbiguousName(node, name, info);
					//return false;
					throw new NotImplementedException();
				}
			}
			return true;
		}
		
		void CheckParameters(IMethodBinding method, MethodInvocationExpression mie)
		{	
			ExpressionCollection args = mie.Arguments;
			if (method.ParameterCount != args.Count)
			{
				Errors.MethodArgumentCount(mie, method);
			}
			
			for (int i=0; i<args.Count; ++i)
			{
				Type expressionType = BindingManager.GetBoundType(args[i]);
				Type parameterType = method.GetParameterType(i);
				if (expressionType != parameterType)
				{
					Errors.MethodSignature(mie, GetSignature(mie), GetSignature(method));
					break;
				}
			}
		}
		
		Type[] GetParameterTypes(Method method)
		{
			ParameterDeclarationCollection parameters = method.Parameters;
			Type[] types = new Type[parameters.Count];
			for (int i=0; i<types.Length; ++i)
			{
				types[i] = BindingManager.GetBoundType(parameters[i].Type);
			}
			return types;
		}
		
		string GetSignature(MethodInvocationExpression mie)
		{
			StringBuilder sb = new StringBuilder("(");
			foreach (Expression arg in mie.Arguments)
			{
				if (sb.Length > 1)
				{
					sb.Append(", ");
				}
				sb.Append(BindingManager.GetBoundType(arg));
			}
			sb.Append(")");
			return sb.ToString();
		}
		
		string GetSignature(IMethodBinding binding)
		{
			StringBuilder sb = new StringBuilder(binding.MethodInfo.DeclaringType.FullName);
			sb.Append(".");
			sb.Append(binding.MethodInfo.Name);
			sb.Append("(");
			for (int i=0; i<binding.ParameterCount; ++i)
			{				
				if (i>0) 
				{
					sb.Append(", ");
				}
				sb.Append(binding.GetParameterType(i).FullName);
			}
			sb.Append(") as ");
			sb.Append(binding.ReturnType.Type.FullName);
			return sb.ToString();
		}
	}
}
