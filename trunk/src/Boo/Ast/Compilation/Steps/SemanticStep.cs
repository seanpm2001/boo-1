using System;
using System.Text;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using Boo;
using Boo.Ast;
using Boo.Ast.Compilation;
using Boo.Ast.Compilation.Binding;
using List=Boo.Lang.List;

namespace Boo.Ast.Compilation.Steps
{		
	/// <summary>
	/// Step 4.
	/// </summary>
	public class SemanticStep : AbstractNamespaceSensitiveCompilerStep
	{
		ModuleBuilder _moduleBuilder;
		
		TypeBuilder _typeBuilder;
		
		Method _method;		
		
		IMethodBinding RuntimeServices_IsMatchBinding;
		
		public override void Run()
		{			
			_moduleBuilder = AssemblySetupStep.GetModuleBuilder(CompilerContext);
			
			RuntimeServices_IsMatchBinding = (IMethodBinding)BindingManager.RuntimeServicesBinding.Resolve("IsMatch");
			
			Switch(CompileUnit);
		}		
		
		public override void OnModule(Boo.Ast.Module module)
		{
			TypeAttributes attributes = TypeAttributes.Public|TypeAttributes.Sealed;
			_typeBuilder = _moduleBuilder.DefineType(module.FullyQualifiedName, attributes);
			
			BindingManager.Bind(module, _typeBuilder);			
			
			PushNamespace(new ModuleNameSpace(BindingManager, module));
			
			module.Attributes.Switch(this);
			module.Members.Switch(this);
			
			PopNamespace();
		}
		
		public override void OnMethod(Method method)
		{
			_method = method;
			
			ProcessParameters(method);
			ProcessReturnType(method);			
			
			MethodBuilder builder = _typeBuilder.DefineMethod(method.Name,
				                     MethodAttributes.Static|MethodAttributes.Private,
				                     BindingManager.GetBoundType(method.ReturnType),
				                     GetParameterTypes(method));
			
			InternalMethodBinding binding = new InternalMethodBinding(BindingManager, method, builder);
			BindingManager.Bind(method, binding);
			
			PushNamespace(binding);
			method.Body.Switch(this);
			PopNamespace();
		}
		
		public override void OnTypeReference(TypeReference node)
		{
			IBinding info = ResolveQualifiedName(node.Name);
			if (null == info || BindingType.TypeReference != info.BindingType)
			{
				Errors.NameNotType(node, node.Name);
				BindingManager.Error(node);
			}
			else
			{
				BindingManager.Bind(node, info);
			}
		}
		
		public override void OnBoolLiteralExpression(BoolLiteralExpression node)
		{
			BindingManager.Bind(node, BindingManager.BoolTypeBinding);
		}
		
		public override void OnIntegerLiteralExpression(IntegerLiteralExpression node)
		{
			BindingManager.Bind(node, BindingManager.IntTypeBinding);
		}
		
		public override void OnStringLiteralExpression(StringLiteralExpression node)
		{
			BindingManager.Bind(node, BindingManager.StringTypeBinding);
		}
		
		public override void LeaveStringFormattingExpression(StringFormattingExpression node)
		{
			BindingManager.Bind(node, BindingManager.StringTypeBinding);
		}
		
		public override void OnReferenceExpression(ReferenceExpression node)
		{
			IBinding info = ResolveName(node, node.Name);
			if (null != info)
			{
				BindingManager.Bind(node, info);
			}
			else
			{
				BindingManager.Error(node);
			}
		}
		
		public override void LeaveMemberReferenceExpression(MemberReferenceExpression node)
		{
			IBinding nodeBinding = ErrorBinding.Default;
			
			IBinding binding = GetBinding(node.Target);
			if (BindingType.Error != binding.BindingType)
			{
				ITypedBinding typedBinding = binding as ITypedBinding;
				if (null != typedBinding)
				{
					binding = typedBinding.BoundType;
				}
			
				IBinding member = ((INameSpace)binding).Resolve(node.Name);				
				if (null == member)
				{										
					Errors.MemberNotFound(node, binding.Name);
				}
				else
				{
					nodeBinding = member;
				}
			}
			
			BindingManager.Bind(node, nodeBinding);
		}
		
		public override void LeaveIfStatement(IfStatement node)
		{
			Type type = BindingManager.GetBoundType(node.Expression);
			if (BindingManager.BoolType != type)
			{
				Errors.BoolExpressionRequired(node.Expression, type);
			}
		}
		
