namespace Boo.Ast.Compilation.Binding
{
	class ModuleNamespace : INamespace
	{
		Module _module;
		
		BindingManager _bindingManager;
		
		INamespace[] _using;
		
		public ModuleNamespace(BindingManager bindingManager, Module module)
		{
			_bindingManager = bindingManager;
			_module = module;
			_using = new INamespace[_module.Using.Count];
			for (int i=0; i<_using.Length; ++i)
			{
				_using[i] = (INamespace)bindingManager.GetBinding(_module.Using[i]);
			}
		}
		
		public IBinding Resolve(string name)
		{			
			foreach (TypeMember member in _module.Members)
			{
				if (name == member.Name)
				{
					return _bindingManager.GetBinding(member);
				}
			}			
			
			foreach (INamespace ns in _using)
			{
				// todo: resolve name in all namespaces...
				IBinding binding = ns.Resolve(name);
				if (null != binding)
				{					
					return binding;
				}
			}
			return null;
		}
	}
}
