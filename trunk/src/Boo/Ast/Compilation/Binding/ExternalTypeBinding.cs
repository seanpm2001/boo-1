using System;
using System.Reflection;

namespace Boo.Ast.Compilation.Binding
{
	public class ExternalTypeBinding : NamespaceBindingCache, ITypeBinding
	{
		const BindingFlags DefaultBindingFlags = BindingFlags.Public|BindingFlags.Static|BindingFlags.Instance;
		
		BindingManager _bindingManager;
		
		Type _type;
		
		IConstructorBinding[] _constructors;
		
		internal ExternalTypeBinding(BindingManager manager, Type type)
		{
			if (Types.Void == type)
			{
				throw new ArgumentException("type");
			}
			_bindingManager = manager;
			_type = type;
		}
		
		public string Name
		{
			get
			{
				return _type.Name;
			}
		}
		
		public BindingType BindingType
		{
			get
			{
				return BindingType.Type;
			}
		}
		
		public ITypeBinding BoundType
		{
			get
			{
				return this;
			}
		}
		
		public Type Type
		{
			get
			{
				return _type;
			}
		}
		
		public IConstructorBinding[] GetConstructors()
		{
			if (null == _constructors)
			{
				ConstructorInfo[] ctors = _type.GetConstructors(BindingFlags.Public|BindingFlags.Instance);
				_constructors = new IConstructorBinding[ctors.Length];
				for (int i=0; i<_constructors.Length; ++i)
				{
					_constructors[i] = new ExternalConstructorBinding(_bindingManager, ctors[i]);
				}
			}
			return _constructors;
		}
		
		public IBinding Resolve(string name)
		{						
			bool found;
			IBinding binding = ResolveFromCache(name, out found);
			if (found)
			{
				return binding;
			}			
			
			System.Reflection.MemberInfo[] members = _type.GetMember(name, DefaultBindingFlags);
			if (members.Length > 0)
			{				
				binding = _bindingManager.ToBinding(members);
			}
			return Cache(name, binding);
		}
		
		public override string ToString()
		{
			return _type.AssemblyQualifiedName;
		}
	}
}