		public override void OnForStatement(ForStatement node)
		{
			node.Iterator.Switch(this);
			
			ITypeBinding iteratorType = GetTypeBinding(node.Iterator);
			CheckIterator(node.Iterator, iteratorType);
			ProcessDeclarationsForIterator(node.Declarations, iteratorType, true);
			
			PushNamespace(new DeclarationsNameSpace(BindingManager, node.Declarations));
			node.Statements.Switch(this);
			PopNamespace();
		}
		
		public override void OnUnpackStatement(UnpackStatement node)
		{
			node.Expression.Switch(this);
			
			ITypeBinding expressionType = (ITypeBinding)GetBinding(node.Expression);
			ProcessDeclarationsForIterator(node.Declarations, expressionType, false);			
		}
		
		public override void OnBinaryExpression(BinaryExpression node)
		{
			if (node.Operator == BinaryOperatorType.Assign &&
			    NodeType.ReferenceExpression == node.Left.NodeType)
			{
				// Auto local declaration:
				// assign to unknown reference implies local
				// declaration
				ReferenceExpression reference = (ReferenceExpression)node.Left;
				node.Right.Switch(this);
					
				ITypeBinding expressionTypeInfo = BindingManager.GetTypeBinding(node.Right);
				
				IBinding info = Resolve(reference.Name);					
				if (null == info)
				{
					DeclareLocal(reference, new Local(reference), expressionTypeInfo);
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
				base.OnBinaryExpression(node);
			}
		}
		
		public override void LeaveBinaryExpression(BinaryExpression node)
		{			
			switch (node.Operator)
			{		
				case BinaryOperatorType.Match:
				{
					ExpressionCollection args = new ExpressionCollection();
					args.Add(node.Left);
					args.Add(node.Right);
					
					if (CheckParameters(node, RuntimeServices_IsMatchBinding, args))
					{
						// todo; trocar Bind e BindOperator por um 
						// unico Bind(node, new OperatorBinding())
						BindingManager.Bind(node, RuntimeServices_IsMatchBinding);					
					}
					else
					{
						BindingManager.Error(node);
					}
					break;
				}
				
				case BinaryOperatorType.Inequality:
				{
					ResolveOperator("op_Inequality", node);
					break;
				}
				
				case BinaryOperatorType.Equality:
				{
					ResolveOperator("op_Equality", node);
					break;
				}
				
				default:
				{
					// expression type is the same as the right expression's
					BindingManager.Bind(node, BindingManager.GetBinding(node.Right));
					break;
				}
			}
		}		
		
		public override void OnMethodInvocationExpression(MethodInvocationExpression node)
		{			
			node.Target.Switch(this);
			node.Arguments.Switch(this);
			
			IBinding targetBinding = BindingManager.GetBinding(node.Target);
			if (BindingType.Ambiguous == targetBinding.BindingType)
			{		
				IBinding[] bindings = ((AmbiguousBinding)targetBinding).Bindings;
				targetBinding = ResolveMethodReference(node, node.Arguments, bindings);				
				if (null == targetBinding)
				{
					return;
				}
				BindingManager.Bind(node.Target, targetBinding);
			}	
			
			switch (targetBinding.BindingType)
			{				
				case BindingType.Method:
				{				
					IBinding nodeBinding = ErrorBinding.Default;
					
					IMethodBinding targetMethod = (IMethodBinding)targetBinding;
					if (CheckParameters(node, targetMethod, node.Arguments))
					{
						if (node.NamedArguments.Count > 0)
						{
							Errors.NamedParametersNotAllowed(node.NamedArguments[0]);							
						}
						else
						{			
							if (CheckTargetContext(node.Target, targetMethod))
							{
								nodeBinding = targetMethod.ReturnType;
							}
						}
					}
					
					BindingManager.Bind(node, nodeBinding);
					break;
				}
				
				case BindingType.TypeReference:
				{					
					ITypeBinding typeBinding = ((ITypedBinding)targetBinding).BoundType;					
					ResolveNamedArguments(typeBinding, node);
					
					IConstructorBinding ctorBinding = FindCorrectConstructor(typeBinding, node);
					if (null != ctorBinding)
					{
						// rebind the target now we know
						// it is a constructor call
						BindingManager.Bind(node.Target, ctorBinding);
						// expression result type is a new object
						// of type
						BindingManager.Bind(node, typeBinding);
					}
					break;
				}
				
				case BindingType.Error:
				{
					BindingManager.Error(node);
					break;
				}
				
				default:
				{
					Errors.NotImplemented(node, targetBinding.ToString());
					break;
				}
			}
		}	
		
		IBinding ResolveName(Node node, string name)
		{
			IBinding binding = Resolve(name);
			CheckNameResolution(node, name, binding);
			return binding;
		}
		
		bool CheckNameResolution(Node node, string name, IBinding binding)
		{
			if (null == binding)
			{
				Errors.UnknownName(node, name);			
				return false;
			}
			return true;
		}
		
		void ResolveNamedArguments(ITypeBinding typeBinding, MethodInvocationExpression node)
		{
			foreach (ExpressionPair arg in node.NamedArguments)
			{			
				arg.Second.Switch(this);				
				
				ReferenceExpression name = arg.First as ReferenceExpression;
				if (null == name)
				{
					Errors.NamedParameterMustBeReference(arg);
					continue;				
				}
				
				IBinding member = typeBinding.Resolve(name.Name);
				if (null == member)
				{
					Errors.NotAPublicFieldOrProperty(node, typeBinding.Type, name.Name);
					continue;
				}
				
				BindingManager.Bind(arg.First, member);				
				
				Type memberType = ((ITypedBinding)member).BoundType.Type;
				Type expressionType = BindingManager.GetBoundType(arg.Second);
				if (!IsAssignableFrom(memberType, expressionType))
				{
					Errors.IncompatibleExpressionType(arg, memberType, expressionType);
				}
			}
		}
		
		bool CheckParameters(Node sourceNode, IMethodBinding method, ExpressionCollection args)
		{				
			if (method.ParameterCount != args.Count)
			{
				Errors.MethodArgumentCount(sourceNode, method, args.Count);
				return false;
			}	
			
			for (int i=0; i<args.Count; ++i)
			{
				Type expressionType = BindingManager.GetBoundType(args[i]);
				Type parameterType = method.GetParameterType(i);
				if (!IsAssignableFrom(parameterType, expressionType) &&
				    !CanBeReachedByDownCast(parameterType, expressionType))
				{
					Errors.MethodSignature(sourceNode, GetSignature(args), GetSignature(method));
					return false;
				}
			}
			
			return true;
		}
		
		void CheckIterator(Expression iterator, ITypeBinding binding)
		{			
			Type type = binding.Type;
			if (type.IsArray)
			{
				if (type.GetArrayRank() > 1)
				{
					Errors.InvalidArray(iterator);
				}
			}
		}		
		
		bool CheckTargetContext(Expression targetContext, IMemberBinding member)
		{
			if (!member.IsStatic)					  
			{			
				if (NodeType.MemberReferenceExpression == targetContext.NodeType)
				{				
					Expression targetReference = ((MemberReferenceExpression)targetContext).Target;
					if (BindingType.TypeReference == GetBinding(targetReference).BindingType)
					{						
						Errors.MemberNeedsInstance(targetContext, member.ToString());
						return false;
					}
				}
			}
			return true;
		}
		
		static bool IsAssignableFrom(Type expectedType, Type actualType)
		{
			return expectedType.IsAssignableFrom(actualType);
		}
		
		static bool CanBeReachedByDownCast(Type expectedType, Type actualType)
		{
			return actualType.IsAssignableFrom(expectedType);
		}		
		
		IConstructorBinding FindCorrectConstructor(ITypeBinding typeBinding, MethodInvocationExpression mie)
		{
			return (IConstructorBinding)ResolveMethodReference(mie, mie.Arguments, typeBinding.GetConstructors());
		}
		
		class BindingScore : IComparable
		{
			public IBinding Binding;
			public int Score;
			
			public BindingScore(IBinding binding, int score)
			{
				Binding = binding;
				Score = score;
			}
			
			public int CompareTo(object other)
			{
				return ((BindingScore)other).Score-Score;
			}
			
			public override string ToString()
			{
				return Binding.ToString();
			}
		}
		
		IBinding ResolveMethodReference(Node node, ExpressionCollection args, IBinding[] bindings)
		{			
			List scores = new List();
			for (int i=0; i<bindings.Length; ++i)
			{				
				IBinding binding = bindings[i];
				IMethodBinding mb = binding as IMethodBinding;
				if (null != mb)
				{					
					if (args.Count == mb.ParameterCount)
					{
						int score = 0;
						for (int argIndex=0; argIndex<args.Count; ++argIndex)
						{
							Type expressionType = BindingManager.GetBoundType(args[argIndex]);
							Type parameterType = mb.GetParameterType(argIndex);						
							
							if (parameterType == expressionType)
							{
								// exact match scores 3
								score += 3;
							}
							else if (IsAssignableFrom(parameterType, expressionType))
							{
								// upcast scores 2
								score += 2;
							}
							else if (CanBeReachedByDownCast(parameterType, expressionType))
							{
								// downcast scores 1
								score += 1;
							}
							else
							{
								score = -1;
								break;
							}
						}						
						
						if (score >= 0)
						{
							// only positive scores are compatible
							scores.Add(new BindingScore(binding, score));						
						}
					}
				}
			}		
			
			if (1 == scores.Count)
			{
				return ((BindingScore)scores[0]).Binding;
			}
			
			if (scores.Count > 1)
			{
				scores.Sort();
				
				BindingScore first = (BindingScore)scores[0];
				BindingScore second = (BindingScore)scores[1];
				if (first.Score > second.Score)
				{
					return first.Binding;
				}
				// todo: remove from scores, all the lesser
				// scored bindings
				
				Errors.AmbiguousName(node, "", scores);
			}
			else
			{
				Errors.NoApropriateOverloadFound(node, GetSignature(args), bindings[0].Name);
			}
			BindingManager.Error(node);	
			return null;
		}
		
		void ResolveOperator(string name, BinaryExpression node)
		{
			ITypeBinding boundType = ((ITypedBinding)BindingManager.GetBinding(node.Left)).BoundType;
			IBinding binding = boundType.Resolve(name);
			if (null == binding)
			{
				BindingManager.Error(node);
			}
			else
			{
				// todo: check parameters
				// todo: resolve when ambiguous
				BindingManager.Bind(node, binding);
			}
		}
		
		void DeclareLocal(Node sourceNode, Local local, ITypeBinding localType)
		{			
			LocalBinding binding = new LocalBinding(local, localType);
			BindingManager.Bind(local, binding);
			
			_method.Locals.Add(local);
			BindingManager.Bind(sourceNode, binding);
		}
		
		void ProcessDeclarationsForIterator(DeclarationCollection declarations, ITypeBinding iteratorType, bool declarePrivateLocals)
		{
			ITypeBinding defaultDeclType = BindingManager.ObjectTypeBinding;
			
			if (iteratorType.Type.IsArray)
			{		
				defaultDeclType = BindingManager.ToTypeBinding(iteratorType.Type.GetElementType());
			}
			
			foreach (Declaration d in declarations)
			{				
				if (null == d.Type)
				{
					d.Type = new TypeReference(defaultDeclType.Type.FullName);
					BindingManager.Bind(d.Type, defaultDeclType);
				}
				else
				{
					d.Type.Switch(this);
					// todo: check types here
				}
				
				DeclareLocal(d, new Local(d), BindingManager.GetTypeBinding(d.Type));
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
					BindingManager.Bind(parameter.Type, BindingManager.ToTypeReference(BindingManager.ObjectTypeBinding));
				}		
				else
				{
					parameter.Type.Switch(this);
				}
				Binding.ParameterBinding binding = new Binding.ParameterBinding(parameter, GetTypeBinding(parameter.Type), i);
				BindingManager.Bind(parameter, binding);
			}
		}
		
		void ProcessReturnType(Method method)
		{
			if (null == method.ReturnType)
			{
				// Por enquanto, valor de retorno apenas void
				method.ReturnType = new TypeReference("void");
				BindingManager.Bind(method.ReturnType, BindingManager.ToTypeReference(BindingManager.VoidTypeBinding));
			}
			else
			{
				if (!BindingManager.IsBound(method.ReturnType))
				{
					method.ReturnType.Switch(this);
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
		
		string GetSignature(ExpressionCollection args)
		{
			StringBuilder sb = new StringBuilder("(");
			foreach (Expression arg in args)
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
			return BindingManager.GetSignature(binding);
		}		
	}
}
